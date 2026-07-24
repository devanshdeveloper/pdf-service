using NextWeb.DocumentPlatform.Domain;

namespace NextWeb.DocumentPlatform.Engine;

public interface IDocumentRegistry
{
    DocumentMetadata GetMetadata(string documentType, string templateName);
    void Register(DocumentMetadata metadata);
}
