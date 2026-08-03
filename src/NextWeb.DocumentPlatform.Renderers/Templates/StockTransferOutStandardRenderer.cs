using System.Threading;
using System.Threading.Tasks;
using NextWeb.DocumentPlatform.Engine;
using NextWeb.DocumentPlatform.Domain;

namespace NextWeb.DocumentPlatform.Renderers.Templates;

public class StockTransferOutStandardRenderer : IDocumentRenderer
{
    private const string AccentColor = "#7C3AED";
    private const string AccentLight = "#EDE9FE";

    public string DocumentType => "StockTransferOut";
    public string TemplateName => "Standard";

    public Task<byte[]> RenderAsync(string jsonPayload, DocumentConfiguration config, CancellationToken cancellationToken) =>
        StockTransferPdfComposer.RenderAsync(jsonPayload, "Stock Transfer Out", AccentColor, AccentLight, cancellationToken);
}
