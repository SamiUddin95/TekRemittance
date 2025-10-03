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
    }
}
