using System.Collections.Generic;
using NextWeb.DocumentPlatform.Application.Models;

namespace NextWeb.DocumentPlatform.Renderers.Design;

public static class PdfFormat
{
    public static string Currency(decimal amount, string currencyCode)
    {
        string symbol = currencyCode switch
        {
            "USD" => "$",
            "EUR" => "€",
            "GBP" => "£",
            "INR" => "₹",
            _ => currencyCode + " "
        };
        return $"{symbol}{amount:N2}";
    }

    public static string AddressLines(AddressDto? address)
    {
        if (address == null) return string.Empty;
        if (!string.IsNullOrWhiteSpace(address.FullAddress)) return address.FullAddress;

        var lines = new List<string>();
        var street = $"{address.StreetAddress} {address.Apartment}".Trim();
        if (!string.IsNullOrWhiteSpace(street)) lines.Add(street);

        var cityLine = $"{address.City}, {address.State} {address.PostalCode}".Trim(',', ' ');
        if (!string.IsNullOrWhiteSpace(cityLine)) lines.Add(cityLine);
        if (!string.IsNullOrWhiteSpace(address.Country)) lines.Add(address.Country);

        return string.Join("\n", lines);
    }

    public static string OptionalDate(string text, DateTime? date) =>
        date.HasValue ? $"{text} ({date.Value:dd MMM yyyy})" : text;
}
