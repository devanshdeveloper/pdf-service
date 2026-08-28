using System.Text.Json.Serialization;

namespace NextWeb.DocumentPlatform.Application.Models;

public class CategoryDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}
