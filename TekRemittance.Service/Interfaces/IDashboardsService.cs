using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Models.dto;

namespace TekRemittance.Service.Interfaces
{
    public interface IDashboardsService
    {
       

        Task<object> GetDashboardDataAsync(string dateRange);

        Task<object> GetbarChartDataAsync(string dateRange);
        Task<TransactionModeCountDTO> GetTransactionModeCountsAsync(string dateRange);
        Task<List<RecentTransactionDTO>> GetLast10RemittancesAsync();

        Task<List<RemittanceInfo>> GetTransactionModeListAsync(string dateRange, string mode);




    }
}
