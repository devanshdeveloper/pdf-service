using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NextWeb.DocumentPlatform.Application.Models;

public class WorkCenterTimingDataModel
{
    [JsonPropertyName("document")]
    public WorkCenterTimingDto? Document { get; set; }

    [JsonPropertyName("business")]
    public BusinessDto? Business { get; set; }

    [JsonPropertyName("settings")]
    public VoucherSettingsDto? Settings { get; set; }
}

public class WorkCenterTimingDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("timezone")]
    public string Timezone { get; set; } = string.Empty;

    [JsonPropertyName("configuration")]
    public string Configuration { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("standardHours")]
    public WorkCenterTimingStandardHoursDto? StandardHours { get; set; }

    [JsonPropertyName("workingDays")]
    public Dictionary<string, WorkCenterTimingDayDto>? WorkingDays { get; set; }

    [JsonPropertyName("breaks")]
    public List<WorkCenterTimingBreakDto> Breaks { get; set; } = new();

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }
}

public class WorkCenterTimingStandardHoursDto
{
    [JsonPropertyName("from")]
    public string From { get; set; } = string.Empty;

    [JsonPropertyName("to")]
    public string To { get; set; } = string.Empty;
}

public class WorkCenterTimingDayDto
{
    [JsonPropertyName("isWorkingDay")]
    public bool IsWorkingDay { get; set; }

    [JsonPropertyName("from")]
    public string? From { get; set; }

    [JsonPropertyName("to")]
    public string? To { get; set; }
}

public class WorkCenterTimingBreakDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("from")]
    public string From { get; set; } = string.Empty;

    [JsonPropertyName("to")]
    public string To { get; set; } = string.Empty;

    [JsonPropertyName("isPaid")]
    public bool IsPaid { get; set; }
}
