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
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ErpForwardingService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> GetDocumentDataAsync(HttpRequestContext requestContext, CancellationToken cancellationToken)
    {
        var baseUrl = _configuration["DocumentPlatform:UpstreamApi:BaseUrl"];
        if (string.IsNullOrEmpty(baseUrl))
        {
            throw new Exception("Upstream API BaseUrl is not configured.");
        }

        // Parse id from query string e.g. "?id=123"
        var queryDictionary = System.Web.HttpUtility.ParseQueryString(requestContext.QueryString);
        var id = queryDictionary["id"];

        if (string.IsNullOrEmpty(id))
        {
            throw new Exception("Missing 'id' parameter in query string.");
        }

        // Construct target URL
        var targetUrl = $"{baseUrl.TrimEnd('/')}/api/voucher/{id}/print";
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
