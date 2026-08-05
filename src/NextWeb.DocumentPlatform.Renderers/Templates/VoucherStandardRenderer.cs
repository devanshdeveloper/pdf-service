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
using NextWeb.DocumentPlatform.Renderers;

namespace NextWeb.DocumentPlatform.Renderers.Templates;

public class VoucherStandardRenderer : IDocumentRenderer
{
    private const string AccentColor = "#4338CA";
    private const string AccentLight = "#EEF2FF";
    private const string SurfaceMuted = "#F3F4F6";
    private const string TextMuted = "#6B7280";
    private const string BorderLight = "#E5E7EB";

    public string DocumentType => "Voucher";
    public string TemplateName => "Standard";

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
        var currency = ResolveCurrency(doc, settings, biz);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1.2f, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(9).FontFamily("Arial").FontColor(Colors.Black));

                page.Content().Column(col =>
                {
                    col.Item().Element(c => ComposeHeader(c, doc, biz, settings));
                    col.Item().PaddingTop(16).Element(c => ComposeParties(c, doc));
                    col.Item().PaddingTop(12).Element(c => ComposeOptionalMeta(c, doc));
                    col.Item().PaddingTop(16).Element(c => ComposeLineItems(c, doc, currency));
                    col.Item().PaddingTop(12).Element(c => ComposeTotalsBlock(c, doc, currency));
                    if (ShouldShowTaxes(doc))
                    {
                        col.Item().PaddingTop(16).Element(c => ComposeTaxSummary(c, doc, currency));
                    }
                    col.Item().PaddingTop(16).Element(c => ComposeFooter(c, doc, biz, settings));
                });
            });
        });

        return Task.FromResult(document.GeneratePdf());
    }

    private static bool ShouldShowTaxes(DocumentDto doc) => VoucherTypes.ShouldShowTaxes(doc.Type);

    private static bool ShouldShowShipping(DocumentDto doc) => VoucherTypes.ShouldShowShipping(doc.Type);

    private static bool ShouldShowPaymentStatus(DocumentDto doc) => VoucherTypes.ShouldShowPaymentStatus(doc.Type);

    private static string ResolveCurrency(DocumentDto doc, VoucherSettingsDto? settings, BusinessDto biz)
    {
        if (!string.IsNullOrWhiteSpace(doc.Currency)) return doc.Currency;
        if (!string.IsNullOrWhiteSpace(settings?.DefaultCurrency)) return settings.DefaultCurrency;
        if (!string.IsNullOrWhiteSpace(biz.BaseCurrency)) return biz.BaseCurrency;
        return "INR";
    }

    private static string FormatCurrency(decimal amount, string currencyCode)
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

    private static string GetPartyName(PartyDto? party)
    {
        if (party == null) return string.Empty;
        if (!string.IsNullOrWhiteSpace(party.BusinessName)) return party.BusinessName;
        return $"{party.FirstName} {party.LastName}".Trim();
    }

    private static string FormatAddressLines(AddressDto? address)
    {
        if (address == null) return string.Empty;
        if (!string.IsNullOrWhiteSpace(address.FullAddress)) return address.FullAddress;

        var lines = new List<string>();
        var street = $"{address.StreetAddress} {address.Apartment}".Trim();
        if (!string.IsNullOrWhiteSpace(street)) lines.Add(street);

        var cityLine = $"{address.City}, {address.State} {address.PostalCode}".Trim(',', ' ');
        if (!string.IsNullOrWhiteSpace(cityLine)) lines.Add(cityLine);
        if (!string.IsNullOrWhiteSpace(address.Country)) lines.Add(address.Country);

        return string.Join("\n", lines);
    }

    private static string GetNotes(DocumentDto doc, VoucherSettingsDto? settings)
    {
        if (!string.IsNullOrWhiteSpace(doc.Notes)) return doc.Notes!;
        return settings?.Defaults?.Notes ?? string.Empty;
    }

    private static string GetTerms(DocumentDto doc, VoucherSettingsDto? settings)
    {
        if (!string.IsNullOrWhiteSpace(doc.Terms)) return doc.Terms!;
        return settings?.Defaults?.Terms ?? string.Empty;
    }

    private static ComputedTaxDto? FindTax(IEnumerable<ComputedTaxDto> taxes, string name) =>
        taxes.FirstOrDefault(t => string.Equals(t.Name, name, StringComparison.OrdinalIgnoreCase));

    private void ComposeHeader(IContainer container, DocumentDto doc, BusinessDto biz, VoucherSettingsDto? settings)
    {
        container.Column(col =>
        {
            col.Item().Row(row =>
            {
                row.RelativeItem().Column(left =>
                {
                    left.Item().Text(biz.Name).FontFamily("Montserrat").FontSize(16).SemiBold().FontColor(AccentColor);
                    if (!string.IsNullOrWhiteSpace(biz.Description))
                        left.Item().PaddingTop(2).Text(biz.Description).FontSize(8).FontColor(TextMuted);

                    if (biz.Address != null)
                    {
                        var addrText = FormatAddressLines(biz.Address);
                        if (!string.IsNullOrWhiteSpace(addrText))
                            left.Item().PaddingTop(6).Text(addrText).FontSize(8).LineHeight(1.3f);
                    }

                    if (!string.IsNullOrWhiteSpace(biz.Gst))
                        left.Item().PaddingTop(4).Text($"GSTIN: {biz.Gst}").FontSize(8);

                    var contactParts = new List<string>();
                    if (!string.IsNullOrWhiteSpace(biz.Phone)) contactParts.Add(biz.Phone);
                    if (!string.IsNullOrWhiteSpace(biz.Email)) contactParts.Add(biz.Email);
                    if (contactParts.Count > 0)
                        left.Item().PaddingTop(2).Text(string.Join(" · ", contactParts)).FontSize(8).FontColor(TextMuted);
                });

                row.ConstantItem(200).AlignRight().Column(right =>
                {
                    right.Item().Text(doc.Type).FontFamily("Montserrat").FontSize(20).SemiBold().FontColor(AccentColor);
                    right.Item().PaddingTop(4).Text($"#{doc.Number}").FontSize(11).SemiBold();

                    right.Item().PaddingTop(8).Row(chipRow =>
                    {
                        chipRow.AutoItem().Element(c => ComposeStatusChip(c, doc.Status, false));
                        if (ShouldShowPaymentStatus(doc) && !string.IsNullOrWhiteSpace(doc.PaymentStatus))
                        {
                            chipRow.AutoItem().PaddingLeft(6).Element(c => ComposeStatusChip(c, doc.PaymentStatus, true));
                        }
                    });

                    right.Item().PaddingTop(10).Table(meta =>
                    {
                        meta.ColumnsDefinition(cols =>
                        {
                            cols.ConstantColumn(70);
                            cols.RelativeColumn();
                        });

                        void MetaRow(string label, string value)
                        {
                            meta.Cell().PaddingVertical(2).Text(label).FontSize(8).FontColor(TextMuted);
                            meta.Cell().PaddingVertical(2).AlignRight().Text(value).FontSize(8).SemiBold();
                        }

                        MetaRow("Date", doc.Date.ToString("dd MMM yyyy"));
                        if (doc.DueDate != default)
                            MetaRow("Due Date", doc.DueDate.ToString("dd MMM yyyy"));
                        if (!string.IsNullOrWhiteSpace(doc.PaymentMethod))
                            MetaRow("Payment", doc.PaymentMethod);
                    });
                });
            });

            col.Item().PaddingTop(12).LineHorizontal(1).LineColor(BorderLight);
        });
    }

    private void ComposeStatusChip(IContainer container, string label, bool isPayment)
    {
        var (bg, fg) = GetStatusColors(label, isPayment);
        container
            .Background(bg)
            .PaddingVertical(3)
            .PaddingHorizontal(8)
            .Text(label)
            .FontSize(8)
            .SemiBold()
            .FontColor(fg);
    }

    private static (string bg, string fg) GetStatusColors(string status, bool isPayment)
    {
        var normalized = status.Trim().ToLowerInvariant();
        if (isPayment)
        {
            if (normalized is "paid") return ("#DCFCE7", "#166534");
            if (normalized is "unpaid" or "overdue") return ("#FEF3C7", "#92400E");
            if (normalized.Contains("partial")) return ("#FFEDD5", "#9A3412");
        }
        else
        {
            if (normalized is "approved") return (AccentLight, AccentColor);
            if (normalized is "draft" or "pending") return ("#FEF3C7", "#92400E");
            if (normalized is "rejected" or "cancelled") return ("#FEE2E2", "#991B1B");
        }
        return (SurfaceMuted, TextMuted);
    }

    private void ComposeParties(IContainer container, DocumentDto doc)
    {
        var showShipping = ShouldShowShipping(doc);
        var consignee = doc.Consignee ?? doc.Party;

        container.Row(row =>
        {
            row.RelativeItem().Element(c => ComposePartyCard(c, "Bill To", doc.Party));
            if (showShipping)
            {
                row.ConstantItem(16);
                row.RelativeItem().Element(c => ComposePartyCard(c, "Ship To", consignee));
            }
        });
    }

    private void ComposePartyCard(IContainer container, string title, PartyDto? party)
    {
        container.Background(SurfaceMuted).Padding(12).Column(col =>
        {
            col.Item().Text(title).FontSize(8).SemiBold().FontColor(TextMuted);
            col.Item().PaddingTop(4).Text(GetPartyName(party)).FontSize(10).SemiBold();

            var address = title == "Ship To" ? party?.ShippingAddress : party?.BillingAddress;
            var addressText = FormatAddressLines(address);
            if (!string.IsNullOrWhiteSpace(addressText))
                col.Item().PaddingTop(4).Text(addressText).FontSize(8).LineHeight(1.35f);

            if (address?.StateCode is { Length: > 0 })
                col.Item().PaddingTop(2).Text($"State Code: {address.StateCode}").FontSize(8).FontColor(TextMuted);

            if (!string.IsNullOrWhiteSpace(party?.Gst))
                col.Item().PaddingTop(2).Text($"GSTIN: {party.Gst}").FontSize(8);
        });
    }

    private void ComposeOptionalMeta(IContainer container, DocumentDto doc)
    {
        var rows = new List<(string Label, string Value)>();

        if (!string.IsNullOrWhiteSpace(doc.ReferenceNumber))
            rows.Add(("Reference", FormatOptionalDate(doc.ReferenceNumber, doc.ReferenceDate)));
        if (!string.IsNullOrWhiteSpace(doc.DeliveryNote))
            rows.Add(("Delivery Note", FormatOptionalDate(doc.DeliveryNote, doc.DeliveryNoteDate)));
        if (!string.IsNullOrWhiteSpace(doc.BuyersOrderNo))
            rows.Add(("Buyer Order", FormatOptionalDate(doc.BuyersOrderNo, doc.BuyersOrderDate)));
        if (!string.IsNullOrWhiteSpace(doc.DispatchDocNo))
            rows.Add(("Dispatch Doc", doc.DispatchDocNo!));
        if (!string.IsNullOrWhiteSpace(doc.DispatchedThrough))
            rows.Add(("Dispatched Via", doc.DispatchedThrough!));
        if (!string.IsNullOrWhiteSpace(doc.Destination))
            rows.Add(("Destination", doc.Destination!));
        if (!string.IsNullOrWhiteSpace(doc.TermsOfDelivery))
            rows.Add(("Delivery Terms", doc.TermsOfDelivery!));
        if (!string.IsNullOrWhiteSpace(doc.Address))
            rows.Add(("Address", doc.Address));

        if (rows.Count == 0) return;

        container.Column(col =>
        {
            col.Item().Text("Additional Details").FontSize(8).SemiBold().FontColor(TextMuted);
            col.Item().PaddingTop(6).Table(table =>
            {
                table.ColumnsDefinition(cols =>
                {
                    cols.ConstantColumn(100);
                    cols.RelativeColumn();
                });

                foreach (var (label, value) in rows)
                {
                    table.Cell().PaddingVertical(2).Text(label).FontSize(8).FontColor(TextMuted);
                    table.Cell().PaddingVertical(2).Text(value).FontSize(8);
                }
            });
        });
    }

    private static string FormatOptionalDate(string text, DateTime? date) =>
        date.HasValue ? $"{text} ({date.Value:dd MMM yyyy})" : text;

    private static string GetUnitLabel(UnitDto? unit)
    {
        if (unit == null) return string.Empty;
        if (!string.IsNullOrWhiteSpace(unit.Value)) return unit.Value;
        return unit.Name;
    }

    private static string TaxPercentHeader(ComputedTaxDto? tax, string fallback) =>
        $"{(string.IsNullOrWhiteSpace(tax?.Name) ? fallback : tax.Name)} %";

    private static string TaxAmountHeader(ComputedTaxDto? tax, string fallback) =>
        $"{(string.IsNullOrWhiteSpace(tax?.Name) ? fallback : tax.Name)} Amt";

    private void ComposeLineItems(IContainer container, DocumentDto doc, string currency)
    {
        bool showActual = VoucherTypes.ShouldShowActualQuantity(doc.Type);

        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(24);
                columns.RelativeColumn(3);
                columns.RelativeColumn();
                columns.RelativeColumn();
                if (showActual)
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                }
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
            });

            table.Header(header =>
            {
                header.Cell().Element(HeaderCell).Text("#");
                header.Cell().Element(HeaderCell).Text("Description");
                header.Cell().Element(HeaderCell).Text("HSN/SAC");
                header.Cell().Element(HeaderCell).AlignRight().Text("Qty");
                if (showActual)
                {
                    header.Cell().Element(HeaderCell).Text("Unit");
                    header.Cell().Element(HeaderCell).AlignRight().Text("Actual Qty");
                    header.Cell().Element(HeaderCell).Text("Actual Unit");
                }
                header.Cell().Element(HeaderCell).AlignRight().Text("Rate");
                header.Cell().Element(HeaderCell).AlignRight().Text("Disc");
                header.Cell().Element(HeaderCell).AlignRight().Text("Tax%");
                header.Cell().Element(HeaderCell).AlignRight().Text("Amount");

                static IContainer HeaderCell(IContainer c) =>
                    c.Background(AccentColor)
                        .PaddingVertical(6)
                        .PaddingHorizontal(6)
                        .DefaultTextStyle(x => x.FontSize(8).SemiBold().FontColor(Colors.White));
            });

            decimal totalQty = 0;
            decimal productsTotal = 0;
            int index = 0;

            foreach (var item in doc.Products)
            {
                index++;
                totalQty += item.Quantity;
                productsTotal += item.Amount;
                bool shaded = index % 2 == 0;

                string hsn = !string.IsNullOrWhiteSpace(item.Hsn)
                    ? item.Hsn
                    : item.Product?.Hsn ?? string.Empty;

                decimal totalTaxRate = item.Taxes?
                    .Where(t => t.Type == "percentage" || string.IsNullOrEmpty(t.Type))
                    .Sum(t => t.Value) ?? 0;

                table.Cell().Element(c => RowCell(c, shaded)).Text(index.ToString());
                table.Cell().Element(c => RowCell(c, shaded))
                    .Text(ProductDisplayHelper.FormatLineItemName(item.Name, item.Product));
                table.Cell().Element(c => RowCell(c, shaded)).Text(hsn);
                table.Cell().Element(c => RowCell(c, shaded)).AlignRight()
                    .Text(showActual
                        ? $"{item.Quantity:0.##}"
                        : $"{item.Quantity:0.##} {GetUnitLabel(item.Unit)}".Trim());
                if (showActual)
                {
                    table.Cell().Element(c => RowCell(c, shaded)).Text(GetUnitLabel(item.Unit));
                    table.Cell().Element(c => RowCell(c, shaded)).AlignRight()
                        .Text(item.TransactionQuantity.HasValue ? $"{item.TransactionQuantity:0.##}" : "—");
                    table.Cell().Element(c => RowCell(c, shaded))
                        .Text(GetUnitLabel(item.TransactionUnit));
                }
                table.Cell().Element(c => RowCell(c, shaded)).AlignRight()
                    .Text(FormatCurrency(item.Price, currency));
                table.Cell().Element(c => RowCell(c, shaded)).AlignRight()
                    .Text(item.DiscountValue > 0 ? $"{item.DiscountValue}" : "—");
                table.Cell().Element(c => RowCell(c, shaded)).AlignRight()
                    .Text(totalTaxRate > 0 ? $"{totalTaxRate}%" : "—");
                table.Cell().Element(c => RowCell(c, shaded)).AlignRight()
                    .Text(FormatCurrency(item.Amount, currency)).SemiBold();
            }

            static IContainer RowCell(IContainer c, bool shaded) =>
                shaded
                    ? c.Background(SurfaceMuted).PaddingVertical(5).PaddingHorizontal(6)
                    : c.PaddingVertical(5).PaddingHorizontal(6);

            uint trailingSpan = showActual ? 6u : 3u;
            table.Cell().ColumnSpan(3).Element(FooterCell).AlignRight().Text("Items Total").SemiBold();
            table.Cell().Element(FooterCell).AlignRight().Text($"{totalQty:0.##}").SemiBold();
            table.Cell().ColumnSpan(trailingSpan).Element(FooterCell);
            table.Cell().Element(FooterCell).AlignRight()
                .Text(FormatCurrency(productsTotal, currency)).SemiBold();

            static IContainer FooterCell(IContainer c) =>
                c.BorderTop(1).BorderColor(BorderLight).PaddingVertical(6).PaddingHorizontal(6);
        });
    }

    private void ComposeTotalsBlock(IContainer container, DocumentDto doc, string currency)
    {
        container.AlignRight().Width(240).Background(AccentLight).Padding(12).Column(col =>
        {
            void TotalRow(string label, string value, bool bold = false)
            {
                col.Item().PaddingVertical(2).Row(row =>
                {
                    if (bold)
                    {
                        row.RelativeItem().Text(label).FontSize(8).SemiBold();
                        row.ConstantItem(90).AlignRight().Text(value).FontSize(10).SemiBold();
                    }
                    else
                    {
                        row.RelativeItem().Text(label).FontSize(8);
                        row.ConstantItem(90).AlignRight().Text(value).FontSize(8);
                    }
                });
            }

            TotalRow("Subtotal", FormatCurrency(doc.TotalTaxableAmount, currency));

            if (doc.TotalDiscount > 0)
                TotalRow("Discount", $"-{FormatCurrency(doc.TotalDiscount, currency)}");

            if (ShouldShowTaxes(doc) && doc.TotalTaxAmount > 0)
                TotalRow("Tax", FormatCurrency(doc.TotalTaxAmount, currency));

            if (ShouldShowShipping(doc) && doc.Cost.Count > 0)
            {
                foreach (var cost in doc.Cost)
                    TotalRow(cost.Name, FormatCurrency(cost.Value, currency));
            }

            col.Item().PaddingTop(4).LineHorizontal(1).LineColor(AccentColor);
            col.Item().PaddingTop(6).Row(row =>
            {
                row.RelativeItem().Text("Grand Total").FontSize(11).SemiBold().FontColor(AccentColor);
                row.ConstantItem(90).AlignRight()
                    .Text(FormatCurrency(doc.TotalAmount, currency))
                    .FontSize(12)
                    .SemiBold()
                    .FontColor(AccentColor);
            });

            if (!string.IsNullOrWhiteSpace(doc.AmountInWords))
            {
                col.Item().PaddingTop(8).Text(text =>
                {
                    text.Span("In words: ").FontSize(7).SemiBold().FontColor(TextMuted);
                    text.Span(doc.AmountInWords).FontSize(7);
                });
            }
        });
    }

    private void ComposeTaxSummary(IContainer container, DocumentDto doc, string currency)
    {
        if (doc.Summary == null || doc.Summary.Count == 0) return;

        bool isIgst = doc.TaxType == "IGST";
        bool isUtgst = doc.TaxType == "CGST_UTGST";
        var referenceTaxes = doc.Summary.FirstOrDefault()?.ComputedTaxes ?? new List<ComputedTaxDto>();
        var referenceCgst = FindTax(referenceTaxes, "CGST");
        var referenceSgst = FindTax(referenceTaxes, "SGST");
        var referenceUtgst = FindTax(referenceTaxes, "UTGST");
        var referenceIgst = FindTax(referenceTaxes, "IGST");
        string centralTaxName = referenceCgst?.Name ?? "CGST";
        string stateTaxName = isUtgst ? referenceUtgst?.Name ?? "UTGST" : referenceSgst?.Name ?? "SGST";
        string igstTaxName = referenceIgst?.Name ?? "IGST";

        container.Column(col =>
        {
            col.Item().Text("Tax Summary").FontSize(9).SemiBold().FontColor(AccentColor);
            col.Item().PaddingTop(6).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2);
                    columns.RelativeColumn();
                    if (isIgst)
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    }
                    else
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    }
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Element(TaxHeader).Text("HSN/SAC");
                    header.Cell().Element(TaxHeader).AlignRight().Text("Taxable Value");
                    if (isIgst)
                    {
                        header.Cell().Element(TaxHeader).AlignRight().Text(TaxPercentHeader(referenceIgst, igstTaxName));
                        header.Cell().Element(TaxHeader).AlignRight().Text(TaxAmountHeader(referenceIgst, igstTaxName));
                    }
                    else
                    {
                        header.Cell().Element(TaxHeader).AlignRight().Text(TaxPercentHeader(referenceCgst, centralTaxName));
                        header.Cell().Element(TaxHeader).AlignRight().Text(TaxAmountHeader(referenceCgst, centralTaxName));
                        header.Cell().Element(TaxHeader).AlignRight().Text(TaxPercentHeader(isUtgst ? referenceUtgst : referenceSgst, stateTaxName));
                        header.Cell().Element(TaxHeader).AlignRight().Text(TaxAmountHeader(isUtgst ? referenceUtgst : referenceSgst, stateTaxName));
                    }
                    header.Cell().Element(TaxHeader).AlignRight().Text("Total Tax");

                    static IContainer TaxHeader(IContainer c) =>
                        c.Background(SurfaceMuted).Padding(5).DefaultTextStyle(x => x.FontSize(8).SemiBold());
                });

                decimal sumTaxable = 0;
                decimal sumCentral = 0;
                decimal sumState = 0;
                decimal sumIgst = 0;
                decimal sumTotalTax = 0;

                foreach (var item in doc.Summary)
                {
                    var igst = FindTax(item.ComputedTaxes, "IGST");
                    var cgst = FindTax(item.ComputedTaxes, "CGST");
                    var sgst = FindTax(item.ComputedTaxes, "SGST");
                    var utgst = FindTax(item.ComputedTaxes, "UTGST");

                    sumTaxable += item.TaxableValue;
                    sumTotalTax += item.TotalTaxAmount;
                    if (isIgst)
                        sumIgst += igst?.Amount ?? 0;
                    else
                    {
                        sumCentral += cgst?.Amount ?? 0;
                        sumState += isUtgst ? utgst?.Amount ?? 0 : sgst?.Amount ?? 0;
                    }

                    table.Cell().Element(TaxCell).Text(item.Hsn);
                    table.Cell().Element(TaxCell).AlignRight()
                        .Text(FormatCurrency(item.TaxableValue, currency));

                    if (isIgst)
                    {
                        table.Cell().Element(TaxCell).AlignRight().Text(igst != null ? $"{igst.Value}%" : "—");
                        table.Cell().Element(TaxCell).AlignRight()
                            .Text(FormatCurrency(igst?.Amount ?? 0, currency));
                    }
                    else
                    {
                        table.Cell().Element(TaxCell).AlignRight().Text(cgst != null ? $"{cgst.Value}%" : "—");
                        table.Cell().Element(TaxCell).AlignRight()
                            .Text(FormatCurrency(cgst?.Amount ?? 0, currency));
                        if (isUtgst)
                        {
                            table.Cell().Element(TaxCell).AlignRight().Text(utgst != null ? $"{utgst.Value}%" : "—");
                            table.Cell().Element(TaxCell).AlignRight()
                                .Text(FormatCurrency(utgst?.Amount ?? 0, currency));
                        }
                        else
                        {
                            table.Cell().Element(TaxCell).AlignRight().Text(sgst != null ? $"{sgst.Value}%" : "—");
                            table.Cell().Element(TaxCell).AlignRight()
                                .Text(FormatCurrency(sgst?.Amount ?? 0, currency));
                        }
                    }

                    table.Cell().Element(TaxCell).AlignRight()
                        .Text(FormatCurrency(item.TotalTaxAmount, currency));
                }

                table.Cell().Element(TaxFooter).AlignRight().Text("Total").SemiBold();
                table.Cell().Element(TaxFooter).AlignRight()
                    .Text(FormatCurrency(sumTaxable, currency)).SemiBold();
                if (isIgst)
                {
                    table.Cell().Element(TaxFooter);
                    table.Cell().Element(TaxFooter).AlignRight()
                        .Text(FormatCurrency(sumIgst, currency)).SemiBold();
                }
                else
                {
                    table.Cell().Element(TaxFooter);
                    table.Cell().Element(TaxFooter).AlignRight()
                        .Text(FormatCurrency(sumCentral, currency)).SemiBold();
                    table.Cell().Element(TaxFooter);
                    table.Cell().Element(TaxFooter).AlignRight()
                        .Text(FormatCurrency(sumState, currency)).SemiBold();
                }
                table.Cell().Element(TaxFooter).AlignRight()
                    .Text(FormatCurrency(sumTotalTax, currency)).SemiBold();

                static IContainer TaxCell(IContainer c) =>
                    c.BorderBottom(1).BorderColor(BorderLight).Padding(5).DefaultTextStyle(x => x.FontSize(8));

                static IContainer TaxFooter(IContainer c) =>
                    c.BorderTop(1).BorderColor(BorderLight).Padding(5).DefaultTextStyle(x => x.FontSize(8));
            });

            if (!string.IsNullOrWhiteSpace(doc.TotalTaxAmountInWords))
            {
                col.Item().PaddingTop(6).Text(text =>
                {
                    text.Span("Tax amount in words: ").FontSize(8).SemiBold();
                    text.Span(doc.TotalTaxAmountInWords).FontSize(8);
                });
            }
        });
    }

    private void ComposeFooter(IContainer container, DocumentDto doc, BusinessDto biz, VoucherSettingsDto? settings)
    {
        var bank = doc.Bank;
        var notes = GetNotes(doc, settings);
        var terms = GetTerms(doc, settings);

        container.Column(col =>
        {
            if (bank != null)
            {
                col.Item().PaddingTop(8).Column(b =>
                {
                    b.Item().Text("Bank Details").FontSize(8).SemiBold().FontColor(TextMuted);
                    b.Item().PaddingTop(4).DefaultTextStyle(x => x.FontSize(8)).Text(text =>
                    {
                        text.Span($"{bank.BankName}").SemiBold();
                        if (!string.IsNullOrWhiteSpace(bank.Branch))
                            text.Span($" · {bank.Branch}");
                    });
                    b.Item().PaddingTop(2).Text(
                        $"A/C: {bank.AccountNumber} · IFSC: {bank.Ifsc} · {bank.AccountType}").FontSize(8);
                    if (!string.IsNullOrWhiteSpace(bank.BranchAddress))
                        b.Item().PaddingTop(2).Text(bank.BranchAddress).FontSize(7).FontColor(TextMuted);
                });
            }

            if (!string.IsNullOrWhiteSpace(notes))
            {
                col.Item().PaddingTop(10).Column(c =>
                {
                    c.Item().Text("Notes").FontSize(8).SemiBold().FontColor(TextMuted);
                    c.Item().PaddingTop(2).Text(notes).FontSize(8);
                });
            }

            if (!string.IsNullOrWhiteSpace(terms))
            {
                col.Item().PaddingTop(10).Column(c =>
                {
                    c.Item().Text("Terms & Conditions").FontSize(8).SemiBold().FontColor(TextMuted);
                    c.Item().PaddingTop(2).Text(terms).FontSize(8);
                });
            }

            col.Item().PaddingTop(20).Row(row =>
            {
                row.RelativeItem();
                row.ConstantItem(180).AlignRight().Column(c =>
                {
                    c.Item().Text($"For {biz.Name}").FontSize(8).SemiBold();
                    c.Item().PaddingTop(24).Text("Authorised Signatory").FontSize(8).SemiBold().FontColor(TextMuted);
                });
            });
        });
    }
}
