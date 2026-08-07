using System.Collections.Generic;
using System.Text.Json.Serialization;
using NextWeb.DocumentPlatform.Application.Models;

namespace NextWeb.DocumentPlatform.Application.Models;

/// <summary>
/// Print envelope for generic reports (mirrors voucher { document, business, settings }).
/// </summary>
public class ReportDataModel
{
    [JsonPropertyName("document")]
    public ReportDocumentDto? Document { get; set; }

    [JsonPropertyName("business")]
    public BusinessDto? Business { get; set; }

    [JsonPropertyName("settings")]
    public ReportSettingsDto? Settings { get; set; }
}

public class ReportSettingsDto
{
    [JsonPropertyName("pdf_template")]
    public string PdfTemplate { get; set; } = "Standard";
}

public class ReportDocumentDto
{
    [JsonPropertyName("reportType")]
    public string ReportType { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("generatedAt")]
    public string GeneratedAt { get; set; } = string.Empty;

    [JsonPropertyName("filters")]
    public Dictionary<string, object?> Filters { get; set; } = new();

    [JsonPropertyName("columns")]
    public List<ReportColumnDto> Columns { get; set; } = new();

    [JsonPropertyName("rows")]
    public List<Dictionary<string, object?>> Rows { get; set; } = new();

    [JsonPropertyName("summary")]
    public ReportSummaryDto? Summary { get; set; }
}

public class ReportColumnDto
{
    [JsonPropertyName("key")]
    public string Key { get; set; } = string.Empty;

    [JsonPropertyName("header")]
    public string Header { get; set; } = string.Empty;
}

public class ReportSummaryDto
{
    [JsonPropertyName("totalRecords")]
    public int TotalRecords { get; set; }

    [JsonPropertyName("presentCount")]
    public int PresentCount { get; set; }

    [JsonPropertyName("absentCount")]
    public int AbsentCount { get; set; }

    [JsonPropertyName("lateCount")]
    public int LateCount { get; set; }

    [JsonPropertyName("excusedCount")]
    public int ExcusedCount { get; set; }

    [JsonPropertyName("attendanceRate")]
    public double AttendanceRate { get; set; }
}
