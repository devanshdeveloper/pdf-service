namespace NextWeb.DocumentPlatform.Application.Models;

/// <summary>
/// Voucher type string constants aligned with edu-manage-be voucher.js.
/// </summary>
public static class VoucherTypes
{
    public const string Voucher = "Voucher";
    public const string PurchaseOrder = "Purchase Order";
    public const string Purchase = "Purchase";
    public const string PurchaseReturn = "Purchase Return";
    public const string CashPurchase = "Cash Purchase";
    public const string Quotation = "Quotation";
    public const string Sale = "Sale";
    public const string SaleReturn = "Sale Return";
    public const string CashSale = "Cash Sale";
    public const string CashPurchaseReturn = "Cash Purchase Return";
    public const string CashSaleReturn = "Cash Sale Return";
    public const string CashPurchaseOrder = "Cash Purchase Order";

    public static bool IsCashVoucher(string? type) =>
        !string.IsNullOrWhiteSpace(type) &&
        type.Contains("Cash", StringComparison.OrdinalIgnoreCase);

    public static bool ShouldShowTaxes(string? type) => !IsCashVoucher(type);

    public static bool ShouldShowShipping(string? type) =>
        type is Sale or SaleReturn or Purchase or PurchaseReturn;

    public static bool ShouldShowPaymentStatus(string? type) =>
        type is Sale or Purchase or PurchaseReturn or SaleReturn;

    public static bool ShouldShowActualQuantity(string? type) =>
        type is PurchaseOrder
            or Purchase
            or PurchaseReturn
            or CashPurchase
            or CashPurchaseReturn
            or CashPurchaseOrder;
}
