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
    public class ExchangeRebateService : IExchangeRebateService
    {
        private readonly IExchangeRebateRepository _repository;
        public ExchangeRebateService(IExchangeRebateRepository repository)
        {
            _repository = repository;
        }
        public async Task<ExchangeRebateResultDto> GetExchangeRebateAsync(ExchangeRebateRequestDTO request)
        {
            return await _repository.GetExchangeRebateAsync(request);
        }
        public async Task<AgentRebateSharingResultDto> GetAgentRebateSharingAsync(ExchangeRebateRequestDTO request)
        {
            return await _repository.GetAgentRebateSharingAsync(request);
        }
        public async Task<AgentRebateSharingDetailResultDto> GetAgentRebateSharingByIdAsync(Guid agentId, AgentRebateDetailRequestDTO request)
        {
            return await _repository.GetAgentRebateSharingByIdAsync(agentId, request);
        }
    }
}
