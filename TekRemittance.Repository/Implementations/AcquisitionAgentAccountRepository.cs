using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Entities.Data;
using TekRemittance.Repository.Enums;
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

        public async Task<PagedResult<AcquisitionAgentAccountDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10 ,string? accountnumber = null, string? agentname = null, StatusesEnums? status = null)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query =
     from a in _context.AgentAccounts.AsNoTracking()
     join ag in _context.AcquisitionAgents.AsNoTracking()
         on a.AgentId equals ag.Id
     select new { a, ag };

            if (!string.IsNullOrWhiteSpace(accountnumber))
                query = query.Where(x => x.a.AccountNumber.Contains(accountnumber.Trim()));

            if (!string.IsNullOrWhiteSpace(agentname))
                query = query.Where(x => x.ag.AgentName.Contains(agentname.Trim()));

            if (status == StatusesEnums.Active)
                query = query.Where(x => x.a.IsActive == true);

            if (status == StatusesEnums.Inactive)
                query = query.Where(x => x.a.IsActive == false);

            var totalCount = await query.CountAsync();
            var items = await query
                   .OrderByDescending(x => x.a.UpdatedOn?? x.a.CreatedOn)
                   .Skip((pageNumber - 1) * pageSize)
                   .Take(pageSize)
                   .Select(x => new AcquisitionAgentAccountDTO
                   {
                       Id = x.a.Id,
                       AccountNumber = x.a.AccountNumber,
                       Approve = x.a.Approve,
                       AccountTitle = x.a.AccountTitle,
                       AccountType = x.a.AccountType,
                       IsActive = x.a.IsActive,
                       AgentId = x.a.AgentId,
                       AgentName = x.ag.AgentName,
                       CreatedBy = x.a.CreatedBy,
                       CreatedOn = x.a.CreatedOn,
                       UpdatedBy = x.a.UpdatedBy,
                       UpdatedOn = x.a.UpdatedOn
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
                    AccountNumber = a.AccountNumber,
                    Approve = a.Approve,
                    AccountTitle = a.AccountTitle,
                    AccountType = a.AccountType,
                    IsActive = a.IsActive,
                    AgentId = a.AgentId,
                    CreatedBy = a.CreatedBy,
                    CreatedOn = a.CreatedOn,
                    UpdatedBy = a.UpdatedBy,
                    UpdatedOn = a.UpdatedOn
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
                Approve = false,
                AccountTitle = dto.AccountTitle,
                AccountType = dto.AccountType,
                IsActive = dto.IsActive,
                AgentId = dto.AgentId,
                CreatedBy = dto.CreatedBy ?? "system",
                CreatedOn = DateTime.UtcNow,
                UpdatedBy = dto.UpdatedBy ?? "system",
                UpdatedOn = DateTime.UtcNow
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
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                UpdatedBy = entity.UpdatedBy,
                UpdatedOn = entity.UpdatedOn
            };
        }
       
        public async Task<AcquisitionAgentAccountDTO?> UpdateAsync(AcquisitionAgentAccountDTO dto)
        {
            var existing = await _context.AgentAccounts
                .FirstOrDefaultAsync(a => a.Id == dto.Id); 

            if (existing == null)
                return null;


            existing.AccountNumber = dto.AccountNumber;
            existing.Approve = false;
            existing.AccountTitle = dto.AccountTitle;
            existing.AccountType = dto.AccountType;
            existing.IsActive = dto.IsActive;
            existing.AgentId = dto.AgentId;
            existing.UpdatedBy = dto.UpdatedBy;
            existing.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new AcquisitionAgentAccountDTO
            {
                Id = existing.Id,
                AccountNumber = existing.AccountNumber,
                Approve = existing.Approve,
                AccountTitle = existing.AccountTitle,
                AccountType = existing.AccountType,
                IsActive = existing.IsActive,
                AgentId = existing.AgentId,
                CreatedBy = existing.CreatedBy,
                CreatedOn = existing.CreatedOn,
                UpdatedBy = existing.UpdatedBy,
                UpdatedOn = existing.UpdatedOn
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
