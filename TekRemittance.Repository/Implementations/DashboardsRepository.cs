using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Entities.Data;
using TekRemittance.Repository.Enums;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Repository.Models.dto;

namespace TekRemittance.Repository.Implementations
{
    public class DashboardsRepository:IDashboardsRepository
    {
        private readonly AppDbContext _context;
        public DashboardsRepository(AppDbContext context)
        {
            _context = context;
        }

        

        private decimal ExtractAmount(string dataJson)
        {
            if (string.IsNullOrWhiteSpace(dataJson))
                return 0;

            try
            {
                using var doc = JsonDocument.Parse(dataJson);

                if (doc.RootElement.TryGetProperty("Amount", out JsonElement amountElement))
                {
                    var amountStr = amountElement.GetString()?.Trim();

                    if (decimal.TryParse(
                        amountStr,
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out decimal amount))
                    {
                        return amount;
                    }
                }
            }
            catch
            {
            }

            return 0;
        }

        public async Task<object> GetDashboardDataAsync(string dateRange)
        {
            var query = _context.RemittanceInfos.AsQueryable();

            var allowedDateRanges = new[] { "today", "weekly", "monthly", "annual" };

            if (!string.IsNullOrEmpty(dateRange) &&
                !allowedDateRanges.Contains(dateRange.ToLower()))
            {
                throw new ArgumentException("Invalid dateRange value");
            }

            DateTime today = DateTime.Today;
            DateTime? startDate = null;

            if (!string.IsNullOrEmpty(dateRange))
            {
                if (dateRange.ToLower() == "today")
                    startDate = today;
                else if (dateRange.ToLower() == "weekly")
                    startDate = today.AddDays(-7);
                else if (dateRange.ToLower() == "monthly")
                    startDate = today.AddMonths(-1);
                else if (dateRange.ToLower() == "annual")
                    startDate = today.AddYears(-1);
            }

            if (startDate.HasValue)
            {
                query = query.Where(x => x.Date.HasValue &&
                                         x.Date.Value.Date >= startDate.Value.Date);
            }

            decimal failedAmount =
                query.Where(x => x.Status == "AML" || x.Status == "R" || x.Status == "RE")
                     .AsEnumerable()
                     .Sum(x => ExtractAmount(x.DataJson));

            int failedCount = await query.CountAsync(x =>
                x.Status == "AML" || x.Status == "R" || x.Status == "RE");

            
            decimal successAmount =
                query.Where(x => x.Status == "A")
                     .AsEnumerable()
                     .Sum(x => ExtractAmount(x.DataJson));

            int successCount = await query.CountAsync(x => x.Status == "A");

            int totalCount = failedCount + successCount;

            decimal successPercentage =
                totalCount == 0 ? 0 : Math.Round((decimal)successCount / totalCount * 100, 2);

            decimal totalAmount = query.AsEnumerable().Sum(x => ExtractAmount(x.DataJson));
 
            return new
            {
                failedAmount,
                failedCount,
                successAmount,
                successCount,
                successPercentage,
                totalCount,
                totalAmount
                
            };
        }

        public async Task<List<barGraphDto>> GetbarChartDataAsync(string dateRange)
        {
            var param = new SqlParameter("@Period", dateRange);

            return await _context.barGraphDtos
                .FromSqlRaw("EXEC GetBarGraphData @Period", param)
                .ToListAsync();
        }

        public async Task<TransactionModeCountDTO> GetTransactionModeCountsAsync(string dateRange)
        {
            var query = _context.RemittanceInfos.AsQueryable();

            DateTime today = DateTime.Today;
            DateTime? startDate = null;

            if (!string.IsNullOrEmpty(dateRange))
            {
                switch (dateRange.ToLower())
                {
                    case "today": startDate = today; break;
                    case "weekly": startDate = today.AddDays(-7); break;
                    case "monthly": startDate = today.AddMonths(-1); break;
                    case "annual": startDate = today.AddYears(-1); break;
                    default: throw new ArgumentException("Invalid dateRange value");
                }
            }

            if (startDate.HasValue)
                query = query.Where(x => x.Date.HasValue && x.Date.Value.Date >= startDate.Value.Date);

            var list = await query.ToListAsync();

            int ftCount = list.Count(x => x.ModeOfTransaction.ToString() == "FT");
            int ibftCount = list.Count(x => x.ModeOfTransaction.ToString() == "IBFT");
            int rtgsCount = list.Count(x => x.ModeOfTransaction.ToString() == "RTGS");
            int totalCount = ftCount + ibftCount + rtgsCount;

            return new TransactionModeCountDTO
            {
                FTCount = ftCount,
                IBFTCount = ibftCount,
                RTGSCount = rtgsCount,
                TotalCount= totalCount

            };
        }

