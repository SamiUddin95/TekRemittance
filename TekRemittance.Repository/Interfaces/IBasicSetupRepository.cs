using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;

namespace TekRemittance.Repository.Interfaces
{
    public interface IBasicSetupRepository
    {
        Task<IEnumerable<Country>> GetAllAsync();
        Task<Country?> GetByIdAsync(Guid id);
        Task<Country> AddAsync(Country country);
        Task<Country?> UpdateAsync(Country country);
        Task<bool> DeleteAsync(Guid id);
    }
}
