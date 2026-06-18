using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
    public class ExchangeRebateRepository : IExchangeRebateRepository
    {
        private readonly AppDbContext _context;

        public ExchangeRebateRepository(AppDbContext context)
        {
            _context = context;
        }

        private async Task<decimal> GetConfigValueAsync(string key)
        {
            var config = await _context.ApplicationConfig
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Key == key);

            if (config == null || !decimal.TryParse(config.Value, out var value))
                return 0;

            return value;
        }


            public async Task<ExchangeRebateResultDto> GetExchangeRebateAsync(ExchangeRebateRequestDTO request)
            {
                var usdLimit = await GetConfigValueAsync("USDlimit");
                var sarLimit = await GetConfigValueAsync("SARlimit");
                var rebateAmountPerTransaction = await GetConfigValueAsync("RebateAmountPerTransaction");

                decimal usdAmountThreshold = (decimal)(request.ExchangeRateUSD * usdLimit);

                var baseQuery = _context.RemittanceInfos
                    .AsNoTracking()
                    .Where(x => x.Date >= request.FromDate && x.Date <= request.ToDate.AddDays(1));

                if (request.AgentId.HasValue)
                {
                    baseQuery = baseQuery.Where(x => x.AgentId == request.AgentId.Value);
                }

                var rows = await baseQuery
                    .Select(x => new { x.AgentId, x.Date, x.DataJson, x.Status, x.PostingTime })
                    .ToListAsync();

                var agentIds = rows.Select(r => r.AgentId).Distinct().ToList();
                var agentNames = await _context.AcquisitionAgents
                    .AsNoTracking()
                    .Where(a => agentIds.Contains(a.Id))
                    .ToDictionaryAsync(a => a.Id, a => a.AgentName);

                var parsedRows = new List<(DateTime? Date, decimal Amount, ExchangeRebateItemDto Item)>();

                foreach (var row in rows)
                {
                    if (string.IsNullOrEmpty(row.DataJson))
                        continue;

                    try
                    {
                        using var doc = JsonDocument.Parse(row.DataJson);
                        var root = doc.RootElement;

                        if (!root.TryGetProperty("Amount", out var amountElement))
                            continue;

                        if (!amountElement.TryGetDecimal(out var amount))
                            continue;

                        var item = new ExchangeRebateItemDto
                        {
                            XPIN = root.TryGetProperty("XPIN", out var xpin) ? xpin.GetString() : null,
                            BeneficiaryName = root.TryGetProperty("BeneficiaryName", out var bn) ? bn.GetString() : null,
                            BeneficiaryAccountNumber = root.TryGetProperty("BeneficiaryAccountNumber", out var ban) ? ban.GetString() : null,
                            TransactionDate = row.Date.HasValue ? row.Date.Value.ToString("dd/MM/yy") : null,
                            RemitterName = root.TryGetProperty("RemitterName", out var rn) ? rn.GetString() : null,
                            RemitterPassportNumber = root.TryGetProperty("PassportNumberOfRemitter", out var pn) ? pn.GetString() : null,
                            CountryCode = root.TryGetProperty("CountryCode", out var cc) ? cc.GetString() : null,
                            AgentName = agentNames.TryGetValue(row.AgentId, out var name) ? name : null,
                            Amount = amount,
                            Status = row.Status switch
                            {
                                "P" => "Pending",
                                "A" => "Approved",
                                "RE" => "Rejected",
                                "AML" => "AntiMoneyLoundering",
                                "R" => "Return",
                                "U" => "UnAuthorize"
                            },

                            PostingTime = DateTime.TryParse(row.PostingTime, out var pt) ? pt.ToString("dd/MM/yy") : null,
                            UsdLimit = usdLimit,
                            SarLimit = sarLimit,
                        };

                        parsedRows.Add((row.Date, amount, item));
                    }
                    catch (JsonException)
                    {
                        continue;
                    }
                }

                var details = new List<ExchangeRebateItemDto>();

                //  Single transaction ka amount khud threshold se >= ho
                var individualMatches = parsedRows.Where(r => r.Amount >= usdAmountThreshold).ToList();
                details.AddRange(individualMatches.Select(r => r.Item));

                // Same account, current date, multiple transactions,
                // aur combined sum threshold >= ho to hi rebate eligible
                var remaining = parsedRows.Where(r => r.Amount < usdAmountThreshold).ToList();

                var groupedByAccount = remaining
                    .Where(r => r.Date.HasValue
                                && r.Date.Value.Date == DateTime.Today
                                && !string.IsNullOrEmpty(r.Item.AccountNumber))
                    .GroupBy(r => r.Item.AccountNumber)
                    .Where(g => g.Count() > 1 && g.Sum(x => x.Amount) >= usdAmountThreshold);

                foreach (var group in groupedByAccount)
                {
                    details.AddRange(group.Select(r => r.Item));
                }

                var totalCount = details.Count;
                decimal totalAmount = details.Sum(d => d.Amount);

                decimal sarRebate = totalCount * sarLimit;
                decimal rebatePkr = sarRebate * request.ExchangeRateSAR;

            int pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
            int pageSize = request.PageSize < 1 ? 10 : request.PageSize;

            var pagedItems = details
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new ExchangeRebateResultDto
                {
                    TotalTxns = totalCount,
                    TotalRebateSAR = sarRebate,
                    TotalRebatePKR = rebatePkr,
                    TotalPKR = totalAmount,
                    ExchangeRateSAR = request.ExchangeRateSAR,
                    ExchangeRateUSD = request.ExchangeRateUSD,
                //Items = details,
                Items = new PagedResult<ExchangeRebateItemDto>
                {
                    Items = pagedItems,
                    TotalCount = totalCount
                },
            };
            }


        public async Task<AgentRebateSharingResultDto> GetAgentRebateSharingAsync(ExchangeRebateRequestDTO request)
        {
            var usdLimit = await GetConfigValueAsync("USDlimit");
            var sarLimit = await GetConfigValueAsync("SARlimit");

            decimal usdAmountThreshold = (decimal)(request.ExchangeRateUSD * usdLimit);

            var rows = await _context.RemittanceInfos
                .AsNoTracking()
                .Where(x =>
                    x.Date >= request.FromDate &&
                    x.Date <= request.ToDate.AddDays(-1))
                .Select(x => new
                {
                    x.AgentId,
                    x.DataJson,
                    x.Status,
                    x.Date
                })
                .ToListAsync();

            int totalTransactions = rows.Count;
            int eligibleTransactionCount = 0;

            foreach (var row in rows)
            {
                if (string.IsNullOrEmpty(row.DataJson))
                    continue;

                try
                {
                    using var doc = JsonDocument.Parse(row.DataJson);
                    var root = doc.RootElement;

                    if (!root.TryGetProperty("Amount", out var amountElement))
                        continue;

                    decimal amount = amountElement.GetDecimal();

                    if (amount >= usdAmountThreshold)
                    {
                        eligibleTransactionCount++;
                    }
                }
                catch
                {
                    continue;
                }
            }

            decimal totalRebateSAR = eligibleTransactionCount * sarLimit;
            decimal totalRebatePKR = totalRebateSAR * request.ExchangeRateSAR;

            var agents = await _context.AcquisitionAgents
                .AsNoTracking()
                .Include(a => a.Country)
                .Select(a => new
                {
                    a.Id,
                    a.AgentName,
                    a.RebateSharing,
                    CountryName = a.Country != null ? a.Country.CountryName : null
                })
                .ToListAsync();

            int totalAgents = agents.Count;

            var agentItems = new List<AgentRebateSharingItemDto>();

            foreach (var agent in agents)
            {
                var agentRows = rows.Where(r => r.AgentId == agent.Id).ToList();

                int agentTotalTransactions = agentRows.Count;
                int agentEligibleCount = 0;
                string agentStatus = null;
                DateTime? latestDate = null;

                foreach (var row in agentRows)
                {
                    if (string.IsNullOrEmpty(row.DataJson))
                        continue;

                    try
                    {
                        using var doc = JsonDocument.Parse(row.DataJson);
                        var root = doc.RootElement;

                        if (!root.TryGetProperty("Amount", out var amountElement))
                            continue;

                        decimal amount = amountElement.GetDecimal();

                        if (amount >= usdAmountThreshold)
                        {
                            agentEligibleCount++;
                        }
                    }
                    catch
                    {
                        continue;
                    }

                
                }

                decimal agentRebateSAR = agentEligibleCount * sarLimit;
                decimal agentRebatePKR = agentRebateSAR * request.ExchangeRateSAR;

                agentItems.Add(new AgentRebateSharingItemDto
                {
                    AgentId = agent.Id,
                    AgentName = agent.AgentName,
                    CountryName = agent.CountryName,
                    TotalTransactionCount = agentTotalTransactions,
                    EligibleTransactionCount = agentEligibleCount,
                    RebateSAR = agentRebateSAR,
                    RebatePKR = agentRebatePKR,
                    SharingPercentage = agent.RebateSharing ?? 0,
                    AgentShare = agentRebatePKR * ((agent.RebateSharing ?? 0) / 100),
                    
                });
            }

            return new AgentRebateSharingResultDto
            {
                TotalTransactions = totalTransactions,
                EligibleTransactionCount = eligibleTransactionCount,
                TotalRebateSAR = totalRebateSAR,
                TotalRebatePKR = totalRebatePKR,
                TotalAgents = totalAgents,
                Agents = agentItems
            };
        }

        public async Task<AgentRebateSharingDetailResultDto> GetAgentRebateSharingByIdAsync(Guid agentId, AgentRebateDetailRequestDTO request)
        {
            var usdLimit = await GetConfigValueAsync("USDlimit");
            var sarLimit = await GetConfigValueAsync("SARlimit");

            decimal usdAmountThreshold = (decimal)(request.ExchangeRateUSD * usdLimit);

      
            var agent = await _context.AcquisitionAgents
                .AsNoTracking()
                .Include(a => a.Country)
                .Where(a => a.Id == agentId)
                .Select(a => new
                {
                    a.Id,
                    a.AgentName,
                    a.RebateSharing,
                    CountryName = a.Country != null ? a.Country.CountryName : null
                })
                .FirstOrDefaultAsync();

            if (agent == null)
                throw new InvalidOperationException("Agent not found.");

            var rows = await _context.RemittanceInfos
                .AsNoTracking()
                .Where(x =>
                    x.AgentId == agentId &&
                    x.Date >= request.FromDate &&
                    x.Date <= request.ToDate.AddDays(-1))
                .Select(x => new
                {
                    x.DataJson,
                    x.Date
                })
                .ToListAsync();

            var transactions = new List<AgentRebateTransactionItemDto>();

            foreach (var row in rows)
            {
                if (string.IsNullOrEmpty(row.DataJson))
                    continue;

                try
                {
                    using var doc = JsonDocument.Parse(row.DataJson);
                    var root = doc.RootElement;

                    if (!root.TryGetProperty("Amount", out var amountElement))
                        continue;

                    decimal amount = amountElement.GetDecimal();

                    if (amount < usdAmountThreshold)
                        continue;

                    string xpin = root.TryGetProperty("XPIN", out var xpinEl) ? xpinEl.GetString() : null;
                    string beneficiaryName = root.TryGetProperty("BeneficiaryName", out var bnEl) ? bnEl.GetString() : null;

                    decimal rebatePKR = sarLimit * request.ExchangeRateSAR;
                    decimal agentSharePKR = rebatePKR * ((agent.RebateSharing ?? 0) / 100);

                    transactions.Add(new AgentRebateTransactionItemDto
                    {
                        xpin = xpin,
                        Date = row.Date.HasValue ? row.Date.Value.ToString("dd/MM/yyyy") : null,
                        Beneficiary = beneficiaryName,
                        AmountPKR = amount,
                        RebatePKR = rebatePKR,
                        agentshare = agentSharePKR
                    });
                }
                catch
                {
                    continue;
                }
            }

            decimal totalAmountPKR = transactions.Sum(t => t.AmountPKR);
            decimal totalRebatePKR = transactions.Sum(t => t.RebatePKR);
            decimal totalAgentSharePKR = transactions.Sum(t => t.agentshare);

            return new AgentRebateSharingDetailResultDto
            {
                AgentId = agent.Id,
                AgentName = agent.AgentName,
                CountryName = agent.CountryName,
                Transactions = transactions,
                TotalAmountPKR = totalAmountPKR,
                TotalRebatePKR = totalRebatePKR,
                TotalAgentSharePKR = totalAgentSharePKR,
                SharingPercent = agent.RebateSharing ?? 0,
            };
        }


    }
}