        public async Task<List<RecentTransactionDTO>> GetLast10RemittancesAsync()
        {
            var list = await (
                from r in _context.RemittanceInfos
                join a in _context.AcquisitionAgents
                    on r.AgentId equals a.Id
                orderby r.UpdatedOn descending
                select new
                {
                    r,
                    a.AgentName
                }
            )
            .Take(10)
            .ToListAsync();

            var result = list.Select(x => new RecentTransactionDTO
            {
                AgentName = x.AgentName,
                XPIN = x.r.Xpin,
                Date = x.r.Date,
                AccountNumber = x.r.AccountNumber,
                AccountTitle = x.r.AccountTitle,
                Amount = ExtractAmount(x.r.DataJson),
                Status = x.r.Status switch
                {
                    "A" => "Completed",
                    "R" => "Cancelled",
                    "P" => "Processing",
                    "AML" => "Cancelled",
                    "U" => "Processing",
                    "RE" => "Cancelled",
                },
                ModeOfTransaction = x.r.ModeOfTransaction
            })
                .ToList();

            return result;
        }

        public async Task<List<RecentTransactionDTO>> GetTransactionModeListAsync(
         string dateRange,
         string mode)
        {
            var query = _context.RemittanceInfos.AsQueryable();

            DateTime today = DateTime.Today;
            DateTime? startDate = null;

            if (!string.IsNullOrWhiteSpace(dateRange))
            {
                startDate = dateRange.ToLower() switch
                {
                    "today" => today,
                    "weekly" => today.AddDays(-7),
                    "monthly" => today.AddMonths(-1),
                    "annual" => today.AddYears(-1),
                    _ => throw new ArgumentException("Invalid dateRange value")
                };
            }

            if (startDate.HasValue)
            {
                query = query.Where(x =>
                    x.Date.HasValue &&
                    x.Date.Value.Date >= startDate.Value.Date);
            }

            var list = await query
                .OrderByDescending(x => x.Date)
                .Select(x => new
                {
                    x.AgentId,
                    x.Xpin,
                    x.Date,
                    x.AccountNumber,
                    x.AccountTitle,
                    x.DataJson,
                    x.Status,
                    x.ModeOfTransaction
                })
                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(mode))
            {
                var modeUpper = mode.ToUpper();

                if (!new[] { "FT", "IBFT", "RTGS" }.Contains(modeUpper))
                    throw new ArgumentException("Invalid mode value");

                list = list
                    .Where(x => x.ModeOfTransaction.ToString() == modeUpper)
                    .ToList();
            }

            var agentIds = list.Select(x => x.AgentId).Distinct().ToList();

            var agents = await _context.AcquisitionAgents
                .Where(a => agentIds.Contains(a.Id))
                .ToDictionaryAsync(a => a.Id, a => a.AgentName);

            return list.Select(x => new RecentTransactionDTO
            {
                AgentName = agents.ContainsKey(x.AgentId) ? agents[x.AgentId] : null,
                XPIN = x.Xpin,
                Date = x.Date,
                AccountNumber = x.AccountNumber,
                AccountTitle = x.AccountTitle,
                Amount = ExtractAmount(x.DataJson),
                Status = x.Status switch
                {
                    "A" => "Completed",
                    "R" => "Cancelled",
                    "P" => "Processing",
                    "AML" => "Cancelled",
                    "U" => "Processing",
                    "RE" => "Cancelled",
                    _ => x.Status
                },

                ModeOfTransaction = x.ModeOfTransaction
            }).ToList();
        }

    }
}
