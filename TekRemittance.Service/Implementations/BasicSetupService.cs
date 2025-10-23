using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Service.Interfaces;
using TekRemittance.Web.Models.dto;

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
        public async Task<PagedResult<countryDTO>> GetAllCountriesAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _repository.GetAllAsync(pageNumber, pageSize);
        }

        public async Task<countryDTO?> GetCountryByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<countryDTO> CreateCountryAsync(countryDTO country)
        {
            return await _repository.AddAsync(country);
        }

        public async Task<countryDTO?> UpdateCountryAsync(countryDTO country)
        {
            var updated = await _repository.UpdateAsync(country);
            if (updated == null) return null;

            return new countryDTO
            {
                Id = updated.Id,
                CountryCode = updated.CountryCode,
                CountryName = updated.CountryName,
                IsActive = updated.IsActive,
                CreatedBy = updated.CreatedBy,
                CreatedOn = updated.CreatedOn,
                UpdatedBy = updated.UpdatedBy,
                UpdatedOn = updated.UpdatedOn
            };
        }

        public async Task<bool> DeleteCountryAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }
        #endregion

        #region Bank
        public async Task<PagedResult<bankDTO>> GetAllBankAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _repository.GetAllBankAsync(pageNumber, pageSize);
        }

        public async Task<bankDTO?> GetBankByIdAsync(Guid id)
        {
            return await _repository.GetBankByIdAsync(id);
        }

        public async Task<bankDTO> CreateBankAsync(bankDTO bank)
        {
            return await _repository.AddBankAsync(bank);
        }

        public async Task<bankDTO?> UpdateBankAsync(bankDTO bank)
        {
            var updated = await _repository.UpdateBankAsync(bank);
            if (updated == null) return null;

            return new bankDTO
            {
                Id = updated.Id,
                BankCode = updated.BankCode,
                BankName = updated.BankName,
                IMD = updated.IMD,
                Website = updated.Website,
                Allases = updated.Allases,
                PhoneNo = updated.PhoneNo,
                Description = updated.Description,
                IsActive = updated.IsActive,
                CreatedBy = updated.CreatedBy,
                CreatedOn = updated.CreatedOn,
                UpdatedBy = updated.UpdatedBy,
                UpdatedOn = updated.UpdatedOn
            };
        }

        public async Task<bool> DeleteBankAsync(Guid id)
        {
            return await _repository.DeleteBankAsync(id);
        }
        #endregion
        #region Province
        public async Task<PagedResult<provinceDTO>> GetAllProvinceAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _repository.GetAllProvinceAsync(pageNumber, pageSize);
        }

        public async Task<provinceDTO?> GetProvinceByIdAsync(Guid id)
        {
            return await _repository.GetProvinceByIdAsync(id);
        }

        public async Task<provinceDTO> CreateProvinceAsync(provinceDTO province)
        {
            return await _repository.AddProvinceAsync(province);
        }

        public async Task<provinceDTO?> UpdateProvinceAsync(provinceDTO province)
        {
            var updated = await _repository.UpdateProvinceAsync(province);
            if (updated == null) return null;

            // Map entity -> DTO
            return new provinceDTO
            {
                Id = updated.Id,
                ProvinceCode = updated.ProvinceCode,
                ProvinceName = updated.ProvinceName,
                CountryId = updated.CountryId,
                IsActive = updated.IsActive,
                UpdatedBy = updated.UpdatedBy,
                UpdatedOn = updated.UpdatedOn
            };
        }

        public async Task<bool> DeleteProvinceAsync(Guid id)
        {
            return await _repository.DeleteProvinceAsync(id);
        }
        #endregion

        #region City
        public async Task<PagedResult<cityDTO>> GetAllCityAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _repository.GetAllCityAsync(pageNumber, pageSize);
        }

        public async Task<cityDTO?> GetCityByIdAsync(Guid id)
        {
            return await _repository.GetCityByIdAsync(id);
        }

        public async Task<cityDTO> CreateCityAsync(cityDTO city)
        {
            return await _repository.AddCityAsync(city);
        }

        public async Task<cityDTO?> UpdateCityAsync(cityDTO city)
        {
            var updated = await _repository.UpdateCityAsync(city);
            if (updated == null) return null;

            return new cityDTO
            {
                Id = updated.Id,
                CityCode = updated.CityCode,
                CityName = updated.CityName,
                CountryId = updated.CountryId,
                ProvinceId = updated.ProvinceId,
                IsActive = updated.IsActive,
                UpdatedBy = updated.UpdatedBy,
                UpdatedOn = updated.UpdatedOn,
                CreatedBy = updated.CreatedBy,
                CreatedOn = updated.CreatedOn
            };
        }

        public async Task<bool> DeleteCityAsync(Guid id)
        {
            return await _repository.DeleteCityAsync(id);
        }
        #endregion

    }
}
