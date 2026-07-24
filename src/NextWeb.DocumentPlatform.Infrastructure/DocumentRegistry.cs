using Microsoft.Extensions.Configuration;
using NextWeb.DocumentPlatform.Domain;
using NextWeb.DocumentPlatform.Engine;

namespace NextWeb.DocumentPlatform.Infrastructure;

public class DocumentRegistry : IDocumentRegistry
{
    private readonly IConfiguration _configuration;

    public DocumentRegistry(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DocumentMetadata GetMetadata(string documentType, string templateName)
    {
        string resolvedType = documentType.StartsWith("voucher/", System.StringComparison.OrdinalIgnoreCase) 
            ? "Voucher" 
            : documentType;

        var section = _configuration.GetSection($"DocumentRegistry:{resolvedType}:{templateName}");
        if (!section.Exists())
        {
            throw new System.Exception($"Registry configuration not found for {resolvedType}/{templateName}");
        }

        var config = section.Get<DocumentConfiguration>();
        return new DocumentMetadata
        {
            DocumentType = resolvedType,
            TemplateName = templateName,
            Configuration = config ?? new DocumentConfiguration()
        };
    }

    public void Register(DocumentMetadata metadata)
    {
        throw new System.NotImplementedException("Registry is configured via appsettings.json currently.");
    }
}
