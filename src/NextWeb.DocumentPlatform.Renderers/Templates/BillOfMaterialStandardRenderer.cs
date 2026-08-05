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
using NextWeb.DocumentPlatform.Renderers;

namespace NextWeb.DocumentPlatform.Renderers.Templates;

public class BillOfMaterialStandardRenderer : IDocumentRenderer
{
    private const string AccentColor = "#2563EB";
    private const string AccentLight = "#DBEAFE";
    private const string SurfaceMuted = "#F3F4F6";
    private const string TextMuted = "#6B7280";
    private const string BorderLight = "#E5E7EB";

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
                page.Size(PageSizes.A4);
                page.Margin(1.2f, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(9).FontFamily("Arial").FontColor(Colors.Black));

                page.Content().Column(col =>
                {
                    col.Item().Element(c => ComposeHeader(c, doc, biz));
                    col.Item().PaddingTop(14).Element(c => ComposeOutputDetails(c, doc));
                    if (doc.Fields.Count > 0)
                        col.Item().PaddingTop(14).Element(c => ComposeCustomFields(c, doc));
                    col.Item().PaddingTop(16).Element(c => ComposeComponents(c, doc));
                    if (doc.Operations.Count > 0)
                        col.Item().PaddingTop(16).Element(c => ComposeOperations(c, doc));
                    col.Item().PaddingTop(16).Element(c => ComposeFooter(c, doc, biz));
                });
            });
        });

        return Task.FromResult(document.GeneratePdf());
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

                row.ConstantItem(210).AlignRight().Column(right =>
                {
                    right.Item().Text("Bill of Material").FontFamily("Montserrat").FontSize(18).SemiBold().FontColor(AccentColor);
                    right.Item().PaddingTop(4).Text($"#{doc.Number}").FontSize(11).SemiBold();
                    if (!string.IsNullOrWhiteSpace(doc.Name))
                        right.Item().PaddingTop(2).Text(doc.Name).FontSize(9).FontColor(TextMuted);

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

                        if (!string.IsNullOrWhiteSpace(doc.Type))
                            MetaRow("Type", doc.Type);
                        if (doc.CreatedAt.HasValue)
                            MetaRow("Created", FormatDate(doc.CreatedAt));
                        if (doc.UpdatedAt.HasValue)
                            MetaRow("Updated", FormatDate(doc.UpdatedAt));
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
            "rejected" => ("#FEE2E2", "#991B1B"),
            _ => (SurfaceMuted, TextMuted)
        };
    }

    private void ComposeOutputDetails(IContainer container, BillOfMaterialDto doc)
    {
        container.Column(col =>
        {
            col.Item().Text("Output Details").FontSize(9).SemiBold().FontColor(AccentColor);

            col.Item().PaddingTop(8).Background(SurfaceMuted).Padding(12).Column(details =>
            {
                details.Item().Row(row =>
                {
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().Text("To Produce").FontSize(8).FontColor(TextMuted);
                        c.Item().PaddingTop(2).Text(doc.ToProduce?.Name ?? "—").FontSize(10).SemiBold();
                    });

                    row.RelativeItem().AlignRight().Column(c =>
                    {
                        c.Item().AlignRight().Text("Output Quantity").FontSize(8).FontColor(TextMuted);
                        c.Item().PaddingTop(2).AlignRight()
                            .Text($"{doc.Quantity:0.##} {GetUnitLabel(doc.Unit)}".Trim())
                            .FontSize(10).SemiBold().FontColor(AccentColor);
                    });
                });

                if (doc.TransactionQuantity.HasValue)
                {
                    details.Item().PaddingTop(8).Row(row =>
                    {
                        row.RelativeItem().Text("Actual Quantity").FontSize(8).FontColor(TextMuted);
                        row.RelativeItem().AlignRight()
                            .Text($"{doc.TransactionQuantity:0.##} {GetUnitLabel(doc.TransactionUnit)}".Trim())
                            .FontSize(8).SemiBold();
                    });
                }
            });

            if (!string.IsNullOrWhiteSpace(doc.Description))
            {
                col.Item().PaddingTop(10).Column(desc =>
                {
                    desc.Item().Text("Description").FontSize(8).SemiBold().FontColor(TextMuted);
                    desc.Item().PaddingTop(2).Text(doc.Description).FontSize(8);
                });
            }
        });
    }

    private void ComposeCustomFields(IContainer container, BillOfMaterialDto doc)
    {
        container.Column(col =>
        {
            col.Item().Text("Custom Fields").FontSize(9).SemiBold().FontColor(AccentColor);

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

                    table.Cell().PaddingVertical(3).Text(field.Label).FontSize(8).FontColor(TextMuted);
                    table.Cell().PaddingVertical(3).Text(value).FontSize(8);
                }
            });
        });
    }

    private void ComposeComponents(IContainer container, BillOfMaterialDto doc)
    {
        container.Column(col =>
        {
            col.Item().Text("Components").FontSize(9).SemiBold().FontColor(AccentColor);

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
                    header.Cell().Element(HeaderCell).Text("#");
                    header.Cell().Element(HeaderCell).Text("Component");
                    header.Cell().Element(HeaderCell).AlignRight().Text("Qty");
                    header.Cell().Element(HeaderCell).Text("Unit");
                    header.Cell().Element(HeaderCell).AlignRight().Text("Actual");
                    header.Cell().Element(HeaderCell).Text("Act. Unit");
                    header.Cell().Element(HeaderCell).AlignRight().Text("Waste %");

                    static IContainer HeaderCell(IContainer c) =>
                        c.Background(SurfaceMuted)
                            .PaddingVertical(6)
                            .PaddingHorizontal(4)
                            .DefaultTextStyle(x => x.FontSize(7).SemiBold());
                });

                int index = 0;
                foreach (var item in doc.Components)
                {
                    index++;
                    bool shaded = index % 2 == 0;
                    var productName = ProductDisplayHelper.FormatLineItemName(null, item.Product);

                    table.Cell().Element(c => RowCell(c, shaded)).Text(index.ToString());
                    table.Cell().Element(c => RowCell(c, shaded)).Text(productName).SemiBold();
                    table.Cell().Element(c => RowCell(c, shaded)).AlignRight()
                        .Text($"{item.Quantity:0.##}");
                    table.Cell().Element(c => RowCell(c, shaded)).Text(GetUnitLabel(item.Unit));
                    table.Cell().Element(c => RowCell(c, shaded)).AlignRight()
                        .Text(item.TransactionQuantity.HasValue ? $"{item.TransactionQuantity:0.##}" : "—");
                    table.Cell().Element(c => RowCell(c, shaded))
                        .Text(item.TransactionUnit != null ? GetUnitLabel(item.TransactionUnit) : "—");
                    table.Cell().Element(c => RowCell(c, shaded)).AlignRight()
                        .Text($"{item.WastagePercent:0.##}%");
                }

                static IContainer RowCell(IContainer c, bool shaded) =>
                    shaded
                        ? c.Background(SurfaceMuted).PaddingVertical(5).PaddingHorizontal(4)
                        : c.PaddingVertical(5).PaddingHorizontal(4);
            });
        });
    }

    private void ComposeOperations(IContainer container, BillOfMaterialDto doc)
    {
        container.Column(col =>
        {
            col.Item().Text("Manufacturing Operations").FontSize(9).SemiBold().FontColor(AccentColor);

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
                    header.Cell().Element(HeaderCell).Text("#");
                    header.Cell().Element(HeaderCell).Text("Operation");
                    header.Cell().Element(HeaderCell).Text("Blocked By");

                    static IContainer HeaderCell(IContainer c) =>
                        c.Background(SurfaceMuted)
                            .PaddingVertical(6)
                            .PaddingHorizontal(6)
                            .DefaultTextStyle(x => x.FontSize(8).SemiBold());
                });

                int index = 0;
                foreach (var op in doc.Operations)
                {
                    index++;
                    bool shaded = index % 2 == 0;
                    var blockedBy = op.BlockedByOperations.Count > 0
                        ? string.Join(", ", op.BlockedByOperations.Select(o => o.Name).Where(n => !string.IsNullOrWhiteSpace(n)))
                        : "—";

                    table.Cell().Element(c => RowCell(c, shaded)).Text(index.ToString());
                    table.Cell().Element(c => RowCell(c, shaded)).Text(op.Operation?.Name ?? "—").SemiBold();
                    table.Cell().Element(c => RowCell(c, shaded)).Text(blockedBy).FontColor(TextMuted);
                }

                static IContainer RowCell(IContainer c, bool shaded) =>
                    shaded
                        ? c.Background(SurfaceMuted).PaddingVertical(5).PaddingHorizontal(6)
                        : c.PaddingVertical(5).PaddingHorizontal(6);
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
                    c.Item().Text("Notes").FontSize(8).SemiBold().FontColor(TextMuted);
                    c.Item().PaddingTop(2).Text(doc.Notes!).FontSize(8);
                });
            }

            if (!string.IsNullOrWhiteSpace(doc.Terms))
            {
                col.Item().PaddingTop(10).Column(c =>
                {
                    c.Item().Text("Terms & Conditions").FontSize(8).SemiBold().FontColor(TextMuted);
                    c.Item().PaddingTop(2).Text(doc.Terms!).FontSize(8);
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
