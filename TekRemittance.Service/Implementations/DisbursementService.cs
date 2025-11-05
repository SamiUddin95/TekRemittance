using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Service.Interfaces;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Service.Implementations
{
    public class DisbursementService : IDisbursementService
    {
        private readonly IDisbursementRepository _repo;

        public DisbursementService(IDisbursementRepository repo)
        {
            _repo = repo;
        }
        public async Task<PagedResult<KeyValuePair<string, List<string>>>> GetDataByAgentIdAsync(Guid agentId, int pageNumber = 1, int pageSize = 50)
        {
            return await _repo.GetDataByAgentIdAsync(agentId, pageNumber, pageSize);
        }
    }
}