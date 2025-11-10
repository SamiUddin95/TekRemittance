using Microsoft.EntityFrameworkCore;
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
   public  class DisbursementRepository : IDisbursementRepository
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
                {}
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

            var query = _context.RemittanceInfos
                .Where(r => r.AgentId == agentId && r.Status == "P");
                //.AsNoTracking();

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(r => r.RowNumber) 
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new RemitttanceInfosStatusDTO
                {
                    Id = r.Id,
                    AgentId = r.AgentId,
                    TemplateId=r.TemplateId,
                    UploadId=r.UploadId,
                    RowNumber = r.RowNumber,
                    DataJson = r.DataJson,
                    Error = r.Error,
                    Status = r.Status,
                    CreatedOn = r.CreatedOn
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

            var query = _context.RemittanceInfos
                .Where(r => r.AgentId == agentId && r.Status == "U")
                .AsNoTracking();

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(r => r.CreatedOn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new RemitttanceInfosStatusDTO
                {
                    Id = r.Id,
                    AgentId = r.AgentId,
                    TemplateId = r.TemplateId,
                    UploadId = r.UploadId,
                    RowNumber = r.RowNumber,
                    DataJson = r.DataJson,
                    Error = r.Error,
                    Status = r.Status,
                    CreatedOn = r.CreatedOn
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

            var query = _context.RemittanceInfos
              .Where(r => r.AgentId == agentId && r.Status == "RE")
              .AsNoTracking();

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(r => r.CreatedOn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new RemitttanceInfosStatusDTO
                {
                    Id = r.Id,
                    AgentId = r.AgentId,
                    TemplateId = r.TemplateId,
                    UploadId = r.UploadId,
                    RowNumber = r.RowNumber,
                    DataJson = r.DataJson,
                    Error = r.Error,
                    Status = r.Status,
                    CreatedOn = r.CreatedOn
                })
                .ToListAsync();

            return new PagedResult<RemitttanceInfosStatusDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
        }
        public async Task<PagedResult<RemitttanceInfosStatusDTO>> GetByAgentIdWithStatusRAsync(Guid agentId, int pageNumber=1, int pageSize=10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 50;

            var query = _context.RemittanceInfos
                .Where(r => r.AgentId == agentId && r.Status == "R")
                .AsNoTracking();

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(r => r.CreatedOn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new RemitttanceInfosStatusDTO
                {
                    Id = r.Id,
                    AgentId = r.AgentId,
                    TemplateId=r.TemplateId,
                    UploadId=r.UploadId,
                    RowNumber = r.RowNumber,
                    DataJson = r.DataJson,
                    Error = r.Error,
                    Status = r.Status,
                    CreatedOn = r.CreatedOn
                })
                .ToListAsync();

            return new PagedResult<RemitttanceInfosStatusDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
        }
        public async Task<PagedResult<RemitttanceInfosStatusDTO>> GetByAgentIdWithStatusAAsync(Guid agentId, int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 50;

            var query = _context.RemittanceInfos
                .Where(r => r.AgentId == agentId && r.Status == "A")
                .AsNoTracking();

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(r => r.CreatedOn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new RemitttanceInfosStatusDTO
                {
                    Id = r.Id,
                    AgentId = r.AgentId,
                    TemplateId = r.TemplateId,
                    UploadId = r.UploadId,
                    RowNumber = r.RowNumber,
                    DataJson = r.DataJson,
                    Error = r.Error,
                    Status = r.Status,
                    CreatedOn = r.CreatedOn
                })
                .ToListAsync();

            return new PagedResult<RemitttanceInfosStatusDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
        }

    }
}
