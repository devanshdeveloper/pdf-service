using System;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Examples;

public class InvoiceTemplate : IDocument
{
    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(1.5f, Unit.Centimetre);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

            page.Header().Element(ComposeHeader);
            page.Content().PaddingVertical(1, Unit.Centimetre).Column(ComposeContent);
            page.Footer().Element(ComposeFooter);
        });
    }

    private void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                col.Item().Text("TAX INVOICE").FontSize(20).SemiBold().FontColor(Colors.Blue.Darken2);
                col.Item().Text("Invoice #10234");
                col.Item().Text($"Date: {DateTime.Now:d}");
            });

            row.ConstantItem(150).AlignRight().Column(col =>
            {
                col.Item().Text("Enterprise Corp").SemiBold();
                col.Item().Text("123 Business Rd, Tech City");
                col.Item().Text("GSTIN: 12ABCDE3456F7Z8");
            });
        });
    }

    private void ComposeContent(ColumnDescriptor col)
    {
        col.Item().Border(1).Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(40);
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
            });

            table.Header(header =>
            {
                header.Cell().Element(HeaderStyle).Text("#");
                header.Cell().Element(HeaderStyle).Text("Item");
                header.Cell().Element(HeaderStyle).AlignRight().Text("Qty");
                header.Cell().Element(HeaderStyle).AlignRight().Text("Price");

                static IContainer HeaderStyle(IContainer container) =>
                    container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).PaddingHorizontal(5).BorderBottom(1).Background(Colors.Grey.Lighten3);
            });

            for (int i = 1; i <= 3; i++)
            {
                table.Cell().Element(CellStyle).Text(i.ToString());
                table.Cell().Element(CellStyle).Text($"Consulting Service {i}");
                table.Cell().Element(CellStyle).AlignRight().Text("1");
                table.Cell().Element(CellStyle).AlignRight().Text("$500.00");

                static IContainer CellStyle(IContainer container) =>
                    container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5).PaddingHorizontal(5);
            }

            table.Cell().ColumnSpan(3).Element(TotalStyle).AlignRight().Text("Total").SemiBold();
            table.Cell().Element(TotalStyle).AlignRight().Text("$1500.00").SemiBold();

            static IContainer TotalStyle(IContainer container) =>
                container.PaddingVertical(5).PaddingHorizontal(5).Background(Colors.Grey.Lighten4);
        });
    }

    private void ComposeFooter(IContainer container)
    {
        container.AlignCenter().Text(x =>
        {
            x.Span("Page ");
            x.CurrentPageNumber();
            x.Span(" of ");
            x.TotalPages();
        });
    }
}
