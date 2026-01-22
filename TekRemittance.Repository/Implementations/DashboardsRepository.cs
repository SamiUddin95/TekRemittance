using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities.Data;
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

            return new
            {
                failedAmount,
                failedCount,
                successAmount,
                successCount,
                successPercentage
            };
        }

    }
}
