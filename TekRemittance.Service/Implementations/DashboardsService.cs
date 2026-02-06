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
        public async Task<List<AgentPerformanceDTO>> GetAgentPerformanceAsync()
        {
            return await _repo.GetAgentPerformanceAsync();
        }
        public async Task<List<TopBankTransactionDTO>> GetTopBankTransactionAsync()
        {
            return await _repo.GetTopBankTransactionAsync();
        }
        public async Task<List<TransactionStatusByChannelDTO>> GetTransactionStatusByChannelAsync()
        {
            return await _repo.GetTransactionStatusByChannelAsync();
        }
        public async Task<SummaryDTO> GetIncomingSummaryAsync()
        {
            return await _repo.GetIncomingSummaryAsync();
        }
        public async Task<SummaryDTO> GetOutgoingSummaryAsync()
        {
            return await _repo.GetOutgoingSummaryAsync();
        }
        public async Task<List<EPRCDTO>> GetEPRCAsync()
        {
            return await _repo.GetEPRCAsync();
        }
        public async Task<List<ChannelsDTO>> GetChannelsAsync()
        {
            return await _repo.GetChannelsAsync();
        }






    }
}
