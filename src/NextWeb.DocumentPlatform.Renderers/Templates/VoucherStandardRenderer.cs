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
using NextWeb.DocumentPlatform.Renderers.Design;

namespace NextWeb.DocumentPlatform.Renderers.Templates;

public class VoucherStandardRenderer : IDocumentRenderer
{
    private static readonly PdfSemantics S = new(PdfThemes.Voucher);

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
                PdfPageSetup.ConfigureA4(page, S);

                page.Content().Column(col =>
                {
                    ComposeHeader(col.Item(), doc, biz, settings);
                    col.Item().PaddingTop(PdfPrimitives.SectionGap).Element(c => ComposeParties(c, doc));
                    col.Item().PaddingTop(PdfPrimitives.SectionGap).Element(c => ComposeOptionalMeta(c, doc));
                    col.Item().PaddingTop(PdfPrimitives.SectionGap).Element(c => ComposeLineItems(c, doc, currency));
                    col.Item().PaddingTop(PdfPrimitives.SectionGap).Element(c => ComposeTotalsBlock(c, doc, currency));
                    if (ShouldShowTaxes(doc))
                    {
                        col.Item().PaddingTop(PdfPrimitives.SectionGap).Element(c => ComposeTaxSummary(c, doc, currency));
                    }
                    col.Item().PaddingTop(PdfPrimitives.SectionGap).Element(c => ComposeFooter(c, doc, biz, settings));
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

    private static string FormatCurrency(decimal amount, string currencyCode) =>
        PdfFormat.Currency(amount, currencyCode);

    private static string FormatAddressLines(AddressDto? address) =>
        PdfFormat.AddressLines(address);

    private static string GetPartyName(PartyDto? party)
    {
        if (party == null) return string.Empty;
        if (!string.IsNullOrWhiteSpace(party.BusinessName)) return party.BusinessName;
        return $"{party.FirstName} {party.LastName}".Trim();
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
                    left.Item().Text(biz.Name).FontFamily("Montserrat").FontSize(16).SemiBold().FontColor(S.TextAccent);
                    if (!string.IsNullOrWhiteSpace(biz.Description))
                        left.Item().PaddingTop(2).Text(biz.Description).FontSize(8).FontColor(S.TextMuted);

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
                        left.Item().PaddingTop(2).Text(string.Join(" · ", contactParts)).FontSize(8).FontColor(S.TextMuted);
                });

                row.ConstantItem(200).AlignRight().Column(right =>
                {
                    right.Item().Text(doc.Type).FontFamily("Montserrat").FontSize(20).SemiBold().FontColor(S.TextAccent);
                    right.Item().PaddingTop(4).Text($"#{doc.Number}").FontSize(11).SemiBold();

                    right.Item().PaddingTop(8).Row(chipRow =>
                    {
                        PdfComponents.StatusChip(chipRow.AutoItem(), S, doc.Status);
                        if (ShouldShowPaymentStatus(doc) && !string.IsNullOrWhiteSpace(doc.PaymentStatus))
                        {
                            PdfComponents.StatusChip(chipRow.AutoItem().PaddingLeft(6), S, doc.PaymentStatus, isPayment: true);
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
                            meta.Cell().PaddingVertical(2).Text(label).FontSize(8).FontColor(S.TextMuted);
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

            PdfComponents.HorizontalDivider(col.Item(), S);
        });
    }

    private void ComposeParties(IContainer container, DocumentDto doc)
    {
        var showShipping = ShouldShowShipping(doc);
        var consignee = doc.Consignee ?? doc.Party;

        container.Row(row =>
        {
            ComposePartyCard(row.RelativeItem(), "Bill To", doc.Party);
            if (showShipping)
            {
                row.ConstantItem(16);
                ComposePartyCard(row.RelativeItem(), "Ship To", consignee);
            }
        });
    }

    private void ComposePartyCard(IContainer container, string title, PartyDto? party)
    {
        container.Background(S.SurfaceCard).Padding(PdfPrimitives.CardPadding).Column(col =>
        {
            col.Item().Text(title).FontSize(8).SemiBold().FontColor(S.TextMuted);
            col.Item().PaddingTop(4).Text(GetPartyName(party)).FontSize(10).SemiBold();

            var address = title == "Ship To" ? party?.ShippingAddress : party?.BillingAddress;
            var addressText = FormatAddressLines(address);
            if (!string.IsNullOrWhiteSpace(addressText))
                col.Item().PaddingTop(4).Text(addressText).FontSize(8).LineHeight(1.35f);

            if (address?.StateCode is { Length: > 0 })
                col.Item().PaddingTop(2).Text($"State Code: {address.StateCode}").FontSize(8).FontColor(S.TextMuted);

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
            col.Item().Text("Additional Details").FontSize(8).SemiBold().FontColor(S.TextMuted);
            col.Item().PaddingTop(6).Table(table =>
            {
                table.ColumnsDefinition(cols =>
                {
                    cols.ConstantColumn(100);
                    cols.RelativeColumn();
                });

                foreach (var (label, value) in rows)
                {
                    table.Cell().PaddingVertical(2).Text(label).FontSize(8).FontColor(S.TextMuted);
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
                columns.ConstantColumn(PdfPrimitives.TableColIndex);
                columns.RelativeColumn(5);
                columns.ConstantColumn(PdfPrimitives.TableColHsn);
                columns.ConstantColumn(PdfPrimitives.TableColQty);
                if (showActual)
                {
                    columns.ConstantColumn(PdfPrimitives.TableColUnit);
                    columns.ConstantColumn(PdfPrimitives.TableColQty);
                    columns.ConstantColumn(PdfPrimitives.TableColUnit);
                }
                columns.ConstantColumn(PdfPrimitives.TableColRate);
                columns.ConstantColumn(PdfPrimitives.TableColDisc);
                columns.ConstantColumn(PdfPrimitives.TableColTaxPct);
                columns.ConstantColumn(PdfPrimitives.TableColAmount);
            });

            table.Header(header =>
            {
                HeaderCell(header.Cell()).Text("#");
                HeaderCell(header.Cell()).Text("Description");
                HeaderCell(header.Cell()).Text("HSN/SAC");
                HeaderCell(header.Cell()).AlignRight().Text("Qty");
                if (showActual)
                {
                    HeaderCell(header.Cell()).Text("Unit");
                    HeaderCell(header.Cell()).AlignRight().Text("Actual Qty");
                    HeaderCell(header.Cell()).Text("Actual Unit");
                }
                HeaderCell(header.Cell()).AlignRight().Text("Rate");
                HeaderCell(header.Cell()).AlignRight().Text("Disc");
                HeaderCell(header.Cell()).AlignRight().Text("Tax%");
                HeaderCell(header.Cell()).AlignRight().Text("Amount");

                IContainer HeaderCell(IContainer c) =>
                    PdfTable.HeaderCellPrimary(c, S)
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

                PdfTable.RowSingleLine(table.Cell(), S, shaded, index.ToString());
                PdfTable.RowDescription(table.Cell(), S, shaded, ProductDisplayHelper.FormatLineItemName(item.Name, item.Product));
                PdfTable.RowSingleLine(table.Cell(), S, shaded, hsn);
                PdfTable.RowSingleLine(
                    table.Cell(), S, shaded,
                    showActual
                        ? $"{item.Quantity:0.##}"
                        : $"{item.Quantity:0.##} {GetUnitLabel(item.Unit)}".Trim(),
                    alignRight: true);
                if (showActual)
                {
                    PdfTable.RowSingleLine(table.Cell(), S, shaded, GetUnitLabel(item.Unit));
                    PdfTable.RowSingleLine(
                        table.Cell(), S, shaded,
                        item.TransactionQuantity.HasValue ? $"{item.TransactionQuantity:0.##}" : null,
                        alignRight: true);
                    PdfTable.RowSingleLine(table.Cell(), S, shaded, GetUnitLabel(item.TransactionUnit));
                }
                PdfTable.TableAmountCell(table.Cell(), S, shaded, FormatCurrency(item.Price, currency));
                PdfTable.RowSingleLine(
                    table.Cell(), S, shaded,
                    item.DiscountValue > 0 ? $"{item.DiscountValue}" : null,
                    alignRight: true);
                PdfTable.RowSingleLine(
                    table.Cell(), S, shaded,
                    totalTaxRate > 0 ? $"{totalTaxRate}%" : null,
                    alignRight: true);
                PdfTable.TableAmountCell(
                    table.Cell(), S, shaded, FormatCurrency(item.Amount, currency), semiBold: true);
            }

            uint trailingSpan = showActual ? 6u : 3u;
            PdfTable.FooterText(table.Cell().ColumnSpan(3), S, "Items Total", alignRight: true, semiBold: true, fontSize: 8);
            PdfTable.FooterText(table.Cell(), S, $"{totalQty:0.##}", alignRight: true, semiBold: true, fontSize: 8);
            PdfTable.FooterCell(table.Cell().ColumnSpan(trailingSpan), S);
            PdfTable.TableAmountFooterCell(
                table.Cell(), S, FormatCurrency(productsTotal, currency), semiBold: true);
        });
    }

    private void ComposeTotalsBlock(IContainer container, DocumentDto doc, string currency)
    {
        container.AlignRight().Width(PdfPrimitives.TotalsBlockWidth).Background(S.SurfaceAccent).Padding(PdfPrimitives.CardPadding).Column(col =>
        {
            void TotalRow(string label, string value, bool bold = false)
            {
                col.Item().PaddingVertical(1).Row(row =>
                {
                    if (bold)
                    {
                        row.RelativeItem().Text(label).FontSize(8).SemiBold();
                        PdfTable.Amount(
                            row.ConstantItem(PdfPrimitives.TotalsValueWidth).ExtendHorizontal(),
                            S,
                            value,
                            semiBold: true,
                            fontSize: 10);
                    }
                    else
                    {
                        row.RelativeItem().Text(label).FontSize(8);
                        PdfTable.Amount(
                            row.ConstantItem(PdfPrimitives.TotalsValueWidth).ExtendHorizontal(),
                            S,
                            value);
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

            col.Item().PaddingTop(4).LineHorizontal(1).LineColor(S.TextAccent);
            col.Item().PaddingTop(4).Row(row =>
            {
                row.RelativeItem().Text("Grand Total").FontSize(11).SemiBold().FontColor(S.TextAccent);
                PdfTable.Amount(
                    row.ConstantItem(PdfPrimitives.TotalsValueWidth).ExtendHorizontal().DefaultTextStyle(x => x.FontColor(S.TextAccent)),
                    S,
                    FormatCurrency(doc.TotalAmount, currency),
                    semiBold: true,
                    fontSize: PdfPrimitives.FontSizeTotal);
            });

            if (!string.IsNullOrWhiteSpace(doc.AmountInWords))
            {
                col.Item().PaddingTop(8).Text(text =>
                {
                    text.Span("In words: ").FontSize(7).SemiBold().FontColor(S.TextMuted);
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
            col.Item().Text("Tax Summary").FontSize(9).SemiBold().FontColor(S.TextAccent);
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
                    TaxHeader(header.Cell()).Text("HSN/SAC");
                    TaxHeader(header.Cell()).AlignRight().Text("Taxable Value");
                    if (isIgst)
                    {
                        TaxHeader(header.Cell()).AlignRight().Text(TaxPercentHeader(referenceIgst, igstTaxName));
                        TaxHeader(header.Cell()).AlignRight().Text(TaxAmountHeader(referenceIgst, igstTaxName));
                    }
                    else
                    {
                        TaxHeader(header.Cell()).AlignRight().Text(TaxPercentHeader(referenceCgst, centralTaxName));
                        TaxHeader(header.Cell()).AlignRight().Text(TaxAmountHeader(referenceCgst, centralTaxName));
                        TaxHeader(header.Cell()).AlignRight().Text(TaxPercentHeader(isUtgst ? referenceUtgst : referenceSgst, stateTaxName));
                        TaxHeader(header.Cell()).AlignRight().Text(TaxAmountHeader(isUtgst ? referenceUtgst : referenceSgst, stateTaxName));
                    }
                    TaxHeader(header.Cell()).AlignRight().Text("Total Tax");

                    static IContainer TaxHeader(IContainer c) =>
                        c.Background(S.SurfaceCard).Padding(5).DefaultTextStyle(x => x.FontSize(8).SemiBold());
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

                    TaxCell(table.Cell()).Text(item.Hsn);
                    TaxCell(table.Cell()).AlignRight()
                        .Text(FormatCurrency(item.TaxableValue, currency));

                    if (isIgst)
                    {
                        TaxCell(table.Cell()).AlignRight().Text(igst != null ? $"{igst.Value}%" : "—");
                        TaxCell(table.Cell()).AlignRight()
                            .Text(FormatCurrency(igst?.Amount ?? 0, currency));
                    }
                    else
                    {
                        TaxCell(table.Cell()).AlignRight().Text(cgst != null ? $"{cgst.Value}%" : "—");
                        TaxCell(table.Cell()).AlignRight()
                            .Text(FormatCurrency(cgst?.Amount ?? 0, currency));
                        if (isUtgst)
                        {
                            TaxCell(table.Cell()).AlignRight().Text(utgst != null ? $"{utgst.Value}%" : "—");
                            TaxCell(table.Cell()).AlignRight()
                                .Text(FormatCurrency(utgst?.Amount ?? 0, currency));
                        }
                        else
                        {
                            TaxCell(table.Cell()).AlignRight().Text(sgst != null ? $"{sgst.Value}%" : "—");
                            TaxCell(table.Cell()).AlignRight()
                                .Text(FormatCurrency(sgst?.Amount ?? 0, currency));
                        }
                    }

                    TaxCell(table.Cell()).AlignRight()
                        .Text(FormatCurrency(item.TotalTaxAmount, currency));
                }

                TaxFooter(table.Cell()).AlignRight().Text("Total").SemiBold();
                TaxFooter(table.Cell()).AlignRight()
                    .Text(FormatCurrency(sumTaxable, currency)).SemiBold();
                if (isIgst)
                {
                    TaxFooter(table.Cell());
                    TaxFooter(table.Cell()).AlignRight()
                        .Text(FormatCurrency(sumIgst, currency)).SemiBold();
                }
                else
                {
                    TaxFooter(table.Cell());
                    TaxFooter(table.Cell()).AlignRight()
                        .Text(FormatCurrency(sumCentral, currency)).SemiBold();
                    TaxFooter(table.Cell());
                    TaxFooter(table.Cell()).AlignRight()
                        .Text(FormatCurrency(sumState, currency)).SemiBold();
                }
                TaxFooter(table.Cell()).AlignRight()
                    .Text(FormatCurrency(sumTotalTax, currency)).SemiBold();

                static IContainer TaxCell(IContainer c) =>
                    c.BorderBottom(1).BorderColor(S.BorderDefault).Padding(5).DefaultTextStyle(x => x.FontSize(8));

                static IContainer TaxFooter(IContainer c) =>
                    c.BorderTop(1).BorderColor(S.BorderDefault).Padding(5).DefaultTextStyle(x => x.FontSize(8));
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
                    b.Item().Text("Bank Details").FontSize(8).SemiBold().FontColor(S.TextMuted);
                    b.Item().PaddingTop(4).DefaultTextStyle(x => x.FontSize(8)).Text(text =>
                    {
                        text.Span($"{bank.BankName}").SemiBold();
                        if (!string.IsNullOrWhiteSpace(bank.Branch))
                            text.Span($" · {bank.Branch}");
                    });
                    b.Item().PaddingTop(2).Text(
                        $"A/C: {bank.AccountNumber} · IFSC: {bank.Ifsc} · {bank.AccountType}").FontSize(8);
                    if (!string.IsNullOrWhiteSpace(bank.BranchAddress))
                        b.Item().PaddingTop(2).Text(bank.BranchAddress).FontSize(7).FontColor(S.TextMuted);
                });
            }

            if (!string.IsNullOrWhiteSpace(notes))
            {
                col.Item().PaddingTop(10).Column(c =>
                {
                    c.Item().Text("Notes").FontSize(8).SemiBold().FontColor(S.TextMuted);
                    c.Item().PaddingTop(2).Text(notes).FontSize(8);
                });
            }

            if (!string.IsNullOrWhiteSpace(terms))
            {
                col.Item().PaddingTop(10).Column(c =>
                {
                    c.Item().Text("Terms & Conditions").FontSize(8).SemiBold().FontColor(S.TextMuted);
                    c.Item().PaddingTop(2).Text(terms).FontSize(8);
                });
            }

            col.Item().PaddingTop(PdfPrimitives.FooterSignatoryGap).Row(row =>
            {
                row.RelativeItem();
                row.ConstantItem(180).AlignRight().Column(c =>
                {
                    c.Item().Text($"For {biz.Name}").FontSize(8).SemiBold();
                    c.Item().PaddingTop(24).Text("Authorised Signatory").FontSize(8).SemiBold().FontColor(S.TextMuted);
                });
            });
        });
    }
}
