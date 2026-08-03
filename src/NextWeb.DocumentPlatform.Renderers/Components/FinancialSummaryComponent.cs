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
            row.RelativeItem();

            row.ConstantItem(250).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Cell().PaddingVertical(2).Text("Subtotal:");
                table.Cell().PaddingVertical(2).AlignRight().Text($"${_model.TotalTaxableAmount:N2}");

                if (_model.TotalDiscount > 0)
                {
                    table.Cell().PaddingVertical(2).Text("Discount:");
                    table.Cell().PaddingVertical(2).AlignRight().Text($"-${_model.TotalDiscount:N2}");
                }

                if (VoucherTypes.ShouldShowTaxes(_model.Type) && _model.TotalTaxAmount > 0)
                {
                    table.Cell().PaddingVertical(2).Text("Tax:");
                    table.Cell().PaddingVertical(2).AlignRight().Text($"${_model.TotalTaxAmount:N2}");
                }

                var totalCost = _model.Cost.Sum(c => c.Value);
                if (VoucherTypes.ShouldShowShipping(_model.Type) && totalCost > 0)
                {
                    table.Cell().PaddingVertical(2).Text("Add. Costs:");
                    table.Cell().PaddingVertical(2).AlignRight().Text($"${totalCost:N2}");
                }

                table.Cell().PaddingTop(5).Text("Grand Total:").SemiBold();
                table.Cell().PaddingTop(5).AlignRight().Text($"${_model.TotalAmount:N2}").SemiBold();
            });
        });
    }
}
