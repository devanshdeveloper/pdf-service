namespace NextWeb.DocumentPlatform.Domain;

public class DocumentConfiguration
{
    public bool IsEnabled { get; set; } = true;
    public string SchemaVersion { get; set; } = "1.0";
    public string FallbackTemplate { get; set; } = string.Empty;
}
