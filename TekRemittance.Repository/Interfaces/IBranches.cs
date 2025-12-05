using System;
using System.Threading.Tasks;
using TekRemittance.Repository.DTOs;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Repository.Interfaces
{
    public interface IBranchesRepository
    {
        Task<PagedResult<BranchDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? agentname = null, string? code = null, string? agentbranchname = null);
        Task<BranchDTO?> GetByIdAsync(Guid id);
        Task<BranchDTO> AddAsync(BranchDTO dto);
        Task<BranchDTO?> UpdateAsync(BranchDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
