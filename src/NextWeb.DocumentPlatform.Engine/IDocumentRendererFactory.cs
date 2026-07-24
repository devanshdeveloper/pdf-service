namespace NextWeb.DocumentPlatform.Engine;

public interface IDocumentRendererFactory
{
    IDocumentRenderer GetRenderer(string documentType, string templateName);
}
