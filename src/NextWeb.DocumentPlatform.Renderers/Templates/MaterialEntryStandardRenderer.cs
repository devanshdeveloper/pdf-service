using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using NextWeb.DocumentPlatform.Engine;
using NextWeb.DocumentPlatform.Domain;
using NextWeb.DocumentPlatform.Application.Models;

namespace NextWeb.DocumentPlatform.Renderers.Templates;

public class MaterialEntryStandardRenderer : IDocumentRenderer
{
    private const string AccentColor = "#4338CA";
    private const string AccentLight = "#EEF2FF";
    private const string SurfaceMuted = "#F3F4F6";
    private const string TextMuted = "#6B7280";
    private const string BorderLight = "#E5E7EB";

    public string DocumentType => "MaterialEntry";
    public string TemplateName => "Standard";

    public Task<byte[]> RenderAsync(string jsonPayload, DocumentConfiguration config, CancellationToken cancellationToken)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var response = JsonSerializer.Deserialize<ApiResponse<MaterialEntryDataModel>>(jsonPayload, options);
        var model = response?.Data;

        if (model?.Document == null || model.Business == null)
        {
            throw new Exception("Invalid JSON payload or missing document/business data for Material Entry.");
        }

        var doc = model.Document;
        var biz = model.Business;
        var settings = model.Settings;
        var locationName = ResolveLocationName(doc, model.Location);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1.2f, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(9).FontFamily("Arial").FontColor(Colors.Black));

                page.Content().Column(col =>
                {
                    col.Item().Element(c => ComposeHeader(c, doc, biz, locationName));
                    col.Item().PaddingTop(16).Element(c => ComposeProducts(c, doc));
                    col.Item().PaddingTop(16).Element(c => ComposeFooter(c, doc, biz, settings));
                });
            });
        });

        return Task.FromResult(document.GeneratePdf());
    }

    private static string ResolveLocationName(MaterialEntryDto doc, LocationDto? topLevelLocation)
    {
        if (!string.IsNullOrWhiteSpace(doc.Location?.Name))
            return doc.Location.Name;
        if (!string.IsNullOrWhiteSpace(topLevelLocation?.Name))
            return topLevelLocation.Name;
        return string.Empty;
    }

    private static string FormatAddressLines(AddressDto? address)
    {
        if (address == null) return string.Empty;
        if (!string.IsNullOrWhiteSpace(address.FullAddress)) return address.FullAddress;

        var lines = new List<string>();
        var street = $"{address.StreetAddress} {address.Apartment}".Trim();
        if (!string.IsNullOrWhiteSpace(street)) lines.Add(street);

        var cityLine = $"{address.City}, {address.State} {address.PostalCode}".Trim(',', ' ');
        if (!string.IsNullOrWhiteSpace(cityLine)) lines.Add(cityLine);
        if (!string.IsNullOrWhiteSpace(address.Country)) lines.Add(address.Country);

        return string.Join("\n", lines);
    }

    private static string GetNotes(MaterialEntryDto doc, MaterialEntrySettingsDto? settings)
    {
        if (!string.IsNullOrWhiteSpace(doc.Notes)) return doc.Notes!;
        return settings?.Defaults?.Notes ?? string.Empty;
    }

    private static string GetTerms(MaterialEntryDto doc, MaterialEntrySettingsDto? settings)
    {
        if (!string.IsNullOrWhiteSpace(doc.Terms)) return doc.Terms!;
        return settings?.Defaults?.Terms ?? string.Empty;
    }

    private static string GetProductName(MaterialEntryProductDto item)
    {
        if (!string.IsNullOrWhiteSpace(item.Name)) return item.Name;
        return item.Product?.Name ?? string.Empty;
    }

    private static string GetHsn(MaterialEntryProductDto item)
    {
        return item.Product?.Hsn ?? string.Empty;
    }

    private static string GetUnitLabel(UnitDto? unit)
    {
        if (unit == null) return string.Empty;
        if (!string.IsNullOrWhiteSpace(unit.Value)) return unit.Value;
        return unit.Name;
    }

    private static string GetProductDetails(MaterialEntryProductDto item)
    {
        var fields = item.Product?.Fields?
            .Where(f => !string.IsNullOrWhiteSpace(f.Value))
            .Select(f => $"{f.Label}: {f.Value}")
            .ToList() ?? new List<string>();

        return string.Join(" · ", fields);
    }

    private void ComposeHeader(
        IContainer container,
        MaterialEntryDto doc,
        BusinessDto biz,
        string locationName)
    {
        container.Column(col =>
        {
            col.Item().Row(row =>
            {
                row.RelativeItem().Column(left =>
                {
                    left.Item().Text(biz.Name).FontFamily("Montserrat").FontSize(16).SemiBold().FontColor(AccentColor);
                    if (!string.IsNullOrWhiteSpace(biz.Description))
                        left.Item().PaddingTop(2).Text(biz.Description).FontSize(8).FontColor(TextMuted);

                    if (biz.Address != null)
                    {
                        var addrText = FormatAddressLines(biz.Address);
                        if (!string.IsNullOrWhiteSpace(addrText))
                            left.Item().PaddingTop(6).Text(addrText).FontSize(8).LineHeight(1.3f);
                    }

                    if (!string.IsNullOrWhiteSpace(biz.Gst))
                        left.Item().PaddingTop(4).Text($"GSTIN: {biz.Gst}").FontSize(8);

                    var contactParts = new List<string>();
                    if (!string.IsNullOrWhiteSpace(biz.Phone)) contactParts.Add(biz.Phone);
                    if (!string.IsNullOrWhiteSpace(biz.Email)) contactParts.Add(biz.Email);
                    if (contactParts.Count > 0)
                        left.Item().PaddingTop(2).Text(string.Join(" · ", contactParts)).FontSize(8).FontColor(TextMuted);
                });

                row.ConstantItem(200).AlignRight().Column(right =>
                {
                    right.Item().Text(doc.Type).FontFamily("Montserrat").FontSize(20).SemiBold().FontColor(AccentColor);
                    right.Item().PaddingTop(4).Text($"#{doc.Number}").FontSize(11).SemiBold();

                    if (!string.IsNullOrWhiteSpace(doc.Status))
                    {
                        right.Item().PaddingTop(8).AlignRight()
                            .Element(c => ComposeStatusChip(c, doc.Status));
                    }

                    right.Item().PaddingTop(10).Table(meta =>
                    {
                        meta.ColumnsDefinition(cols =>
                        {
                            cols.ConstantColumn(70);
                            cols.RelativeColumn();
                        });

                        void MetaRow(string label, string value)
                        {
                            meta.Cell().PaddingVertical(2).Text(label).FontSize(8).FontColor(TextMuted);
                            meta.Cell().PaddingVertical(2).AlignRight().Text(value).FontSize(8).SemiBold();
                        }

                        MetaRow("Date", doc.Date.ToString("dd MMM yyyy"));
                        if (!string.IsNullOrWhiteSpace(locationName))
                            MetaRow("Location", locationName);
                        if (!string.IsNullOrWhiteSpace(doc.ReferenceNumber))
                            MetaRow("Reference", doc.ReferenceNumber);
                    });
                });
            });

            col.Item().PaddingTop(12).LineHorizontal(1).LineColor(BorderLight);
        });
    }

    private void ComposeStatusChip(IContainer container, string label)
    {
        var (bg, fg) = GetStatusColors(label);
        container
            .Background(bg)
            .PaddingVertical(3)
            .PaddingHorizontal(8)
            .Text(label)
            .FontSize(8)
            .SemiBold()
            .FontColor(fg);
    }

    private static (string bg, string fg) GetStatusColors(string status)
    {
        var normalized = status.Trim().ToLowerInvariant();
        return normalized switch
        {
            "approved" => (AccentLight, AccentColor),
            "draft" or "pending" => ("#FEF3C7", "#92400E"),
            "rejected" or "cancelled" => ("#FEE2E2", "#991B1B"),
            _ => (SurfaceMuted, TextMuted)
        };
    }

    private void ComposeProducts(IContainer container, MaterialEntryDto doc)
    {
        container.Column(col =>
        {
            col.Item().Text("Items").FontSize(9).SemiBold().FontColor(AccentColor);

            col.Item().PaddingTop(6).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(24);
                    columns.RelativeColumn(3);
                    columns.ConstantColumn(52);
                    columns.RelativeColumn(2);
                    columns.ConstantColumn(48);
                    columns.ConstantColumn(44);
                });

                table.Header(header =>
                {
                    header.Cell().Element(HeaderCell).Text("#");
                    header.Cell().Element(HeaderCell).Text("Product");
                    header.Cell().Element(HeaderCell).Text("HSN");
                    header.Cell().Element(HeaderCell).Text("Details");
                    header.Cell().Element(HeaderCell).AlignRight().Text("Qty");
                    header.Cell().Element(HeaderCell).AlignRight().Text("Unit");

                    static IContainer HeaderCell(IContainer c) =>
                        c.Background(SurfaceMuted)
                            .PaddingVertical(6)
                            .PaddingHorizontal(6)
                            .DefaultTextStyle(x => x.FontSize(8).SemiBold());
                });

                decimal totalQty = 0;
                int index = 0;

                foreach (var item in doc.Products)
                {
                    index++;
                    totalQty += item.Quantity;
                    bool shaded = index % 2 == 0;
                    var productName = GetProductName(item);
                    var details = GetProductDetails(item);

                    table.Cell().Element(c => RowCell(c, shaded)).Text(index.ToString());
                    table.Cell().Element(c => RowCell(c, shaded)).Column(cell =>
                    {
                        cell.Item().Text(productName).SemiBold();
                    });
                    table.Cell().Element(c => RowCell(c, shaded)).Text(GetHsn(item));
                    table.Cell().Element(c => RowCell(c, shaded)).Text(
                        string.IsNullOrWhiteSpace(details) ? "—" : details).FontColor(TextMuted);
                    table.Cell().Element(c => RowCell(c, shaded)).AlignRight()
                        .Text($"{item.Quantity:0.##}");
                    table.Cell().Element(c => RowCell(c, shaded)).AlignRight()
                        .Text(GetUnitLabel(item.Unit));
                }

                static IContainer RowCell(IContainer c, bool shaded) =>
                    shaded
                        ? c.Background(SurfaceMuted).PaddingVertical(5).PaddingHorizontal(6)
                        : c.PaddingVertical(5).PaddingHorizontal(6);

                if (doc.Products.Count > 0)
                {
                    table.Cell().ColumnSpan(4).Element(FooterCell).AlignRight().Text("Total Quantity").SemiBold();
                    table.Cell().Element(FooterCell).AlignRight().Text($"{totalQty:0.##}").SemiBold();
                    table.Cell().Element(FooterCell);

                    static IContainer FooterCell(IContainer c) =>
                        c.BorderTop(1).BorderColor(BorderLight).PaddingVertical(6).PaddingHorizontal(6);
                }
            });
        });
    }

    private void ComposeFooter(
        IContainer container,
        MaterialEntryDto doc,
        BusinessDto biz,
        MaterialEntrySettingsDto? settings)
    {
        var notes = GetNotes(doc, settings);
        var terms = GetTerms(doc, settings);

        container.Column(col =>
        {
            if (!string.IsNullOrWhiteSpace(notes))
            {
                col.Item().Column(c =>
                {
                    c.Item().Text("Notes").FontSize(8).SemiBold().FontColor(TextMuted);
                    c.Item().PaddingTop(2).Text(notes).FontSize(8);
                });
            }

            if (!string.IsNullOrWhiteSpace(terms))
            {
                col.Item().PaddingTop(10).Column(c =>
                {
                    c.Item().Text("Terms & Conditions").FontSize(8).SemiBold().FontColor(TextMuted);
                    c.Item().PaddingTop(2).Text(terms).FontSize(8);
                });
            }

            col.Item().PaddingTop(20).Row(row =>
            {
                row.RelativeItem();
                row.ConstantItem(180).AlignRight().Column(c =>
                {
                    c.Item().Text($"For {biz.Name}").FontSize(8).SemiBold();

                    var signature = doc.Signature;
                    if (signature != null && !string.IsNullOrWhiteSpace(signature.Url))
                    {
                        var imageBytes = TryLoadImage(signature.Url);
                        if (imageBytes != null)
                        {
                            c.Item().PaddingTop(4).Height(48).Image(imageBytes).FitArea();
                        }
                        else
                        {
                            c.Item().PaddingTop(24);
                        }
                    }
                    else
                    {
                        c.Item().PaddingTop(24);
                    }

                    var signatoryLabel = !string.IsNullOrWhiteSpace(signature?.Name)
                        ? signature.Name
                        : "Authorised Signatory";
                    c.Item().PaddingTop(4).Text(signatoryLabel).FontSize(8).SemiBold().FontColor(TextMuted);
                });
            });
        });
    }

    private static byte[]? TryLoadImage(string url)
    {
        try
        {
            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
            return client.GetByteArrayAsync(url).GetAwaiter().GetResult();
        }
        catch
        {
            return null;
        }
    }
}
