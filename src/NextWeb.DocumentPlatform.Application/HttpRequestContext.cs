using System.Collections.Generic;

namespace NextWeb.DocumentPlatform.Application;

public class HttpRequestContext
{
    public string RouteDocumentType { get; set; } = string.Empty;
    public string RouteTemplateName { get; set; } = string.Empty;
    
    // Auth and forwarding details
    public Dictionary<string, string> Headers { get; set; } = new();
    public Dictionary<string, string> Cookies { get; set; } = new();
    public string QueryString { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}
