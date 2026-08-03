using System;
using System.Linq;

namespace NextWeb.DocumentPlatform.Infrastructure;

public static class DocumentRegistryKeyNormalizer
{
    public static string NormalizeDocumentType(string documentType)
    {
        if (string.IsNullOrWhiteSpace(documentType))
            return documentType;

        if (documentType.StartsWith("voucher/", StringComparison.OrdinalIgnoreCase))
            return "Voucher";

        return ToPascalCase(documentType);
    }

    public static string NormalizeTemplateName(string templateName)
    {
        if (string.IsNullOrWhiteSpace(templateName))
            return templateName;

        return ToPascalCase(templateName);
    }

    private static string ToPascalCase(string value)
    {
        var parts = value.Split('-', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
            return value;

        return string.Concat(parts.Select(part =>
        {
            if (part.Length == 0) return string.Empty;
            return char.ToUpperInvariant(part[0]) + part[1..];
        }));
    }
}
