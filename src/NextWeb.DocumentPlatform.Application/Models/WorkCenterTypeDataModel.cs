using System;
using System.Text.Json.Serialization;

namespace NextWeb.DocumentPlatform.Application.Models;

public class WorkCenterTypeDataModel
{
    [JsonPropertyName("document")]
    public WorkCenterTypeDto? Document { get; set; }

    [JsonPropertyName("business")]
    public BusinessDto? Business { get; set; }

    [JsonPropertyName("settings")]
    public VoucherSettingsDto? Settings { get; set; }
}

public class WorkCenterTypeDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("costTemplate")]
    public CostTemplateDto? CostTemplate { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }
}
