using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekRemittance.Repository.Models.dto;

namespace TekRemittance.Repository.Interfaces
{
    public interface IDashboardsRepository
    {
        Task<DashBoardsDTO> GetDisbursementDashboardAsync(string dateRange);
        Task<DisbursementCountDTO> GetDisbursementCountAsync(string dateRange);
        Task<DashBoardsDTO> GetDisbursementSuccessAsync(string dateRange);
        Task<DisbursementCountDTO> GetDisbursementSuccessCountAsync(string dateRange);

        Task<DisbursementSuccessPercentageDTO> GetDisbursementSuccessPercentageAsync(string? dateRange);

    }
}
