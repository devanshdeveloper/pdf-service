using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using NextWeb.DocumentPlatform.Engine;

namespace NextWeb.DocumentPlatform.Infrastructure;

public class DocumentRendererFactory : IDocumentRendererFactory
{
    private readonly IServiceProvider _serviceProvider;

    public DocumentRendererFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IDocumentRenderer GetRenderer(string documentType, string templateName)
    {
        string resolvedType = DocumentRegistryKeyNormalizer.NormalizeDocumentType(documentType);
        string resolvedTemplate = DocumentRegistryKeyNormalizer.NormalizeTemplateName(templateName);

        var renderers = _serviceProvider.GetServices<IDocumentRenderer>();
        var renderer = renderers.FirstOrDefault(r =>
            DocumentRegistryKeyNormalizer.NormalizeDocumentType(r.DocumentType)
                .Equals(resolvedType, StringComparison.OrdinalIgnoreCase) &&
            DocumentRegistryKeyNormalizer.NormalizeTemplateName(r.TemplateName)
                .Equals(resolvedTemplate, StringComparison.OrdinalIgnoreCase));

        if (renderer == null)
        {
            throw new Exception($"No renderer found for type '{documentType}' (resolved to '{resolvedType}') and template '{templateName}' (resolved to '{resolvedTemplate}')");
        }

        return renderer;
    }
}
