using System.Threading;
using System.Threading.Tasks;

namespace NextWeb.DocumentPlatform.Application;

public interface IErpForwardingService
{
    Task<string> GetDocumentDataAsync(HttpRequestContext requestContext, CancellationToken cancellationToken);
}
