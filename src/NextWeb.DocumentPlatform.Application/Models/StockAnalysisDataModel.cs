using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NextWeb.DocumentPlatform.Application.Models;

public class StockAnalysisDataModel
{
    public StockAnalysisDocumentDto Document { get; set; } = new();
    public BusinessDto? Business { get; set; }
    
    [JsonPropertyName("settings")]
    public StockAnalysisSettingsDto? Settings { get; set; }
}

public class StockAnalysisSettingsDto
{
    [JsonPropertyName("pdf_template")]
    public string? PdfTemplate { get; set; }

    [JsonPropertyName("location_name")]
    public string? LocationName { get; set; }

    [JsonPropertyName("category_name")]
    public string? CategoryName { get; set; }
}

public class StockAnalysisDocumentDto
{
    public List<StockAnalysisProductDto> Items { get; set; } = new();
}

public class StockAnalysisProductDto : ProductDto
{
    [JsonPropertyName("stock")]
    public decimal? Stock { get; set; }

    [JsonPropertyName("current_location_actual_stock")]
    public decimal? CurrentLocationActualStock { get; set; }
    
    [JsonPropertyName("current_location_stock")]
    public decimal? CurrentLocationStock { get; set; }
    
    [JsonPropertyName("per_unit_price")]
    public decimal? PerUnitPrice { get; set; }
    
    [JsonPropertyName("total_price")]
    public decimal? TotalPrice { get; set; }
    
    [JsonPropertyName("average_conversion_factor")]
    public decimal? AverageConversionFactor { get; set; }
    
    [JsonPropertyName("per_actual_unit_price")]
    public decimal? PerActualUnitPrice { get; set; }

    [JsonPropertyName("min_unit_price")]
    public decimal? MinUnitPrice { get; set; }

    [JsonPropertyName("max_unit_price")]
    public decimal? MaxUnitPrice { get; set; }

    [JsonPropertyName("stock_value_at_actual")]
    public decimal? StockValueAtActual { get; set; }
}
