using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public async Task<DashBoardsDTO> GetDisbursementDashboardAsync(string dateRange)
        {
            return await _repo.GetDisbursementDashboardAsync(dateRange);
        }
        public async Task<DashBoardsDTO> GetDisbursementDashboardSuccessAsync(string dateRange)
        {
            return await _repo.GetDisbursementSuccessAsync(dateRange);
        }



        public async Task<DisbursementCountDTO> GetDisbursementCountAsync(string dateRange)
        {
            return await _repo.GetDisbursementCountAsync(dateRange);
        }

        public async Task<DisbursementCountDTO> GetDisbursementSuccessCountAsync(string dateRange)
        {
            return await _repo.GetDisbursementSuccessCountAsync(dateRange);
        }

        public async Task<DisbursementSuccessPercentageDTO> GetDisbursementSuccessPercentageAsync(string? dateRange)
        {
            return await _repo.GetDisbursementSuccessPercentageAsync(dateRange);
        }



    }
}
