using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NextWeb.DocumentPlatform.Application.Models;

public class VoucherDataModel
{
    [JsonPropertyName("document")]
    public DocumentDto? Document { get; set; }

    [JsonPropertyName("business")]
    public BusinessDto? Business { get; set; }

    [JsonPropertyName("settings")]
    public VoucherSettingsDto? Settings { get; set; }

    [JsonPropertyName("taxSummary")]
    public List<TaxSummaryDto> TaxSummary { get; set; } = new();
}

public class DocumentDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    

    [JsonPropertyName("number")]
    public string Number { get; set; } = string.Empty;

    [JsonPropertyName("party")]
    public PartyDto? Party { get; set; }

    [JsonPropertyName("consignee")]
    public PartyDto? Consignee { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("dueDate")]
    public DateTime DueDate { get; set; }

    [JsonPropertyName("referenceNumber")]
    public string ReferenceNumber { get; set; } = string.Empty;

    [JsonPropertyName("referenceDate")]
    public DateTime? ReferenceDate { get; set; }

    [JsonPropertyName("dispatchDocNo")]
    public string DispatchDocNo { get; set; } = string.Empty;

    [JsonPropertyName("deliveryNote")]
    public string DeliveryNote { get; set; } = string.Empty;


    [JsonPropertyName("deliveryNoteDate")]
    public DateTime? DeliveryNoteDate { get; set; }

    [JsonPropertyName("buyersOrderNo")]
    public string BuyersOrderNo { get; set; } = string.Empty;

    [JsonPropertyName("buyersOrderDate")]
    public DateTime? BuyersOrderDate { get; set; }

    [JsonPropertyName("dispatchedThrough")]
    public string DispatchedThrough { get; set; } = string.Empty;

    [JsonPropertyName("destination")]
    public string Destination { get; set; } = string.Empty;

    [JsonPropertyName("termsOfDelivery")]
    public string TermsOfDelivery { get; set; } = string.Empty;

    [JsonPropertyName("paymentMethod")]
    public string PaymentMethod { get; set; } = string.Empty;

    [JsonPropertyName("address")]
    public string Address { get; set; } = string.Empty;

    [JsonPropertyName("products")]
    public List<VoucherProductDto> Products { get; set; } = new();

    [JsonPropertyName("notes")]
    public string Notes { get; set; } = string.Empty;

    [JsonPropertyName("terms")]
    public string Terms { get; set; } = string.Empty;

    [JsonPropertyName("cost")]
    public List<CostDto> Cost { get; set; } = new();

    [JsonPropertyName("discountType")]
    public string DiscountType { get; set; } = string.Empty;

    [JsonPropertyName("discountValue")]
    public decimal DiscountValue { get; set; }

    [JsonPropertyName("signature")]
    public SignatureDto? Signature { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("paymentStatus")]
    public string PaymentStatus { get; set; } = string.Empty;

    [JsonPropertyName("subtotal")]
    public decimal Subtotal { get; set; }

    [JsonPropertyName("totalDiscountAmount")]
    public decimal TotalDiscountAmount { get; set; }

    [JsonPropertyName("totalTaxAmount")]
    public decimal TotalTaxAmount { get; set; }

    [JsonPropertyName("totalCostAmount")]
    public decimal TotalCostAmount { get; set; }

    [JsonPropertyName("grandTotal")]
    public decimal GrandTotal { get; set; }

    [JsonPropertyName("roundOff")]
    public decimal RoundOff { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("taxType")]
    public string TaxType { get; set; } = string.Empty;

    [JsonPropertyName("amountInWords")]
    public string AmountInWords { get; set; } = string.Empty;

    [JsonPropertyName("totalTaxAmountInWords")]
    public string TotalTaxAmountInWords { get; set; } = string.Empty;
}

public class BusinessDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("phone")]
    public string Phone { get; set; } = string.Empty;

    [JsonPropertyName("gst")]
    public string Gst { get; set; } = string.Empty;

    [JsonPropertyName("address")]
    public AddressDto? Address { get; set; }
}

public class VoucherSettingsDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("logo")]
    public string Logo { get; set; } = string.Empty;

    [JsonPropertyName("template")]
    public string Template { get; set; } = string.Empty;

    [JsonPropertyName("pdf_template")]
    public string PdfTemplate { get; set; } = "Standard";

    [JsonPropertyName("prefix")]
    public string Prefix { get; set; } = string.Empty;

    [JsonPropertyName("counter")]
    public int Counter { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("defaultCurrency")]
    public string DefaultCurrency { get; set; } = string.Empty;

    [JsonPropertyName("defaults")]
    public DefaultsDto? Defaults { get; set; }
}

