using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NextWeb.DocumentPlatform.Application;
using NextWeb.DocumentPlatform.Application.Commands;

namespace NextWeb.DocumentPlatform.Api.Controllers;

[ApiController]
[Route("render")]
public class RenderController : ControllerBase
{
    private readonly IMediator _mediator;

    public RenderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{model}/{id}")]
    public async Task<IActionResult> RenderDocument([FromRoute] string model, [FromRoute] string id)
    {
        var context = new HttpRequestContext
        {
            RouteDocumentType = model,
            RouteDocumentId = id,
            QueryString = Request.QueryString.Value ?? string.Empty
        };

        foreach (var header in Request.Headers)
        {
            context.Headers[header.Key] = header.Value.ToString();
        }

        var command = new RenderDocumentCommand { RequestContext = context };

        try
        {
            var pdfBytes = await _mediator.Send(command);
            return File(pdfBytes, "application/pdf", $"{model}-{id}.pdf");
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}
