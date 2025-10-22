using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Service.Interfaces
{
    public interface IAgentFileTemplateFieldService
    {
        Task<IEnumerable<agentFileTemplateFieldDTO>> GetByTemplateIdAsync(Guid templateId);
        Task<agentFileTemplateFieldDTO> CreateAsync(agentFileTemplateFieldDTO dto);
        Task<agentFileTemplateFieldDTO?> UpdateAsync(agentFileTemplateFieldDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
