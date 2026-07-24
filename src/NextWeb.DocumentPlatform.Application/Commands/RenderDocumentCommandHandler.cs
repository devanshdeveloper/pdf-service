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
        // 1. Get Metadata & configuration
        var metadata = _registry.GetMetadata(request.RequestContext.RouteDocumentType, request.RequestContext.RouteTemplateName);

        if (!metadata.Configuration.IsEnabled)
        {
            throw new System.Exception("Template is disabled.");
        }

        // 2. Fetch Data
        string jsonPayload = await _erpService.GetDocumentDataAsync(request.RequestContext, cancellationToken);

        // 3. Resolve Renderer
        var renderer = _rendererFactory.GetRenderer(request.RequestContext.RouteDocumentType, request.RequestContext.RouteTemplateName);

        // 4. Render
        return await renderer.RenderAsync(jsonPayload, metadata.Configuration, cancellationToken);
    }
}
