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

public class WorkCenterTimingStandardRenderer : IDocumentRenderer
{
    private static readonly PdfSemantics S = new(PdfThemes.WorkCenter);

    public string DocumentType => "WorkCenterTiming";
    public string TemplateName => "Standard";

    public Task<byte[]> RenderAsync(string jsonPayload, DocumentConfiguration config, CancellationToken cancellationToken)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var response = JsonSerializer.Deserialize<ApiResponse<WorkCenterTimingDataModel>>(jsonPayload, options);
        var model = response?.Data;

        if (model?.Document == null || model.Business == null)
        {
            throw new Exception("Invalid JSON payload or missing document/business data for Work Center Timing.");
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
                    col.Item().PaddingTop(PdfPrimitives.SectionGap).Element(c => ComposeDays(c, doc));
                    if (doc.Breaks != null && doc.Breaks.Count > 0)
                    {
                        col.Item().PaddingTop(PdfPrimitives.SectionGap).Element(c => ComposeBreaks(c, doc));
                    }
                });
            });
        });

        return Task.FromResult(document.GeneratePdf());
    }

    private static string FormatAddressLines(AddressDto? address) => PdfFormat.AddressLines(address);

    private void ComposeHeader(IContainer container, WorkCenterTimingDto doc, BusinessDto biz)
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

                row.ConstantItem(230).AlignRight().Column(right =>
                {
                    right.Item().Text("Work Center Timing").FontFamily(S.FontFamilyDisplay).FontSize(S.FontDisplayLg).SemiBold().FontColor(S.TextAccent);
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

                        if (!string.IsNullOrWhiteSpace(doc.Configuration))
                            MetaRow("Configuration", doc.Configuration);
                        if (!string.IsNullOrWhiteSpace(doc.Timezone))
                            MetaRow("Timezone", doc.Timezone);
                        
                        if (doc.StandardHours != null)
                            MetaRow("Standard Hours", $"{doc.StandardHours.From} - {doc.StandardHours.To}");

                        if (doc.CreatedAt.HasValue)
                            MetaRow("Created At", doc.CreatedAt.Value.ToString("dd MMM yyyy"));
                    });
                });
            });

            PdfComponents.HorizontalDivider(col.Item(), S);
        });
    }

    private void ComposeDays(IContainer container, WorkCenterTimingDto doc)
    {
        if (doc.WorkingDays == null || doc.WorkingDays.Count == 0) return;

        container.Column(col =>
        {
            PdfComponents.SectionTitle(col.Item(), S, "Working Days");

            col.Item().PaddingTop(6).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(2);
                });

                table.Header(header =>
                {
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(S.FontCaption).SemiBold()).Text("Day");
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(S.FontCaption).SemiBold()).Text("Is Working");
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(S.FontCaption).SemiBold()).Text("From");
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(S.FontCaption).SemiBold()).Text("To");
                });

                int index = 0;
                var days = new[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
                
                foreach (var day in days)
                {
                    if (doc.WorkingDays.TryGetValue(day, out var details))
                    {
                        index++;
                        bool shaded = index % 2 == 0;

                        PdfTable.RowSingleLine(table.Cell(), S, shaded, day, semiBold: true);
                        PdfTable.RowSingleLine(table.Cell(), S, shaded, details.IsWorkingDay ? "Yes" : "No");
                        PdfTable.RowSingleLine(table.Cell(), S, shaded, details.IsWorkingDay ? (details.From ?? "-") : "-");
                        PdfTable.RowSingleLine(table.Cell(), S, shaded, details.IsWorkingDay ? (details.To ?? "-") : "-");
                    }
                }
                PdfTable.FooterCell(table.Cell(), S);
            });
        });
    }

    private void ComposeBreaks(IContainer container, WorkCenterTimingDto doc)
    {
        container.Column(col =>
        {
            PdfComponents.SectionTitle(col.Item(), S, "Breaks");

            col.Item().PaddingTop(6).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(24);
                    columns.RelativeColumn(3);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(2);
                });

                table.Header(header =>
                {
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(S.FontCaption).SemiBold()).Text("#");
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(S.FontCaption).SemiBold()).Text("Break Name");
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(S.FontCaption).SemiBold()).Text("From");
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(S.FontCaption).SemiBold()).Text("To");
                    PdfTable.HeaderCellNeutral(header.Cell(), S).DefaultTextStyle(x => x.FontSize(S.FontCaption).SemiBold()).Text("Paid");
                });

                int index = 0;

                foreach (var item in doc.Breaks)
                {
                    index++;
                    bool shaded = index % 2 == 0;

                    PdfTable.RowSingleLine(table.Cell(), S, shaded, index.ToString());
                    PdfTable.RowDescription(table.Cell(), S, shaded, item.Name, semiBold: true);
                    PdfTable.RowSingleLine(table.Cell(), S, shaded, item.From);
                    PdfTable.RowSingleLine(table.Cell(), S, shaded, item.To);
                    PdfTable.RowSingleLine(table.Cell(), S, shaded, item.IsPaid ? "Yes" : "No");
                }
                PdfTable.FooterCell(table.Cell(), S);
            });
        });
    }
}
