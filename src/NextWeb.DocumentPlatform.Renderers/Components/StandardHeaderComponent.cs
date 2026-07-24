using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using NextWeb.DocumentPlatform.Application.Models;

namespace NextWeb.DocumentPlatform.Renderers.Components;

public class StandardHeaderComponent : IComponent
{
    private readonly DocumentDto _model;

    public StandardHeaderComponent(DocumentDto model)
    {
        _model = model;
    }

    public void Compose(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Text($"{_model.Type.ToUpper()}").FontSize(20).SemiBold();
                column.Item().Text($"# {_model.Number}").FontSize(14);
                column.Item().Text($"Date: {_model.Date:d}").FontSize(10);
                if (_model.DueDate != default)
                {
                    column.Item().Text($"Due Date: {_model.DueDate:d}").FontSize(10);
                }
            });

            row.RelativeItem().AlignRight().Column(column =>
            {
                if (_model.Party != null)
                {
                    column.Item().Text("Billed To:").SemiBold();
                    column.Item().Text(_model.Party.BusinessName ?? $"{_model.Party.FirstName} {_model.Party.LastName}");
                    
                    if (_model.Party.BillingAddress != null)
                    {
                        var addr = _model.Party.BillingAddress;
                        column.Item().Text($"{addr.StreetAddress}, {addr.Apartment}");
                        column.Item().Text($"{addr.City}, {addr.State} {addr.PostalCode}");
                        column.Item().Text(addr.Country);
                    }
                    if (!string.IsNullOrEmpty(_model.Party.Gst))
                    {
                        column.Item().Text($"GST: {_model.Party.Gst}");
                    }
                }
            });
        });
    }
}
