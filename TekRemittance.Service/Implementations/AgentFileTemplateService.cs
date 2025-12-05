using System;
using System.Threading.Tasks;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Repository.Models.dto;
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
        public Task<PagedResult<agentFileTemplateDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10,string? name = null, string? agentname = null, string? sheetname = null) => _repo.GetAllAsync(pageNumber, pageSize,name,agentname,sheetname);
        public Task<PagedResult<KeyValuePair<string, List<string>>>> GetDataByUploadIdAsync(Guid UploadId, int pageNumber = 1, int pageSize = 50)
            => _repo.GetDataByUploadIdAsync(UploadId, pageNumber, pageSize);

        
    }
}
