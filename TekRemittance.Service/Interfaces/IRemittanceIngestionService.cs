using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TekRemittance.Service.Interfaces
{
    public interface IRemittanceIngestionService
    {
        Task<(Guid UploadId, int RowCount)> IngestAsync(Guid agentId, Guid? templateId, IFormFile file, bool hasHeader);
    }
}
