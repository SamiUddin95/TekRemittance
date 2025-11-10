using TekRemittance.Repository.DTOs;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Service.Interfaces;
using System;
using System.Threading.Tasks;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Service.Implementations
{
    public class PasswordPolicyService : IPasswordPolicyService
    {
        private readonly IPasswordPolicyRepository _repository;

        public PasswordPolicyService(IPasswordPolicyRepository repository)
        {
            _repository = repository;
        }

        public Task<PagedResult<PasswordPolicyDto>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
            => _repository.GetAllAsync(pageNumber, pageSize);

        public Task<PasswordPolicyDto?> GetByIdAsync(Guid id)
            => _repository.GetByIdAsync(id);

        public Task<PasswordPolicyDto> CreateAsync(PasswordPolicyDto policy)
            => _repository.AddAsync(policy);

        public Task<PasswordPolicyDto?> UpdateAsync(PasswordPolicyDto policy)
            => _repository.UpdateAsync(policy);

        public Task<bool> DeleteAsync(Guid id)
            => _repository.DeleteAsync(id);
    }
}
