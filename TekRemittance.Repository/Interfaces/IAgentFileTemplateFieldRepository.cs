using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Repository.Interfaces
{
    public interface IAgentFileTemplateFieldRepository
    {
        Task<IEnumerable<agentFileTemplateFieldDTO>> GetByTemplateIdAsync(Guid templateId);
        Task<agentFileTemplateFieldDTO> CreateAsync(agentFileTemplateFieldDTO dto);
        Task<agentFileTemplateFieldDTO?> UpdateAsync(agentFileTemplateFieldDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
