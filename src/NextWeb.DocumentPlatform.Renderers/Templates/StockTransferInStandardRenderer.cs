using System.Threading;
using System.Threading.Tasks;
using NextWeb.DocumentPlatform.Engine;
using NextWeb.DocumentPlatform.Domain;

using NextWeb.DocumentPlatform.Renderers.Design;

namespace NextWeb.DocumentPlatform.Renderers.Templates;

public class StockTransferInStandardRenderer : IDocumentRenderer
{
    public string DocumentType => "StockTransferIn";
    public string TemplateName => "Standard";

    public Task<byte[]> RenderAsync(string jsonPayload, DocumentConfiguration config, CancellationToken cancellationToken) =>
        StockTransferPdfComposer.RenderAsync(jsonPayload, "Stock Transfer In", PdfThemes.StockTransferIn, cancellationToken);
}
