using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities.Data;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Repository.Implementations
{
    public class AgentFileTemplateFieldRepository : IAgentFileTemplateFieldRepository
    {
        private readonly AppDbContext _context;
        public AgentFileTemplateFieldRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<agentFileTemplateFieldDTO>> GetByTemplateIdAsync(Guid templateId)
        {
            return await _context.AgentFileTemplateFields.AsNoTracking()
                .Where(f => f.TemplateId == templateId)
                .OrderBy(f => f.FieldOrder)
                .Select(f => new agentFileTemplateFieldDTO
                {
                    Id = f.Id,
                    TemplateId = f.TemplateId,
                    FieldOrder = f.FieldOrder,
                    FieldName = f.FieldName,
                    FieldType = f.FieldType,
                    Required = f.Required,
                    Enabled = f.Enabled,
                    StartIndex = f.StartIndex,
                    Length = f.Length
                }).ToListAsync();
        }

        public async Task<agentFileTemplateFieldDTO> CreateAsync(agentFileTemplateFieldDTO dto)
        {
            // Validate unique FieldOrder within template
            if (await _context.AgentFileTemplateFields.AnyAsync(f => f.TemplateId == dto.TemplateId && f.FieldOrder == dto.FieldOrder))
            {
                throw new ArgumentException("Field order already exists for this template.");
            }

            var entity = new Repository.Entities.AgentFileTemplateField
            {
                Id = Guid.NewGuid(),
                TemplateId = dto.TemplateId,
                FieldOrder = dto.FieldOrder,
                FieldName = dto.FieldName?.Trim() ?? string.Empty,
                FieldType = dto.FieldType,
                Required = dto.Required,
                Enabled = dto.Enabled,
                StartIndex = dto.StartIndex,
                Length = dto.Length
            };

            await _context.AgentFileTemplateFields.AddAsync(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            return dto;
        }

        public async Task<agentFileTemplateFieldDTO?> UpdateAsync(agentFileTemplateFieldDTO dto)
        {
            var existing = await _context.AgentFileTemplateFields.FirstOrDefaultAsync(f => f.Id == dto.Id);
            if (existing == null) return null;

            // Validate unique FieldOrder within template excluding self
            if (await _context.AgentFileTemplateFields.AnyAsync(f => f.Id != dto.Id && f.TemplateId == dto.TemplateId && f.FieldOrder == dto.FieldOrder))
            {
                throw new ArgumentException("Field order already exists for this template.");
            }

            existing.TemplateId = dto.TemplateId;
            existing.FieldOrder = dto.FieldOrder;
            existing.FieldName = dto.FieldName?.Trim() ?? string.Empty;
            existing.FieldType = dto.FieldType;
            existing.Required = dto.Required;
            existing.Enabled = dto.Enabled;
            existing.StartIndex = dto.StartIndex;
            existing.Length = dto.Length;

            await _context.SaveChangesAsync();

            return new agentFileTemplateFieldDTO
            {
                Id = existing.Id,
                TemplateId = existing.TemplateId,
                FieldOrder = existing.FieldOrder,
                FieldName = existing.FieldName,
                FieldType = existing.FieldType,
                Required = existing.Required,
                Enabled = existing.Enabled,
                StartIndex = existing.StartIndex,
                Length = existing.Length
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.AgentFileTemplateFields.FirstOrDefaultAsync(f => f.Id == id);
            if (existing == null) return false;
            _context.AgentFileTemplateFields.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
