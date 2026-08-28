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
        var settings = model.Settings;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                // Use Landscape A4 for wider tables
                page.Size(PageSizes.A4.Landscape());
                page.Margin(PdfPrimitives.PageMarginCm, Unit.Centimetre);
                page.PageColor(S.SurfacePage);
                page.DefaultTextStyle(x => x.FontSize(PdfPrimitives.FontSizeBody).FontFamily(S.FontFamilyBody).FontColor(S.TextPrimary));

                page.Header().Element(c => ComposePageHeader(c, biz, settings));
                page.Footer().Element(c => ComposePageFooter(c, biz));

                page.Content().PaddingVertical(PdfPrimitives.SectionGap).Column(col =>
                {
                    col.Item().Element(c => ComposeTable(c, doc, biz));
                });
            });
        });

        return Task.FromResult(document.GeneratePdf());
    }

    private static string FormatAddressLines(AddressDto? address) => PdfFormat.AddressLines(address);

    private void ComposePageHeader(IContainer container, BusinessDto? biz, StockAnalysisSettingsDto? settings)
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

                row.ConstantItem(250).AlignRight().Column(right => 
                {
                    right.Item().AlignRight().Text("STOCK ANALYSIS REPORT")
                        .FontFamily(S.FontFamilyDisplay)
                        .FontSize(14)
                        .SemiBold()
                        .FontColor(S.TextAccent);
                        
                    if (!string.IsNullOrWhiteSpace(settings?.LocationName))
                    {
                        right.Item().AlignRight().PaddingTop(4).Text($"Location: {settings.LocationName}")
                            .FontSize(S.FontCaption).FontColor(S.TextMuted).SemiBold();
                    }
                    if (!string.IsNullOrWhiteSpace(settings?.CategoryName))
                    {
                        right.Item().AlignRight().PaddingTop(1).Text($"Category: {settings.CategoryName}")
                            .FontSize(S.FontCaption).FontColor(S.TextMuted).SemiBold();
                    }
                        
                    right.Item().AlignRight().PaddingTop(4).Text($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm}")
                        .FontSize(S.FontCaption)
                        .FontColor(S.TextMuted);
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

    private void ComposeTable(IContainer container, StockAnalysisDocumentDto doc, BusinessDto? biz)
    {
        var currency = biz?.BaseCurrency ?? "";

        container.Table(table =>
        {
            table.ColumnsDefinition(cols =>
            {
                cols.RelativeColumn(3); // Name
                cols.RelativeColumn();  // Per Unit Price
                cols.RelativeColumn();  // Min. Unit Price
                cols.RelativeColumn();  // Max. Unit Price
                cols.RelativeColumn();  // Avg Conv Fac
                cols.RelativeColumn();  // Total Stock
                cols.RelativeColumn();  // Total Stock Value (now Total Price)
                cols.RelativeColumn();  // Location Stock
                cols.RelativeColumn();  // Per Act. Unit Price
                cols.RelativeColumn();  // Location Actual Stock
            });

            table.Header(header =>
            {
                void HeaderCell(string text, bool right = false)
                {
                    var cell = header.Cell().Background(S.SurfaceCard).Padding(4);
                    if (right) cell = cell.AlignRight();
                    cell.Text(text).FontSize(7).SemiBold().FontColor(S.TextMuted);
                }

                HeaderCell("PRODUCT NAME");
                HeaderCell("PER UNIT PRICE", true);
                HeaderCell("MIN. UNIT PRICE", true);
                HeaderCell("MAX. UNIT PRICE", true);
                HeaderCell("AVG CONV. FAC", true);
                HeaderCell("TOTAL STOCK", true);
                HeaderCell("TOTAL STOCK VALUE", true);
                HeaderCell("LOCATION STOCK", true);
                HeaderCell("PER ACT. UNIT PRICE", true);
                HeaderCell("LOCATION ACT. STOCK", true);
            });

            if (doc.Items == null || doc.Items.Count == 0)
            {
                table.Cell().ColumnSpan(10).Padding(8)
                    .AlignCenter().Text("No products found for the selected filters.")
                    .FontSize(9).FontColor(S.TextMuted);
                return;
            }

            var groupedItems = doc.Items
                .SelectMany(i => i.Categories != null && i.Categories.Any()
                    ? i.Categories.Select(c => new { CategoryName = !string.IsNullOrWhiteSpace(c.Name) ? c.Name : "Uncategorized", Item = i })
                    : new[] { new { CategoryName = "Uncategorized", Item = i } })
                .GroupBy(x => x.CategoryName)
                .OrderBy(g => g.Key);

            foreach (var group in groupedItems)
            {
                // Category Header
                table.Cell().ColumnSpan(10).Background(Colors.Grey.Lighten4).PaddingVertical(4).PaddingHorizontal(4)
                    .Text(group.Key.ToUpperInvariant())
                    .FontSize(8).SemiBold().FontColor(S.TextAccent);

                foreach (var gItem in group)
                {
                    var item = gItem.Item;
                    
                    void TextCell(string text, bool right = false)
                    {
                        var cell = table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(4);
                        if (right) cell = cell.AlignRight();
                        cell.Text(text).FontSize(7);
                    }

                    string FormatMoney(decimal? val) => val.HasValue ? $"{currency} {val.Value.ToString("N2", CultureInfo.InvariantCulture)}" : "-";
                    string FormatNumber(decimal? val) => val.HasValue ? val.Value.ToString("N2", CultureInfo.InvariantCulture) : "-";

                    var productName = ProductDisplayHelper.FormatLineItemName(null, item);
                    var unitName = item.Unit?.Name ?? "";
                    bool hasTransUnit = item.TransactionUnit != null;
                    var transUnitName = hasTransUnit ? item.TransactionUnit?.Name ?? unitName : "";

                    TextCell(productName);
                    TextCell(FormatMoney(item.PerUnitPrice), true);
                    TextCell(FormatMoney(item.MinUnitPrice), true);
                    TextCell(FormatMoney(item.MaxUnitPrice), true);
                    
                    TextCell(hasTransUnit ? FormatNumber(item.AverageConversionFactor) : "N/A", true);
                    TextCell($"{FormatNumber(item.Stock)} {unitName}", true);
                    
                    // As requested: "Remove Total Price and Put Total Stock Value - ${business.currency} ${product.total_price}"
                    TextCell(FormatMoney(item.TotalPrice), true);
                    
                    TextCell($"{FormatNumber(item.CurrentLocationStock)} {unitName}", true);
                    TextCell(hasTransUnit ? FormatMoney(item.PerActualUnitPrice) : "N/A", true);
                    TextCell(hasTransUnit ? $"{FormatNumber(item.CurrentLocationActualStock)} {transUnitName}" : "N/A", true);
                }
            }
        });
    }
}

