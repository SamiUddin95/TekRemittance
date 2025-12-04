using System;
using System.Threading.Tasks;
using TekRemittance.Repository.Enums;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Service.Interfaces
{
    public interface IAcquisitionAgentsService
    {
        Task<PagedResult<acquisitionAgentDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? code = null, string? agentname = null, StatusesEnums? status = null);
        Task<acquisitionAgentDTO?> GetByIdAsync(Guid id);
        Task<acquisitionAgentDTO> CreateAsync(acquisitionAgentDTO dto);
        Task<acquisitionAgentDTO?> UpdateAsync(acquisitionAgentDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
