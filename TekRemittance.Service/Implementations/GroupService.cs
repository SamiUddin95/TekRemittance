using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Repository.Models.dto;
using TekRemittance.Service.Interfaces;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Service.Implementations
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _repo;

        public GroupService(IGroupRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<GroupDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? name = null)
        {
            return await _repo.GetAllAsync(pageNumber, pageSize, name);
        }

        public async Task<GroupDTO?> GetByIdAsync(Guid id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<GroupDTO> CreateAsync(GroupDTO dto)
        {
            return await _repo.AddAsync(dto);
        }

        public async Task<GroupDTO?> UpdateAsync(GroupDTO dto)
        {
            return await _repo.UpdateAsync(dto);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<bool> SetUsersAsync(Guid groupId, IEnumerable<Guid> userIds)
        {
            return await _repo.SetUsersAsync(groupId, userIds);
        }

        public async Task<bool> SetPermissionsAsync(Guid groupId, IEnumerable<Guid> permissionIds)
        {
            return await _repo.SetPermissionsAsync(groupId, permissionIds);
        }

        public async Task<List<Guid>> GetUsersByGroupIdAsync(Guid groupId)
        {
            return await _repo.GetUsersByGroupIdAsync(groupId);
        }

        public async Task<List<Guid>> GetPermissionsByGroupIdAsync(Guid groupId)
        {
            return await _repo.GetPermissionsByGroupIdAsync(groupId);
        }
    }
}
