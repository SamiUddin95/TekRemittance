using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;

namespace TekRemittance.Service.Interfaces
{
    public interface IBasicSetupService
    {
        Task<IEnumerable<Country>> GetAllCountriesAsync();
        Task<Country?> GetCountryByIdAsync(Guid id);
        Task<Country> CreateCountryAsync(Country country);
        Task<Country?> UpdateCountryAsync(Country country);
        Task<bool> DeleteCountryAsync(Guid id);
    }
}
