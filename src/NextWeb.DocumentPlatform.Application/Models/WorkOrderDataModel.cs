using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NextWeb.DocumentPlatform.Application.Models;

public class WorkOrderDataModel
{
    [JsonPropertyName("document")]
    public WorkOrderDto? Document { get; set; }

    [JsonPropertyName("business")]
    public BusinessDto? Business { get; set; }

    [JsonPropertyName("settings")]
    public WorkOrderSettingsDto? Settings { get; set; }
}

public class WorkOrderDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("number")]
    public string Number { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("quantity")]
    public decimal Quantity { get; set; }

    [JsonPropertyName("unit")]
    public UnitDto? Unit { get; set; }

    [JsonPropertyName("toProduce")]
    public ProductDto? ToProduce { get; set; }

    [JsonPropertyName("transactionQuantity")]
    public decimal? TransactionQuantity { get; set; }

    [JsonPropertyName("transactionUnit")]
    public UnitDto? TransactionUnit { get; set; }

    [JsonPropertyName("fields")]
    public List<WorkOrderCustomFieldDto> Fields { get; set; } = new();

    [JsonPropertyName("components")]
    public List<WorkOrderComponentDto> Components { get; set; } = new();

    [JsonPropertyName("frozenCostBreakdown")]
    public List<WorkOrderFrozenCostDto> FrozenCostBreakdown { get; set; } = new();

    [JsonPropertyName("frozenTotalCost")]
    public decimal FrozenTotalCost { get; set; }

    [JsonPropertyName("operations")]
    public List<WorkOrderOperationDto> Operations { get; set; } = new();

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("notes")]
    public string Notes { get; set; } = string.Empty;

    [JsonPropertyName("terms")]
    public string Terms { get; set; } = string.Empty;

    [JsonPropertyName("signature")]
    public SignatureDto? Signature { get; set; }
}

public class WorkOrderComponentDto
{
    [JsonPropertyName("product")]
    public ProductDto? Product { get; set; }

    [JsonPropertyName("quantity")]
    public decimal Quantity { get; set; }

    [JsonPropertyName("unit")]
    public UnitDto? Unit { get; set; }

    [JsonPropertyName("transactionQuantity")]
    public decimal? TransactionQuantity { get; set; }

    [JsonPropertyName("transactionUnit")]
    public UnitDto? TransactionUnit { get; set; }
}

public class WorkOrderFrozenCostDto
{
    [JsonPropertyName("componentProduct")]
    public ProductDto? ComponentProduct { get; set; }

    [JsonPropertyName("quantityUsed")]
    public decimal QuantityUsed { get; set; }

    [JsonPropertyName("unitCost")]
    public decimal UnitCost { get; set; }

    [JsonPropertyName("totalCost")]
    public decimal TotalCost { get; set; }

    [JsonPropertyName("unit")]
    public UnitDto? Unit { get; set; }
}

public class WorkOrderOperationDto
{
    [JsonPropertyName("operation")]
    public string Operation { get; set; } = string.Empty;

    [JsonPropertyName("blockedByOperations")]
    public List<string> BlockedByOperations { get; set; } = new();
}

public class WorkOrderSettingsDto
{
    [JsonPropertyName("prefix")]
    public string Prefix { get; set; } = string.Empty;

    [JsonPropertyName("counter")]
    public int Counter { get; set; }

    [JsonPropertyName("template")]
    public string Template { get; set; } = string.Empty;

    [JsonPropertyName("pdf_template")]
    public string PdfTemplate { get; set; } = "Standard";

    [JsonPropertyName("types")]
    public List<string> Types { get; set; } = new();
}

public class WorkOrderCustomFieldDto
{
    [JsonPropertyName("label")]
    public string Label { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public System.Text.Json.JsonElement Value { get; set; }
}
