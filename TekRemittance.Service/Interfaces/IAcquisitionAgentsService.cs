using System;
using System.Threading.Tasks;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Service.Interfaces
{
    public interface IAcquisitionAgentsService
    {
        Task<PagedResult<acquisitionAgentDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
        Task<acquisitionAgentDTO?> GetByIdAsync(Guid id);
        Task<acquisitionAgentDTO> CreateAsync(acquisitionAgentDTO dto);
        Task<acquisitionAgentDTO?> UpdateAsync(acquisitionAgentDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
