using System;
using System.Threading.Tasks;
using TekRemittance.Repository.DTOs;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Service.Interfaces
{
    public interface IBranchesService
    {
        Task<PagedResult<BranchDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
        Task<BranchDTO?> GetByIdAsync(Guid id);
        Task<BranchDTO> CreateAsync(BranchDTO dto);
        Task<BranchDTO?> UpdateAsync(BranchDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
