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
using NextWeb.DocumentPlatform.Renderers.Design;

namespace NextWeb.DocumentPlatform.Renderers.Templates;

public class MaterialEntryStandardRenderer : IDocumentRenderer
{
    private static readonly PdfSemantics S = new(PdfThemes.MaterialEntry);

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
                PdfPageSetup.ConfigureA4(page, S);

                page.Content().Column(col =>
                {
                    col.Item().Element(c => ComposeHeader(c, doc, biz, locationName));
                    col.Item().PaddingTop(PdfPrimitives.SectionGap).Element(c => ComposeProducts(c, doc));
                    col.Item().PaddingTop(PdfPrimitives.SectionGap).Element(c => ComposeFooter(c, doc, biz, settings));
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

    private static string FormatAddressLines(AddressDto? address) => PdfFormat.AddressLines(address);

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

    private static string GetProductName(MaterialEntryProductDto item) =>
        ProductDisplayHelper.FormatLineItemName(
            !string.IsNullOrWhiteSpace(item.Name) ? item.Name : null,
            item.Product);

    private static string GetHsn(MaterialEntryProductDto item) =>
        item.Product?.Hsn ?? string.Empty;

    private static string GetUnitLabel(UnitDto? unit)
    {
        if (unit == null) return string.Empty;
        if (!string.IsNullOrWhiteSpace(unit.Value)) return unit.Value;
        return unit.Name;
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
                    left.Item().Text(biz.Name).FontFamily(S.FontFamilyDisplay).FontSize(S.FontDisplayMd).SemiBold().FontColor(S.TextAccent);
                    if (!string.IsNullOrWhiteSpace(biz.Description))
                        left.Item().PaddingTop(2).Text(biz.Description).FontSize(S.FontCaption).FontColor(S.TextMuted);

                    if (biz.Address != null)
                    {
                        var addrText = FormatAddressLines(biz.Address);
                        if (!string.IsNullOrWhiteSpace(addrText))
                            left.Item().PaddingTop(6).Text(addrText).FontSize(S.FontCaption).LineHeight(1.3f);
                    }

                    if (!string.IsNullOrWhiteSpace(biz.Gst))
                        left.Item().PaddingTop(4).Text($"GSTIN: {biz.Gst}").FontSize(S.FontCaption);

                    var contactParts = new List<string>();
                    if (!string.IsNullOrWhiteSpace(biz.Phone)) contactParts.Add(biz.Phone);
                    if (!string.IsNullOrWhiteSpace(biz.Email)) contactParts.Add(biz.Email);
                    if (contactParts.Count > 0)
                        left.Item().PaddingTop(2).Text(string.Join(" · ", contactParts)).FontSize(S.FontCaption).FontColor(S.TextMuted);
                });

                row.ConstantItem(200).AlignRight().Column(right =>
                {
                    right.Item().Text(doc.Type).FontFamily(S.FontFamilyDisplay).FontSize(S.FontDisplayLg).SemiBold().FontColor(S.TextAccent);
                    right.Item().PaddingTop(4).Text($"#{doc.Number}").FontSize(11).SemiBold();

                    if (!string.IsNullOrWhiteSpace(doc.Status))
                    {
                        right.Item().PaddingTop(8).AlignRight()
                            .Element(c => PdfComponents.StatusChip(c, S, doc.Status));
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
                            meta.Cell().PaddingVertical(2).Text(label).FontSize(S.FontCaption).FontColor(S.TextMuted);
                            meta.Cell().PaddingVertical(2).AlignRight().Text(value).FontSize(S.FontCaption).SemiBold();
                        }

                        MetaRow("Date", doc.Date.ToString("dd MMM yyyy"));
                        if (!string.IsNullOrWhiteSpace(locationName))
                            MetaRow("Location", locationName);
                        if (!string.IsNullOrWhiteSpace(doc.ReferenceNumber))
                            MetaRow("Reference", doc.ReferenceNumber);
                    });
                });
            });

            PdfComponents.HorizontalDivider(col.Item(), S);
        });
    }

    private void ComposeProducts(IContainer container, MaterialEntryDto doc)
    {
        container.Column(col =>
        {
            PdfComponents.SectionTitle(col.Item(), S, "Items");

            col.Item().PaddingTop(6).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(24);
                    columns.RelativeColumn(4);
                    columns.ConstantColumn(52);
                    columns.ConstantColumn(48);
                    columns.ConstantColumn(44);
                });

                table.Header(header =>
                {
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(S.FontCaption).SemiBold()).Text("#");
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(S.FontCaption).SemiBold()).Text("Product");
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(S.FontCaption).SemiBold()).Text("HSN");
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(S.FontCaption).SemiBold()).AlignRight().Text("Qty");
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(S.FontCaption).SemiBold()).AlignRight().Text("Unit");
                });

                decimal totalQty = 0;
                int index = 0;

                foreach (var item in doc.Products)
                {
                    index++;
                    totalQty += item.Quantity;
                    bool shaded = index % 2 == 0;
                    var productName = GetProductName(item);

                    PdfTable.RowSingleLine(table.Cell(), S, shaded, index.ToString());
                    PdfTable.RowDescription(table.Cell(), S, shaded, productName, semiBold: true);
                    PdfTable.RowSingleLine(table.Cell(), S, shaded, GetHsn(item));
                    PdfTable.RowSingleLine(table.Cell(), S, shaded, $"{item.Quantity:0.##}", alignRight: true);
                    PdfTable.RowSingleLine(table.Cell(), S, shaded, GetUnitLabel(item.Unit), alignRight: true);
                }

                if (doc.Products.Count > 0)
                {
                    PdfTable.FooterText(table.Cell().ColumnSpan(3), S, "Total Quantity", alignRight: true, semiBold: true);
                    PdfTable.FooterText(table.Cell(), S, $"{totalQty:0.##}", alignRight: true, semiBold: true);
                    PdfTable.FooterCell(table.Cell(), S);
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
                    c.Item().Text("Notes").FontSize(S.FontCaption).SemiBold().FontColor(S.TextMuted);
                    c.Item().PaddingTop(2).Text(notes).FontSize(S.FontCaption);
                });
            }

            if (!string.IsNullOrWhiteSpace(terms))
            {
                col.Item().PaddingTop(10).Column(c =>
                {
                    c.Item().Text("Terms & Conditions").FontSize(S.FontCaption).SemiBold().FontColor(S.TextMuted);
                    c.Item().PaddingTop(2).Text(terms).FontSize(S.FontCaption);
                });
            }

            col.Item().PaddingTop(20).Row(row =>
            {
                row.RelativeItem();
                row.ConstantItem(180).AlignRight().Column(c =>
                {
                    c.Item().Text($"For {biz.Name}").FontSize(S.FontCaption).SemiBold();

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
                    c.Item().PaddingTop(4).Text(signatoryLabel).FontSize(S.FontCaption).SemiBold().FontColor(S.TextMuted);
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
