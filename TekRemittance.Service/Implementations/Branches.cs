using System;
using System.Threading.Tasks;
using TekRemittance.Repository.DTOs;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Service.Interfaces;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Service.Implementations
{
    public class BranchesService : IBranchesService
    {
        private readonly IBranchesRepository _repo;

        public BranchesService(IBranchesRepository repo)
        {
            _repo = repo;
        }
        public async Task<PagedResult<BranchDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _repo.GetAllAsync(pageNumber, pageSize);
        }
        public async Task<BranchDTO?> GetByIdAsync(Guid id)
        {
            return await _repo.GetByIdAsync(id);
        }
        public async Task<BranchDTO> CreateAsync(BranchDTO dto)
        {
            return await _repo.AddAsync(dto);
        }
        public async Task<BranchDTO?> UpdateAsync(BranchDTO dto)
        {
            return await _repo.UpdateAsync(dto);
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
