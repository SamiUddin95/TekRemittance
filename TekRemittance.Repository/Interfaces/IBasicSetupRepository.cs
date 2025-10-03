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
        //Country
        Task<IEnumerable<Country>> GetAllAsync();
        Task<Country?> GetByIdAsync(Guid id);
        Task<Country> AddAsync(Country country);
        Task<Country?> UpdateAsync(Country country);
        Task<bool> DeleteAsync(Guid id);

        //Province
        Task<IEnumerable<Province>> GetAllProvinceAsync();
        Task<Province?> GetProvinceByIdAsync(Guid id);
        Task<Province> AddProvinceAsync(Province province);
        Task<Province?> UpdateProvinceAsync(Province province);
        Task<bool> DeleteProvinceAsync(Guid id);


        //City
        Task<IEnumerable<City>> GetAllCityAsync();
        Task<City?> GetCityByIdAsync(Guid id);
        Task<City> AddCityAsync(City province);
        Task<City?> UpdateCityAsync(City province);
        Task<bool> DeleteCityAsync(Guid id);
    }
}
