using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities.Data;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Web.Models.dto;
using TekRemittance.Repository.Models.dto;
using System.Text.Json;
//using System.Text.Json;

namespace TekRemittance.Repository.Implementations
{
    public class AgentFileTemplateRepository : IAgentFileTemplateRepository
    {
        private readonly AppDbContext _context;

        public AgentFileTemplateRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<agentFileTemplateDTO?> GetByAgentIdAsync(Guid agentId)
        {
            return await _context.AgentFileTemplates.AsNoTracking()
                .Where(t => t.AgentId == agentId)
                .Select(t => new agentFileTemplateDTO
                {
                    Id = t.Id,
                    AgentId = t.AgentId,
                    Name = t.Name,
                    SheetName = t.SheetName,
                    Format = t.Format,
                    IsFixedLength = t.IsFixedLength,
                    DelimiterEnabled = t.DelimiterEnabled,
                    Delimiter = t.Delimiter,
                    IsActive = t.IsActive,
                    CreatedBy = t.CreatedBy,
                    CreatedOn = t.CreatedOn,
                    UpdatedBy = t.UpdatedBy,
                    UpdatedOn = t.UpdatedOn,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<agentFileTemplateDTO> CreateAsync(agentFileTemplateDTO dto)
        {
            if (await _context.AgentFileTemplates.AnyAsync(t => t.AgentId == dto.AgentId))
            {
                throw new ArgumentException("Template already exists for this agent.");
            }
            if (dto.DelimiterEnabled && string.IsNullOrWhiteSpace(dto.Delimiter))
            {
                throw new ArgumentException("Delimiter is required when DelimiterEnabled is true.");
            }

            var entity = new AgentFileTemplate
            {
                Id = Guid.NewGuid(),
                AgentId = dto.AgentId,
                Name = dto.Name?.Trim() ?? string.Empty,
                SheetName = dto.SheetName,
                Format = dto.Format,
                IsFixedLength = dto.IsFixedLength,
                DelimiterEnabled = dto.DelimiterEnabled,
                Delimiter = dto.Delimiter,
                IsActive = dto.IsActive,
                CreatedBy = dto.CreatedBy ?? "system",
                CreatedOn = DateTime.UtcNow,
                UpdatedBy = dto.UpdatedBy ?? "system",
                UpdatedOn = DateTime.UtcNow
            };

            await _context.AgentFileTemplates.AddAsync(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            dto.CreatedOn = entity.CreatedOn;
            dto.UpdatedOn = entity.UpdatedOn;
            return dto;
        }

        public async Task<agentFileTemplateDTO?> UpdateAsync(agentFileTemplateDTO dto)
        {
            var existing = await _context.AgentFileTemplates.FirstOrDefaultAsync(t => t.Id == dto.Id);
            if (existing == null) return null;

            if (await _context.AgentFileTemplates.AnyAsync(t => t.AgentId == dto.AgentId && t.Id != dto.Id))
            {
                throw new ArgumentException("Template already exists for this agent.");
            }
            if (dto.DelimiterEnabled && string.IsNullOrWhiteSpace(dto.Delimiter))
            {
                throw new ArgumentException("Delimiter is required when DelimiterEnabled is true.");
            }

            existing.AgentId = dto.AgentId;
            existing.Name = dto.Name?.Trim() ?? string.Empty;
            existing.SheetName = dto.SheetName;
            existing.Format = dto.Format;
            existing.IsFixedLength = dto.IsFixedLength;
            existing.DelimiterEnabled = dto.DelimiterEnabled;
            existing.Delimiter = dto.Delimiter;
            existing.IsActive = dto.IsActive;
            existing.UpdatedBy = dto.UpdatedBy;
            existing.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new agentFileTemplateDTO
            {
                Id = existing.Id,
                AgentId = existing.AgentId,
                Name = existing.Name,
                SheetName = existing.SheetName,
                Format = existing.Format,
                IsFixedLength = existing.IsFixedLength,
                DelimiterEnabled = existing.DelimiterEnabled,
                Delimiter = existing.Delimiter,
                IsActive = existing.IsActive,
                CreatedBy = existing.CreatedBy,
                CreatedOn = existing.CreatedOn,
                UpdatedBy = existing.UpdatedBy,
                UpdatedOn = existing.UpdatedOn
            };
        }

        public async Task<bool> DeleteByAgentIdAsync(Guid agentId)
        {
            var template = await _context.AgentFileTemplates.FirstOrDefaultAsync(t => t.AgentId == agentId);
            if (template == null) return false;

            // delete fields first
            var fields = _context.AgentFileTemplateFields.Where(f => f.TemplateId == template.Id);
            _context.AgentFileTemplateFields.RemoveRange(fields);
            _context.AgentFileTemplates.Remove(template);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<agentFileTemplateDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? name = null, string? agentname = null, string? sheetname = null)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query =
               from t in _context.AgentFileTemplates
               join ag in _context.AcquisitionAgents on t.AgentId equals ag.Id
               select new { t, ag };

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => x.t.Name.Contains(name.Trim()));

            if (!string.IsNullOrWhiteSpace(agentname))
                query = query.Where(x => x.ag.AgentName.Contains(agentname.Trim()));

            if (!string.IsNullOrWhiteSpace(sheetname))
                query = query.Where(x => x.t.SheetName.Contains(sheetname.Trim()));


            var totalCount = await query.CountAsync();


            var items = await query
                .OrderByDescending(x => x.t.UpdatedOn??x.t.CreatedOn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new agentFileTemplateDTO
                {
                    Id = x.t.Id,
                    AgentId = x.t.AgentId,
                    Name = x.t.Name,
                    SheetName = x.t.SheetName,
                    Format = x.t.Format,
                    IsFixedLength = x.t.IsFixedLength,
                    DelimiterEnabled = x.t.DelimiterEnabled,
                    Delimiter = x.t.Delimiter,
                    IsActive = x.t.IsActive,
                    CreatedBy = x.t.CreatedBy,
                    CreatedOn = x.t.CreatedOn,
                    UpdatedBy = x.t.UpdatedBy,
                    UpdatedOn = x.t.UpdatedOn,
                    AgentName = x.ag.AgentName
                })
                .ToListAsync();

            return new PagedResult<agentFileTemplateDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<PagedResult<object>> GetDataByUploadIdAsync(Guid UploadId, int pageNumber = 1, int pageSize = 50)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 50;

            var baseQuery = _context.RemittanceInfos
                .Where(a => a.UploadId == UploadId);

            var totalCount = await baseQuery.CountAsync();

            var records = await baseQuery
                .OrderBy(a => a.RowNumber)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new
                {
                    a.DataJson,
                    a.AccountTitle,
                    a.AccountNumber,
                    
                })
                .ToListAsync();

            var aggregatedDict = new Dictionary<string, List<string>>();

            string accountTitle = "";
            string accountNumber = "";
            

            foreach (var record in records)
            {
                accountTitle = record.AccountTitle ?? "";
                accountNumber = record.AccountNumber ?? "";
                

                var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(record.DataJson);
                if (dict == null) continue;

                foreach (var kv in dict)
                {
                    if (!aggregatedDict.ContainsKey(kv.Key))
                        aggregatedDict[kv.Key] = new List<string>();

                    aggregatedDict[kv.Key].Add(kv.Value?.ToString() ?? "");
                }
            }

            var finalList = aggregatedDict.Select(kv => new
            {
                key = kv.Key,
                value = kv.Value,
                AccountTitle = accountTitle,
                AccountNumber = accountNumber,

            }).ToList();

            return new PagedResult<object>
            {
                Items = finalList,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }


    }
}
