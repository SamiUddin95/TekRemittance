using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekRemittance.Repository.Models.dto;

namespace TekRemittance.Service.Interfaces
{
    public interface IDashboardsService
    {
        Task<DashBoardsDTO> GetDisbursementDashboardAsync(string dateRange);
        Task<DisbursementCountDTO> GetDisbursementCountAsync(string dateRange);
        Task<DashBoardsDTO> GetDisbursementDashboardSuccessAsync(string dateRange);
        Task<DisbursementCountDTO> GetDisbursementSuccessCountAsync(string dateRange);
        Task<DisbursementSuccessPercentageDTO> GetDisbursementSuccessPercentageAsync(string? dateRange);



    }
}
