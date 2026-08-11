using System;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using NextWeb.DocumentPlatform.Application.Models;
using NextWeb.DocumentPlatform.Domain;
using NextWeb.DocumentPlatform.Engine;
using NextWeb.DocumentPlatform.Renderers.Design;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace NextWeb.DocumentPlatform.Renderers.Templates;

public class StockAnalysisStandardRenderer : IDocumentRenderer
{
    private static readonly PdfSemantics S = new(PdfThemes.Reports);

    public string DocumentType => "StockAnalysis";
    public string TemplateName => "Standard";

    public Task<byte[]> RenderAsync(string jsonPayload, DocumentConfiguration config, CancellationToken cancellationToken)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var response = JsonSerializer.Deserialize<ApiResponse<StockAnalysisDataModel>>(jsonPayload, options);
        var model = response?.Data;

        if (model?.Document == null)
        {
            throw new Exception("Invalid JSON payload or missing document data.");
        }

        var doc = model.Document;
        var biz = model.Business;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                // Use Landscape A4 for wider tables
                page.Size(PageSizes.A4.Landscape());
                page.Margin(PdfPrimitives.PageMarginCm, Unit.Centimetre);
                page.PageColor(S.SurfacePage);
                page.DefaultTextStyle(x => x.FontSize(PdfPrimitives.FontSizeBody).FontFamily(S.FontFamilyBody).FontColor(S.TextPrimary));

                page.Header().Element(c => ComposePageHeader(c, biz));
                page.Footer().Element(c => ComposePageFooter(c, biz));

                page.Content().PaddingVertical(PdfPrimitives.SectionGap).Column(col =>
                {
                    col.Item().Element(ComposeTitleBlock);
                    col.Item().PaddingTop(PdfPrimitives.SectionGap).Element(c => ComposeTable(c, doc));
                });
            });
        });

        return Task.FromResult(document.GeneratePdf());
    }

    private static string FormatAddressLines(AddressDto? address) => PdfFormat.AddressLines(address);

    private void ComposePageHeader(IContainer container, BusinessDto? biz)
    {
        container.Column(col =>
        {
            col.Item().Row(row =>
            {
                row.RelativeItem().Column(left =>
                {
                    left.Item().Text(biz?.Name ?? "Stock Analysis")
                        .FontFamily(S.FontFamilyDisplay)
                        .FontSize(S.FontDisplayMd)
                        .SemiBold()
                        .FontColor(S.TextAccent);

                    if (biz?.Address != null)
                    {
                        var addr = FormatAddressLines(biz.Address);
                        if (!string.IsNullOrWhiteSpace(addr))
                            left.Item().PaddingTop(4).Text(addr).FontSize(S.FontCaption).LineHeight(1.3f);
                    }
                });
            });

            PdfComponents.HorizontalDivider(col.Item().PaddingTop(6), S);
        });
    }

    private void ComposePageFooter(IContainer container, BusinessDto? biz)
    {
        container.Column(col =>
        {
            PdfComponents.HorizontalDivider(col.Item(), S);
            col.Item().PaddingTop(4).Row(row =>
            {
                var contact = string.Join(" · ", new[] { biz?.Phone, biz?.Email }.Where(x => !string.IsNullOrWhiteSpace(x)));
                row.RelativeItem().Text(contact).FontSize(7).FontColor(S.TextMuted);
                row.ConstantItem(80).AlignRight().Text(text =>
                {
                    text.Span("Page ").FontSize(7).FontColor(S.TextMuted);
                    text.CurrentPageNumber().FontSize(7).FontColor(S.TextMuted);
                    text.Span(" / ").FontSize(7).FontColor(S.TextMuted);
                    text.TotalPages().FontSize(7).FontColor(S.TextMuted);
                });
            });
        });
    }

    private void ComposeTitleBlock(IContainer container)
    {
        container.Column(col =>
        {
            col.Item().Text("Stock Analysis Report")
                .FontFamily(S.FontFamilyDisplay)
                .FontSize(S.FontDisplayLg)
                .SemiBold()
                .FontColor(S.TextAccent);
            
            col.Item().PaddingTop(4).Text($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm}")
                .FontSize(S.FontCaption)
                .FontColor(S.TextMuted);
        });
    }

    private string ExtractUnitName(object? unitObj)
    {
        if (unitObj is JsonElement el && el.ValueKind == JsonValueKind.Object)
        {
            if (el.TryGetProperty("name", out var nameProp))
            {
                return nameProp.GetString() ?? string.Empty;
            }
        }
        return string.Empty;
    }

    private void ComposeTable(IContainer container, StockAnalysisDocumentDto doc)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(cols =>
            {
                cols.RelativeColumn(2); // Name
                cols.RelativeColumn();  // Type
                cols.RelativeColumn();  // Unit
                cols.RelativeColumn();  // Actual Stock
                cols.RelativeColumn();  // Stock
                cols.RelativeColumn();  // Avg Unit Price
                cols.RelativeColumn();  // Total Value
                cols.RelativeColumn();  // Avg Conv
                cols.RelativeColumn();  // Avg Actual Price
            });

            table.Header(header =>
            {
                void HeaderCell(string text, bool right = false)
                {
                    var cell = header.Cell().Background(S.SurfaceCard).Padding(4);
                    if (right) cell.AlignRight();
                    cell.Text(text).FontSize(8).SemiBold().FontColor(S.TextMuted);
                }

                HeaderCell("PRODUCT NAME");
                HeaderCell("TYPE");
                HeaderCell("UNIT");
                HeaderCell("ACTUAL STOCK", true);
                HeaderCell("STOCK", true);
                HeaderCell("AVG UNIT PRICE", true);
                HeaderCell("TOTAL VALUE", true);
                HeaderCell("AVG CONV", true);
                HeaderCell("AVG ACTUAL PRICE", true);
            });

            if (doc.Items == null || doc.Items.Count == 0)
            {
                table.Cell().ColumnSpan(9).Padding(8)
                    .AlignCenter().Text("No products found for the selected filters.")
                    .FontSize(9).FontColor(S.TextMuted);
                return;
            }

            foreach (var item in doc.Items)
            {
                void TextCell(string text, bool right = false)
                {
                    var cell = table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(4);
                    if (right) cell.AlignRight();
                    cell.Text(text).FontSize(8);
                }

                void DecimalCell(decimal? val)
                {
                    TextCell(val?.ToString("N2", CultureInfo.InvariantCulture) ?? "0.00", true);
                }

                TextCell(item.Name ?? string.Empty);
                TextCell(item.ProductType ?? string.Empty);
                TextCell(ExtractUnitName(item.Unit));
                
                DecimalCell(item.CurrentLocationActualStock);
                DecimalCell(item.CurrentLocationStock);
                DecimalCell(item.PerUnitPrice);
                DecimalCell(item.TotalPrice);
                DecimalCell(item.AverageConversionFactor);
                DecimalCell(item.PerActualUnitPrice);
            }
        });
    }
}
