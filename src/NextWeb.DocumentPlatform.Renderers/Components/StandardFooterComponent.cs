using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using NextWeb.DocumentPlatform.Application.Models;

namespace NextWeb.DocumentPlatform.Renderers.Components;

public class StandardFooterComponent : IComponent
{
    private readonly DocumentDto _model;

    public StandardFooterComponent(DocumentDto model)
    {
        _model = model;
    }

    public void Compose(IContainer container)
    {
        container.Column(column =>
        {
            if (!string.IsNullOrEmpty(_model.Notes))
            {
                column.Item().PaddingBottom(5).Text(text =>
                {
                    text.Span("Notes: ").SemiBold();
                    text.Span(_model.Notes);
                });
            }

            if (!string.IsNullOrEmpty(_model.Terms))
            {
                column.Item().PaddingBottom(5).Text(text =>
                {
                    text.Span("Terms & Conditions: ").SemiBold();
                    text.Span(_model.Terms);
                });
            }

            if (_model.Party?.Bank != null)
            {
                column.Item().PaddingBottom(5).Text(text =>
                {
                    text.Span("Bank Details: ").SemiBold();
                    text.Span($"{_model.Party.Bank.BankName} - {_model.Party.Bank.AccountNumber} (IFSC: {_model.Party.Bank.Ifsc})");
                });
            }

            column.Item().AlignCenter().Text(x =>
            {
                x.CurrentPageNumber();
                x.Span(" / ");
                x.TotalPages();
            });
        });
    }
}
