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
                header.Cell().Element(CellStyle).Text("#");
                header.Cell().Element(CellStyle).Text("Item");
                header.Cell().Element(CellStyle).AlignRight().Text("Qty");
                header.Cell().Element(CellStyle).AlignRight().Text("Price");
                header.Cell().Element(CellStyle).AlignRight().Text("Tax");
                header.Cell().Element(CellStyle).AlignRight().Text("Total");

                static IContainer CellStyle(IContainer container)
                {
                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                }
            });

            if (_model.Products != null)
            {
                int index = 1;
                foreach (var item in _model.Products)
                {
                    table.Cell().Element(CellStyle).Text(index++.ToString());
                    table.Cell().Element(CellStyle).Text(item.Name);
                    table.Cell().Element(CellStyle).AlignRight().Text($"{item.Quantity} {item.Unit?.Name}");
                    table.Cell().Element(CellStyle).AlignRight().Text($"${item.Price:N2}");
                    table.Cell().Element(CellStyle).AlignRight().Text($"${item.TaxAmount:N2}");
                    table.Cell().Element(CellStyle).AlignRight().Text($"${item.Amount:N2}");
                    
                    static IContainer CellStyle(IContainer container)
                    {
                        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                    }
                }
            }
        });
    }
}
