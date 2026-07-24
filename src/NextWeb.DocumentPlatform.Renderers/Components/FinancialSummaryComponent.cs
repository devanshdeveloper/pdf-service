using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using NextWeb.DocumentPlatform.Application.Models;

namespace NextWeb.DocumentPlatform.Renderers.Components;

public class FinancialSummaryComponent : IComponent
{
    private readonly DocumentDto _model;

    public FinancialSummaryComponent(DocumentDto model)
    {
        _model = model;
    }

    public void Compose(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem(); // Spacer

            row.ConstantItem(250).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Cell().PaddingVertical(2).Text("Subtotal:");
                table.Cell().PaddingVertical(2).AlignRight().Text($"${_model.Subtotal:N2}");

                if (_model.TotalDiscountAmount > 0)
                {
                    table.Cell().PaddingVertical(2).Text("Discount:");
                    table.Cell().PaddingVertical(2).AlignRight().Text($"-${_model.TotalDiscountAmount:N2}");
                }

                if (_model.TotalTaxAmount > 0)
                {
                    table.Cell().PaddingVertical(2).Text("Tax:");
                    table.Cell().PaddingVertical(2).AlignRight().Text($"${_model.TotalTaxAmount:N2}");
                }

                if (_model.TotalCostAmount > 0)
                {
                    table.Cell().PaddingVertical(2).Text("Add. Costs:");
                    table.Cell().PaddingVertical(2).AlignRight().Text($"${_model.TotalCostAmount:N2}");
                }

                if (_model.RoundOff != 0)
                {
                    table.Cell().PaddingVertical(2).Text("Round Off:");
                    table.Cell().PaddingVertical(2).AlignRight().Text($"${_model.RoundOff:N2}");
                }

                table.Cell().PaddingTop(5).Text("Grand Total:").SemiBold();
                table.Cell().PaddingTop(5).AlignRight().Text($"${_model.GrandTotal:N2}").SemiBold();
            });
        });
    }
}
