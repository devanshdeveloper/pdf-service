using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NextWeb.DocumentPlatform.Application.Models;

public class StockAnalysisDataModel
{
    public StockAnalysisDocumentDto Document { get; set; } = new();
    public BusinessDto? Business { get; set; }
}

public class StockAnalysisDocumentDto
{
    public List<StockAnalysisProductDto> Items { get; set; } = new();
}

public class StockAnalysisProductDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ProductType { get; set; } = string.Empty;
    
    // We map nested fields to generic objects or dynamic to allow flexibility,
    // or strongly type them if we know the structure.
    public object? Unit { get; set; }
    
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
}
