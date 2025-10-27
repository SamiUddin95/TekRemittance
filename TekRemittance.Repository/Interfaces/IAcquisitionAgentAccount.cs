using System.Collections.Generic;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Models.dto;

namespace TekRemittance.Repository.Interfaces
{
    public interface IAcquisitionAgentAccountRepository
    {
       Task <IEnumerable<AcquisitionAgentAccountDTO>> GetAllAsync();
       Task <AcquisitionAgentAccountDTO?> GetByNameAsync(string agentAccountName);
       Task<AcquisitionAgentAccountDTO> AddAsync(AcquisitionAgentAccountDTO entity);
       Task<AcquisitionAgentAccountDTO?> UpdateAsync(AcquisitionAgentAccountDTO entity);
       Task<bool> DeleteAsync(string agentAccountName);
       Task<AcquisitionAgentAccountDTO> Save(AcquisitionAgentAccountDTO entity);
       Task CreateAccount(AcquisitionAgentAccount entity);
    }
}
