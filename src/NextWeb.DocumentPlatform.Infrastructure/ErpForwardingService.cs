using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NextWeb.DocumentPlatform.Application;

namespace NextWeb.DocumentPlatform.Infrastructure;

public class ErpForwardingService : IErpForwardingService
{
    private readonly string _baseUpstreamUrl;
    private readonly HttpClient _httpClient;

    public ErpForwardingService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _baseUpstreamUrl = configuration["DocumentPlatform:UpstreamApi:BaseUrl"] ?? throw new ArgumentException("BaseUrl is not configured.");
    }

    public async Task<string> GetDocumentDataAsync(HttpRequestContext requestContext, CancellationToken cancellationToken)
    {
        var model = requestContext.RouteDocumentType;
        var id = requestContext.RouteDocumentId;

        if (string.IsNullOrEmpty(model) || string.IsNullOrEmpty(id))
        {
            throw new Exception("Missing 'model' or 'id' route parameters.");
        }

        // Construct target URL — upstream ERP routes use kebab-case (e.g. material-entry), not PascalCase.
        var upstreamModel = UpstreamApiRouteResolver.Resolve(model);
        var targetUrl = $"{_baseUpstreamUrl.TrimEnd('/')}/api/{upstreamModel}/{id}/print";
        if (!string.IsNullOrEmpty(requestContext.QueryString))
        {
            var query = requestContext.QueryString;
            if (!query.StartsWith('?'))
                query = "?" + query;
            targetUrl += query;
        }
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, targetUrl);

        // Forward headers
        var excludedHeaders = new[] { "host", "connection", "accept-encoding", "content-length", "cookie", "content-type" };
        foreach (var header in requestContext.Headers)
        {
            if (excludedHeaders.Contains(header.Key.ToLowerInvariant()))
            {
                continue;
            }

            requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        // Send request
        using var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
        
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to fetch data from upstream API. Status: {response.StatusCode}, Content: {responseContent}");
        }

        return responseContent;
    }
}
