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
        public async Task<PagedResult<KeyValuePair<string, List<string>>>> GetDataByAgentIdAsync(Guid agentId, int pageNumber = 1, int pageSize = 10)
        {
            return await _repo.GetDataByAgentIdAsync(agentId, pageNumber, pageSize);
        }
        public async Task<PagedResult<RemitttanceInfosStatusDTO>> GetByAgentIdWithStatusPAsync(Guid agentId, int pageNumber = 1, int pageSize = 10, string? accountnumber = null, string? xpin = null, string? date = null)
        {
            return await _repo.GetByAgentIdWithStatusPAsync(agentId, pageNumber, pageSize,accountnumber,xpin,date);
        }
        public async Task<PagedResult<RemitttanceInfosStatusDTO>> GetByAgentIdWithStatusUAsync(Guid agentId, int pageNumber = 1, int pageSize = 10, string? accountnumber = null, string? xpin = null, string? date = null)
        {
            return await _repo.GetByAgentIdWithStatusUAsync(agentId, pageNumber, pageSize,accountnumber,xpin,date);
        }
        public async Task<PagedResult<RemitttanceInfosStatusDTO>> GetByAgentIdWithStatusREAsync(Guid agentId, int pageNumber = 1, int pageSize = 10, string? accountnumber = null, string? xpin = null, string? date = null)
        {
            return await _repo.GetByAgentIdWithStatusREAsync(agentId, pageNumber, pageSize,accountnumber,xpin,date);
        }
        public async Task<PagedResult<RemitttanceInfosStatusDTO>> GetByAgentIdWithStatusRAsync(Guid agentId, int pageNumber = 1, int pageSize = 10, string? accountnumber = null, string? xpin = null, string? date = null)
        {
            return await _repo.GetByAgentIdWithStatusRAsync(agentId, pageNumber, pageSize,accountnumber,xpin,date);
        }
        public async Task<PagedResult<RemitttanceInfosStatusDTO>> GetByAgentIdWithStatusAAsync(Guid agentId, int pageNumber = 1, int pageSize = 10, string? accountnumber = null, string? xpin = null, string? date = null)
        {
            return await _repo.GetByAgentIdWithStatusAAsync(agentId, pageNumber, pageSize,accountnumber,xpin,date);
        }
        public async Task<(bool isSuccess, string message,string Xpin)> RemitApproveAsync(string xpin, Guid? userId)
        {
            return await _repo.RemitApproveAsync(xpin, userId);
        }
        public async Task<RemittanceInfoModelDTO> RemitRejectAsync(string xpin, Guid? userId)
        {
            return await _repo.RemitRejectAsync(xpin, userId);
        }

        public async Task<RemittanceInfoModelDTO> RemitAuthorizeAsync(string xpin, Guid? userId)
        {
            return await _repo.RemitAuthorizeAsync(xpin, userId);
        }

        public async Task<RemittanceInfoModelDTO> RemitRepairAsync(string xpin, Guid? userId)
        {
            return await _repo.RemitRepairAsync(xpin, userId);
        }

        public async Task<RemittanceInfoModelDTO> RemitReverseAsync(string xpin, Guid? userId)
        {
            return await _repo.RemitReverseAsync(xpin, userId);
        }
    }


}