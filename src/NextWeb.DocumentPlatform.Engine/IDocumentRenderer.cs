using System.Threading;
using System.Threading.Tasks;
using NextWeb.DocumentPlatform.Domain;

namespace NextWeb.DocumentPlatform.Engine;

public interface IDocumentRenderer
{
    string DocumentType { get; } 
    string TemplateName { get; }
    
    Task<byte[]> RenderAsync(string jsonPayload, DocumentConfiguration config, CancellationToken cancellationToken);
}
