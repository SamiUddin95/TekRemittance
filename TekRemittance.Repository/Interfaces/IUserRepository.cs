using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<userDTO>> GetAllAsync(userSearchDTO search);
        Task<userDTO?> GetByIdAsync(Guid id);
        Task<userDTO> AddAsync(userDTO user, string passwordHash);
        Task<userDTO?> UpdateAsync(userDTO user);
        Task<bool> DeleteAsync(Guid id);
        Task<(Guid Id, string PasswordHash, bool IsActive)?> GetAuthByLoginAsync(string loginName);
        Task<bool> UpdateIsSuperviseAsync(Guid id, bool isSupervise);
        Task<bool> UpdateNameAndPasswordAsync(Guid id, string name, string passwordHash);
    }
}
