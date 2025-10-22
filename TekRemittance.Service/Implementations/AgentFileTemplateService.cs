using System;
using System.Threading.Tasks;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Service.Interfaces;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Service.Implementations
{
    public class AgentFileTemplateService : IAgentFileTemplateService
    {
        private readonly IAgentFileTemplateRepository _repo;
        public AgentFileTemplateService(IAgentFileTemplateRepository repo)
        {
            _repo = repo;
        }

        public Task<agentFileTemplateDTO?> GetByAgentIdAsync(Guid agentId) => _repo.GetByAgentIdAsync(agentId);
        public Task<agentFileTemplateDTO> CreateAsync(agentFileTemplateDTO dto) => _repo.CreateAsync(dto);
        public Task<agentFileTemplateDTO?> UpdateAsync(agentFileTemplateDTO dto) => _repo.UpdateAsync(dto);
        public Task<bool> DeleteByAgentIdAsync(Guid agentId) => _repo.DeleteByAgentIdAsync(agentId);
    }
}
