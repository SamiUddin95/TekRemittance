using System;
using System.Threading.Tasks;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Service.Interfaces
{
    public interface IUserService
    {
        Task<PagedResult<userDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
        Task<userDTO?> GetByIdAsync(Guid id);
        Task<userDTO> CreateAsync(userDTO user, string password);
        Task<userDTO?> UpdateAsync(userDTO user);
        Task<bool> DeleteAsync(Guid id);
        Task<userDTO?> ValidateCredentialsAsync(string loginName, string password);
    }
}
