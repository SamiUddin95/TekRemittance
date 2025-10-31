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
                    UpdatedOn = t.UpdatedOn
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

        public async Task<PagedResult<agentFileTemplateDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _context.AgentFileTemplates.AsNoTracking();
            var totalCount = await query.CountAsync();

            var items = await (
                 from t in query
                 join ag in _context.AcquisitionAgents on t.AgentId equals ag.Id
                 orderby t.Name
                 select new agentFileTemplateDTO
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
                     AgentName = ag.AgentName 
                 }
             )
             .Skip((pageNumber - 1) * pageSize)
             .Take(pageSize)
             .ToListAsync();

            return new PagedResult<agentFileTemplateDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<Dictionary<string, List<string>>> GetDataByAgentIdAsync(Guid agentId)
        {
            var records = await _context.RemittanceInfos
                .Where(a => a.AgentId == agentId)
                .Select(a => a.DataJson)
                .ToListAsync();

            var groupedData = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

            foreach (var json in records)
            {
                if (string.IsNullOrWhiteSpace(json)) continue;

                try
                {
                    var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

                    if (dict == null) continue;

                    foreach (var kv in dict)
                    {
                        
                        var value = kv.Value?.ToString()?.Trim();
                        if (string.IsNullOrEmpty(value))
                            continue;

                        if (!groupedData.ContainsKey(kv.Key))
                            groupedData[kv.Key] = new List<string>();


                        //if (!groupedData[kv.Key].Contains(value))
                        //    groupedData[kv.Key].Add(value);

                        groupedData[kv.Key].Add(value);

                    }
                }
                catch
                {  
                }
            }

            return groupedData;
        }

       
    }
}
