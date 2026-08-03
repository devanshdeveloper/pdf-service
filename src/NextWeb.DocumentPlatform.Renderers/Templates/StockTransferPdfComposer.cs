using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using NextWeb.DocumentPlatform.Domain;
using NextWeb.DocumentPlatform.Application.Models;

namespace NextWeb.DocumentPlatform.Renderers.Templates;

internal static class StockTransferPdfComposer
{
    private const string SurfaceMuted = "#F3F4F6";
    private const string TextMuted = "#6B7280";
    private const string BorderLight = "#E5E7EB";

    public static Task<byte[]> RenderAsync(
        string jsonPayload,
        string documentTitle,
        string accentColor,
        string accentLight,
        CancellationToken cancellationToken)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var response = JsonSerializer.Deserialize<ApiResponse<StockTransferDataModel>>(jsonPayload, options);
        var model = response?.Data;

        if (model?.Document == null || model.Business == null)
        {
            throw new Exception($"Invalid JSON payload or missing document/business data for {documentTitle}.");
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
                    col.Item().Element(c => ComposeHeader(c, doc, biz, documentTitle, accentColor, accentLight));
                    col.Item().PaddingTop(14).Element(c => ComposeRoute(c, doc, accentColor));
                    col.Item().PaddingTop(16).Element(c => ComposeItems(c, doc, accentColor));
                    col.Item().PaddingTop(16).Element(c => ComposeWorkflow(c, doc, accentColor));
                    if (doc.Comments.Count > 0)
                        col.Item().PaddingTop(16).Element(c => ComposeComments(c, doc, accentColor));
                    col.Item().PaddingTop(20).Element(c => ComposeFooter(c, biz));
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

    private static string FormatUserLabel(UserRefDto? user)
    {
        if (user == null) return string.Empty;
        var name = $"{user.FirstName} {user.LastName}".Trim();
        if (!string.IsNullOrWhiteSpace(name)) return name;
        return user.Username;
    }

    private static string FormatDate(DateTime? date) =>
        date.HasValue ? date.Value.ToString("dd MMM yyyy", CultureInfo.InvariantCulture) : string.Empty;

    private static string FormatDateTime(DateTime? date) =>
        date.HasValue ? date.Value.ToString("dd MMM yyyy HH:mm", CultureInfo.InvariantCulture) : string.Empty;

    private static string GetProductName(StockTransferItemDto item) =>
        item.Product?.Name ?? string.Empty;

    private static void ComposeHeader(
        IContainer container,
        StockTransferDto doc,
        BusinessDto biz,
        string documentTitle,
        string accentColor,
        string accentLight)
    {
        container.Column(col =>
        {
            col.Item().Row(row =>
            {
                row.RelativeItem().Column(left =>
                {
                    left.Item().Text(biz.Name).FontFamily("Montserrat").FontSize(16).SemiBold().FontColor(accentColor);
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
                    right.Item().Text(documentTitle).FontFamily("Montserrat").FontSize(18).SemiBold().FontColor(accentColor);
                    right.Item().PaddingTop(4).Text(doc.Name).FontSize(11).SemiBold();

                    if (!string.IsNullOrWhiteSpace(doc.Status))
                    {
                        right.Item().PaddingTop(8).AlignRight()
                            .Element(c => ComposeStatusChip(c, doc.Status, accentColor, accentLight));
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

    private static void ComposeStatusChip(IContainer container, string label, string accentColor, string accentLight)
    {
        var (bg, fg) = GetStatusColors(label, accentColor, accentLight);
        container
            .Background(bg)
            .PaddingVertical(3)
            .PaddingHorizontal(8)
            .Text(CultureInfo.InvariantCulture.TextInfo.ToTitleCase(label))
            .FontSize(8)
            .SemiBold()
            .FontColor(fg);
    }

    private static (string bg, string fg) GetStatusColors(string status, string accentColor, string accentLight)
    {
        var normalized = status.Trim().ToLowerInvariant();
        return normalized switch
        {
            "approved" => (accentLight, accentColor),
            "draft" or "pending" => ("#FEF3C7", "#92400E"),
            "rejected" => ("#FEE2E2", "#991B1B"),
            _ => (SurfaceMuted, TextMuted)
        };
    }

    private static void ComposeRoute(IContainer container, StockTransferDto doc, string accentColor)
    {
        container.Column(col =>
        {
            col.Item().Text("Transfer Route").FontSize(9).SemiBold().FontColor(accentColor);

            col.Item().PaddingTop(8).Background(SurfaceMuted).Padding(12).Row(row =>
            {
                row.RelativeItem().Column(from =>
                {
                    from.Item().Text("From").FontSize(8).FontColor(TextMuted);
                    from.Item().PaddingTop(2).Text(doc.FromLocation?.Name ?? "—").FontSize(10).SemiBold();
                });

                row.ConstantItem(40).AlignCenter().AlignMiddle()
                    .Text("→").FontSize(14).FontColor(accentColor).SemiBold();

                row.RelativeItem().AlignRight().Column(to =>
                {
                    to.Item().AlignRight().Text("To").FontSize(8).FontColor(TextMuted);
                    to.Item().PaddingTop(2).AlignRight().Text(doc.ToLocation?.Name ?? "—").FontSize(10).SemiBold().FontColor(accentColor);
                });
            });

            if (!string.IsNullOrWhiteSpace(doc.Description))
            {
                col.Item().PaddingTop(10).Column(desc =>
                {
                    desc.Item().Text("Description").FontSize(8).SemiBold().FontColor(TextMuted);
                    desc.Item().PaddingTop(2).Text(doc.Description!).FontSize(8);
                });
            }
        });
    }

    private static void ComposeItems(IContainer container, StockTransferDto doc, string accentColor)
    {
        container.Column(col =>
        {
            col.Item().Text("Transfer Items").FontSize(9).SemiBold().FontColor(accentColor);

            col.Item().PaddingTop(6).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(24);
                    columns.RelativeColumn(3);
                    columns.ConstantColumn(72);
                    columns.ConstantColumn(48);
                });

                table.Header(header =>
                {
                    header.Cell().Element(HeaderCell).Text("#");
                    header.Cell().Element(HeaderCell).Text("Product");
                    header.Cell().Element(HeaderCell).Text("SKU");
                    header.Cell().Element(HeaderCell).AlignRight().Text("Qty");

                    static IContainer HeaderCell(IContainer c) =>
                        c.Background(SurfaceMuted)
                            .PaddingVertical(6)
                            .PaddingHorizontal(6)
                            .DefaultTextStyle(x => x.FontSize(8).SemiBold());
                });

                decimal totalQty = 0;
                int index = 0;

                foreach (var item in doc.Items)
                {
                    index++;
                    totalQty += item.Quantity;
                    bool shaded = index % 2 == 0;

                    table.Cell().Element(c => RowCell(c, shaded)).Text(index.ToString());
                    table.Cell().Element(c => RowCell(c, shaded)).Text(GetProductName(item)).SemiBold();
                    table.Cell().Element(c => RowCell(c, shaded)).Text(item.Product?.Sku ?? "—");
                    table.Cell().Element(c => RowCell(c, shaded)).AlignRight()
                        .Text($"{item.Quantity:0.##}");
                }

                static IContainer RowCell(IContainer c, bool shaded) =>
                    shaded
                        ? c.Background(SurfaceMuted).PaddingVertical(5).PaddingHorizontal(6)
                        : c.PaddingVertical(5).PaddingHorizontal(6);

                if (doc.Items.Count > 0)
                {
                    table.Cell().ColumnSpan(3).Element(FooterCell).AlignRight().Text("Total Quantity").SemiBold();
                    table.Cell().Element(FooterCell).AlignRight().Text($"{totalQty:0.##}").SemiBold();

                    static IContainer FooterCell(IContainer c) =>
                        c.BorderTop(1).BorderColor(BorderLight).PaddingVertical(6).PaddingHorizontal(6);
                }
            });
        });
    }

    private static void ComposeWorkflow(IContainer container, StockTransferDto doc, string accentColor)
    {
        container.Column(col =>
        {
            col.Item().Text("Approval Workflow").FontSize(9).SemiBold().FontColor(accentColor);

            col.Item().PaddingTop(6).Table(table =>
            {
                table.ColumnsDefinition(cols =>
                {
                    cols.ConstantColumn(100);
                    cols.RelativeColumn();
                });

                void Row(string label, string value)
                {
                    if (string.IsNullOrWhiteSpace(value)) return;
                    table.Cell().PaddingVertical(3).Text(label).FontSize(8).FontColor(TextMuted);
                    table.Cell().PaddingVertical(3).Text(value).FontSize(8);
                }

                Row("Requested By", FormatUserLabel(doc.RequestedBy));
                Row("Approved By", FormatUserLabel(doc.ApprovedBy));
                Row("Rejected By", FormatUserLabel(doc.RejectedBy));
            });
        });
    }

    private static void ComposeComments(IContainer container, StockTransferDto doc, string accentColor)
    {
        container.Column(col =>
        {
            col.Item().Text("Comments").FontSize(9).SemiBold().FontColor(accentColor);

            foreach (var comment in doc.Comments.Where(c => !string.IsNullOrWhiteSpace(c.Message)))
            {
                col.Item().PaddingTop(6).Background(SurfaceMuted).Padding(8).Column(c =>
                {
                    c.Item().Row(row =>
                    {
                        row.RelativeItem().Text(FormatUserLabel(comment.Author)).FontSize(8).SemiBold();
                        row.AutoItem().Text(FormatDateTime(comment.CreatedAt)).FontSize(7).FontColor(TextMuted);
                    });
                    c.Item().PaddingTop(4).Text(comment.Message).FontSize(8);
                });
            }
        });
    }

    private static void ComposeFooter(IContainer container, BusinessDto biz)
    {
        container.Row(row =>
        {
            row.RelativeItem();
            row.ConstantItem(180).AlignRight().Column(c =>
            {
                c.Item().Text($"For {biz.Name}").FontSize(8).SemiBold();
                c.Item().PaddingTop(24).Text("Authorised Signatory").FontSize(8).SemiBold().FontColor(TextMuted);
            });
        });
    }
}
