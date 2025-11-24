using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Entities.Data;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Repository.Models.dto;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Repository.Implementations
{
    public class GroupRepository : IGroupRepository
    {
        private readonly AppDbContext _context;

        public GroupRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<GroupDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? name = null)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _context.Groups.AsNoTracking().Where(g => g.IsActive);

            if (!string.IsNullOrEmpty(name))
                query = query.Where(g => g.Name.Contains(name));

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(g => g.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(g => new GroupDTO
                {
                    Id = g.Id,
                    Name = g.Name,
                    Description = g.Description,
                    IsActive = g.IsActive,
                    CreatedBy = g.CreatedBy,
                    CreatedOn = g.CreatedOn,
                    UpdatedBy = g.UpdatedBy,
                    UpdatedOn = g.UpdatedOn
                })
                .ToListAsync();

            return new PagedResult<GroupDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<GroupDTO?> GetByIdAsync(Guid id)
        {
            var group = await _context.Groups
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id && g.IsActive);

            if (group == null) return null;

            return new GroupDTO
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description,
                IsActive = group.IsActive,
                CreatedBy = group.CreatedBy,
                CreatedOn = group.CreatedOn,
                UpdatedBy = group.UpdatedBy,
                UpdatedOn = group.UpdatedOn
            };
        }

        public async Task<GroupDTO> AddAsync(GroupDTO dto)
        {
            var entity = new Group
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                IsActive = true,
                CreatedBy = dto.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };

            _context.Groups.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            dto.IsActive = entity.IsActive;
            dto.CreatedOn = entity.CreatedOn;
            return dto;
        }

        public async Task<GroupDTO?> UpdateAsync(GroupDTO dto)
        {
            var entity = await _context.Groups
                .FirstOrDefaultAsync(g => g.Id == dto.Id && g.IsActive);

            if (entity == null) return null;

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.UpdatedBy = dto.UpdatedBy;
            entity.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            dto.IsActive = entity.IsActive;
            dto.CreatedBy = entity.CreatedBy;
            dto.CreatedOn = entity.CreatedOn;
            dto.UpdatedOn = entity.UpdatedOn;
            return dto;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Groups
                .FirstOrDefaultAsync(g => g.Id == id && g.IsActive);

            if (entity == null) return false;

            entity.IsActive = false;
            entity.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetUsersAsync(Guid groupId, IEnumerable<Guid> userIds)
        {
            var group = await _context.Groups.FindAsync(groupId);
            if (group == null || !group.IsActive) return false;

            var existing = _context.UserGroups.Where(ug => ug.GroupId == groupId);
            _context.UserGroups.RemoveRange(existing);

            var now = DateTime.UtcNow;
            foreach (var userId in userIds.Distinct())
            {
                _context.UserGroups.Add(new UserGroup
                {
                    Id = Guid.NewGuid(),
                    GroupId = groupId,
                    UserId = userId,
                    CreatedOn = now
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetPermissionsAsync(Guid groupId, IEnumerable<Guid> permissionIds)
        {
            var group = await _context.Groups.FindAsync(groupId);
            if (group == null || !group.IsActive) return false;

            var existing = _context.GroupPermissions.Where(gp => gp.GroupId == groupId);
            _context.GroupPermissions.RemoveRange(existing);

            var now = DateTime.UtcNow;
            foreach (var permissionId in permissionIds.Distinct())
            {
                _context.GroupPermissions.Add(new GroupPermission
                {
                    Id = Guid.NewGuid(),
                    GroupId = groupId,
                    PermissionId = permissionId,
                    CreatedOn = now
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Guid>> GetUsersByGroupIdAsync(Guid groupId)
        {
            return await _context.UserGroups
                .Where(ug => ug.GroupId == groupId)
                .Select(ug => ug.UserId)
                .ToListAsync();
        }

        public async Task<List<Guid>> GetPermissionsByGroupIdAsync(Guid groupId)
        {
            return await _context.GroupPermissions
                .Where(gp => gp.GroupId == groupId)
                .Select(gp => gp.PermissionId)
                .ToListAsync();
        }
    }
}
