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



            var items = await (
            from a in _context.AgentAccounts
            join ag in _context.AcquisitionAgents on a.AgentId equals ag.Id
            orderby a.AccountTitle
            select new AcquisitionAgentAccountDTO
            {
                Id = a.Id,
                AccountNumber = a.AccountNumber,
                AgentId = a.AgentId,
                Approve = a.Approve,
                AccountTitle = a.AccountTitle,
                AccountType = a.AccountType,
                IsActive = a.IsActive,
                AgentName = ag.AgentName 
            }
        )
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
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
                    AccountNumber = a.AccountNumber,
                    AgentId = a.AgentId,
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
                .FirstOrDefaultAsync(a => a.AccountTitle.ToLower() == dto.AccountTitle.ToLower());

            if (existingAccount != null)
            {
                throw new Exception($"Account name '{dto.AccountTitle}' already exists.");
            }
            var entity = new AgentAccount
            {
                Id = Guid.NewGuid(),
                AccountNumber = dto.AccountNumber,
                AgentId = dto.AgentId,
                Approve = false,
                AccountTitle = dto.AccountTitle,
                AccountType = dto.AccountType,
                IsActive = dto.IsActive,
            };
            await _context.AgentAccounts.AddAsync(entity);
            await _context.SaveChangesAsync();

            return new AcquisitionAgentAccountDTO
            {
                Id = entity.Id,
                AccountNumber = entity.AccountNumber,
                AgentId = entity.AgentId,
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

            existing.AccountNumber = dto.AccountNumber;
            existing.AgentId = dto.AgentId;
            existing.Approve = false;
            existing.AccountTitle = dto.AccountTitle;
            existing.AccountType = dto.AccountType;
            existing.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            return new AcquisitionAgentAccountDTO
            {
                Id = existing.Id,
                AccountNumber = existing.AccountNumber,
                AgentId = existing.AgentId,
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
