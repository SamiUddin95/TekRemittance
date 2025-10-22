using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Service.Interfaces;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Service.Implementations
{
    public class AgentFileTemplateFieldService : IAgentFileTemplateFieldService
    {
        private readonly IAgentFileTemplateFieldRepository _repo;
        public AgentFileTemplateFieldService(IAgentFileTemplateFieldRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<agentFileTemplateFieldDTO>> GetByTemplateIdAsync(Guid templateId) => _repo.GetByTemplateIdAsync(templateId);
        public Task<agentFileTemplateFieldDTO> CreateAsync(agentFileTemplateFieldDTO dto) => _repo.CreateAsync(dto);
        public Task<agentFileTemplateFieldDTO?> UpdateAsync(agentFileTemplateFieldDTO dto) => _repo.UpdateAsync(dto);
        public Task<bool> DeleteAsync(Guid id) => _repo.DeleteAsync(id);
    }
}
