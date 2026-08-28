using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using NextWeb.DocumentPlatform.Application.Models;

namespace NextWeb.DocumentPlatform.Application.Serialization;

/// <summary>
/// Handles deserialization of an array of categories, where each element can be either an ObjectId string or a populated CategoryDto object.
/// </summary>
public sealed class CategoryListJsonConverter : JsonConverter<List<CategoryDto>>
{
    public override List<CategoryDto> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var list = new List<CategoryDto>();
        if (reader.TokenType != JsonTokenType.StartArray)
            return list;
            
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var id = reader.GetString();
                if (!string.IsNullOrWhiteSpace(id))
                {
                    list.Add(new CategoryDto { Id = id });
                }
            }
            else if (reader.TokenType == JsonTokenType.StartObject)
            {
                var cat = JsonSerializer.Deserialize<CategoryDto>(ref reader, options);
                if (cat != null)
                {
                    list.Add(cat);
                }
            }
            else
            {
                reader.Skip();
            }
        }
        return list;
    }

    public override void Write(Utf8JsonWriter writer, List<CategoryDto> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
