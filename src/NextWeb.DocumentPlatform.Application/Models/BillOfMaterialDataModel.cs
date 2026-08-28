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

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

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

    [JsonPropertyName("total_costs")]
    public decimal? TotalCosts { get; set; }

    [JsonPropertyName("cost_per_unit")]
    public decimal? CostPerUnit { get; set; }

    [JsonPropertyName("cost_per_transaction_unit")]
    public decimal? CostPerTransactionUnit { get; set; }

    [JsonPropertyName("total_material_costs")]
    public decimal? TotalMaterialCosts { get; set; }

    [JsonPropertyName("total_operation_costs")]
    public decimal? TotalOperationCosts { get; set; }

    [JsonPropertyName("bom_depth")]
    public int? BomDepth { get; set; }
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
    public BillOfMaterialDto? ChildBom { get; set; }

    [JsonPropertyName("effectiveQuantity")]
    public decimal? EffectiveQuantity { get; set; }

    [JsonPropertyName("transactionQuantityWastage")]
    public decimal? TransactionQuantityWastage { get; set; }

    [JsonPropertyName("effectiveTransactionQuantity")]
    public decimal? EffectiveTransactionQuantity { get; set; }

    [JsonPropertyName("cost_per_unit")]
    public decimal? CostPerUnit { get; set; }

    [JsonPropertyName("cost_per_transaction_unit")]
    public decimal? CostPerTransactionUnit { get; set; }

    [JsonPropertyName("line_item_cost")]
    public decimal? LineItemCost { get; set; }

    [JsonPropertyName("cost_source")]
    public string? CostSource { get; set; }

    [JsonPropertyName("price")]
    public decimal? Price { get; set; }

    [JsonPropertyName("livePrice")]
    public bool? LivePrice { get; set; }
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

    [JsonPropertyName("computedCosts")]
    public BomOperationComputedCostsDto? ComputedCosts { get; set; }
}

public class BomOperationComputedCostsDto
{
    [JsonPropertyName("totalCost")]
    public decimal TotalCost { get; set; }
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
