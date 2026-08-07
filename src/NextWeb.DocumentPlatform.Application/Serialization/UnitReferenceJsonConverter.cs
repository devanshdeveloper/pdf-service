using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using NextWeb.DocumentPlatform.Application.Models;

namespace NextWeb.DocumentPlatform.Application.Serialization;

/// <summary>
/// Mongo refs may be sent as an ObjectId string or a populated Unit document.
/// </summary>
public sealed class UnitReferenceJsonConverter : JsonConverter<UnitDto?>
{
    public override UnitDto? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        if (reader.TokenType == JsonTokenType.String)
        {
            var id = reader.GetString();
            return string.IsNullOrWhiteSpace(id) ? null : new UnitDto { Id = id };
        }

        if (reader.TokenType == JsonTokenType.StartObject)
            return JsonSerializer.Deserialize<UnitDto>(ref reader, options);

        throw new JsonException($"Unexpected token {reader.TokenType} when deserializing UnitDto.");
    }

    public override void Write(Utf8JsonWriter writer, UnitDto? value, JsonSerializerOptions options)
    {
        if (value == null)
            writer.WriteNullValue();
        else
            JsonSerializer.Serialize(writer, value, options);
    }
}