public class DefaultsDto
{
    [JsonPropertyName("terms")]
    public string Terms { get; set; } = string.Empty;

    [JsonPropertyName("notes")]
    public string Notes { get; set; } = string.Empty;
}

public class PartyDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("businessType")]
    public string BusinessType { get; set; } = string.Empty;

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("phone")]
    public string Phone { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("businessName")]
    public string BusinessName { get; set; } = string.Empty;

    [JsonPropertyName("billingAddress")]
    public AddressDto? BillingAddress { get; set; }

    [JsonPropertyName("shippingAddress")]
    public AddressDto? ShippingAddress { get; set; }

    [JsonPropertyName("bank")]
    public BankDto? Bank { get; set; }

    [JsonPropertyName("gst")]
    public string Gst { get; set; } = string.Empty;
}

public class AddressDto
{
    [JsonPropertyName("streetAddress")]
    public string StreetAddress { get; set; } = string.Empty;

    [JsonPropertyName("apartment")]
    public string Apartment { get; set; } = string.Empty;

    [JsonPropertyName("city")]
    public string City { get; set; } = string.Empty;

    [JsonPropertyName("state")]
    public string State { get; set; } = string.Empty;

    [JsonPropertyName("stateCode")]
    public string StateCode { get; set; } = string.Empty;

    [JsonPropertyName("postalCode")]
    public string PostalCode { get; set; } = string.Empty;

    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;
}

public class BankDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("bankName")]
    public string BankName { get; set; } = string.Empty;

    [JsonPropertyName("branch")]
    public string Branch { get; set; } = string.Empty;

    [JsonPropertyName("accountType")]
    public string AccountType { get; set; } = string.Empty;

    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("ifsc")]
    public string Ifsc { get; set; } = string.Empty;
}

public class VoucherProductDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("product")]
    public ProductDto? Product { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("quantity")]
    public decimal Quantity { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("unit")]
    public UnitDto? Unit { get; set; }

    [JsonPropertyName("discountType")]
    public string DiscountType { get; set; } = string.Empty;

    [JsonPropertyName("discountValue")]
    public decimal DiscountValue { get; set; }

    [JsonPropertyName("discountAmount")]
    public decimal DiscountAmount { get; set; }

    [JsonPropertyName("taxes")]
    public List<TaxDto> Taxes { get; set; } = new();

    [JsonPropertyName("taxAmount")]
    public decimal TaxAmount { get; set; }

    [JsonPropertyName("taxableAmount")]
    public decimal TaxableAmount { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }
}

public class ProductDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("sku")]
    public string Sku { get; set; } = string.Empty;

    [JsonPropertyName("hsn")]
    public string Hsn { get; set; } = string.Empty;
}

public class UnitDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;
}

public class TaxDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public decimal Value { get; set; }

    [JsonPropertyName("taxType")]
    public string TaxType { get; set; } = string.Empty;

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }
}

public class CostDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public decimal Value { get; set; }
}

public class SignatureDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
}

public class TaxSummaryDto
{
    [JsonPropertyName("hsn")]
    public string Hsn { get; set; } = string.Empty;

    [JsonPropertyName("taxableAmount")]
    public decimal TaxableAmount { get; set; }

    [JsonPropertyName("cgstRate")]
    public decimal CgstRate { get; set; }

    [JsonPropertyName("cgstAmount")]
    public decimal CgstAmount { get; set; }

    [JsonPropertyName("sgstRate")]
    public decimal SgstRate { get; set; }

    [JsonPropertyName("sgstAmount")]
    public decimal SgstAmount { get; set; }

    [JsonPropertyName("igstRate")]
    public decimal IgstRate { get; set; }

    [JsonPropertyName("igstAmount")]
    public decimal IgstAmount { get; set; }

    [JsonPropertyName("utgstRate")]
    public decimal UtgstRate { get; set; }

    [JsonPropertyName("utgstAmount")]
    public decimal UtgstAmount { get; set; }

    [JsonPropertyName("cessAmount")]
    public decimal CessAmount { get; set; }

    [JsonPropertyName("totalAmount")]
    public decimal TotalAmount { get; set; }
}
