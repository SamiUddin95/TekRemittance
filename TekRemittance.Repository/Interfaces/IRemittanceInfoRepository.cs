using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Models.dto;

namespace TekRemittance.Repository.Interfaces
{
    public interface IRemittanceInfoRepository
    {
        Task<Guid> CreateUploadAsync(Guid agentId, Guid templateId, string fileName);
        Task UpdateUploadAsync(Guid uploadId, int rowCount, bool success, string? errorMessage);
        Task AddRangeAsync(IEnumerable<RemittanceInfo> rows);
        Task<(IEnumerable<AgentFileUploadDTO> Items, int TotalCount)> GetByUploadAsync(int pageNumber = 1, int pageSize = 50, string? templatename = null, string? filename = null);
  
    }
}
