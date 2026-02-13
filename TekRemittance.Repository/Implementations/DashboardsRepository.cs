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


        //private decimal ExtractAmount(string? json)
        //{
        //    if (string.IsNullOrWhiteSpace(json))
        //        return 0m;

        //    try
        //    {
        //        var doc = JsonDocument.Parse(json);

        //        if (doc.RootElement.TryGetProperty("Amount", out var amountProp))
        //            return amountProp.GetDecimal();

        //        if (doc.RootElement.TryGetProperty("amount", out var amountPropLower))
        //            return amountPropLower.GetDecimal();

        //        return 0m;
        //    }
        //    catch
        //    {
        //        return 0m;
        //    }
        //}

        //private string? ExtractXpin(string? json)
        //{
        //    if (string.IsNullOrWhiteSpace(json))
        //        return null;

        //    try
        //    {
        //        var doc = JsonDocument.Parse(json);

        //        if (doc.RootElement.TryGetProperty("XPIN", out var xpinProp))
        //            return xpinProp.GetString();

        //        if (doc.RootElement.TryGetProperty("xpin", out var xpinPropLower))
        //            return xpinPropLower.GetString();

        //        return null;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}



        //public async Task<object> GetDashboardDataAsync(string dateRange)
        //{
        //    var query = _context.RemittanceInfos.AsQueryable();

        //    var allowedDateRanges = new[] { "today", "weekly", "monthly", "annual" };

        //    if (!string.IsNullOrEmpty(dateRange) &&
        //        !allowedDateRanges.Contains(dateRange.ToLower()))
        //    {
        //        throw new ArgumentException("Invalid dateRange value");
        //    }

        //    DateTime today = DateTime.Today;
        //    DateTime? startDate = null;

        //    if (!string.IsNullOrEmpty(dateRange))
        //    {
        //        if (dateRange.ToLower() == "today")
        //            startDate = today;
        //        else if (dateRange.ToLower() == "weekly")
        //            startDate = today.AddDays(-7);
        //        else if (dateRange.ToLower() == "monthly")
        //            startDate = today.AddMonths(-1);
        //        else if (dateRange.ToLower() == "annual")
        //            startDate = today.AddYears(-1);
        //    }

        //    if (startDate.HasValue)
        //    {
        //        query = query.Where(x => x.Date.HasValue &&
        //                                 x.Date.Value.Date >= startDate.Value.Date);
        //    }

        //    decimal failedAmount =
        //        query.Where(x => x.Status == "AML" || x.Status == "R" || x.Status == "RE")
        //             .AsEnumerable()
        //             .Sum(x => ExtractAmount(x.DataJson));

        //    int failedCount = await query.CountAsync(x =>
        //        x.Status == "AML" || x.Status == "R" || x.Status == "RE");

            
        //    decimal successAmount =
        //        query.Where(x => x.Status == "A")
        //             .AsEnumerable()
        //             .Sum(x => ExtractAmount(x.DataJson));

        //    int successCount = await query.CountAsync(x => x.Status == "A");

        //    int totalCount = failedCount + successCount;

        //    decimal successPercentage =
        //        totalCount == 0 ? 0 : Math.Round((decimal)successCount / totalCount * 100, 2);

        //    decimal totalAmount = query.AsEnumerable().Sum(x => ExtractAmount(x.DataJson));
 
        //    return new
        //    {
        //        failedAmount,
        //        failedCount,
        //        successAmount,
        //        successCount,
        //        successPercentage,
        //        totalCount,
        //        totalAmount
                
        //    };
        //}

        //public async Task<List<barGraphDto>> GetbarChartDataAsync(string dateRange)
        //{
        //    var param = new SqlParameter("@Period", dateRange);

        //    return await _context.barGraphDtos
        //        .FromSqlRaw("EXEC GetBarGraphData @Period", param)
        //        .ToListAsync();
        //}

        //public async Task<TransactionModeCountDTO> GetTransactionModeCountsAsync(string dateRange)
        //{
        //    var query = _context.RemittanceInfos.AsQueryable();

        //    DateTime today = DateTime.Today;
        //    DateTime? startDate = null;

        //    if (!string.IsNullOrEmpty(dateRange))
        //    {
        //        switch (dateRange.ToLower())
        //        {
        //            case "today": startDate = today; break;
        //            case "weekly": startDate = today.AddDays(-7); break;
        //            case "monthly": startDate = today.AddMonths(-1); break;
        //            case "annual": startDate = today.AddYears(-1); break;
        //            default: throw new ArgumentException("Invalid dateRange value");
        //        }
        //    }

        //    if (startDate.HasValue)
        //        query = query.Where(x => x.Date.HasValue && x.Date.Value.Date >= startDate.Value.Date);

        //    var list = await query.ToListAsync();

        //    int ftCount = list.Count(x => x.ModeOfTransaction.ToString() == "FT");
        //    int ibftCount = list.Count(x => x.ModeOfTransaction.ToString() == "IBFT");
        //    int rtgsCount = list.Count(x => x.ModeOfTransaction.ToString() == "RTGS");
        //    int totalCount = ftCount + ibftCount + rtgsCount;

        //    return new TransactionModeCountDTO
        //    {
        //        FTCount = ftCount,
        //        IBFTCount = ibftCount,
        //        RTGSCount = rtgsCount,
        //        TotalCount= totalCount

        //    };
        //}

        //public async Task<List<RecentTransactionDTO>> GetLast10RemittancesAsync()
        //{
        //    var list = await (
        //        from r in _context.RemittanceInfos
        //        join a in _context.AcquisitionAgents
        //            on r.AgentId equals a.Id
        //        orderby r.UpdatedOn descending
        //        select new
        //        {
        //            r,
        //            a.AgentName
        //        }
        //    )
        //    .Take(10)
        //    .ToListAsync();

        //    var result = list.Select(x => new RecentTransactionDTO
        //    {
        //        AgentName = x.AgentName,
        //        XPIN = ExtractXpin(x.r.DataJson),
        //        Date = x.r.Date,
        //        AccountNumber = x.r.AccountNumber,
        //        AccountTitle = x.r.AccountTitle,
        //        Amount = ExtractAmount(x.r.DataJson),
        //        Status = x.r.Status switch
        //        {
        //            "A" => "Completed",
        //            "R" => "Cancelled",
        //            "P" => "Processing",
        //            "AML" => "Cancelled",
        //            "U" => "Processing",
        //            "RE" => "Cancelled",
        //        },
        //        ModeOfTransaction = x.r.ModeOfTransaction
        //    })
        //        .ToList();

        //    return result;
        //}

        //public async Task<List<RecentTransactionDTO>> GetTransactionModeListAsync(
        // string dateRange,
        // string mode)
        //{
        //    var query = _context.RemittanceInfos.AsQueryable();

        //    DateTime today = DateTime.Today;
        //    DateTime? startDate = null;

        //    if (!string.IsNullOrWhiteSpace(dateRange))
        //    {
        //        startDate = dateRange.ToLower() switch
        //        {
        //            "today" => today,
        //            "weekly" => today.AddDays(-7),
        //            "monthly" => today.AddMonths(-1),
        //            "annual" => today.AddYears(-1),
        //            _ => throw new ArgumentException("Invalid dateRange value")
        //        };
        //    }

        //    if (startDate.HasValue)
        //    {
        //        query = query.Where(x =>
        //            x.Date.HasValue &&
        //            x.Date.Value.Date >= startDate.Value.Date);
        //    }

        //    var list = await query
        //        .OrderByDescending(x => x.Date)
        //        .Select(x => new
        //        {
        //            x.AgentId,
        //            x.Xpin,
        //            x.Date,
        //            x.AccountNumber,
        //            x.AccountTitle,
        //            x.DataJson,
        //            x.Status,
        //            x.ModeOfTransaction
        //        })
        //        .ToListAsync();

        //    if (!string.IsNullOrWhiteSpace(mode))
        //    {
        //        var modeUpper = mode.ToUpper();

        //        if (!new[] { "FT", "IBFT", "RTGS" }.Contains(modeUpper))
        //            throw new ArgumentException("Invalid mode value");

        //        list = list
        //            .Where(x => x.ModeOfTransaction.ToString() == modeUpper)
        //            .ToList();
        //    }

        //    var agentIds = list.Select(x => x.AgentId).Distinct().ToList();

        //    var agents = await _context.AcquisitionAgents
        //        .Where(a => agentIds.Contains(a.Id))
        //        .ToDictionaryAsync(a => a.Id, a => a.AgentName);

        //    return list.Select(x => new RecentTransactionDTO
        //    {
        //        AgentName = agents.ContainsKey(x.AgentId) ? agents[x.AgentId] : null,
        //        XPIN = x.Xpin,
        //        Date = x.Date,
        //        AccountNumber = x.AccountNumber,
        //        AccountTitle = x.AccountTitle,
        //        Amount =ExtractAmount(x.DataJson),
        //        Status = x.Status switch
        //        {
        //            "A" => "Completed",
        //            "R" => "Cancelled",
        //            "P" => "Processing",
        //            "AML" => "Cancelled",
        //            "U" => "Processing",
        //            "RE" => "Cancelled",
        //            _ => x.Status
        //        },

        //        ModeOfTransaction = x.ModeOfTransaction
        //    }).ToList();
        //}

        
        public async Task<List<AgentPerformanceDTO>> GetAgentPerformanceAsync()
        {
            return await _context.TransactionDetail
                .GroupBy(x => x.AgentName)
                .Select(g => new
                {
                    AgentName = g.Key,
                    TotalTransactions = g.Count(),
                    TotalAmountValue = g.Sum(x =>
                        string.IsNullOrWhiteSpace(x.Amount)
                            ? 0
                            : Convert.ToDecimal(x.Amount))
                })
                .OrderByDescending(x => x.TotalAmountValue)
                .Select(x => new AgentPerformanceDTO
                {
                    AgentName = x.AgentName,
                    TotalTransactions = x.TotalTransactions,
                    TotalAmount = (x.TotalAmountValue / 1_000_000)
                                    .ToString("0.00") + "M"
                })
                .ToListAsync();
        }

        public async Task<List<TopBankTransactionDTO>> GetTopBankTransactionAsync()
        {
            return await _context.TransactionDetail
                .GroupBy(x => x.BankName)
                .Select(g => new
                {
                    BankName = g.Key,
                    TotalTransactions = g.Count(),
                    TotalAmountValue = g.Sum(x =>
                        string.IsNullOrWhiteSpace(x.Amount)
                            ? 0
                            : Convert.ToDecimal(x.Amount))
                })
                .OrderByDescending(x => x.TotalAmountValue)
                .Select(x => new TopBankTransactionDTO
                {
                    BankName = x.BankName,
                    TotalTransactions = x.TotalTransactions,
                    TotalAmount = (x.TotalAmountValue / 1_000_000m)
                                    .ToString("0.00") + "M"
                })
                .ToListAsync();
        }


        public async Task<List<TransactionStatusByChannelDTO>> GetTransactionStatusByChannelAsync()
        {
            var data = await _context.TransactionDetail
                .GroupBy(x => x.TransactionType)
                .Select(g => new
                {
                    TransactionType = g.Key,
                    Total = g.Count(),

                    IncomingCount = g.Count(x => x.ChannelType == "Incoming"),
                    OutgoingCount = g.Count(x => x.ChannelType == "Outgoing"),
                    PendingCount = g.Count(x => x.Status == "Pending")
                })
                .ToListAsync();

            return data.Select(x => new TransactionStatusByChannelDTO
            {
                TransactionType = x.TransactionType,

                IncomingPercentage = x.Total == 0 ? 0 :
                    Math.Round((decimal)x.IncomingCount / x.Total * 100, 2),

                OutgoingPercentage = x.Total == 0 ? 0 :
                    Math.Round((decimal)x.OutgoingCount / x.Total * 100, 2),

                PendingPercentage = x.Total == 0 ? 0 :
                    Math.Round((decimal)x.PendingCount / x.Total * 100, 2)

            }).ToList();
        }
        public async Task<SummaryDTO> GetIncomingSummaryAsync()
        {
            var data = await _context.TransactionDetail
                .Where(x => x.ChannelType == "Incoming")
                .ToListAsync();

            var totalCount = data.Count;

            var totalAmountValue = data.Sum(x =>
                string.IsNullOrWhiteSpace(x.Amount)
                    ? 0
                    : Convert.ToDecimal(x.Amount));

            var successCount = data.Count(x => x.Status == "Success");
            var failureCount = data.Count(x => x.Status == "Failure");

            var validStatusCount = successCount + failureCount;

            return new SummaryDTO
            {
                TotalCount = totalCount,

               
                TotalAmount = (totalAmountValue / 1_000_000m).ToString("0.00") + "M",

                SuccessPercentage = validStatusCount == 0 ? 0 :
                    Math.Round((decimal)successCount / validStatusCount * 100, 2),

                FailurePercentage = validStatusCount == 0 ? 0 :
                    Math.Round((decimal)failureCount / validStatusCount * 100, 2)
            };
        }

        public async Task<SummaryDTO> GetOutgoingSummaryAsync()
        {
            var data = await _context.TransactionDetail
                .Where(x => x.ChannelType == "Outgoing")
                .ToListAsync();

            var totalCount = data.Count;

            var totalAmountValue = data.Sum(x =>
                string.IsNullOrWhiteSpace(x.Amount)
                    ? 0
                    : Convert.ToDecimal(x.Amount));

            var successCount = data.Count(x => x.Status == "Success");
            var failureCount = data.Count(x => x.Status == "Failure");

            var validStatusCount = successCount + failureCount;

            return new SummaryDTO
            {
                TotalCount = totalCount,

                TotalAmount = (totalAmountValue / 1_000_000m).ToString("0.00") + "M",

                SuccessPercentage = validStatusCount == 0 ? 0 :
                    Math.Round((decimal)successCount / validStatusCount * 100, 2),

                FailurePercentage = validStatusCount == 0 ? 0 :
                    Math.Round((decimal)failureCount / validStatusCount * 100, 2)
            };
        }

        public async Task<List<EPRCDTO>> GetEPRCAsync()
        {
            return await _context.EPRC
                .Select(x => new EPRCDTO
                {
                    Id = x.Id,
                    TotalEPRCGenerated = x.TotalEPRCGenerated,
                    EPRCVerified = x.EPRCVerified,
                    NotVerfied = x.NotVerfied
                })
                .ToListAsync();
        }
        public async Task<List<ChannelsDTO>> GetChannelsAsync()
        {
            return await _context.Channels
                .Select(x => new ChannelsDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    CreatedBy = x.CreatedBy,
                    UpdatedBy = x.UpdatedBy,
                    CreatedOn = x.CreatedOn,
                    UpdatedOn = x.UpdatedOn
                })
                .ToListAsync();
        }




    }
}
