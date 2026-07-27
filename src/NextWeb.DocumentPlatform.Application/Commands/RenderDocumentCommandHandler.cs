using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NextWeb.DocumentPlatform.Engine;

namespace NextWeb.DocumentPlatform.Application.Commands;

public class RenderDocumentCommandHandler : IRequestHandler<RenderDocumentCommand, byte[]>
{
    private readonly IErpForwardingService _erpService;
    private readonly IDocumentRegistry _registry;
    private readonly IDocumentRendererFactory _rendererFactory;

    public RenderDocumentCommandHandler(
        IErpForwardingService erpService,
        IDocumentRegistry registry,
        IDocumentRendererFactory rendererFactory)
    {
        _erpService = erpService;
        _registry = registry;
        _rendererFactory = rendererFactory;
    }

    public async Task<byte[]> Handle(RenderDocumentCommand request, CancellationToken cancellationToken)
    {
        // 1. Fetch Data
        string jsonPayload = await _erpService.GetDocumentDataAsync(request.RequestContext, cancellationToken);

        // 2. Resolve Template Name from settings
        string templateName = "Standard";
        try
        {
            using var doc = System.Text.Json.JsonDocument.Parse(jsonPayload);
            if (doc.RootElement.TryGetProperty("data", out var dataElement) &&
                dataElement.TryGetProperty("settings", out var settingsElement) &&
                settingsElement.TryGetProperty("pdf_template", out var pdfTemplateElement) &&
                pdfTemplateElement.ValueKind == System.Text.Json.JsonValueKind.String)
            {
                var val = pdfTemplateElement.GetString();
                if (!string.IsNullOrWhiteSpace(val))
                {
                    templateName = val;
                }
            }
        }
        catch
        {
            // fallback to default
        }

        // 3. Get Metadata & configuration
        var metadata = _registry.GetMetadata(request.RequestContext.RouteDocumentType, templateName);

        if (!metadata.Configuration.IsEnabled)
        {
            throw new System.Exception("Template is disabled.");
        }

        // 4. Resolve Renderer
        var renderer = _rendererFactory.GetRenderer(request.RequestContext.RouteDocumentType, templateName);

        // 5. Render
        return await renderer.RenderAsync(jsonPayload, metadata.Configuration, cancellationToken);
    }
}
