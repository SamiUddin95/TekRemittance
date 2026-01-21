using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TekRemittance.Service.Interfaces
{
    public interface ISsrsRenderService
    {
        Task<(byte[] Content, string ContentType, string FileExtension)> RenderAsync(
            string reportPath,
            string format,
            IDictionary<string, string>? parameters,
            CancellationToken cancellationToken = default);
    }
}
