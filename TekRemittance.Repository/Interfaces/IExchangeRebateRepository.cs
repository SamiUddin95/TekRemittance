using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekRemittance.Repository.Models.dto;

namespace TekRemittance.Repository.Interfaces
{
    public interface IExchangeRebateRepository
    {
        Task<ExchangeRebateResultDto> GetExchangeRebateAsync(ExchangeRebateRequestDTO request);
        Task<AgentRebateSharingResultDto> GetAgentRebateSharingAsync(ExchangeRebateRequestDTO request);
        Task<AgentRebateSharingDetailResultDto> GetAgentRebateSharingByIdAsync(Guid agentId, AgentRebateDetailRequestDTO request);

    }
}
