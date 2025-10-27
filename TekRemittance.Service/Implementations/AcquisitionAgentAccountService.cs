using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Repository.Models.dto;
using TekRemittance.Service.Interfaces;
using TekRemittance.Repository.Entities.Data;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Service.Services
{
    public class AcquisitionAgentAccountService : IAcquisitionAgentAccountService
    {
        private readonly IAcquisitionAgentAccountRepository _repository;
        

        public AcquisitionAgentAccountService(IAcquisitionAgentAccountRepository repository)
        {
            _repository = repository;
        }
        public async Task<PagedResult<AcquisitionAgentAccountDTO>> GetAllAccounts(int pageNumber = 1, int pageSize = 10)
        {
            return await _repository.GetAllAsync(pageNumber, pageSize);
        }
        public async Task<AcquisitionAgentAccountDTO?> GetAccountByName(string agentAccountName)
        {
            return await _repository.GetByNameAsync(agentAccountName);
        }

        public async Task<AcquisitionAgentAccountDTO> CreateAccount(AcquisitionAgentAccountDTO account)
        {
            return await _repository.AddAsync(account);
        }
        public async Task<AcquisitionAgentAccountDTO> UpdateAccount(AcquisitionAgentAccountDTO account)
        {
           return await _repository.UpdateAsync(account);
           
        }
        public async Task<bool> DeleteAccount(string agentAccountName)
        {
            return await _repository.DeleteAsync(agentAccountName);
        }
        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
