using System.Collections.Generic;
using System.Threading.Tasks;
using TekRemittance.Repository.Models.dto;

namespace TekRemittance.Repository.Interfaces
{
    public interface IRemittanceInfoQueryRepository
    {
        Task<(IEnumerable<RemittanceInfoListItemDTO> Items, int TotalCount)> GetRemittanceInfosAsync(string? accountNumber, int pageNumber = 1, int pageSize = 50);
    }
}
