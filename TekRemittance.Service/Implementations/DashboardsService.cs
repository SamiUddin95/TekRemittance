using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Repository.Models.dto;
using TekRemittance.Service.Interfaces;

namespace TekRemittance.Service.Implementations
{
    public class DashboardsService:IDashboardsService
    {

        private readonly IDashboardsRepository _repo;

        public DashboardsService(IDashboardsRepository repo)
        {
            _repo = repo;
        }
        

        public async Task<object> GetDashboardDataAsync(string dateRange)
        {
            return await _repo.GetDashboardDataAsync(dateRange);
        }


        public async Task<object> GetbarChartDataAsync(string dateRange)
        {
            return await _repo.GetbarChartDataAsync(dateRange);
        }

        public async Task<TransactionModeCountDTO> GetTransactionModeCountsAsync(string dateRange)
        {
            return await _repo.GetTransactionModeCountsAsync(dateRange);
        }

        public async Task<List<RecentTransactionDTO>> GetLast10RemittancesAsync()
        {
            return await _repo.GetLast10RemittancesAsync();
        }
      
        public async Task<List<RecentTransactionDTO>> GetTransactionModeListAsync(string dateRange, string mode)
        {
            return await _repo.GetTransactionModeListAsync(dateRange, mode);
        }


    }
}
