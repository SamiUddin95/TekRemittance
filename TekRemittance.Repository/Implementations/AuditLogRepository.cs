using System.Threading.Tasks;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Entities.Data;
using TekRemittance.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TekRemittance.Web.Models.dto;
using TekRemittance.Repository.Models.dto;
using System.Text.Json;
using TekRemittance.Repository.Enums;

namespace TekRemittance.Repository.Implementations
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly AppDbContext _context;
        public AuditLogRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(AuditLog log)
        {
            await _context.AuditLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<AuditLog>> QueryAsync(
            string? entityName,
            Guid? entityId,
            string? action,
            string? performedBy,
            DateTime? from,
            DateTime? to,
            int pageNumber = 1,
            int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _context.AuditLogs.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(entityName))
                query = query.Where(a => a.EntityName == entityName);
            if (entityId.HasValue && entityId.Value != Guid.Empty)
                query = query.Where(a => a.EntityId == entityId.Value);
            if (!string.IsNullOrWhiteSpace(action))
                query = query.Where(a => a.Action == action);
            if (!string.IsNullOrWhiteSpace(performedBy))
                query = query.Where(a => a.PerformedBy == performedBy);
            if (from.HasValue)
                query = query.Where(a => a.PerformedOn >= from.Value);
            if (to.HasValue)
                query = query.Where(a => a.PerformedOn <= to.Value);

            var total = await query.CountAsync();
            var items = await query
                .OrderByDescending(a => a.PerformedOn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<AuditLog>
            {
                Items = items,
                TotalCount = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
   
        public async Task<PagedResult<AuditLogDTO>> GetAllAuditLogsAsync(int pageNumber = 1, int pageSize = 10, string? action = null, string? performedby = null,string? entityName=null)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;


            var query = _context.AuditLogs.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(performedby))
                query = query.Where(a => a.PerformedBy.Contains(performedby.Trim()));
            if (!string.IsNullOrWhiteSpace(action))
                query = query.Where(a => a.Action.Contains(action.Trim()));
            if (!string.IsNullOrWhiteSpace(entityName))
                query = query.Where(a => a.EntityName.Contains(entityName.Trim()));

            var totalCount = await query.CountAsync();

            var items = await query
               .OrderByDescending(a => a.PerformedOn)
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
                 .Select(a => new AuditLogDTO
                 {
                     Id = a.Id,
                     EntityName = a.EntityName,
                     Action = a.Action,
                     User = a.PerformedBy,
                     Time = a.PerformedOn,

                     OldValues_Internal = a.OldValues,
                     NewValues_Internal = a.NewValues
                 }).ToListAsync();

            foreach (var item in items)
            {
                item.Details = GetChangedFields(item.OldValues_Internal,
        item.NewValues_Internal,
        item.EntityName,
        item.Action);

                item.OldValues_Internal = null;
                item.NewValues_Internal = null;
            }

            return new PagedResult<AuditLogDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

        }

        private List<FieldChangeDTO> GetChangedFields(
    string oldJson,
    string newJson,
    string entityName,
    string action)
        {
            var changes = new List<FieldChangeDTO>();

            string message = $"{entityName} {action} successfully";

            if (string.IsNullOrEmpty(oldJson) && !string.IsNullOrEmpty(newJson))
            {
                changes.Add(new FieldChangeDTO { Field = "Message", NewValue = message });
                return changes;
            }

            if (!string.IsNullOrEmpty(oldJson) && string.IsNullOrEmpty(newJson))
            {
                changes.Add(new FieldChangeDTO { Field = "Message", NewValue = message });
                return changes;
            }

            if (!string.IsNullOrEmpty(oldJson) && !string.IsNullOrEmpty(newJson))
            {
                var oldDict = JsonSerializer.Deserialize<Dictionary<string, object>>(oldJson);
                var newDict = JsonSerializer.Deserialize<Dictionary<string, object>>(newJson);

                foreach (var oldItem in oldDict)
                {
                    var key = oldItem.Key;

                    if (newDict.ContainsKey(key))
                    {
                        string oldVal = oldItem.Value?.ToString();
                        string newVal = newDict[key]?.ToString();

                        if (oldVal != newVal)
                        {
                            changes.Add(new FieldChangeDTO
                            {
                                Field = key,
                                NewValue = newVal
                            });
                        }
                    }
                }
            }

            changes.Add(new FieldChangeDTO
            {
                Field = "Message",
                NewValue = message
            });

            return changes;
        }




    }
}
