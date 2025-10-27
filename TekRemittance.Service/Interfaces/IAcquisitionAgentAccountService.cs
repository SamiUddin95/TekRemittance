using System.Collections.Generic;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Models.dto;
using TekRemittance.Web.Models.dto;
namespace TekRemittance.Service.Interfaces
{
    public interface IAcquisitionAgentAccountService
    {
       Task<PagedResult<AcquisitionAgentAccountDTO>> GetAllAccounts(int pageNumber = 1, int pageSize = 10);
       Task<AcquisitionAgentAccountDTO?> GetAccountById(Guid id);
       Task<AcquisitionAgentAccountDTO> CreateAccount(AcquisitionAgentAccountDTO account);
       Task<AcquisitionAgentAccountDTO?> UpdateAccountById(AcquisitionAgentAccountDTO entity);
       Task<bool> DeleteAccountById(Guid id);

    }
}
