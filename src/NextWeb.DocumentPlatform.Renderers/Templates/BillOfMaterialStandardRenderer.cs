using System;
using System.Collections.Generic;
using System.Globalization;
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

public class BillOfMaterialStandardRenderer : IDocumentRenderer
{
    private static readonly PdfSemantics S = new(PdfThemes.BillOfMaterial);

    public string DocumentType => "BillOfMaterial";
    public string TemplateName => "Standard";

    public Task<byte[]> RenderAsync(string jsonPayload, DocumentConfiguration config, CancellationToken cancellationToken)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var response = JsonSerializer.Deserialize<ApiResponse<BillOfMaterialDataModel>>(jsonPayload, options);
        var model = response?.Data;

        if (model?.Document == null || model.Business == null)
        {
            throw new Exception("Invalid JSON payload or missing document/business data for Bill of Material.");
        }

        var doc = model.Document;
        var biz = model.Business;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                PdfPageSetup.ConfigureA4(page, S);

                page.Content().Column(col =>
                {
                    col.Item().Element(c => ComposeHeader(c, doc, biz));
                    col.Item().PaddingTop(PdfPrimitives.SectionGap).Element(c => ComposeOutputDetails(c, doc));
                    if (doc.Fields.Count > 0)
                        col.Item().PaddingTop(PdfPrimitives.SectionGap).Element(c => ComposeCustomFields(c, doc));
                    col.Item().PaddingTop(PdfPrimitives.SectionGap).Element(c => ComposeComponents(c, doc));
                    if (doc.Operations.Count > 0)
                        col.Item().PaddingTop(PdfPrimitives.SectionGap).Element(c => ComposeOperations(c, doc));
                    col.Item().PaddingTop(PdfPrimitives.SectionGap).Element(c => ComposeFooter(c, doc, biz));
                });
            });
        });

        return Task.FromResult(document.GeneratePdf());
    }

    private static string FormatAddressLines(AddressDto? address) => PdfFormat.AddressLines(address);

    private static string FormatDate(DateTime? date) =>
        date.HasValue ? date.Value.ToString("dd MMM yyyy", CultureInfo.InvariantCulture) : string.Empty;

    private static string GetUnitLabel(UnitDto? unit)
    {
        if (unit == null) return string.Empty;
        if (!string.IsNullOrWhiteSpace(unit.Value)) return unit.Value;
        return unit.Name;
    }

    private static string FormatFieldValue(JsonElement value)
    {
        return value.ValueKind switch
        {
            JsonValueKind.String => value.GetString() ?? string.Empty,
            JsonValueKind.Number => value.ToString(),
            JsonValueKind.True => "Yes",
            JsonValueKind.False => "No",
            JsonValueKind.Null or JsonValueKind.Undefined => string.Empty,
            _ => value.ToString()
        };
    }

    private void ComposeHeader(IContainer container, BillOfMaterialDto doc, BusinessDto biz)
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

                row.ConstantItem(210).AlignRight().Column(right =>
                {
                    right.Item().Text("Bill of Material").FontFamily(S.FontFamilyDisplay).FontSize(S.FontDisplayLg).SemiBold().FontColor(S.TextAccent);
                    right.Item().PaddingTop(4).Text($"#{doc.Number}").FontSize(11).SemiBold();
                    if (!string.IsNullOrWhiteSpace(doc.Name))
                        right.Item().PaddingTop(2).Text(doc.Name).FontSize(S.FontCaption).FontColor(S.TextMuted);

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

                        if (!string.IsNullOrWhiteSpace(doc.Type))
                            MetaRow("Type", doc.Type);
                        if (doc.CreatedAt.HasValue)
                            MetaRow("Created", FormatDate(doc.CreatedAt));
                        if (doc.UpdatedAt.HasValue)
                            MetaRow("Updated", FormatDate(doc.UpdatedAt));
                    });
                });
            });

            PdfComponents.HorizontalDivider(col.Item(), S);
        });
    }

    private void ComposeOutputDetails(IContainer container, BillOfMaterialDto doc)
    {
        container.Column(col =>
        {
            PdfComponents.SectionTitle(col.Item(), S, "Output Details");

            col.Item().PaddingTop(8).Background(S.SurfaceCard).Padding(12).Column(details =>
            {
                details.Item().Row(row =>
                {
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().Text("To Produce").FontSize(S.FontCaption).FontColor(S.TextMuted);
                        c.Item().PaddingTop(2).Text(ProductDisplayHelper.FormatLineItemName(null, doc.ToProduce) ?? "—").FontSize(10).SemiBold();
                    });

                    row.RelativeItem().AlignRight().Column(c =>
                    {
                        c.Item().AlignRight().Text("Output Quantity").FontSize(S.FontCaption).FontColor(S.TextMuted);
                        c.Item().PaddingTop(2).AlignRight()
                            .Text($"{doc.Quantity:0.##} {GetUnitLabel(doc.Unit)}".Trim())
                            .FontSize(10).SemiBold().FontColor(S.TextAccent);
                    });
                });

                if (doc.TransactionQuantity.HasValue)
                {
                    details.Item().PaddingTop(8).Row(row =>
                    {
                        row.RelativeItem().Text("Actual Quantity").FontSize(S.FontCaption).FontColor(S.TextMuted);
                        row.RelativeItem().AlignRight()
                            .Text($"{doc.TransactionQuantity:0.##} {GetUnitLabel(doc.TransactionUnit)}".Trim())
                            .FontSize(S.FontCaption).SemiBold();
                    });
                }
            });

            if (!string.IsNullOrWhiteSpace(doc.Description))
            {
                col.Item().PaddingTop(10).Column(desc =>
                {
                    PdfComponents.SectionLabel(desc.Item(), S, "Description");
                    desc.Item().PaddingTop(2).Text(doc.Description).FontSize(S.FontCaption);
                });
            }
        });
    }

    private void ComposeCustomFields(IContainer container, BillOfMaterialDto doc)
    {
        container.Column(col =>
        {
            PdfComponents.SectionTitle(col.Item(), S, "Custom Fields");

            col.Item().PaddingTop(6).Table(table =>
            {
                table.ColumnsDefinition(cols =>
                {
                    cols.ConstantColumn(120);
                    cols.RelativeColumn();
                });

                foreach (var field in doc.Fields.Where(f => !string.IsNullOrWhiteSpace(f.Label)))
                {
                    var value = FormatFieldValue(field.Value);
                    if (string.IsNullOrWhiteSpace(value)) continue;

                    table.Cell().PaddingVertical(3).Text(field.Label).FontSize(S.FontCaption).FontColor(S.TextMuted);
                    table.Cell().PaddingVertical(3).Text(value).FontSize(S.FontCaption);
                }
            });
        });
    }

    private void ComposeComponents(IContainer container, BillOfMaterialDto doc)
    {
        container.Column(col =>
        {
            PdfComponents.SectionTitle(col.Item(), S, "Components");

            col.Item().PaddingTop(6).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(20);
                    columns.RelativeColumn(3);
                    columns.ConstantColumn(44);
                    columns.ConstantColumn(40);
                    columns.ConstantColumn(52);
                    columns.ConstantColumn(52);
                    columns.ConstantColumn(44);
                });

                table.Header(header =>
                {
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(7).SemiBold()).Text("#");
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(7).SemiBold()).Text("Component");
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(7).SemiBold()).AlignRight().Text("Qty");
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(7).SemiBold()).Text("Unit");
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(7).SemiBold()).AlignRight().Text("Actual");
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(7).SemiBold()).Text("Act. Unit");
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(7).SemiBold()).AlignRight().Text("Waste %");
                });

                int index = 0;
                foreach (var item in doc.Components)
                {
                    index++;
                    bool shaded = index % 2 == 0;
                    var productName = ProductDisplayHelper.FormatLineItemName(null, item.Product);

                    PdfTable.RowSingleLine(table.Cell(), S, shaded, index.ToString());
                    PdfTable.RowDescription(table.Cell(), S, shaded, productName, semiBold: true);
                    PdfTable.RowSingleLine(table.Cell(), S, shaded, $"{item.Quantity:0.##}", alignRight: true);
                    PdfTable.RowSingleLine(table.Cell(), S, shaded, GetUnitLabel(item.Unit));
                    PdfTable.RowSingleLine(
                        table.Cell(), S, shaded,
                        item.TransactionQuantity.HasValue ? $"{item.TransactionQuantity:0.##}" : null,
                        alignRight: true);
                    PdfTable.RowSingleLine(
                        table.Cell(), S, shaded,
                        item.TransactionUnit != null ? GetUnitLabel(item.TransactionUnit) : null);
                    PdfTable.RowSingleLine(table.Cell(), S, shaded, $"{item.WastagePercent:0.##}%", alignRight: true);
                }
            });
        });
    }

    private void ComposeOperations(IContainer container, BillOfMaterialDto doc)
    {
        container.Column(col =>
        {
            PdfComponents.SectionTitle(col.Item(), S, "Manufacturing Operations");

            col.Item().PaddingTop(6).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(24);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(3);
                });

                table.Header(header =>
                {
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(S.FontCaption).SemiBold()).Text("#");
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(S.FontCaption).SemiBold()).Text("Operation");
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(S.FontCaption).SemiBold()).Text("Blocked By");
                });

                int index = 0;
                foreach (var op in doc.Operations)
                {
                    index++;
                    bool shaded = index % 2 == 0;
                    var blockedBy = op.BlockedByOperations.Count > 0
                        ? string.Join(", ", op.BlockedByOperations.Select(o => o.Name).Where(n => !string.IsNullOrWhiteSpace(n)))
                        : "—";

                    PdfTable.RowSingleLine(table.Cell(), S, shaded, index.ToString());
                    PdfTable.RowDescription(table.Cell(), S, shaded, op.Operation?.Name, semiBold: true);
                    PdfTable.RowSingleLine(table.Cell(), S, shaded, blockedBy);
                }
            });
        });
    }

    private void ComposeFooter(IContainer container, BillOfMaterialDto doc, BusinessDto biz)
    {
        container.Column(col =>
        {
            if (!string.IsNullOrWhiteSpace(doc.Notes))
            {
                col.Item().Column(c =>
                {
                    c.Item().Text("Notes").FontSize(S.FontCaption).SemiBold().FontColor(S.TextMuted);
                    c.Item().PaddingTop(2).Text(doc.Notes!).FontSize(S.FontCaption);
                });
            }

            if (!string.IsNullOrWhiteSpace(doc.Terms))
            {
                col.Item().PaddingTop(10).Column(c =>
                {
                    c.Item().Text("Terms & Conditions").FontSize(S.FontCaption).SemiBold().FontColor(S.TextMuted);
                    c.Item().PaddingTop(2).Text(doc.Terms!).FontSize(S.FontCaption);
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
                            c.Item().PaddingTop(4).Height(48).Image(imageBytes).FitArea();
                        else
                            c.Item().PaddingTop(24);
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
