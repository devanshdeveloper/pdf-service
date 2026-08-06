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
using NextWeb.DocumentPlatform.Renderers.Design;

namespace NextWeb.DocumentPlatform.Renderers.Templates;

internal static class StockTransferPdfComposer
{
    public static Task<byte[]> RenderAsync(
        string jsonPayload,
        string documentTitle,
        PdfTheme theme,
        CancellationToken cancellationToken)
    {
        var semantics = new PdfSemantics(theme);
        return RenderAsync(jsonPayload, documentTitle, semantics, cancellationToken);
    }

    public static Task<byte[]> RenderAsync(
        string jsonPayload,
        string documentTitle,
        PdfSemantics semantics,
        CancellationToken cancellationToken)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var response = JsonSerializer.Deserialize<ApiResponse<StockTransferDataModel>>(jsonPayload, options);
        var model = response?.Data;

        if (model?.Document == null)
        {
            throw new Exception($"Invalid JSON payload or missing document data for {documentTitle}.");
        }

        var doc = model.Document;
        var biz = model.Business ?? doc.Business;
        if (biz == null)
        {
            throw new Exception($"Invalid JSON payload or missing business data for {documentTitle}.");
        }

        var s = semantics;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                PdfPageSetup.ConfigureA4(page, s);

                page.Content().Column(col =>
                {
                    col.Item().Element(c => ComposeHeader(c, doc, biz, documentTitle, s));
                    col.Item().PaddingTop(PdfPrimitives.SectionGap).Element(c => ComposeRoute(c, doc, s));
                    col.Item().PaddingTop(PdfPrimitives.SectionGap).Element(c => ComposeItems(c, doc, s));
                    col.Item().PaddingTop(PdfPrimitives.SectionGap).Element(c => ComposeWorkflow(c, doc, s));
                    if (doc.Comments.Count > 0)
                        col.Item().PaddingTop(PdfPrimitives.SectionGap).Element(c => ComposeComments(c, doc, s));
                    col.Item().PaddingTop(PdfPrimitives.FooterSignatoryGap).Element(c => ComposeFooter(c, biz, s));
                });
            });
        });

        return Task.FromResult(document.GeneratePdf());
    }

    private static string FormatAddressLines(AddressDto? address) => PdfFormat.AddressLines(address);

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
        ProductDisplayHelper.FormatLineItemName(null, item.Product);

    private static void ComposeHeader(
        IContainer container,
        StockTransferDto doc,
        BusinessDto biz,
        string documentTitle,
        PdfSemantics s)
    {
        container.Column(col =>
        {
            col.Item().Row(row =>
            {
                row.RelativeItem().Column(left =>
                {
                    left.Item().Text(biz.Name).FontFamily(s.FontFamilyDisplay).FontSize(s.FontDisplayMd).SemiBold().FontColor(s.TextAccent);
                    if (!string.IsNullOrWhiteSpace(biz.Description))
                        left.Item().PaddingTop(2).Text(biz.Description).FontSize(s.FontCaption).FontColor(s.TextMuted);

                    if (biz.Address != null)
                    {
                        var addrText = FormatAddressLines(biz.Address);
                        if (!string.IsNullOrWhiteSpace(addrText))
                            left.Item().PaddingTop(6).Text(addrText).FontSize(s.FontCaption).LineHeight(1.3f);
                    }

                    if (!string.IsNullOrWhiteSpace(biz.Gst))
                        left.Item().PaddingTop(4).Text($"GSTIN: {biz.Gst}").FontSize(s.FontCaption);

                    var contactParts = new List<string>();
                    if (!string.IsNullOrWhiteSpace(biz.Phone)) contactParts.Add(biz.Phone);
                    if (!string.IsNullOrWhiteSpace(biz.Email)) contactParts.Add(biz.Email);
                    if (contactParts.Count > 0)
                        left.Item().PaddingTop(2).Text(string.Join(" · ", contactParts)).FontSize(s.FontCaption).FontColor(s.TextMuted);
                });

                row.ConstantItem(200).AlignRight().Column(right =>
                {
                    right.Item().Text(documentTitle).FontFamily(s.FontFamilyDisplay).FontSize(s.FontDisplayLg).SemiBold().FontColor(s.TextAccent);
                    right.Item().PaddingTop(4).Text(doc.Name).FontSize(11).SemiBold();

                    if (!string.IsNullOrWhiteSpace(doc.Status))
                    {
                        var statusLabel = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(doc.Status);
                        right.Item().PaddingTop(8).AlignRight()
                            .Element(c => PdfComponents.StatusChip(c, s, statusLabel));
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
                            meta.Cell().PaddingVertical(2).Text(label).FontSize(s.FontCaption).FontColor(s.TextMuted);
                            meta.Cell().PaddingVertical(2).AlignRight().Text(value).FontSize(s.FontCaption).SemiBold();
                        }

                        if (doc.CreatedAt.HasValue)
                            MetaRow("Created", FormatDate(doc.CreatedAt));
                        if (doc.UpdatedAt.HasValue)
                            MetaRow("Updated", FormatDate(doc.UpdatedAt));
                    });
                });
            });

            PdfComponents.HorizontalDivider(col.Item(), s);
        });
    }

    private static void ComposeRoute(IContainer container, StockTransferDto doc, PdfSemantics s)
    {
        container.Column(col =>
        {
            PdfComponents.SectionTitle(col.Item(), s, "Transfer Route");

            col.Item().PaddingTop(8).Background(s.SurfaceCard).Padding(12).Row(row =>
            {
                row.RelativeItem().Column(from =>
                {
                    from.Item().Text("From").FontSize(s.FontCaption).FontColor(s.TextMuted);
                    from.Item().PaddingTop(2).Text(doc.FromLocation?.Name ?? "—").FontSize(10).SemiBold();
                });

                row.ConstantItem(40).AlignCenter().AlignMiddle()
                    .Text("→").FontSize(14).FontColor(s.TextAccent).SemiBold();

                row.RelativeItem().AlignRight().Column(to =>
                {
                    to.Item().AlignRight().Text("To").FontSize(s.FontCaption).FontColor(s.TextMuted);
                    to.Item().PaddingTop(2).AlignRight().Text(doc.ToLocation?.Name ?? "—").FontSize(10).SemiBold().FontColor(s.TextAccent);
                });
            });

            if (!string.IsNullOrWhiteSpace(doc.Description))
            {
                col.Item().PaddingTop(10).Column(desc =>
                {
                    PdfComponents.SectionLabel(desc.Item(), s, "Description");
                    desc.Item().PaddingTop(2).Text(doc.Description!).FontSize(s.FontCaption);
                });
            }
        });
    }

    private static void ComposeItems(IContainer container, StockTransferDto doc, PdfSemantics s)
    {
        container.Column(col =>
        {
            PdfComponents.SectionTitle(col.Item(), s, "Transfer Items");

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
                    PdfTable.HeaderCellNeutral(header.Cell(), s).DefaultTextStyle(x => x.FontSize(s.FontCaption).SemiBold()).Text("#");
                    PdfTable.HeaderCellNeutral(header.Cell(), s).DefaultTextStyle(x => x.FontSize(s.FontCaption).SemiBold()).Text("Product");
                    PdfTable.HeaderCellNeutral(header.Cell(), s).DefaultTextStyle(x => x.FontSize(s.FontCaption).SemiBold()).Text("SKU");
                    PdfTable.HeaderCellNeutral(header.Cell(), s).DefaultTextStyle(x => x.FontSize(s.FontCaption).SemiBold()).AlignRight().Text("Qty");
                });

                decimal totalQty = 0;
                int index = 0;

                foreach (var item in doc.Items)
                {
                    index++;
                    totalQty += item.Quantity;
                    bool shaded = index % 2 == 0;

                    PdfTable.RowSingleLine(table.Cell(), s, shaded, index.ToString());
                    PdfTable.RowDescription(table.Cell(), s, shaded, GetProductName(item), semiBold: true);
                    PdfTable.RowSingleLine(table.Cell(), s, shaded, item.Product?.Sku);
                    PdfTable.RowSingleLine(table.Cell(), s, shaded, $"{item.Quantity:0.##}", alignRight: true);
                }

                if (doc.Items.Count > 0)
                {
                    PdfTable.FooterText(table.Cell().ColumnSpan(3), s, "Total Quantity", alignRight: true, semiBold: true);
                    PdfTable.FooterText(table.Cell(), s, $"{totalQty:0.##}", alignRight: true, semiBold: true);
                }
            });
        });
    }

    private static void ComposeWorkflow(IContainer container, StockTransferDto doc, PdfSemantics s)
    {
        container.Column(col =>
        {
            PdfComponents.SectionTitle(col.Item(), s, "Approval Workflow");

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
                    table.Cell().PaddingVertical(3).Text(label).FontSize(s.FontCaption).FontColor(s.TextMuted);
                    table.Cell().PaddingVertical(3).Text(value).FontSize(s.FontCaption);
                }

                Row("Requested By", FormatUserLabel(doc.RequestedBy));
                Row("Approved By", FormatUserLabel(doc.ApprovedBy));
                Row("Rejected By", FormatUserLabel(doc.RejectedBy));
            });
        });
    }

    private static void ComposeComments(IContainer container, StockTransferDto doc, PdfSemantics s)
    {
        container.Column(col =>
        {
            PdfComponents.SectionTitle(col.Item(), s, "Comments");

            foreach (var comment in doc.Comments.Where(c => !string.IsNullOrWhiteSpace(c.Message)))
            {
                col.Item().PaddingTop(6).Background(s.SurfaceCard).Padding(8).Column(c =>
                {
                    c.Item().Row(row =>
                    {
                        row.RelativeItem().Text(FormatUserLabel(comment.Author)).FontSize(s.FontCaption).SemiBold();
                        row.AutoItem().Text(FormatDateTime(comment.CreatedAt)).FontSize(7).FontColor(s.TextMuted);
                    });
                    c.Item().PaddingTop(4).Text(comment.Message).FontSize(s.FontCaption);
                });
            }
        });
    }

    private static void ComposeFooter(IContainer container, BusinessDto biz, PdfSemantics s)
    {
        container.Row(row =>
        {
            row.RelativeItem();
            row.ConstantItem(180).AlignRight().Element(c => PdfComponents.SignatoryBlock(c, biz.Name, s));
        });
    }
}
