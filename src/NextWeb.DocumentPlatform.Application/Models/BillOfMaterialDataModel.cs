using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NextWeb.DocumentPlatform.Application.Models;

public class BillOfMaterialDataModel
{
    [JsonPropertyName("document")]
    public BillOfMaterialDto? Document { get; set; }

    [JsonPropertyName("business")]
    public BusinessDto? Business { get; set; }

    [JsonPropertyName("settings")]
    public BillOfMaterialSettingsDto? Settings { get; set; }
}

public class BillOfMaterialDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("number")]
    public string Number { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("toProduce")]
    public ProductDto? ToProduce { get; set; }

    [JsonPropertyName("quantity")]
    public decimal Quantity { get; set; }

    [JsonPropertyName("unit")]
    public UnitDto? Unit { get; set; }

    [JsonPropertyName("transactionQuantity")]
    public decimal? TransactionQuantity { get; set; }

    [JsonPropertyName("transactionUnit")]
    public UnitDto? TransactionUnit { get; set; }

    [JsonPropertyName("components")]
    public List<BomComponentDto> Components { get; set; } = new();

    [JsonPropertyName("operations")]
    public List<BomOperationDto> Operations { get; set; } = new();

    [JsonPropertyName("fields")]
    public List<BomCustomFieldDto> Fields { get; set; } = new();

    [JsonPropertyName("notes")]
    public string? Notes { get; set; }

    [JsonPropertyName("terms")]
    public string? Terms { get; set; }

    [JsonPropertyName("signature")]
    public SignatureDto? Signature { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}

public class BomComponentDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("product")]
    public ProductDto? Product { get; set; }

    [JsonPropertyName("quantity")]
    public decimal Quantity { get; set; }

    [JsonPropertyName("wastagePercent")]
    public decimal WastagePercent { get; set; }

    [JsonPropertyName("unit")]
    public UnitDto? Unit { get; set; }

    [JsonPropertyName("transactionQuantity")]
    public decimal? TransactionQuantity { get; set; }

    [JsonPropertyName("transactionUnit")]
    public UnitDto? TransactionUnit { get; set; }

    [JsonPropertyName("selectedByCondition")]
    public string? SelectedByCondition { get; set; }

    [JsonPropertyName("childBom")]
    public BomChildReferenceDto? ChildBom { get; set; }
}

public class BomChildReferenceDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("number")]
    public string Number { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class BomOperationDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("operation")]
    public ManufacturingOperationDto? Operation { get; set; }

    [JsonPropertyName("blockedByOperations")]
    public List<ManufacturingOperationDto> BlockedByOperations { get; set; } = new();
}

public class ManufacturingOperationDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class BomCustomFieldDto
{
    [JsonPropertyName("label")]
    public string Label { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public JsonElement Value { get; set; }
}

public class BillOfMaterialSettingsDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("prefix")]
    public string Prefix { get; set; } = string.Empty;

    [JsonPropertyName("counter")]
    public int Counter { get; set; }

    [JsonPropertyName("template")]
    public string Template { get; set; } = string.Empty;

    [JsonPropertyName("types")]
    public List<string> Types { get; set; } = new();

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
}
