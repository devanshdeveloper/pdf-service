using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NextWeb.DocumentPlatform.Application.Models;

public class StockTransferDataModel
{
    [JsonPropertyName("document")]
    public StockTransferDto? Document { get; set; }

    [JsonPropertyName("business")]
    public BusinessDto? Business { get; set; }

    [JsonPropertyName("settings")]
    public StockTransferSettingsDto? Settings { get; set; }
}

public class StockTransferSettingsDto
{
    [JsonPropertyName("pdf_template")]
    public string PdfTemplate { get; set; } = "Standard";
}

public class StockTransferDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("fromLocation")]
    public LocationDto? FromLocation { get; set; }

    [JsonPropertyName("toLocation")]
    public LocationDto? ToLocation { get; set; }

    [JsonPropertyName("items")]
    public List<StockTransferItemDto> Items { get; set; } = new();

    [JsonPropertyName("attachments")]
    public List<string> Attachments { get; set; } = new();

    [JsonPropertyName("requestedBy")]
    public UserRefDto? RequestedBy { get; set; }

    [JsonPropertyName("approvedBy")]
    public UserRefDto? ApprovedBy { get; set; }

    [JsonPropertyName("rejectedBy")]
    public UserRefDto? RejectedBy { get; set; }

    [JsonPropertyName("comments")]
    public List<StockTransferCommentDto> Comments { get; set; } = new();

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("business")]
    public BusinessDto? Business { get; set; }

    [JsonPropertyName("isPublicalyShared")]
    public bool IsPublicalyShared { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}

public class StockTransferItemDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("product")]
    public ProductDto? Product { get; set; }

    [JsonPropertyName("quantity")]
    public decimal Quantity { get; set; }
}

public class StockTransferCommentDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("author")]
    public UserRefDto? Author { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }
}

public class UserRefDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = string.Empty;
}
