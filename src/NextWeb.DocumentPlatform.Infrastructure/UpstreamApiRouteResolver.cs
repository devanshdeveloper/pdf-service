using System;
using System.Text;

namespace NextWeb.DocumentPlatform.Infrastructure;

public static class UpstreamApiRouteResolver
{
    public static string Resolve(string routeDocumentType)
    {
        if (string.IsNullOrWhiteSpace(routeDocumentType))
            return routeDocumentType;

        var slashIndex = routeDocumentType.IndexOf('/');
        if (slashIndex > 0)
        {
            var prefix = routeDocumentType[..slashIndex];
            var suffix = routeDocumentType[(slashIndex + 1)..];
            return $"{ToKebabCase(prefix)}/{suffix}";
        }

        return ToKebabCase(routeDocumentType);
    }

    private static string ToKebabCase(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return value;

        if (value.Contains('-', StringComparison.Ordinal))
            return value.ToLowerInvariant();

        var result = new StringBuilder(value.Length + 4);
        for (var i = 0; i < value.Length; i++)
        {
            var c = value[i];
            if (char.IsUpper(c) && i > 0)
                result.Append('-');

            result.Append(char.ToLowerInvariant(c));
        }

        return result.ToString();
    }
}
