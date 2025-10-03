using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Service.Interfaces;

namespace TekRemittance.Service.Implementations
{
    public class BasicSetupService : IBasicSetupService
    {
        private readonly IBasicSetupRepository _repository;

        public BasicSetupService(IBasicSetupRepository repository)
        {
            _repository = repository;
        }

        #region Country
        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Country?> GetCountryByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Country> CreateCountryAsync(Country country)
        {
            return await _repository.AddAsync(country);
        }

        public async Task<Country?> UpdateCountryAsync(Country country)
        {
            return await _repository.UpdateAsync(country);
        }

        public async Task<bool> DeleteCountryAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }
        #endregion

        #region Province
        public async Task<IEnumerable<Province>> GetAllProvinceAsync()
        {
            return await _repository.GetAllProvinceAsync();
        }

        public async Task<Province?> GetProvinceByIdAsync(Guid id)
        {
            return await _repository.GetProvinceByIdAsync(id);
        }

        public async Task<Province> CreateProvinceAsync(Province province)
        {
            return await _repository.AddProvinceAsync(province);
        }

        public async Task<Province?> UpdateProvinceAsync(Province province)
        {
            return await _repository.UpdateProvinceAsync(province);
        }

        public async Task<bool> DeleteProvinceAsync(Guid id)
        {
            return await _repository.DeleteProvinceAsync(id);
        }
        #endregion

        #region City
        public async Task<IEnumerable<City>> GetAllCityAsync()
        {
            return await _repository.GetAllCityAsync();
        }

        public async Task<City?> GetCityByIdAsync(Guid id)
        {
            return await _repository.GetCityByIdAsync(id);
        }

        public async Task<City> CreateCityAsync(City city)
        {
            return await _repository.AddCityAsync(city);
        }

        public async Task<City?> UpdateCityAsync(City city)
        {
            return await _repository.UpdateCityAsync(city);
        }

        public async Task<bool> DeleteCityAsync(Guid id)
        {
            return await _repository.DeleteCityAsync(id);
        }
        #endregion

    }
}
