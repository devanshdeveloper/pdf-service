namespace NextWeb.DocumentPlatform.Domain;

public class DocumentMetadata
{
    public string DocumentType { get; set; } = string.Empty;
    public string TemplateName { get; set; } = string.Empty;
    public DocumentConfiguration Configuration { get; set; } = new();
}
