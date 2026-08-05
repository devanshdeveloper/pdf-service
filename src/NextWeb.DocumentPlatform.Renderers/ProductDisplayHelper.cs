using System.Collections.Generic;
using System.Linq;
using NextWeb.DocumentPlatform.Application.Models;

namespace NextWeb.DocumentPlatform.Renderers;

public static class ProductDisplayHelper
{
    public static string FormatLineItemName(string? lineName, ProductDto? product)
    {
        var baseName = !string.IsNullOrWhiteSpace(lineName)
            ? lineName
            : product?.Name ?? string.Empty;

        var fieldSuffix = FormatProductFields(product?.Fields);
        if (string.IsNullOrWhiteSpace(fieldSuffix))
            return baseName;

        return string.IsNullOrWhiteSpace(baseName)
            ? fieldSuffix
            : $"{baseName} ({fieldSuffix})";
    }

    public static string FormatProductFields(IEnumerable<LabelValueDto>? fields)
    {
        if (fields == null)
            return string.Empty;

        var parts = fields
            .Where(f => !string.IsNullOrWhiteSpace(f.Value))
            .Select(f => $"{f.Label}: {f.Value}")
            .ToList();

        return string.Join(", ", parts);
    }
}
