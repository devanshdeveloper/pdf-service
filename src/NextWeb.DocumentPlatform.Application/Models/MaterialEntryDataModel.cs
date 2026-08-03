using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NextWeb.DocumentPlatform.Application.Models;

public class MaterialEntryDataModel
{
    [JsonPropertyName("document")]
    public MaterialEntryDto? Document { get; set; }

    [JsonPropertyName("business")]
    public BusinessDto? Business { get; set; }

    [JsonPropertyName("location")]
    public LocationDto? Location { get; set; }

    [JsonPropertyName("settings")]
    public MaterialEntrySettingsDto? Settings { get; set; }
}

public class MaterialEntryDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("number")]
    public string Number { get; set; } = string.Empty;

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("referenceNumber")]
    public string? ReferenceNumber { get; set; }

    [JsonPropertyName("location")]
    public LocationDto? Location { get; set; }

    [JsonPropertyName("products")]
    public List<MaterialEntryProductDto> Products { get; set; } = new();

    [JsonPropertyName("notes")]
    public string? Notes { get; set; }

    [JsonPropertyName("terms")]
    public string? Terms { get; set; }

    [JsonPropertyName("signature")]
    public SignatureDto? Signature { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
}

public class MaterialEntryProductDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("product")]
    public ProductDto? Product { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("quantity")]
    public decimal Quantity { get; set; }

    [JsonPropertyName("unit")]
    public UnitDto? Unit { get; set; }
}

public class MaterialEntrySettingsDto
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

    [JsonPropertyName("defaults")]
    public MaterialEntryDefaultsDto? Defaults { get; set; }
}

public class MaterialEntryDefaultsDto
{
    [JsonPropertyName("terms")]
    public string Terms { get; set; } = string.Empty;

    [JsonPropertyName("notes")]
    public string Notes { get; set; } = string.Empty;

    [JsonPropertyName("signature")]
    public string Signature { get; set; } = string.Empty;
}
