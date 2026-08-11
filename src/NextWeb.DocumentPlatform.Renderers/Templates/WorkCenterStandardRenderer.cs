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

public class WorkCenterStandardRenderer : IDocumentRenderer
{
    private static readonly PdfSemantics S = new(PdfThemes.WorkCenter);

    public string DocumentType => "WorkCenter";
    public string TemplateName => "Standard";

    public Task<byte[]> RenderAsync(string jsonPayload, DocumentConfiguration config, CancellationToken cancellationToken)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var response = JsonSerializer.Deserialize<ApiResponse<WorkCenterDataModel>>(jsonPayload, options);
        var model = response?.Data;

        if (model?.Document == null || model.Business == null)
        {
            throw new Exception("Invalid JSON payload or missing document/business data for Work Center.");
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
                    if (doc.MapItems != null && doc.MapItems.Count > 0)
                    {
                        col.Item().PaddingTop(PdfPrimitives.SectionGap).Element(c => ComposeItems(c, doc));
                    }
                });
            });
        });

        return Task.FromResult(document.GeneratePdf());
    }

    private static string FormatAddressLines(AddressDto? address) => PdfFormat.AddressLines(address);

    private void ComposeHeader(IContainer container, WorkCenterDto doc, BusinessDto biz)
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
                });

                row.ConstantItem(250).AlignRight().Column(right =>
                {
                    right.Item().Text("Work Center").FontFamily(S.FontFamilyDisplay).FontSize(S.FontDisplayLg).SemiBold().FontColor(S.TextAccent);
                    right.Item().PaddingTop(4).Text(doc.Name).FontSize(14).SemiBold();

                    if (!string.IsNullOrWhiteSpace(doc.Status))
                    {
                        right.Item().PaddingTop(8).AlignRight()
                            .Element(c => PdfComponents.StatusChip(c, S, doc.Status));
                    }

                    right.Item().PaddingTop(10).Table(meta =>
                    {
                        meta.ColumnsDefinition(cols =>
                        {
                            cols.ConstantColumn(80);
                            cols.RelativeColumn();
                        });

                        void MetaRow(string label, string value)
                        {
                            meta.Cell().PaddingVertical(2).Text(label).FontSize(S.FontCaption).FontColor(S.TextMuted);
                            meta.Cell().PaddingVertical(2).AlignRight().Text(value).FontSize(S.FontCaption).SemiBold();
                        }

                        if (doc.WorkCenterType != null)
                            MetaRow("Type", doc.WorkCenterType.Name);

                        if (doc.WorkCenterTiming != null)
                            MetaRow("Timing", doc.WorkCenterTiming.Name);

                        if (doc.CostTemplate != null)
                            MetaRow("Cost Template", doc.CostTemplate.Name);

                        if (doc.CreatedAt.HasValue)
                            MetaRow("Created At", doc.CreatedAt.Value.ToString("dd MMM yyyy"));
                    });
                });
            });

            PdfComponents.HorizontalDivider(col.Item(), S);
        });
    }

    private void ComposeItems(IContainer container, WorkCenterDto doc)
    {
        container.Column(col =>
        {
            PdfComponents.SectionTitle(col.Item(), S, "Map Items");

            col.Item().PaddingTop(6).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(3);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(2);
                });

                table.Header(header =>
                {
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(S.FontCaption).SemiBold()).Text("Product");
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(S.FontCaption).SemiBold()).Text("Capacity");
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(S.FontCaption).SemiBold()).Text("Setup Time (sec)");
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(S.FontCaption).SemiBold()).Text("Cleanup Time (sec)");
                });

                int index = 0;

                foreach (var item in doc.MapItems)
                {
                    index++;
                    bool shaded = index % 2 == 0;

                    PdfTable.RowDescription(table.Cell(), S, shaded, item.AppliesToAllItems ? "All Items" : (item.Product?.Name ?? "-"), semiBold: true);
                    PdfTable.RowSingleLine(table.Cell(), S, shaded, item.Capacity.ToString());
                    PdfTable.RowSingleLine(table.Cell(), S, shaded, item.SetupTimeSeconds.ToString());
                    PdfTable.RowSingleLine(table.Cell(), S, shaded, item.CleanupTimeSeconds.ToString());
                }
                PdfTable.FooterCell(table.Cell(), S);
            });
        });
    }
}
