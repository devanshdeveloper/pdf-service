using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NextWeb.DocumentPlatform.Application.Models;

public class WorkCenterDataModel
{
    [JsonPropertyName("document")]
    public WorkCenterDto? Document { get; set; }

    [JsonPropertyName("business")]
    public BusinessDto? Business { get; set; }

    [JsonPropertyName("settings")]
    public VoucherSettingsDto? Settings { get; set; }
}

public class WorkCenterDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("workCenterType")]
    public WorkCenterTypeDto? WorkCenterType { get; set; }

    [JsonPropertyName("costTemplate")]
    public CostTemplateDto? CostTemplate { get; set; }

    [JsonPropertyName("workCenterTiming")]
    public WorkCenterTimingDto? WorkCenterTiming { get; set; }

    [JsonPropertyName("mapItems")]
    public List<WorkCenterMapItemDto> MapItems { get; set; } = new();

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }
}

public class WorkCenterMapItemDto
{
    [JsonPropertyName("product")]
    public WorkCenterMapItemProductDto? Product { get; set; }

    [JsonPropertyName("appliesToAllItems")]
    public bool AppliesToAllItems { get; set; }

    [JsonPropertyName("setupTimeSeconds")]
    public int SetupTimeSeconds { get; set; }

    [JsonPropertyName("cleanupTimeSeconds")]
    public int CleanupTimeSeconds { get; set; }

    [JsonPropertyName("capacity")]
    public int Capacity { get; set; }
}

public class WorkCenterMapItemProductDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}
