using TekRemittance.Repository.DTOs;
using System;
using System.Threading.Tasks;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Service.Interfaces
{
    public interface IPasswordPolicyService
    {
        Task<PagedResult<PasswordPolicyDto>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
        Task<PasswordPolicyDto?> GetByIdAsync(Guid id);
        Task<PasswordPolicyDto> CreateAsync(PasswordPolicyDto policy);
        Task<PasswordPolicyDto?> UpdateAsync(PasswordPolicyDto policy);
        Task<bool> DeleteAsync(Guid id);
    }
}
