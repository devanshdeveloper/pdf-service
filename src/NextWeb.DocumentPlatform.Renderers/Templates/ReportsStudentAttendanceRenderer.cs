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
using NextWeb.DocumentPlatform.Engine;
using NextWeb.DocumentPlatform.Domain;
using NextWeb.DocumentPlatform.Application.Models;
using NextWeb.DocumentPlatform.Renderers.Design;

namespace NextWeb.DocumentPlatform.Renderers.Templates;

/// <summary>
/// PDF template for Student Attendance Report under DocumentType "Reports".
/// Resolved when print payload settings.pdf_template = "StudentAttendance".
/// </summary>
public class ReportsStudentAttendanceRenderer : IDocumentRenderer
{
    private static readonly PdfSemantics S = new(PdfThemes.Reports);

    public string DocumentType => "Reports";
    public string TemplateName => "StudentAttendance";

    public Task<byte[]> RenderAsync(string jsonPayload, DocumentConfiguration config, CancellationToken cancellationToken)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var response = JsonSerializer.Deserialize<ApiResponse<ReportDataModel>>(jsonPayload, options);
        var model = response?.Data;

        if (model?.Document == null)
        {
            throw new Exception("Invalid JSON payload or missing report document data.");
        }

        var doc = model.Document;
        var biz = model.Business;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                PdfPageSetup.ConfigureA4(page, S);

                page.Header().Element(c => ComposePageHeader(c, doc, biz));
                page.Footer().Element(c => ComposePageFooter(c, biz));

                page.Content().Column(col =>
                {
                    col.Item().Element(c => ComposeTitleBlock(c, doc));
                    if (doc.Summary != null)
                        col.Item().PaddingTop(PdfPrimitives.SectionGap).Element(c => ComposeSummary(c, doc.Summary));
                    col.Item().PaddingTop(PdfPrimitives.SectionGap).Element(c => ComposeTable(c, doc));
                });
            });
        });

        return Task.FromResult(document.GeneratePdf());
    }

    private static string CellValue(Dictionary<string, object?> row, string key)
    {
        if (!row.TryGetValue(key, out var value) || value is null) return string.Empty;
        if (value is JsonElement el)
        {
            return el.ValueKind switch
            {
                JsonValueKind.String => el.GetString() ?? string.Empty,
                JsonValueKind.Number => el.ToString(),
                JsonValueKind.True => "Yes",
                JsonValueKind.False => "No",
                JsonValueKind.Null or JsonValueKind.Undefined => string.Empty,
                _ => el.ToString()
            };
        }
        return Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
    }

    private static string FormatAddressLines(AddressDto? address) => PdfFormat.AddressLines(address);

    private void ComposePageHeader(IContainer container, ReportDocumentDto doc, BusinessDto? biz)
    {
        container.Column(col =>
        {
            col.Item().Row(row =>
            {
                row.RelativeItem().Column(left =>
                {
                    left.Item().Text(biz?.Name ?? "Institute")
                        .FontFamily(S.FontFamilyDisplay)
                        .FontSize(S.FontDisplayMd)
                        .SemiBold()
                        .FontColor(S.TextAccent);

                    if (!string.IsNullOrWhiteSpace(biz?.Description))
                        left.Item().PaddingTop(2).Text(biz!.Description).FontSize(S.FontCaption).FontColor(S.TextMuted);

                    if (biz?.Address != null)
                    {
                        var addr = FormatAddressLines(biz.Address);
                        if (!string.IsNullOrWhiteSpace(addr))
                            left.Item().PaddingTop(4).Text(addr).FontSize(S.FontCaption).LineHeight(1.3f);
                    }
                });

                row.ConstantItem(160).AlignRight().Column(right =>
                {
                    right.Item().Text("REPORT").FontSize(8).SemiBold().FontColor(S.TextMuted);
                    right.Item().Text(doc.Title).FontSize(11).SemiBold().FontColor(S.TextAccent);
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

    private void ComposeTitleBlock(IContainer container, ReportDocumentDto doc)
    {
        container.Column(col =>
        {
            col.Item().Text(doc.Title)
                .FontFamily(S.FontFamilyDisplay)
                .FontSize(S.FontDisplayLg)
                .SemiBold()
                .FontColor(S.TextAccent);

            if (!string.IsNullOrWhiteSpace(doc.GeneratedAt))
            {
                col.Item().PaddingTop(4).Text($"Generated: {doc.GeneratedAt}")
                    .FontSize(S.FontCaption)
                    .FontColor(S.TextMuted);
            }
        });
    }

    private void ComposeSummary(IContainer container, ReportSummaryDto summary)
    {
        container.Background(S.SurfaceCard).Padding(PdfPrimitives.CardPadding).Row(row =>
        {
            void Kpi(string label, string value)
            {
                row.RelativeItem().Column(c =>
                {
                    c.Item().Text(label).FontSize(7).FontColor(S.TextMuted);
                    c.Item().PaddingTop(2).Text(value).FontSize(12).SemiBold();
                });
            }

            Kpi("Total", summary.TotalRecords.ToString(CultureInfo.InvariantCulture));
            Kpi("Present", summary.PresentCount.ToString(CultureInfo.InvariantCulture));
            Kpi("Absent", summary.AbsentCount.ToString(CultureInfo.InvariantCulture));
            Kpi("Late", summary.LateCount.ToString(CultureInfo.InvariantCulture));
            Kpi("Rate", $"{summary.AttendanceRate.ToString("0.#", CultureInfo.InvariantCulture)}%");
        });
    }

    private void ComposeTable(IContainer container, ReportDocumentDto doc)
    {
        var columns = doc.Columns?.Count > 0
            ? doc.Columns
            : new List<ReportColumnDto>
            {
                new() { Key = "date", Header = "Date" },
                new() { Key = "studentName", Header = "Student" },
                new() { Key = "classroomName", Header = "Classroom" },
                new() { Key = "subjectName", Header = "Subject" },
                new() { Key = "status", Header = "Status" },
            };

        container.Table(table =>
        {
            table.ColumnsDefinition(cols =>
            {
                foreach (var _ in columns)
                    cols.RelativeColumn();
            });

            table.Header(header =>
            {
                foreach (var col in columns)
                {
                    header.Cell().Background(S.SurfaceCard).Padding(4)
                        .Text(col.Header).FontSize(8).SemiBold().FontColor(S.TextMuted);
                }
            });

            foreach (var row in doc.Rows)
            {
                foreach (var col in columns)
                {
                    table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(4)
                        .Text(CellValue(row, col.Key)).FontSize(8);
                }
            }

            if (doc.Rows.Count == 0)
            {
                table.Cell().ColumnSpan((uint)columns.Count).Padding(8)
                    .AlignCenter().Text("No attendance records for the selected filters.")
                    .FontSize(9).FontColor(S.TextMuted);
            }
        });
    }
}
