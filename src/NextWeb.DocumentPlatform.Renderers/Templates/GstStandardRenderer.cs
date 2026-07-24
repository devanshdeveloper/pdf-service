using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using NextWeb.DocumentPlatform.Engine;
using NextWeb.DocumentPlatform.Domain;
using NextWeb.DocumentPlatform.Application.Models;

namespace NextWeb.DocumentPlatform.Renderers.Templates;

public class GstStandardRenderer : IDocumentRenderer
{
    public string DocumentType => "Voucher";
    public string TemplateName => "gst-standard";

    public Task<byte[]> RenderAsync(string jsonPayload, DocumentConfiguration config, CancellationToken cancellationToken)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var response = JsonSerializer.Deserialize<ApiResponse<VoucherDataModel>>(jsonPayload, options);
        var model = response?.Data;

        if (model?.Document == null || model?.Business == null)
        {
            throw new Exception("Invalid JSON payload or missing document/business data for GST Voucher.");
        }

        var doc = model.Document;
        var biz = model.Business;
        var settings = model.Settings;
        var taxSummary = model.TaxSummary;

        // Consignee fallback: if consignee is not set, fallback to party.
        var consignee = doc.Consignee ?? doc.Party;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1.5f, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(9).FontFamily("Arial"));

                page.Header().Element(c => ComposeHeader(c, doc, biz));
                
                page.Content().PaddingVertical(1, Unit.Centimetre).Column(col =>
                {
                    ComposePartyBlock(col, doc, biz, consignee);
                    ComposeMetaBlock(col, doc);
                    col.Item().PaddingTop(15).Element(c => ComposeLineItems(c, doc));
                    col.Item().PaddingTop(15).Element(c => ComposeTaxSummary(c, doc, taxSummary));
                    col.Item().PaddingTop(20).Element(c => ComposeFooter(c, biz, settings));
                });
            });
        });

        return Task.FromResult(document.GeneratePdf());
    }

    private string FormatCurrency(decimal amount, string currencyCode = "INR")
    {
        string symbol = currencyCode switch
        {
            "USD" => "$",
            "EUR" => "€",
            "GBP" => "£",
            "INR" => "₹",
            _ => currencyCode + " "
        };
        return $"{symbol}{amount:N2}";
    }

    private void ComposeHeader(IContainer container, DocumentDto doc, BusinessDto biz)
    {
        container.Row(row =>
        {
            row.RelativeItem().Text("Tax Invoice").FontSize(18).SemiBold();
        });
    }

    private void ComposePartyBlock(ColumnDescriptor col, DocumentDto doc, BusinessDto biz, PartyDto? consignee)
    {
        col.Item().Border(1).Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
            });

            table.Header(header =>
            {
                header.Cell().Padding(5).BorderRight(1).BorderBottom(1).Background(Colors.Grey.Lighten3).Text("Seller").SemiBold();
                header.Cell().Padding(5).BorderRight(1).BorderBottom(1).Background(Colors.Grey.Lighten3).Text("Consignee (Ship To)").SemiBold();
                header.Cell().Padding(5).BorderBottom(1).Background(Colors.Grey.Lighten3).Text("Buyer (Bill To)").SemiBold();
            });

            table.Cell().Padding(5).BorderRight(1).Column(c => 
            {
                c.Item().Text(biz.Name).SemiBold();
                if (biz.Address != null)
                {
                    c.Item().Text($"{biz.Address.StreetAddress} {biz.Address.Apartment}".Trim());
                    c.Item().Text($"{biz.Address.City}, {biz.Address.State} {biz.Address.PostalCode}".Trim());
                    c.Item().Text($"State Code: {biz.Address.StateCode}");
                }
                c.Item().Text($"GSTIN: {biz.Gst}");
            });

            table.Cell().Padding(5).BorderRight(1).Column(c => 
            {
                c.Item().Text(consignee?.BusinessName ?? $"{consignee?.FirstName} {consignee?.LastName}".Trim()).SemiBold();
                if (consignee?.ShippingAddress != null)
                {
                    c.Item().Text($"{consignee.ShippingAddress.StreetAddress} {consignee.ShippingAddress.Apartment}".Trim());
                    c.Item().Text($"{consignee.ShippingAddress.City}, {consignee.ShippingAddress.State} {consignee.ShippingAddress.PostalCode}".Trim());
                    c.Item().Text($"State Code: {consignee.ShippingAddress.StateCode}");
                }
                c.Item().Text($"GSTIN: {consignee?.Gst}");
            });

            table.Cell().Padding(5).Column(c => 
            {
                c.Item().Text(doc.Party?.BusinessName ?? $"{doc.Party?.FirstName} {doc.Party?.LastName}".Trim()).SemiBold();
                if (doc.Party?.BillingAddress != null)
                {
                    c.Item().Text($"{doc.Party.BillingAddress.StreetAddress} {doc.Party.BillingAddress.Apartment}".Trim());
                    c.Item().Text($"{doc.Party.BillingAddress.City}, {doc.Party.BillingAddress.State} {doc.Party.BillingAddress.PostalCode}".Trim());
                    c.Item().Text($"State Code: {doc.Party.BillingAddress.StateCode}");
                }
                c.Item().Text($"GSTIN: {doc.Party?.Gst}");
            });
        });
    }

    private void ComposeMetaBlock(ColumnDescriptor col, DocumentDto doc)
    {
        col.Item().PaddingTop(10).Border(1).Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
            });

            string GetDate(DateTime? dt) => dt.HasValue ? dt.Value.ToString("d") : "";
            string GetStr(string? val) => string.IsNullOrWhiteSpace(val) ? "" : val;

            table.Cell().BorderRight(1).BorderBottom(1).Padding(4).Text($"Invoice No: {GetStr(doc.Number)}").SemiBold();
            table.Cell().BorderRight(1).BorderBottom(1).Padding(4).Text($"Dated: {doc.Date:d}").SemiBold();
            table.Cell().BorderRight(1).BorderBottom(1).Padding(4).Text($"Delivery Note: {GetStr(doc.DeliveryNote)} {GetDate(doc.DeliveryNoteDate)}".Trim());
            table.Cell().BorderBottom(1).Padding(4).Text($"Mode of Payment: {GetStr(doc.PaymentMethod)}");
            
            table.Cell().BorderRight(1).BorderBottom(1).Padding(4).Text($"Ref No: {GetStr(doc.ReferenceNumber)} {GetDate(doc.ReferenceDate)}".Trim());
            table.Cell().BorderRight(1).BorderBottom(1).Padding(4).Text($"Buyer Order: {GetStr(doc.BuyersOrderNo)} {GetDate(doc.BuyersOrderDate)}".Trim());
            table.Cell().BorderRight(1).BorderBottom(1).Padding(4).Text($"Dispatch Doc No: {GetStr(doc.DispatchDocNo)}");
            table.Cell().BorderBottom(1).Padding(4).Text($"Dispatched Through: {GetStr(doc.DispatchedThrough)}");

            table.Cell().BorderRight(1).Padding(4).Text($"Destination: {GetStr(doc.Destination)}");
            table.Cell().ColumnSpan(3).Padding(4).Text($"Terms of Delivery: {GetStr(doc.TermsOfDelivery)}");
        });
    }

    private void ComposeLineItems(IContainer container, DocumentDto doc)
    {
        string currency = "INR";

        container.Column(col =>
        {
            col.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(30); // #
                    columns.RelativeColumn(3);  // Description
                    columns.RelativeColumn();   // HSN/SAC
                    columns.RelativeColumn();   // Qty
                    columns.RelativeColumn();   // Rate
                    columns.RelativeColumn();   // Disc%
                    columns.RelativeColumn();   // Tax%
                    columns.RelativeColumn();   // Amount
                });

                table.Header(header =>
                {
                    header.Cell().Element(HeaderStyle).Text("#");
                    header.Cell().Element(HeaderStyle).Text("Description");
                    header.Cell().Element(HeaderStyle).Text("HSN/SAC");
                    header.Cell().Element(HeaderStyle).AlignRight().Text("Qty");
                    header.Cell().Element(HeaderStyle).AlignRight().Text("Rate");
                    header.Cell().Element(HeaderStyle).AlignRight().Text("Disc%");
                    header.Cell().Element(HeaderStyle).AlignRight().Text("Tax%");
                    header.Cell().Element(HeaderStyle).AlignRight().Text("Amount");

                    static IContainer HeaderStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).PaddingHorizontal(4).Border(1).Background(Colors.Grey.Lighten3);
                    }
                });

                decimal totalQty = 0;
                decimal productsTotal = 0;

                if (doc.Products != null)
                {
                    int index = 1;
                    foreach (var item in doc.Products)
                    {
                        totalQty += item.Quantity;
                        productsTotal += item.Amount;

                        string hsn = !string.IsNullOrWhiteSpace(item.Product?.Hsn) ? item.Product.Hsn : "";

                        table.Cell().Element(CellStyle).Text(index++.ToString());
                        table.Cell().Element(CellStyle).Text(item.Name ?? ""); 
                        table.Cell().Element(CellStyle).Text(hsn);
                        table.Cell().Element(CellStyle).AlignRight().Text($"{item.Quantity:0.##} {item.Unit?.Name}");
                        table.Cell().Element(CellStyle).AlignRight().Text(FormatCurrency(item.Price, currency));
                        table.Cell().Element(CellStyle).AlignRight().Text($"{item.DiscountValue}");

                        decimal totalTaxRate = item.Taxes?.Where(t => t.Type == "percentage" || string.IsNullOrEmpty(t.Type)).Sum(t => t.Value) ?? 0;
                        table.Cell().Element(CellStyle).AlignRight().Text($"{totalTaxRate}%");

                        table.Cell().Element(CellStyle).AlignRight().Text(FormatCurrency(item.Amount, currency));
                        
                        static IContainer CellStyle(IContainer container)
                        {
                            return container.Border(1).BorderColor(Colors.Black).PaddingVertical(5).PaddingHorizontal(4);
                        }
                    }
                }
                
                table.Cell().ColumnSpan(3).Element(TotalStyle).AlignRight().Text("Total").SemiBold();
                table.Cell().Element(TotalStyle).AlignRight().Text($"{totalQty:0.##}").SemiBold();
                table.Cell().ColumnSpan(3).Element(TotalStyle);
                table.Cell().Element(TotalStyle).AlignRight().Text(FormatCurrency(productsTotal, currency)).SemiBold();

                if (doc.Cost != null && doc.Cost.Any())
                {
                    foreach (var cost in doc.Cost)
                    {
                        table.Cell().ColumnSpan(7).Element(TotalStyle).AlignRight().Text(cost.Name);
                        table.Cell().Element(TotalStyle).AlignRight().Text(FormatCurrency(cost.Value, currency));
                    }
                }

                if (doc.TotalDiscountAmount > 0)
                {
                    table.Cell().ColumnSpan(7).Element(TotalStyle).AlignRight().Text("Discount");
                    table.Cell().Element(TotalStyle).AlignRight().Text($"-{FormatCurrency(doc.TotalDiscountAmount, currency)}");
                }

                table.Cell().ColumnSpan(7).Element(TotalStyle).AlignRight().Text("Grand Total").SemiBold();
                table.Cell().Element(TotalStyle).AlignRight().Text(FormatCurrency(doc.GrandTotal, currency)).SemiBold();
                
                static IContainer TotalStyle(IContainer container)
                {
                    return container.Border(1).PaddingVertical(5).PaddingHorizontal(4).Background(Colors.Grey.Lighten4);
                }
            });

            if (!string.IsNullOrWhiteSpace(doc.AmountInWords))
            {
                col.Item().PaddingTop(5).Row(row =>
                {
                    row.RelativeItem().Text(text =>
                    {
                        text.Span("Amount Chargeable (in words): ").SemiBold();
                        text.Span(doc.AmountInWords);
                    });
                    row.AutoItem().AlignRight().Text("E. & O.E").SemiBold();
                });
            }
        });
    }

    private void ComposeTaxSummary(IContainer container, DocumentDto doc, List<TaxSummaryDto>? taxSummary)
    {
        if (taxSummary == null || !taxSummary.Any())
            return;

        bool isIgst = doc.TaxType == "IGST";
        bool isUtgst = doc.TaxType == "CGST_UTGST";
        string currency = "INR";
        
        container.Column(col =>
        {
            col.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2); // HSN
                    columns.RelativeColumn();  // Taxable Value
                    if (isIgst)
                    {
                        columns.RelativeColumn(); // IGST Rate
                        columns.RelativeColumn(); // IGST Amount
                    }
                    else
                    {
                        columns.RelativeColumn(); // Central Tax Rate
                        columns.RelativeColumn(); // Central Tax Amount
                        columns.RelativeColumn(); // State/UT Tax Rate
                        columns.RelativeColumn(); // State/UT Tax Amount
                    }
                    columns.RelativeColumn(); // Total Tax Amount
                });

                table.Header(header =>
                {
                    header.Cell().Element(HeaderStyle).Text("HSN/SAC");
                    header.Cell().Element(HeaderStyle).AlignRight().Text("Taxable Value");

                    if (isIgst)
                    {
                        header.Cell().Element(HeaderStyle).AlignRight().Text("IGST Rate");
                        header.Cell().Element(HeaderStyle).AlignRight().Text("IGST Amount");
                    }
                    else
                    {
                        header.Cell().Element(HeaderStyle).AlignRight().Text("Central Tax Rate");
                        header.Cell().Element(HeaderStyle).AlignRight().Text("Central Tax Amount");
                        header.Cell().Element(HeaderStyle).AlignRight().Text(isUtgst ? "UT Tax Rate" : "State Tax Rate");
                        header.Cell().Element(HeaderStyle).AlignRight().Text(isUtgst ? "UT Tax Amount" : "State Tax Amount");
                    }

                    header.Cell().Element(HeaderStyle).AlignRight().Text("Total Tax Amount");

                    static IContainer HeaderStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).PaddingHorizontal(4).Border(1).Background(Colors.Grey.Lighten3);
                    }
                });

                foreach (var item in taxSummary)
                {
                    table.Cell().Element(CellStyle).Text(item.Hsn);
                    table.Cell().Element(CellStyle).AlignRight().Text(FormatCurrency(item.TaxableAmount, currency));

                    if (isIgst)
                    {
                        table.Cell().Element(CellStyle).AlignRight().Text($"{item.IgstRate}%");
                        table.Cell().Element(CellStyle).AlignRight().Text(FormatCurrency(item.IgstAmount, currency));
                    }
                    else
                    {
                        table.Cell().Element(CellStyle).AlignRight().Text($"{item.CgstRate}%");
                        table.Cell().Element(CellStyle).AlignRight().Text(FormatCurrency(item.CgstAmount, currency));
                        if (isUtgst)
                        {
                            table.Cell().Element(CellStyle).AlignRight().Text($"{item.UtgstRate}%");
                            table.Cell().Element(CellStyle).AlignRight().Text(FormatCurrency(item.UtgstAmount, currency));
                        }
                        else
                        {
                            table.Cell().Element(CellStyle).AlignRight().Text($"{item.SgstRate}%");
                            table.Cell().Element(CellStyle).AlignRight().Text(FormatCurrency(item.SgstAmount, currency));
                        }
                    }

                    decimal totalTax = isIgst ? item.IgstAmount : (item.CgstAmount + (isUtgst ? item.UtgstAmount : item.SgstAmount));
                    totalTax += item.CessAmount;

                    table.Cell().Element(CellStyle).AlignRight().Text(FormatCurrency(totalTax, currency));

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.Border(1).BorderColor(Colors.Black).PaddingVertical(5).PaddingHorizontal(4);
                    }
                }

                table.Cell().Element(TotalStyle).AlignRight().Text("Total").SemiBold();
                table.Cell().Element(TotalStyle).AlignRight().Text(FormatCurrency(taxSummary.Sum(x => x.TaxableAmount), currency)).SemiBold();

                if (isIgst)
                {
                    table.Cell().Element(TotalStyle);
                    table.Cell().Element(TotalStyle).AlignRight().Text(FormatCurrency(taxSummary.Sum(x => x.IgstAmount), currency)).SemiBold();
                }
                else
                {
                    table.Cell().Element(TotalStyle);
                    table.Cell().Element(TotalStyle).AlignRight().Text(FormatCurrency(taxSummary.Sum(x => x.CgstAmount), currency)).SemiBold();
                    table.Cell().Element(TotalStyle);
                    if (isUtgst)
                    {
                        table.Cell().Element(TotalStyle).AlignRight().Text(FormatCurrency(taxSummary.Sum(x => x.UtgstAmount), currency)).SemiBold();
                    }
                    else
                    {
                        table.Cell().Element(TotalStyle).AlignRight().Text(FormatCurrency(taxSummary.Sum(x => x.SgstAmount), currency)).SemiBold();
                    }
                }

                decimal overallTax = taxSummary.Sum(x => (isIgst ? x.IgstAmount : (x.CgstAmount + (isUtgst ? x.UtgstAmount : x.SgstAmount))) + x.CessAmount);
                table.Cell().Element(TotalStyle).AlignRight().Text(FormatCurrency(overallTax, currency)).SemiBold();

                static IContainer TotalStyle(IContainer container)
                {
                    return container.Border(1).PaddingVertical(5).PaddingHorizontal(4).Background(Colors.Grey.Lighten4);
                }
            });

            if (!string.IsNullOrWhiteSpace(doc.TotalTaxAmountInWords))
            {
                col.Item().PaddingTop(5).Row(row =>
                {
                    row.RelativeItem().Text(text =>
                    {
                        text.Span("Tax Amount (in words): ").SemiBold();
                        text.Span(doc.TotalTaxAmountInWords);
                    });
                });
            }
        });
    }

    private void ComposeFooter(IContainer container, BusinessDto biz, SettingsDto? settings)
    {
        string declaration = !string.IsNullOrWhiteSpace(settings?.Defaults?.Terms)
            ? settings.Defaults.Terms
            : "We declare that this invoice shows the actual price of the goods described and that all particulars are true and correct.";

        container.Row(row =>
        {
            row.RelativeItem().Column(col => 
            {
                col.Item().Text("Declaration:").SemiBold();
                col.Item().Text(declaration);
            });

            row.ConstantItem(200).AlignRight().Column(col => 
            {
                col.Item().Text($"for {biz.Name}").SemiBold();
                col.Item().PaddingTop(40).Text("Authorised Signatory").SemiBold();
            });
        });
    }
}
