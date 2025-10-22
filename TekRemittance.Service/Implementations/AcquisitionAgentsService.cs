using System;
using System.Threading.Tasks;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Service.Interfaces;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Service.Implementations
{
    public class AcquisitionAgentsService : IAcquisitionAgentsService
    {
        private readonly IAcquisitionAgentsRepository _repo;
        public AcquisitionAgentsService(IAcquisitionAgentsRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<acquisitionAgentDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _repo.GetAllAsync(pageNumber, pageSize);
        }

        public async Task<acquisitionAgentDTO?> GetByIdAsync(Guid id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<acquisitionAgentDTO> CreateAsync(acquisitionAgentDTO dto)
        {
            return await _repo.AddAsync(dto);
        }

        public async Task<acquisitionAgentDTO?> UpdateAsync(acquisitionAgentDTO dto)
        {
            return await _repo.UpdateAsync(dto);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
