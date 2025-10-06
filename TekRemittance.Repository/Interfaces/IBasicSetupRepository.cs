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
        Task<IEnumerable<countryDTO>> GetAllAsync();
        Task<countryDTO?> GetByIdAsync(Guid id);
        Task<countryDTO> AddAsync(countryDTO country);
        Task<Country?> UpdateAsync(countryDTO country);
        Task<bool> DeleteAsync(Guid id);

        //Province
        Task<IEnumerable<provinceDTO>> GetAllProvinceAsync();
        Task<provinceDTO?> GetProvinceByIdAsync(Guid id);
        Task<provinceDTO> AddProvinceAsync(provinceDTO province);
        Task<Province?> UpdateProvinceAsync(provinceDTO province);
        Task<bool> DeleteProvinceAsync(Guid id);


        //City
        Task<IEnumerable<cityDTO>> GetAllCityAsync();
        Task<cityDTO?> GetCityByIdAsync(Guid id);
        Task<cityDTO> AddCityAsync(cityDTO city);
        Task<City?> UpdateCityAsync(cityDTO city);
        Task<bool> DeleteCityAsync(Guid id);

        //Bank
        Task<IEnumerable<bankDTO>> GetAllBankAsync();
        Task<bankDTO?> GetBankByIdAsync(Guid id);
        Task<bankDTO> AddBankAsync(bankDTO bank);
        Task<Bank?> UpdateBankAsync(bankDTO bank);
        Task<bool> DeleteBankAsync(Guid id);
    }
}
