using System.Linq;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using NextWeb.DocumentPlatform.Application.Models;

namespace NextWeb.DocumentPlatform.Renderers.Components;

public class DynamicTableComponent : IComponent
{
    private readonly DocumentDto _model;

    public DynamicTableComponent(DocumentDto model)
    {
        _model = model;
    }

    public void Compose(IContainer container)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(30); // #
                columns.RelativeColumn(3);  // Item
                columns.RelativeColumn();   // Qty
                columns.RelativeColumn();   // Price
                columns.RelativeColumn();   // Tax
                columns.RelativeColumn();   // Total
            });

            table.Header(header =>
            {
                HeaderCell(header.Cell()).Text("#");
                HeaderCell(header.Cell()).Text("Item");
                HeaderCell(header.Cell()).AlignRight().Text("Qty");
                HeaderCell(header.Cell()).AlignRight().Text("Price");
                HeaderCell(header.Cell()).AlignRight().Text("Tax");
                HeaderCell(header.Cell()).AlignRight().Text("Total");

                static IContainer HeaderCell(IContainer c) =>
                    c.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
            });

            if (_model.Products != null)
            {
                int index = 1;
                foreach (var item in _model.Products)
                {
                    RowCell(table.Cell()).Text(index++.ToString());
                    RowCell(table.Cell()).Text(item.Name);
                    RowCell(table.Cell()).AlignRight().Text($"{item.Quantity} {item.Unit?.Name}");
                    RowCell(table.Cell()).AlignRight().Text($"${item.Price:N2}");
                    RowCell(table.Cell()).AlignRight().Text($"${item.TaxAmount:N2}");
                    RowCell(table.Cell()).AlignRight().Text($"${item.Amount:N2}");

                    static IContainer RowCell(IContainer c) =>
                        c.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                }
            }
        });
    }
}
