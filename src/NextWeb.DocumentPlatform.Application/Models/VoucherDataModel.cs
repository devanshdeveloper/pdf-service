using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using NextWeb.DocumentPlatform.Application.Serialization;

namespace NextWeb.DocumentPlatform.Application.Models;

public class VoucherDataModel
{
    [JsonPropertyName("document")]
    public DocumentDto? Document { get; set; }

    [JsonPropertyName("business")]
    public BusinessDto? Business { get; set; }

    [JsonPropertyName("location")]
    public LocationDto? Location { get; set; }

    [JsonPropertyName("settings")]
    public VoucherSettingsDto? Settings { get; set; }
}

public class DocumentDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("number")]
    public string Number { get; set; } = string.Empty;

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonPropertyName("exchangeRate")]
    public decimal ExchangeRate { get; set; }

    [JsonPropertyName("taxType")]
    public string TaxType { get; set; } = string.Empty;

    [JsonPropertyName("placeOfSupplyStateCode")]
    public string PlaceOfSupplyStateCode { get; set; } = string.Empty;

    [JsonPropertyName("sellerStateCode")]
    public string SellerStateCode { get; set; } = string.Empty;

    [JsonPropertyName("user")]
    public string User { get; set; } = string.Empty;

    [JsonPropertyName("business")]
    public BusinessDto? Business { get; set; }

    [JsonPropertyName("location")]
    public LocationDto? Location { get; set; }

    [JsonPropertyName("party")]
    public PartyDto? Party { get; set; }

    [JsonPropertyName("consignee")]
    public PartyDto? Consignee { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("dueDate")]
    public DateTime DueDate { get; set; }

    [JsonPropertyName("referenceNumber")]
    public string? ReferenceNumber { get; set; }

    [JsonPropertyName("referenceDate")]
    public DateTime? ReferenceDate { get; set; }

    [JsonPropertyName("dispatchDocNo")]
    public string? DispatchDocNo { get; set; }

    [JsonPropertyName("deliveryNote")]
    public string? DeliveryNote { get; set; }

    [JsonPropertyName("deliveryNoteDate")]
    public DateTime? DeliveryNoteDate { get; set; }

    [JsonPropertyName("buyersOrderNo")]
    public string? BuyersOrderNo { get; set; }

    [JsonPropertyName("buyersOrderDate")]
    public DateTime? BuyersOrderDate { get; set; }

    [JsonPropertyName("dispatchedThrough")]
    public string? DispatchedThrough { get; set; }

    [JsonPropertyName("destination")]
    public string? Destination { get; set; }

    [JsonPropertyName("termsOfDelivery")]
    public string? TermsOfDelivery { get; set; }

    [JsonPropertyName("paymentMethod")]
    public string PaymentMethod { get; set; } = string.Empty;

    [JsonPropertyName("address")]
    public string Address { get; set; } = string.Empty;

    [JsonPropertyName("products")]
    public List<VoucherProductDto> Products { get; set; } = new();

    [JsonPropertyName("bank")]
    public BankDto? Bank { get; set; }

    [JsonPropertyName("notes")]
    public string? Notes { get; set; }

    [JsonPropertyName("terms")]
    public string? Terms { get; set; }

    [JsonPropertyName("cost")]
    public List<CostDto> Cost { get; set; } = new();

    [JsonPropertyName("discountType")]
    public string DiscountType { get; set; } = string.Empty;

    [JsonPropertyName("discountValue")]
    public decimal DiscountValue { get; set; }

    [JsonPropertyName("couponCode")]
    public string? CouponCode { get; set; }

    [JsonPropertyName("signature")]
    public SignatureDto? Signature { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("paymentStatus")]
    public string PaymentStatus { get; set; } = string.Empty;

    [JsonPropertyName("totalDiscount")]
    public decimal TotalDiscount { get; set; }

    [JsonPropertyName("totalTaxableAmount")]
    public decimal TotalTaxableAmount { get; set; }

    [JsonPropertyName("totalTaxAmount")]
    public decimal TotalTaxAmount { get; set; }

    [JsonPropertyName("totalAmount")]
    public decimal TotalAmount { get; set; }

    [JsonPropertyName("summary")]
    public List<VoucherSummaryDto> Summary { get; set; } = new();

    [JsonPropertyName("amountInWords")]
    public string AmountInWords { get; set; } = string.Empty;

    [JsonPropertyName("totalTaxAmountInWords")]
    public string TotalTaxAmountInWords { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("isPublicalyShared")]
    public bool IsPublicalyShared { get; set; }

    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
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

    [JsonPropertyName("phoneNumbers")]
    public List<string> PhoneNumbers { get; set; } = new();

    [JsonPropertyName("website")]
    public string Website { get; set; } = string.Empty;

    [JsonPropertyName("logo")]
    public string Logo { get; set; } = string.Empty;

    [JsonPropertyName("gst")]
    public string Gst { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("user")]
    public string User { get; set; } = string.Empty;

    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; set; }

    [JsonPropertyName("baseCurrency")]
    public string BaseCurrency { get; set; } = string.Empty;

    [JsonPropertyName("address")]
    public AddressDto? Address { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}

