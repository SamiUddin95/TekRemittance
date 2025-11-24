using System;
using System.Threading.Tasks;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Repository.Models.dto;
using TekRemittance.Service.Interfaces;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Service.Implementations
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _repo;

        public PermissionService(IPermissionRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<PermissionDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? name = null)
        {
            return await _repo.GetAllAsync(pageNumber, pageSize, name);
        }

        public async Task<PermissionDTO?> GetByIdAsync(Guid id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<PermissionDTO> CreateAsync(PermissionDTO dto)
        {
            return await _repo.AddAsync(dto);
        }

        public async Task<PermissionDTO?> UpdateAsync(PermissionDTO dto)
        {
            return await _repo.UpdateAsync(dto);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
