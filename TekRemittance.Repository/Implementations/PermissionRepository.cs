using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Entities.Data;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Repository.Models.dto;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Repository.Implementations
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly AppDbContext _context;

        public PermissionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<PermissionDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? name = null)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _context.Permissions.AsNoTracking().Where(p => p.IsActive);

            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Name.Contains(name));

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(p => p.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PermissionDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = p.Category,
                    Module = p.Module,
                    Description = p.Description,
                    IsActive = p.IsActive,
                    CreatedBy = p.CreatedBy,
                    CreatedOn = p.CreatedOn,
                    UpdatedBy = p.UpdatedBy,
                    UpdatedOn = p.UpdatedOn
                })
                .ToListAsync();

            return new PagedResult<PermissionDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<PermissionDTO?> GetByIdAsync(Guid id)
        {
            var permission = await _context.Permissions
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

            if (permission == null) return null;

            return new PermissionDTO
            {
                Id = permission.Id,
                Name = permission.Name,
                Description = permission.Description,
                IsActive = permission.IsActive,
                CreatedBy = permission.CreatedBy,
                CreatedOn = permission.CreatedOn,
                UpdatedBy = permission.UpdatedBy,
                UpdatedOn = permission.UpdatedOn
            };
        }

        public async Task<PermissionDTO> AddAsync(PermissionDTO dto)
        {
            var entity = new Permission
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                IsActive = true,
                CreatedBy = dto.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };

            _context.Permissions.Add(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            dto.IsActive = entity.IsActive;
            dto.CreatedOn = entity.CreatedOn;
            return dto;
        }

        public async Task<PermissionDTO?> UpdateAsync(PermissionDTO dto)
        {
            var entity = await _context.Permissions
                .FirstOrDefaultAsync(p => p.Id == dto.Id && p.IsActive);

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
            var entity = await _context.Permissions
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

            if (entity == null) return false;

            entity.IsActive = false;
            entity.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
