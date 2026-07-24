using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using NextWeb.DocumentPlatform.Engine;
using NextWeb.DocumentPlatform.Domain;
using NextWeb.DocumentPlatform.Application.Models;
using NextWeb.DocumentPlatform.Renderers.Components;

namespace NextWeb.DocumentPlatform.Renderers.Templates;

public class VoucherStandardRenderer : IDocumentRenderer
{
    public string DocumentType => "Voucher";
    public string TemplateName => "Standard";

    public Task<byte[]> RenderAsync(string jsonPayload, DocumentConfiguration config, CancellationToken cancellationToken)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var response = JsonSerializer.Deserialize<ApiResponse<VoucherDataModel>>(jsonPayload, options);
        var model = response?.Data;

        if (model?.Document == null)
        {
            throw new System.Exception("Invalid JSON payload or missing document data for Voucher.");
        }

        var documentData = model.Document;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1.5f, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header().Element(c => c.Component(new StandardHeaderComponent(documentData)));
                
                page.Content().PaddingVertical(1, Unit.Centimetre).Column(col =>
                {
                    col.Item().Component(new DynamicTableComponent(documentData));
                    col.Item().PaddingTop(20).Component(new FinancialSummaryComponent(documentData));
                });
                
                page.Footer().Element(c => c.Component(new StandardFooterComponent(documentData)));
            });
        });

        return Task.FromResult(document.GeneratePdf());
    }
}
