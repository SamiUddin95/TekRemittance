using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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
        public async Task<PagedResult<KeyValuePair<string, List<string>>>> GetDataByAgentIdAsync(Guid agentId, int pageNumber = 1, int pageSize = 50)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 50;

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

                        groupedData[kv.Key].Add(value);
                    }
                }
                catch { }
            }
            var totalCount = groupedData.Count;
            var items = groupedData
                .OrderBy(x => x.Key)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList(); 
            return new PagedResult<KeyValuePair<string, List<string>>>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

    }
}
