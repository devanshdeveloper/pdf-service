using FluentAssertions;
using NextWeb.DocumentPlatform.Application.Models;

namespace NextWeb.DocumentPlatform.UnitTests;

public class VoucherTypesTests
{
    [Theory]
    [InlineData("Cash Sale")]
    [InlineData("Cash Purchase")]
    [InlineData("Cash Sale Return")]
    [InlineData("cash purchase order")]
    public void IsCashVoucher_MatchesCashTypes(string type)
    {
        VoucherTypes.IsCashVoucher(type).Should().BeTrue();
        VoucherTypes.ShouldShowTaxes(type).Should().BeFalse();
    }

    [Fact]
    public void SaleVoucher_ShowsShippingAndPaymentStatus()
    {
        VoucherTypes.ShouldShowShipping(VoucherTypes.Sale).Should().BeTrue();
        VoucherTypes.ShouldShowPaymentStatus(VoucherTypes.Sale).Should().BeTrue();
        VoucherTypes.ShouldShowTaxes(VoucherTypes.Sale).Should().BeTrue();
    }

    [Fact]
    public void PurchaseVoucher_ShowsActualQuantityColumns()
    {
        VoucherTypes.ShouldShowActualQuantity(VoucherTypes.Purchase).Should().BeTrue();
        VoucherTypes.ShouldShowActualQuantity(VoucherTypes.PurchaseOrder).Should().BeTrue();
        VoucherTypes.ShouldShowActualQuantity(VoucherTypes.CashPurchaseReturn).Should().BeTrue();
        VoucherTypes.ShouldShowActualQuantity(VoucherTypes.Sale).Should().BeFalse();
    }

    [Fact]
    public void QuotationVoucher_HidesShippingAndPaymentStatus()
    {
        VoucherTypes.ShouldShowShipping(VoucherTypes.Quotation).Should().BeFalse();
        VoucherTypes.ShouldShowPaymentStatus(VoucherTypes.Quotation).Should().BeFalse();
        VoucherTypes.ShouldShowTaxes(VoucherTypes.Quotation).Should().BeTrue();
    }

    [Fact]
    public void ShippingAndPaymentStatus_UseSeparatePredicates()
    {
        // Predicates are independent even when membership sets overlap today.
        typeof(VoucherTypes).GetMethod(nameof(VoucherTypes.ShouldShowShipping))
            .Should().NotBeNull();
        typeof(VoucherTypes).GetMethod(nameof(VoucherTypes.ShouldShowPaymentStatus))
            .Should().NotBeNull();
        VoucherTypes.ShouldShowShipping(VoucherTypes.SaleReturn).Should().BeTrue();
        VoucherTypes.ShouldShowPaymentStatus(VoucherTypes.SaleReturn).Should().BeTrue();
    }
}