public class LocationDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("business")]
    public string Business { get; set; } = string.Empty;

    [JsonPropertyName("user")]
    public string User { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
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

    [JsonPropertyName("user")]
    public string User { get; set; } = string.Empty;

    [JsonPropertyName("business")]
    public string Business { get; set; } = string.Empty;

    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}

public class DefaultsDto
{
    [JsonPropertyName("terms")]
    public string Terms { get; set; } = string.Empty;

    [JsonPropertyName("notes")]
    public string Notes { get; set; } = string.Empty;

    [JsonPropertyName("bank")]
    public string Bank { get; set; } = string.Empty;

    [JsonPropertyName("signature")]
    public string Signature { get; set; } = string.Empty;
}

public class PartyDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("businessType")]
    public string BusinessType { get; set; } = string.Empty;

    [JsonPropertyName("partyType")]
    public string PartyType { get; set; } = string.Empty;

    [JsonPropertyName("avatar")]
    public string Avatar { get; set; } = string.Empty;

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

    [JsonPropertyName("fields")]
    public List<LabelValueDto> Fields { get; set; } = new();

    [JsonPropertyName("documents")]
    public List<PartyDocumentDto> Documents { get; set; } = new();

    [JsonPropertyName("business")]
    public string Business { get; set; } = string.Empty;

    [JsonPropertyName("user")]
    public string User { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}

public class PartyDocumentDto
{
    [JsonPropertyName("label")]
    public string Label { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;

    [JsonPropertyName("identifier")]
    public string Identifier { get; set; } = string.Empty;
}

public class AddressDto
{
    [JsonPropertyName("streetAddress")]
    public string StreetAddress { get; set; } = string.Empty;

    [JsonPropertyName("apartment")]
    public string Apartment { get; set; } = string.Empty;

    [JsonPropertyName("city")]
    public string City { get; set; } = string.Empty;

    [JsonPropertyName("district")]
    public string District { get; set; } = string.Empty;

    [JsonPropertyName("state")]
    public string State { get; set; } = string.Empty;

    [JsonPropertyName("stateCode")]
    public string StateCode { get; set; } = string.Empty;

    [JsonPropertyName("postalCode")]
    public string PostalCode { get; set; } = string.Empty;

    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;

    [JsonPropertyName("fullAddress")]
    public string FullAddress { get; set; } = string.Empty;

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}

public class BankDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("bankName")]
    public string BankName { get; set; } = string.Empty;

    [JsonPropertyName("branch")]
    public string Branch { get; set; } = string.Empty;

    [JsonPropertyName("branchAddress")]
    public string BranchAddress { get; set; } = string.Empty;

    [JsonPropertyName("accountType")]
    public string AccountType { get; set; } = string.Empty;

    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("ifsc")]
    public string Ifsc { get; set; } = string.Empty;

    [JsonPropertyName("paymentPrefrence")]
    public string PaymentPrefrence { get; set; } = string.Empty;

    [JsonPropertyName("balance")]
    public decimal Balance { get; set; }

    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; set; }

    [JsonPropertyName("user")]
    public string User { get; set; } = string.Empty;

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}

