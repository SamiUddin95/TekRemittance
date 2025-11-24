using System;
using System.Threading.Tasks;
using TekRemittance.Repository.Models.dto;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Repository.Interfaces
{
    public interface IPermissionRepository
    {
        Task<PagedResult<PermissionDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? name = null);
        Task<PermissionDTO?> GetByIdAsync(Guid id);
        Task<PermissionDTO> AddAsync(PermissionDTO dto);
        Task<PermissionDTO?> UpdateAsync(PermissionDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
