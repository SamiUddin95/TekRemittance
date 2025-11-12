using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TekRemittance.Repository.DTOs;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Entities.Data;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Repository.Models.dto;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Repository.Implementations
{
    public class DisbursementRepository : IDisbursementRepository
    {
        private readonly AppDbContext _context;
        public DisbursementRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<KeyValuePair<string, List<string>>>> GetDataByAgentIdAsync(Guid agentId, int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 50;

            var baseQuery = _context.RemittanceInfos
                .Where(a => a.AgentId == agentId);

            var totalCount = await baseQuery.CountAsync();

            var records = await baseQuery
                .OrderBy(a => a.RowNumber)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
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
                        if (string.IsNullOrEmpty(value)) continue;

                        if (!groupedData.ContainsKey(kv.Key))
                            groupedData[kv.Key] = new List<string>();

                        groupedData[kv.Key].Add(value);
                    }
                }
                catch
                { }
            }

            return new PagedResult<KeyValuePair<string, List<string>>>
            {
                Items = groupedData.ToList(),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<PagedResult<RemitttanceInfosStatusDTO>> GetByAgentIdWithStatusPAsync(Guid agentId, int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 50;

            var query = from r in _context.RemittanceInfos
                        join a in _context.AcquisitionAgents
                            on r.AgentId equals a.Id
                        where r.AgentId == agentId && r.Status == "P"
                        select new { r, a.AgentName };

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.r.RowNumber)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new RemitttanceInfosStatusDTO
                {
                    Id = x.r.Id,
                    AgentId = x.r.AgentId,
                    AgentName = x.AgentName,
                    TemplateId = x.r.TemplateId,
                    UploadId = x.r.UploadId,
                    RowNumber = x.r.RowNumber,
                    DataJson = x.r.DataJson,
                    Error = x.r.Error,
                    Status = x.r.Status,
                    CreatedOn = x.r.CreatedOn
                })
                .ToListAsync();

            return new PagedResult<RemitttanceInfosStatusDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<PagedResult<RemitttanceInfosStatusDTO>> GetByAgentIdWithStatusUAsync(Guid agentId, int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 50;

            var query = from r in _context.RemittanceInfos
                        join a in _context.AcquisitionAgents
                            on r.AgentId equals a.Id
                        where r.AgentId == agentId && r.Status == "U"
                        select new { r, a.AgentName };

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.r.RowNumber)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new RemitttanceInfosStatusDTO
                {
                    Id = x.r.Id,
                    AgentId = x.r.AgentId,
                    AgentName = x.AgentName,
                    TemplateId = x.r.TemplateId,
                    UploadId = x.r.UploadId,
                    RowNumber = x.r.RowNumber,
                    DataJson = x.r.DataJson,
                    Error = x.r.Error,
                    Status = x.r.Status,
                    CreatedOn = x.r.CreatedOn
                })
                .ToListAsync();

            return new PagedResult<RemitttanceInfosStatusDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<PagedResult<RemitttanceInfosStatusDTO>> GetByAgentIdWithStatusREAsync(Guid agentId, int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 50;

            var query = from r in _context.RemittanceInfos
                        join a in _context.AcquisitionAgents
                            on r.AgentId equals a.Id
                        where r.AgentId == agentId && r.Status == "RE"
                        select new { r, a.AgentName };

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.r.RowNumber)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new RemitttanceInfosStatusDTO
                {
                    Id = x.r.Id,
                    AgentId = x.r.AgentId,
                    AgentName = x.AgentName,
                    TemplateId = x.r.TemplateId,
                    UploadId = x.r.UploadId,
                    RowNumber = x.r.RowNumber,
                    DataJson = x.r.DataJson,
                    Error = x.r.Error,
                    Status = x.r.Status,
                    CreatedOn = x.r.CreatedOn
                })
                .ToListAsync();

            return new PagedResult<RemitttanceInfosStatusDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<PagedResult<RemitttanceInfosStatusDTO>> GetByAgentIdWithStatusRAsync(Guid agentId, int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 50;

            var query = from r in _context.RemittanceInfos
                        join a in _context.AcquisitionAgents
                            on r.AgentId equals a.Id
                        where r.AgentId == agentId && r.Status == "R"
                        select new { r, a.AgentName };

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.r.RowNumber)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new RemitttanceInfosStatusDTO
                {
                    Id = x.r.Id,
                    AgentId = x.r.AgentId,
                    AgentName = x.AgentName,
                    TemplateId = x.r.TemplateId,
                    UploadId = x.r.UploadId,
                    RowNumber = x.r.RowNumber,
                    DataJson = x.r.DataJson,
                    Error = x.r.Error,
                    Status = x.r.Status,
                    CreatedOn = x.r.CreatedOn
                })
                .ToListAsync();

            return new PagedResult<RemitttanceInfosStatusDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<PagedResult<RemitttanceInfosStatusDTO>> GetByAgentIdWithStatusAAsync(Guid agentId, int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 50;

            var query = from r in _context.RemittanceInfos
                        join a in _context.AcquisitionAgents
                            on r.AgentId equals a.Id
                        where r.AgentId == agentId && r.Status == "A"
                        select new { r, a.AgentName };

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.r.RowNumber)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new RemitttanceInfosStatusDTO
                {
                    Id = x.r.Id,
                    AgentId = x.r.AgentId,
                    AgentName = x.AgentName,
                    TemplateId = x.r.TemplateId,
                    UploadId = x.r.UploadId,
                    RowNumber = x.r.RowNumber,
                    DataJson = x.r.DataJson,
                    Error = x.r.Error,
                    Status = x.r.Status,
                    CreatedOn = x.r.CreatedOn
                })
                .ToListAsync();

            return new PagedResult<RemitttanceInfosStatusDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<(bool isSuccess, string message, string Xpin)> RemitApproveAsync(string xpin, Guid? userId)
        {
            if (userId == null)
                throw new ArgumentNullException(nameof(userId), "UserId cannot be null");

            var userLimit = await _context.Users
                .Where(u => u.Id == userId.Value)
                .Select(u => u.Limit)
                .FirstOrDefaultAsync();


            var remitInfo = await _context.RemittanceInfos
                .FirstOrDefaultAsync(r => r.DataJson.Contains($"\"XPin\":{xpin}"));

            if (remitInfo == null)
                throw new InvalidOperationException("Remittance info not found for given XPin.");

            var jsonDoc = JsonDocument.Parse(remitInfo.DataJson);
            decimal amount = 0;

            if (jsonDoc.RootElement.TryGetProperty("Amount", out JsonElement amountElement))
            {
                if (amountElement.ValueKind == JsonValueKind.String)
                {
                    if (!decimal.TryParse(amountElement.GetString()?.Trim(), out amount))
                    {
                        throw new InvalidOperationException("Amount value is invalid.");
                    }
                }
                else if (amountElement.ValueKind == JsonValueKind.Number)
                {
                    amount = amountElement.GetDecimal();
                }
                else
                {
                    throw new InvalidOperationException("Amount is neither string nor number.");
                }
            }
            else
            {
                return (false, "Amount not found in JSON.", xpin);
            }
            bool status = amount <= userLimit ;

            if (!status)
            {
                remitInfo.Status = "U";
                await _context.SaveChangesAsync();
                return (false, "Remittance unauthorized due to insufficient user limit.", xpin);
            }

            remitInfo.Status = "A";
            await _context.SaveChangesAsync();
            return (true, "Remittance approved successfully.", xpin);
            
        }

        public async Task<RemittanceInfoModelDTO> RemitRejectAsync(string xpin, Guid? userId)
        {
            

            var remitInfo = await _context.RemittanceInfos
                .FirstOrDefaultAsync(r => r.DataJson.Contains($"\"XPin\":{xpin}"));

            if (remitInfo == null)
                throw new InvalidOperationException("Remittance info not found for given XPin.");
            remitInfo.Status = "RE";
            await _context.SaveChangesAsync();

            return new RemittanceInfoModelDTO
            {
                Xpin = xpin,
                UserId = userId,
            };
        }
        public async Task<RemittanceInfoModelDTO> RemitAuthorizeAsync(string xpin, Guid? userId)
        {
            if (userId == null)
                throw new ArgumentNullException(nameof(userId), "UserId cannot be null");

            var remitInfo = await _context.RemittanceInfos
                .FirstOrDefaultAsync(r => r.DataJson.Contains($"\"XPin\":{xpin}"));

            if (remitInfo == null)
                throw new InvalidOperationException("Remittance info not found for given XPin.");
            remitInfo.Status = "A";
            await _context.SaveChangesAsync();

            return new RemittanceInfoModelDTO
            {
                Xpin = xpin,
                UserId = userId,
            };
        }

        public async Task<RemittanceInfoModelDTO> RemitRepairAsync(string xpin, Guid? userId)
        {
            if (userId == null)
                throw new ArgumentNullException(nameof(userId), "UserId cannot be null");

            var remitInfo = await _context.RemittanceInfos
                .FirstOrDefaultAsync(r => r.DataJson.Contains($"\"XPin\":{xpin}"));

            if (remitInfo == null)
                throw new InvalidOperationException("Remittance info not found for given XPin.");
            remitInfo.Status = "R";
            await _context.SaveChangesAsync();

            return new RemittanceInfoModelDTO
            {
                Xpin = xpin,
                UserId = userId,
            };
        }

        public async Task<RemittanceInfoModelDTO> RemitReverseAsync(string xpin, Guid? userId)
        {
            if (userId == null)
                throw new ArgumentNullException(nameof(userId), "UserId cannot be null");

            var remitInfo = await _context.RemittanceInfos
                .FirstOrDefaultAsync(r => r.DataJson.Contains($"\"XPin\":{xpin}"));

            if (remitInfo == null)
                throw new InvalidOperationException("Remittance info not found for given XPin.");
            remitInfo.Status = "P";
            await _context.SaveChangesAsync();

            return new RemittanceInfoModelDTO
            {
                Xpin = xpin,
                UserId = userId,
            };
        }
    }
}
