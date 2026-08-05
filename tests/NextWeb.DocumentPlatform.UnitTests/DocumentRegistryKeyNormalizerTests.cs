using FluentAssertions;
using NextWeb.DocumentPlatform.Infrastructure;

namespace NextWeb.DocumentPlatform.UnitTests;

public class DocumentRegistryKeyNormalizerTests
{
    [Theory]
    [InlineData("stock-transfer-in", "StockTransferIn")]
    [InlineData("stock-transfer-out", "StockTransferOut")]
    [InlineData("bill-of-material", "BillOfMaterial")]
    [InlineData("material-entry", "MaterialEntry")]
    [InlineData("StockTransferIn", "StockTransferIn")]
    public void NormalizeDocumentType_ResolvesKebabAndPascalCase(string input, string expected)
    {
        DocumentRegistryKeyNormalizer.NormalizeDocumentType(input).Should().Be(expected);
    }
}
