using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NextWeb.DocumentPlatform.Application;
using NextWeb.DocumentPlatform.Application.Commands;

namespace NextWeb.DocumentPlatform.Api.Controllers;

[ApiController]
[Route("api/v1/render")]
public class RenderController : ControllerBase
{
    private readonly IMediator _mediator;

    public RenderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("voucher/{type}/{template}")]
    public async Task<IActionResult> RenderVoucher([FromRoute] string type, [FromRoute] string template)
    {
        var context = new HttpRequestContext
        {
            RouteDocumentType = $"voucher/{type}",
            RouteTemplateName = template,
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
            return File(pdfBytes, "application/pdf", $"{type}-{template}.pdf");
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}
