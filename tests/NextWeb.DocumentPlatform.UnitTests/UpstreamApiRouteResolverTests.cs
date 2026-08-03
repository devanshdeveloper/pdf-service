using FluentAssertions;
using NextWeb.DocumentPlatform.Infrastructure;

namespace NextWeb.DocumentPlatform.UnitTests;

public class UpstreamApiRouteResolverTests
{
    [Theory]
    [InlineData("MaterialEntry", "material-entry")]
    [InlineData("material-entry", "material-entry")]
    [InlineData("Voucher", "voucher")]
    [InlineData("voucher", "voucher")]
    [InlineData("StockTransferIn", "stock-transfer-in")]
    [InlineData("StockTransferOut", "stock-transfer-out")]
    [InlineData("voucher/sale", "voucher/sale")]
    public void Resolve_ConvertsDocumentTypeToKebabCase(string input, string expected)
    {
        UpstreamApiRouteResolver.Resolve(input).Should().Be(expected);
    }
}
