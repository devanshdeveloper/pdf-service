using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using NextWeb.DocumentPlatform.Application;

namespace NextWeb.DocumentPlatform.Infrastructure;

public class MockErpForwardingService : IErpForwardingService
{
    public async Task<string> GetDocumentDataAsync(HttpRequestContext requestContext, CancellationToken cancellationToken)
    {
        // For development/testing, we read the JSON payload directly from the context folder
        var path = @"c:\code\next-web-works\pdf-service\context\QUEST PDF.json";
        
        if (!File.Exists(path))
        {
            throw new System.Exception("Mock JSON file not found.");
        }

        var json = await File.ReadAllTextAsync(path, cancellationToken);
        
        // The QUEST PDF.json file contains the OpenAPI spec with the '200' response embedded.
        // We will extract the inner response data for simplicity.
        using var doc = JsonDocument.Parse(json);
        var responseNode = doc.RootElement
            .GetProperty("paths")
            .GetProperty("/api/voucher/6a61be54cfc2809fe2131bfa/print")
            .GetProperty("get")
            .GetProperty("responses")
            .GetProperty("200");
            
        return responseNode.GetRawText();
    }
}
