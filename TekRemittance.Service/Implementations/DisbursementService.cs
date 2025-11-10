using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Repository.Models.dto;
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
        public async Task<PagedResult<RemitttanceInfosStatusDTO>> GetByAgentIdWithStatusPAsync(Guid agentId, int pageNumber = 1, int pageSize = 50)
        {
            return await _repo.GetByAgentIdWithStatusPAsync(agentId, pageNumber, pageSize);
        }
        public async Task<PagedResult<RemitttanceInfosStatusDTO>> GetByAgentIdWithStatusUAsync(Guid agentId, int pageNumber = 1, int pageSize = 50)
        {
            return await _repo.GetByAgentIdWithStatusUAsync(agentId, pageNumber, pageSize);
        }
        public async Task<PagedResult<RemitttanceInfosStatusDTO>> GetByAgentIdWithStatusREAsync(Guid agentId, int pageNumber = 1, int pageSize = 50)
        {
            return await _repo.GetByAgentIdWithStatusREAsync(agentId, pageNumber, pageSize);
        }
        public async Task<PagedResult<RemitttanceInfosStatusDTO>> GetByAgentIdWithStatusRAsync(Guid agentId, int pageNumber = 1, int pageSize = 50)
        {
            return await _repo.GetByAgentIdWithStatusRAsync(agentId, pageNumber, pageSize);
        }
        public async Task<PagedResult<RemitttanceInfosStatusDTO>> GetByAgentIdWithStatusAAsync(Guid agentId, int pageNumber = 1, int pageSize = 50)
        {
            return await _repo.GetByAgentIdWithStatusAAsync(agentId, pageNumber, pageSize);
        }

    }


}