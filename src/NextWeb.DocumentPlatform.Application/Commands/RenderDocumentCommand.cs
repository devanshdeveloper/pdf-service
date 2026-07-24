using MediatR;

namespace NextWeb.DocumentPlatform.Application.Commands;

public class RenderDocumentCommand : IRequest<byte[]>
{
    public HttpRequestContext RequestContext { get; set; } = new();
}
