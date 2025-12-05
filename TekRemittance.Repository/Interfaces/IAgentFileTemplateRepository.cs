using System;
using System.Threading.Tasks;
using TekRemittance.Repository.Models.dto;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Repository.Interfaces
{
    public interface IAgentFileTemplateRepository
    {
        Task<agentFileTemplateDTO?> GetByAgentIdAsync(Guid agentId);
        Task<agentFileTemplateDTO> CreateAsync(agentFileTemplateDTO dto);
        Task<agentFileTemplateDTO?> UpdateAsync(agentFileTemplateDTO dto);
        Task<bool> DeleteByAgentIdAsync(Guid agentId);
        Task<PagedResult<agentFileTemplateDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? name = null, string? agentname = null, string? sheetname = null);
        Task<PagedResult<KeyValuePair<string, List<string>>>> GetDataByUploadIdAsync(Guid UploadId, int pageNumber = 1, int pageSize = 50);

    }
}
