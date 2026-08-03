using System.Threading;
using System.Threading.Tasks;
using NextWeb.DocumentPlatform.Engine;
using NextWeb.DocumentPlatform.Domain;

namespace NextWeb.DocumentPlatform.Renderers.Templates;

public class StockTransferInStandardRenderer : IDocumentRenderer
{
    private const string AccentColor = "#059669";
    private const string AccentLight = "#D1FAE5";

    public string DocumentType => "StockTransferIn";
    public string TemplateName => "Standard";

    public Task<byte[]> RenderAsync(string jsonPayload, DocumentConfiguration config, CancellationToken cancellationToken) =>
        StockTransferPdfComposer.RenderAsync(jsonPayload, "Stock Transfer In", AccentColor, AccentLight, cancellationToken);
}
