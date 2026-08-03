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
        string resolvedType = DocumentRegistryKeyNormalizer.NormalizeDocumentType(documentType);
        string resolvedTemplate = DocumentRegistryKeyNormalizer.NormalizeTemplateName(templateName);

        var section = _configuration.GetSection($"DocumentRegistry:{resolvedType}:{resolvedTemplate}");
        if (!section.Exists())
        {
            throw new System.Exception($"Registry configuration not found for {resolvedType}/{resolvedTemplate}");
        }

        var config = section.Get<DocumentConfiguration>();
        return new DocumentMetadata
        {
            DocumentType = resolvedType,
            TemplateName = resolvedTemplate,
            Configuration = config ?? new DocumentConfiguration()
        };
    }

    public void Register(DocumentMetadata metadata)
    {
        throw new System.NotImplementedException("Registry is configured via appsettings.json currently.");
    }
}
