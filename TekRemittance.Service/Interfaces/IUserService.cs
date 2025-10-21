using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Service.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<userDTO>> GetAllAsync(userSearchDTO search);
        Task<userDTO?> GetByIdAsync(Guid id);
        Task<userDTO> CreateAsync(userDTO user, string password);
        Task<userDTO?> UpdateAsync(userDTO user);
        Task<bool> DeleteAsync(Guid id);
        Task<userDTO?> ValidateCredentialsAsync(string loginName, string password);
        Task<bool> UpdateIsSuperviseAsync(Guid id, bool isSupervise);
        Task<bool> UpdateNameAndPasswordAsync(Guid id, string name, string password);
    }
}
