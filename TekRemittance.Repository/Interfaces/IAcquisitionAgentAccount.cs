using System.Collections.Generic;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Enums;
using TekRemittance.Repository.Models.dto;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Repository.Interfaces
{
    public interface IAcquisitionAgentAccountRepository
    {
       Task<PagedResult<AcquisitionAgentAccountDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? accountnumber = null, string? agentname = null, StatusesEnums? status = null);
       Task<AcquisitionAgentAccountDTO?> GetByIdAsync(Guid id);
       Task<AcquisitionAgentAccountDTO> AddAsync(AcquisitionAgentAccountDTO entity);
       Task<AcquisitionAgentAccountDTO?> UpdateAsync(AcquisitionAgentAccountDTO entity);
       Task<bool> DeleteByIdAsync(Guid id);

    }
}
