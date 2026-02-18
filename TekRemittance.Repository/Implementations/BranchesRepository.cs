using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TekRemittance.Repository.DTOs;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Entities.Data;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Repository.Implementations
{
    public class BranchesRepository : IBranchesRepository
    {
        private readonly AppDbContext _context;
        public BranchesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<BranchDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? agentname = null, string? code = null, string? agentbranchname = null)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _context.Branches
                .Include(b => b.Agent)
                .Include(b => b.Country)
                .Include(b => b.Province)
                .Include(b => b.City)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(agentname))
                query = query.Where(b => b.Agent.AgentName.Contains(agentname.Trim()));
            if (!string.IsNullOrWhiteSpace(code))
                query = query.Where(b => b.Code.Contains(code.Trim()));
            if (!string.IsNullOrWhiteSpace(agentbranchname))
                query = query.Where(b => b.AgentBranchName.Contains(agentbranchname.Trim()));

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(b => b.UpdatedOn??b.CreatedOn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BranchDTO
                {
                    Id = b.Id,
                    AgentId = b.AgentId,
                    AgentName=b.Agent.AgentName,
                    Code = b.Code,
                    AgentBranchName = b.AgentBranchName,
                    Phone1 = b.Phone1,
                    Phone2 = b.Phone2,
                    Fax = b.Fax,
                    Email = b.Email,
                    Address = b.Address,
                    CountryId = b.CountryId,
                    ProvinceId = b.ProvinceId,
                    CityId = b.CityId,
                    //AcquisitionModes = b.AcquisitionModes,
                    //DisbursementModes = b.DisbursementModes,
                    CreatedBy = b.CreatedBy,
                    CreatedOn = b.CreatedOn,
                    UpdatedBy = b.UpdatedBy,
                    UpdatedOn = b.UpdatedOn
                })
                .ToListAsync();

            return new PagedResult<BranchDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<BranchDTO?> GetByIdAsync(Guid id)
        {
            return await _context.Branches
                .Include(b => b.Agent)
                .Include(b => b.Country)
                .Include(b => b.Province)
                .Include(b => b.City)
                .AsNoTracking()
                .Where(b => b.Id == id)
                .Select(b => new BranchDTO
                {
                    Id = b.Id,
                    AgentId = b.AgentId,
                    Code = b.Code,
                    AgentBranchName = b.AgentBranchName,
                    Phone1 = b.Phone1,
                    Phone2 = b.Phone2,
                    Fax = b.Fax,
                    Email = b.Email,
                    Address = b.Address,
                    CountryId = b.CountryId,
                    ProvinceId = b.ProvinceId,
                    CityId = b.CityId,
                    //AcquisitionModes = b.AcquisitionModes,
                    //DisbursementModes = b.DisbursementModes,
                    CreatedBy = b.CreatedBy,
                    CreatedOn = b.CreatedOn,
                    UpdatedBy = b.UpdatedBy,
                    UpdatedOn = b.UpdatedOn
                })
                .FirstOrDefaultAsync();
        }
        public async Task<BranchDTO> AddAsync(BranchDTO dto)
        {
            var code = dto.Code?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(code))
                throw new ArgumentException("Branch code is required.");

            if (await _context.Branches.AnyAsync(b => b.Code.ToLower() == code.ToLower()))
                throw new ArgumentException("Branch code already exists.");

            var name = dto.AgentBranchName?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Branch name is required.");

            if (await _context.Branches.AnyAsync(b => b.AgentBranchName.ToLower() == name.ToLower()))
                throw new ArgumentException("Branch name already exists.");

            var entity = new Branches
            {
                Id = Guid.NewGuid(),
                AgentId = dto.AgentId,
                Code = code,
                AgentBranchName = name,
                Phone1 = dto.Phone1,
                Phone2 = dto.Phone2,
                Fax = dto.Fax,
                Email = dto.Email,
                Address = dto.Address,
                CountryId = dto.CountryId,
                ProvinceId = dto.ProvinceId,
                CityId = dto.CityId,
                //AcquisitionModes = dto.AcquisitionModes,
                //DisbursementModes = dto.DisbursementModes,
                CreatedBy = dto.CreatedBy ?? "system",
                CreatedOn = DateTime.UtcNow,
                UpdatedBy = dto.UpdatedBy ?? "system",
                UpdatedOn = DateTime.UtcNow
            };

            await _context.Branches.AddAsync(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            dto.CreatedBy = entity.CreatedBy;
            dto.CreatedOn = entity.CreatedOn;
            dto.UpdatedBy = entity.UpdatedBy;
            dto.UpdatedOn = entity.UpdatedOn;
            return dto;
        }
        public async Task<BranchDTO?> UpdateAsync(BranchDTO dto)
        {
            var existing = await _context.Branches.FirstOrDefaultAsync(b => b.Id == dto.Id);
            if (existing == null)
                return null;

            var code = dto.Code?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(code))
                throw new ArgumentException("Branch code is required.");

            if (await _context.Branches.AnyAsync(b => b.Id != dto.Id && b.Code.ToLower() == code.ToLower()))
                throw new ArgumentException("Branch code already exists.");

            var name = dto.AgentBranchName?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Branch name is required.");

            if (await _context.Branches.AnyAsync(b => b.Id != dto.Id && b.AgentBranchName.ToLower() == name.ToLower()))
                throw new ArgumentException("Branch name already exists.");

            existing.AgentId = dto.AgentId;
            existing.Code = code;
            existing.AgentBranchName = name;
            existing.Phone1 = dto.Phone1;
            existing.Phone2 = dto.Phone2;
            existing.Fax = dto.Fax;
            existing.Email = dto.Email;
            existing.Address = dto.Address;
            existing.CountryId = dto.CountryId;
            existing.ProvinceId = dto.ProvinceId;
            existing.CityId = dto.CityId;
            //existing.AcquisitionModes = dto.AcquisitionModes;
            //existing.DisbursementModes = dto.DisbursementModes;
            existing.UpdatedBy = dto.UpdatedBy ?? "system";
            existing.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return dto;
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.Branches.FirstOrDefaultAsync(b => b.Id == id);
            if (existing == null)
                return false;

            _context.Branches.Remove(existing);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

        