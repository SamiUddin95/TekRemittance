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
        //Country
        Task<IEnumerable<Country>> GetAllCountriesAsync();
        Task<Country?> GetCountryByIdAsync(Guid id);
        Task<Country> CreateCountryAsync(Country country);
        Task<Country?> UpdateCountryAsync(Country country);
        Task<bool> DeleteCountryAsync(Guid id);

        //Province
        Task<IEnumerable<Province>> GetAllProvinceAsync();
        Task<Province?> GetProvinceByIdAsync(Guid id);
        Task<Province> CreateProvinceAsync(Province province);
        Task<Province?> UpdateProvinceAsync(Province province);
        Task<bool> DeleteProvinceAsync(Guid id);

        //City
        Task<IEnumerable<City>> GetAllCityAsync();
        Task<City?> GetCityByIdAsync(Guid id);
        Task<City> CreateCityAsync(City city);
        Task<City?> UpdateCityAsync(City city);
        Task<bool> DeleteCityAsync(Guid id);
    }
}
