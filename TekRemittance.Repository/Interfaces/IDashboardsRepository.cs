using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Models.dto;

namespace TekRemittance.Repository.Interfaces
{
    public interface IDashboardsRepository
    {
       
        Task<object> GetDashboardDataAsync(string dateRange);

        Task<List<barGraphDto>> GetbarChartDataAsync(string dateRange);
        Task<TransactionModeCountDTO> GetTransactionModeCountsAsync(string dateRange);
        Task<List<RecentTransactionDTO>> GetLast10RemittancesAsync();
       
        Task<List<RecentTransactionDTO>> GetTransactionModeListAsync(string dateRange, string mode);

        Task<List<AgentPerformanceDTO>> GetAgentPerformanceAsync();
        Task<List<TopBankTransactionDTO>> GetTopBankTransactionAsync();
        Task<List<TransactionStatusByChannelDTO>> GetTransactionStatusByChannelAsync();
        Task<SummaryDTO> GetIncomingSummaryAsync();
        Task<SummaryDTO> GetOutgoingSummaryAsync();
        Task<List<EPRCDTO>> GetEPRCAsync();

        Task<List<ChannelsDTO>> GetChannelsAsync();




    }
}