public class VoucherProductDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("product")]
    public ProductDto? Product { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("hsn")]
    public string Hsn { get; set; } = string.Empty;

    [JsonPropertyName("quantity")]
    public decimal Quantity { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("unit")]
    public UnitDto? Unit { get; set; }

    [JsonPropertyName("transactionQuantity")]
    public decimal? TransactionQuantity { get; set; }

    [JsonPropertyName("transactionUnit")]
    public UnitDto? TransactionUnit { get; set; }

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

    [JsonPropertyName("baseAmount")]
    public decimal BaseAmount { get; set; }

    [JsonPropertyName("computedTaxes")]
    public List<ComputedTaxDto> ComputedTaxes { get; set; } = new();
}

public class ProductDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("images")]
    public List<string> Images { get; set; } = new();

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("slug")]
    public string Slug { get; set; } = string.Empty;

    [JsonPropertyName("categories")]
    public List<string> Categories { get; set; } = new();

    [JsonPropertyName("sku")]
    public string Sku { get; set; } = string.Empty;

    [JsonPropertyName("hsn")]
    public string Hsn { get; set; } = string.Empty;

    [JsonPropertyName("unit")]
    [JsonConverter(typeof(UnitReferenceJsonConverter))]
    public UnitDto? Unit { get; set; }

    [JsonPropertyName("transactionUnit")]
    [JsonConverter(typeof(UnitReferenceJsonConverter))]
    public UnitDto? TransactionUnit { get; set; }

    [JsonPropertyName("featured")]
    public bool Featured { get; set; }

    [JsonPropertyName("productType")]
    public string ProductType { get; set; } = string.Empty;

    [JsonPropertyName("taxes")]
    public List<TaxDto> Taxes { get; set; } = new();

    [JsonPropertyName("fields")]
    public List<LabelValueDto> Fields { get; set; } = new();

    [JsonPropertyName("discountType")]
    public string DiscountType { get; set; } = string.Empty;

    [JsonPropertyName("hidden")]
    public bool Hidden { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("business")]
    public string Business { get; set; } = string.Empty;

    [JsonPropertyName("user")]
    public string User { get; set; } = string.Empty;

    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; set; }

    [JsonPropertyName("prices")]
    public List<ProductPriceDto> Prices { get; set; } = new();

    [JsonPropertyName("variants")]
    public List<ProductVariantDto> Variants { get; set; } = new();

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}

public class ProductPriceDto
{
    [JsonPropertyName("min")]
    public decimal Min { get; set; }

    [JsonPropertyName("max")]
    public decimal Max { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }
}

public class ProductVariantDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;

    [JsonPropertyName("product")]
    public string Product { get; set; } = string.Empty;
}

public class UnitDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("user")]
    public string User { get; set; } = string.Empty;

    [JsonPropertyName("business")]
    public string Business { get; set; } = string.Empty;

    [JsonPropertyName("isPredefined")]
    public bool IsPredefined { get; set; }

    [JsonPropertyName("conversionFactor")]
    public decimal ConversionFactor { get; set; }

    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}

public class TaxDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("taxId")]
    public string TaxId { get; set; } = string.Empty;

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

public class ComputedTaxDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public decimal Value { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("taxId")]
    public string TaxId { get; set; } = string.Empty;
}

public class CostDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

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

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("user")]
    public string User { get; set; } = string.Empty;

    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}

public class VoucherSummaryDto
{
    [JsonPropertyName("hsn")]
    public string Hsn { get; set; } = string.Empty;

    [JsonPropertyName("taxableValue")]
    public decimal TaxableValue { get; set; }

    [JsonPropertyName("totalTaxAmount")]
    public decimal TotalTaxAmount { get; set; }

    [JsonPropertyName("computedTaxes")]
    public List<ComputedTaxDto> ComputedTaxes { get; set; } = new();
}

public class LabelValueDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("label")]
    public string Label { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;
}
