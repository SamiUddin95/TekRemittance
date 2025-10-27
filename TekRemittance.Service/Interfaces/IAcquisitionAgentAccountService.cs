using System.Collections.Generic;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Models.dto;
using TekRemittance.Web.Models.dto;
namespace TekRemittance.Service.Interfaces
{
    public interface IAcquisitionAgentAccountService
    {
       Task <IEnumerable<AcquisitionAgentAccountDTO>> GetAllAccounts();
       Task<AcquisitionAgentAccountDTO?> GetAccountByName(string agentAccountName);
       Task<AcquisitionAgentAccountDTO> CreateAccount(AcquisitionAgentAccountDTO account);
       Task<AcquisitionAgentAccountDTO> UpdateAccount(AcquisitionAgentAccountDTO account);
        Task<bool> DeleteAccount(string agentAccountName);
        Task SaveChangesAsync();
    }
}
