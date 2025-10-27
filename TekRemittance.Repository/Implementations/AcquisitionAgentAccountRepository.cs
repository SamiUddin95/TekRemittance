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
    public class AcquisitionAgentAccountRepository : IAcquisitionAgentAccountRepository
    {
        private readonly AppDbContext _context;

        public AcquisitionAgentAccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<AcquisitionAgentAccountDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _context.AgentAccounts.AsNoTracking();
            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(a => a.AgentAccountName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AcquisitionAgentAccountDTO
                {
                    Id = a.Id,
                    AgentAccountName = a.AgentAccountName,
                    AccountNumber = a.AccountNumber,
                    AgentName = a.AgentName,
                    Approve = a.Approve,
                    AccountTitle = a.AccountTitle,
                    AccountType = a.AccountType,
                    IsActive = a.IsActive
                })
                .ToListAsync();

            return new PagedResult<AcquisitionAgentAccountDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<AcquisitionAgentAccountDTO?> GetByIdAsync(Guid id)
        {
            return await _context.AgentAccounts
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new AcquisitionAgentAccountDTO
                {
                    Id = a.Id,
                    AgentAccountName = a.AgentAccountName,
                    AccountNumber = a.AccountNumber,
                    AgentName = a.AgentName,
                    Approve = a.Approve,
                    AccountTitle = a.AccountTitle,
                    AccountType = a.AccountType,
                    IsActive = a.IsActive
                })
                .FirstOrDefaultAsync();
        }
        public async Task<AcquisitionAgentAccountDTO> AddAsync(AcquisitionAgentAccountDTO dto)
        {

            var existingAccount = await _context.AgentAccounts
                .FirstOrDefaultAsync(a => a.AgentAccountName.ToLower() == dto.AgentAccountName.ToLower());

            if (existingAccount != null)
            {
                throw new Exception($"Account name '{dto.AgentAccountName}' already exists.");
            }
            var entity = new AgentAccount
            {
                Id = Guid.NewGuid(),
                AgentAccountName = dto.AgentAccountName,
                AccountNumber = dto.AccountNumber,
                AgentName = dto.AgentName,
                Approve = dto.Approve,
                AccountTitle = dto.AccountTitle,
                AccountType = dto.AccountType,
                IsActive = dto.IsActive,
            };
            await _context.AgentAccounts.AddAsync(entity);
            await _context.SaveChangesAsync();

            return new AcquisitionAgentAccountDTO
            {
                Id = entity.Id,
                AgentAccountName = entity.AgentAccountName,
                AccountNumber = entity.AccountNumber,
                AgentName = entity.AgentName,
                Approve = entity.Approve,
                AccountTitle = entity.AccountTitle,
                AccountType = entity.AccountType,
                IsActive = entity.IsActive,
            };
        }
       
        public async Task<AcquisitionAgentAccountDTO?> UpdateAsync(AcquisitionAgentAccountDTO dto)
        {
            var existing = await _context.AgentAccounts
                .FirstOrDefaultAsync(a => a.Id == dto.Id); 

            if (existing == null)
                return null;

            
            existing.AgentAccountName = dto.AgentAccountName;
            existing.AccountNumber = dto.AccountNumber;
            existing.AgentName = dto.AgentName;
            existing.Approve = dto.Approve;
            existing.AccountTitle = dto.AccountTitle;
            existing.AccountType = dto.AccountType;
            existing.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            return new AcquisitionAgentAccountDTO
            {
                Id = existing.Id,
                AgentAccountName = existing.AgentAccountName,
                AccountNumber = existing.AccountNumber,
                AgentName = existing.AgentName,
                Approve = existing.Approve,
                AccountTitle = existing.AccountTitle,
                AccountType = existing.AccountType,
                IsActive = existing.IsActive
            };
        }
        
        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            var existing = await _context.AgentAccounts
                .FirstOrDefaultAsync(a => a.Id == id);

            if (existing == null)
                return false;

            _context.AgentAccounts.Remove(existing);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
