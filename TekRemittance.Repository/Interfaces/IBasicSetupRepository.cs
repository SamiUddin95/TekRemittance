using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Repository.Interfaces
{
    public interface IBasicSetupRepository
    {
        //Country
        Task<PagedResult<countryDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
        Task<countryDTO?> GetByIdAsync(Guid id);
        Task<countryDTO> AddAsync(countryDTO country);
        Task<Country?> UpdateAsync(countryDTO country);
        Task<bool> DeleteAsync(Guid id);

        //Province
        Task<PagedResult<provinceDTO>> GetAllProvinceAsync(int pageNumber = 1, int pageSize = 10);
        Task<provinceDTO?> GetProvinceByIdAsync(Guid id);
        Task<provinceDTO> AddProvinceAsync(provinceDTO province);
        Task<Province?> UpdateProvinceAsync(provinceDTO province);
        Task<bool> DeleteProvinceAsync(Guid id);

        //City
        Task<PagedResult<cityDTO>> GetAllCityAsync(int pageNumber = 1, int pageSize = 10);
        Task<cityDTO?> GetCityByIdAsync(Guid id);
        Task<cityDTO> AddCityAsync(cityDTO city);
        Task<City?> UpdateCityAsync(cityDTO city);
        Task<bool> DeleteCityAsync(Guid id);

        //Bank
        Task<PagedResult<bankDTO>> GetAllBankAsync(int pageNumber = 1, int pageSize = 10);
        Task<bankDTO?> GetBankByIdAsync(Guid id);
        Task<bankDTO> AddBankAsync(bankDTO bank);
        Task<Bank?> UpdateBankAsync(bankDTO bank);
        Task<bool> DeleteBankAsync(Guid id);
    }
}
