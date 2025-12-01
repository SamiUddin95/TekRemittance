using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TekRemittance.Repository.Models.dto;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Service.Interfaces
{
    public interface IGroupService
    {
        Task<PagedResult<GroupDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? name = null);
        Task<GroupDTO?> GetByIdAsync(Guid id);
        Task<GroupDTO> CreateAsync(GroupDTO dto);
        Task<GroupDTO?> UpdateAsync(GroupDTO dto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> SetUsersAsync(Guid groupId, IEnumerable<Guid> userIds);
        Task<bool> SetPermissionsAsync(Guid groupId, IEnumerable<Guid> permissionIds);
        Task<List<GroupUserDTO>> GetUsersByGroupIdAsync(Guid groupId);
        Task<List<Guid>> GetPermissionsByGroupIdAsync(Guid groupId);
    }
}
