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

        public async Task<DashBoardsDTO> GetDisbursementDashboardAsync(string dateRange)
        {
            var query = _context.RemittanceInfos.AsQueryable();

            // Date filter
            DateTime endDate = DateTime.Today;
            DateTime startDate = DateTime.MinValue;

            if (!string.IsNullOrEmpty(dateRange))
            {
                switch (dateRange.ToLower())
                {
                    case "today":
                        startDate = DateTime.Today;
                        break;
                    case "weekly":
                        startDate = DateTime.Today.AddDays(-7);
                        break;
                    case "monthly":
                        startDate = DateTime.Today.AddMonths(-1);
                        break;
                    case "annual":
                        startDate = DateTime.Today.AddYears(-1);
                        break;
                }
            }

            if (startDate != DateTime.MinValue)
            {
                query = query.Where(x => x.Date.HasValue
                                         && x.Date.Value.Date >= startDate
                                         && x.Date.Value.Date <= endDate);
            }

            
            decimal amlAmount = query
                .Where(x => x.Status == "AML")
                .AsEnumerable()
                .Sum(x => ExtractAmount(x.DataJson));

            decimal rejectAmount = query
                .Where(x => x.Status == "RE")
                .AsEnumerable()
                .Sum(x => ExtractAmount(x.DataJson));

            decimal repairAmount = query
                .Where(x => x.Status == "R")
                .AsEnumerable()
                .Sum(x => ExtractAmount(x.DataJson));

            return new DashBoardsDTO
            {
                TotalAmount = amlAmount + rejectAmount + repairAmount
            };
        }
        public async Task<DashBoardsDTO> GetDisbursementSuccessAsync(string dateRange)
        {
            var query = _context.RemittanceInfos.AsQueryable();

            DateTime endDate = DateTime.Today;
            DateTime startDate = DateTime.MinValue;

            if (!string.IsNullOrEmpty(dateRange))
            {
                switch (dateRange.ToLower())
                {
                    case "today":
                        startDate = DateTime.Today;
                        break;
                    case "weekly":
                        startDate = DateTime.Today.AddDays(-7);
                        break;
                    case "monthly":
                        startDate = DateTime.Today.AddMonths(-1);
                        break;
                    case "annual":
                        startDate = DateTime.Today.AddYears(-1);
                        break;
                }
            }

            if (startDate != DateTime.MinValue)
            {
                query = query.Where(x => x.Date.HasValue
                                         && x.Date.Value.Date >= startDate
                                         && x.Date.Value.Date <= endDate);
            }

            decimal totalAmount = query
                .Where(x => x.Status == "A")
                .AsEnumerable()
                .Sum(x => ExtractAmount(x.DataJson));

            return new DashBoardsDTO
            {
                TotalAmount = totalAmount
            };
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

        public async Task<DisbursementCountDTO> GetDisbursementCountAsync(string dateRange)
        {
            var query = _context.RemittanceInfos.AsQueryable();

            DateTime today = DateTime.Today;
            DateTime startDate = DateTime.MinValue;

            if (!string.IsNullOrEmpty(dateRange))
            {
                if (dateRange.ToLower() == "today")
                {
                    startDate = today;
                }
                else if (dateRange.ToLower() == "weekly")
                {
                    startDate = today.AddDays(-7);
                }
                else if (dateRange.ToLower() == "monthly")
                {
                    startDate = today.AddMonths(-1);
                }
                else if (dateRange.ToLower() == "annual")
                {
                    startDate = today.AddYears(-1);
                }
            }

            if (startDate != DateTime.MinValue)
            {
                query = query.Where(x => x.Date != null && x.Date >= startDate);
            }

            int amlCount = query.Count(x => x.Status == "AML");
            int rejectCount = query.Count(x => x.Status == "RE");
            int repairCount = query.Count(x => x.Status == "R");

            return new DisbursementCountDTO
            {
                TotalCount = amlCount + rejectCount + repairCount
            };
        }

        public async Task<DisbursementCountDTO> GetDisbursementSuccessCountAsync(string dateRange)
        {
            var query = _context.RemittanceInfos.AsQueryable();

            DateTime today = DateTime.Today;
            DateTime startDate = DateTime.MinValue;

            if (!string.IsNullOrEmpty(dateRange))
            {
                if (dateRange.ToLower() == "today")
                {
                    startDate = today;
                }
                else if (dateRange.ToLower() == "weekly")
                {
                    startDate = today.AddDays(-7);
                }
                else if (dateRange.ToLower() == "monthly")
                {
                    startDate = today.AddMonths(-1);
                }
                else if (dateRange.ToLower() == "annual")
                {
                    startDate = today.AddYears(-1);
                }
            }

            if (startDate != DateTime.MinValue)
            {
                query = query.Where(x => !x.Date.HasValue || x.Date.Value.Date >= startDate.Date);
            }

            int amlCount = query.Count(x => x.Status == "A");
           

            return new DisbursementCountDTO
            {
                TotalCount = amlCount 
            };
        }


        public async Task<DisbursementSuccessPercentageDTO> GetDisbursementSuccessPercentageAsync(string? dateRange)
        {
            var query = _context.RemittanceInfos.AsQueryable();

            DateTime today = DateTime.Today;
            DateTime? startDate = null;

            if (!string.IsNullOrEmpty(dateRange))
            {
                switch (dateRange.ToLower())
                {
                    case "today":
                        startDate = today;
                        break;
                    case "weekly":
                        startDate = today.AddDays(-7);
                        break;
                    case "monthly":
                        startDate = today.AddMonths(-1);
                        break;
                    case "annual":
                        startDate = today.AddYears(-1);
                        break;
                }
            }

            
            if (startDate.HasValue)
            {
                query = query.Where(x => x.Date.HasValue && x.Date.Value.Date >= startDate.Value.Date);
            }

            int totalCount = await query.CountAsync();           
            int successCount = await query.CountAsync(x => x.Status == "A"); 

            decimal percentage = totalCount == 0 ? 0 : (decimal)successCount / totalCount * 100;

            return new DisbursementSuccessPercentageDTO
            {
                TotalCount = totalCount,
                SuccessCount = successCount,
                SuccessPercentage = Math.Round(percentage, 2) 
            };
        }







    }
}
