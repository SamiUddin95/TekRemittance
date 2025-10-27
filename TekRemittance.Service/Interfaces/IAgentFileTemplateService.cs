using System;
using System.Threading.Tasks;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Service.Interfaces
{
    public interface IAgentFileTemplateService
    {
        Task<agentFileTemplateDTO?> GetByAgentIdAsync(Guid agentId);
        Task<agentFileTemplateDTO> CreateAsync(agentFileTemplateDTO dto);
        Task<agentFileTemplateDTO?> UpdateAsync(agentFileTemplateDTO dto);
        Task<bool> DeleteByAgentIdAsync(Guid agentId);
        Task<PagedResult<agentFileTemplateDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
    }
}
