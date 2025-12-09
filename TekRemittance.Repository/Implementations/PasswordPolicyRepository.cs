using TekRemittance.Repository.DTOs;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TekRemittance.Web.Models.dto;
using TekRemittance.Repository.Entities.Data;

namespace TekRemittance.Repository.Implementations
{
    public class PasswordPolicyRepository : IPasswordPolicyRepository
    {
        private readonly AppDbContext _context;
        public PasswordPolicyRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<PasswordPolicyDto>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            var query = _context.PasswordPolicy
                .Select(p => new PasswordPolicyDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    NameOther = p.NameOther,
                    Description = p.Description,
                    CountHistory = p.CountHistory,
                    ExpiryDays = p.ExpiryDays,
                    NotifyDays = p.NotifyDays,
                    AccountDisableDays = p.AccountDisableDays,
                    InvalidLoginEntry = p.InvalidLoginEntry,
                    FirstReset = p.FirstReset,
                    CyclicPassword = p.CyclicPassword,
                    CreatedBy = p.CreatedBy,
                    CreatedOn = p.CreatedOn,
                    UpdatedBy = p.UpdatedBy,
                    UpdatedOn = p.UpdatedOn
                });
            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(p => p.UpdatedOn??p.CreatedOn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<PasswordPolicyDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<PasswordPolicyDto?> GetByIdAsync(Guid id)
        {
            return await _context.PasswordPolicy
                .Where(p => p.Id == id)
                .Select(p => new PasswordPolicyDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    NameOther = p.NameOther,
                    Description = p.Description,
                    CountHistory = p.CountHistory,
                    ExpiryDays = p.ExpiryDays,
                    NotifyDays = p.NotifyDays,
                    AccountDisableDays = p.AccountDisableDays,
                    InvalidLoginEntry = p.InvalidLoginEntry,
                    FirstReset = p.FirstReset,
                    CyclicPassword = p.CyclicPassword,
                    CreatedBy = p.CreatedBy,
                    CreatedOn = p.CreatedOn,
                    UpdatedBy = p.UpdatedBy,
                    UpdatedOn = p.UpdatedOn
                }).FirstOrDefaultAsync();
        }
        public async Task<PasswordPolicyDto> AddAsync(PasswordPolicyDto policy)
        {
            var entity = new PasswordPolicy
            {
                Id = Guid.NewGuid(),
                Name = policy.Name,
                NameOther = policy.NameOther,
                Description = policy.Description,
                CountHistory = policy.CountHistory,
                ExpiryDays = policy.ExpiryDays,
                NotifyDays = policy.NotifyDays,
                AccountDisableDays = policy.AccountDisableDays,
                InvalidLoginEntry = policy.InvalidLoginEntry,
                FirstReset = policy.FirstReset,
                CyclicPassword = policy.CyclicPassword,
                CreatedBy = policy.CreatedBy ?? "system",
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                UpdatedBy = policy.UpdatedBy ?? "system",
            };
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            policy.Id = entity.Id;
            policy.CreatedOn = entity.CreatedOn;
            policy.CreatedBy = entity.CreatedBy;
            policy.UpdatedBy = entity.UpdatedBy;
            policy.UpdatedOn = entity.UpdatedOn;

            return policy;
        }
        public async Task<PasswordPolicyDto?> UpdateAsync(PasswordPolicyDto policy)
        {
            var existing = await _context.PasswordPolicy.FirstOrDefaultAsync(p => p.Id == policy.Id);
            if (existing == null) return null;

            existing.Name = policy.Name;
            existing.NameOther = policy.NameOther;
            existing.Description = policy.Description;
            existing.CountHistory = policy.CountHistory;
            existing.ExpiryDays = policy.ExpiryDays;
            existing.NotifyDays = policy.NotifyDays;
            existing.AccountDisableDays = policy.AccountDisableDays;
            existing.InvalidLoginEntry = policy.InvalidLoginEntry;
            existing.FirstReset = policy.FirstReset;
            existing.CyclicPassword = policy.CyclicPassword;
            existing.UpdatedBy = policy.UpdatedBy ?? "system";
            existing.UpdatedOn = DateTime.Now;

            await _context.SaveChangesAsync();

            policy.UpdatedOn = existing.UpdatedOn;
            return policy;
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.PasswordPolicy.FirstOrDefaultAsync(p => p.Id == id);
            if (existing == null) return false;

            _context.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
