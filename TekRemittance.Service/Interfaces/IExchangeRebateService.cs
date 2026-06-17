using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekRemittance.Repository.Models.dto;

namespace TekRemittance.Service.Interfaces
{
    public interface IExchangeRebateService
    {
        Task<ExchangeRebateResultDto> GetExchangeRebateAsync(ExchangeRebateRequestDTO request);

        Task<AgentRebateSharingResultDto> GetAgentRebateSharingAsync(ExchangeRebateRequestDTO request);
        Task<AgentRebateSharingDetailResultDto> GetAgentRebateSharingByIdAsync(Guid agentId, AgentRebateDetailRequestDTO request);

    }
}
