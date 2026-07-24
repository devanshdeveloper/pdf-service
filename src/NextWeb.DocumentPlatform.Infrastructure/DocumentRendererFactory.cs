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
        // For Voucher subtypes, map to base "Voucher"
        string resolvedType = documentType.StartsWith("voucher/", StringComparison.OrdinalIgnoreCase) 
            ? "Voucher" 
            : documentType;

        var renderers = _serviceProvider.GetServices<IDocumentRenderer>();
        var renderer = renderers.FirstOrDefault(r => 
            r.DocumentType.Equals(resolvedType, StringComparison.OrdinalIgnoreCase) && 
            r.TemplateName.Equals(templateName, StringComparison.OrdinalIgnoreCase));

        if (renderer == null)
        {
            throw new Exception($"No renderer found for type '{documentType}' (resolved to '{resolvedType}') and template '{templateName}'");
        }

        return renderer;
    }
}
