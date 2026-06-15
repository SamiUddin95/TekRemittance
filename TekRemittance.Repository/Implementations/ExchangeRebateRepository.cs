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
                .Where(x => x.Date >= request.FromDate && x.Date <= request.ToDate);

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
                            && !string.IsNullOrEmpty(r.Item.BeneficiaryAccountNumber))
                .GroupBy(r => r.Item.BeneficiaryAccountNumber)
                .Where(g => g.Count() > 1 && g.Sum(x => x.Amount) >= usdAmountThreshold);

            foreach (var group in groupedByAccount)
            {
                details.AddRange(group.Select(r => r.Item));
            }

            var totalCount = details.Count;
            decimal totalAmount = details.Sum(d => d.Amount);

            decimal sarRebate = totalCount * sarLimit;
            decimal rebatePkr = sarRebate * request.ExchangeRateSAR;

            return new ExchangeRebateResultDto
            {
                TotalTxns = totalCount,
                TotalRebateSAR = sarRebate,
                TotalRebatePKR = rebatePkr,
                TotalPKR = totalAmount,
                ExchangeRateSAR = request.ExchangeRateSAR,
                ExchangeRateUSD = request.ExchangeRateUSD,
                Items = details,
            };
        }

    }
}
