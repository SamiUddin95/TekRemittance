using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Service.Interfaces
{
    public interface IBasicSetupService
    {
        //Country
        Task<IEnumerable<countryDTO>> GetAllCountriesAsync();
        Task<countryDTO?> GetCountryByIdAsync(Guid id);
        Task<countryDTO> CreateCountryAsync(countryDTO country);
        Task<countryDTO?> UpdateCountryAsync(countryDTO country);
        Task<bool> DeleteCountryAsync(Guid id);

        //Province
        Task<IEnumerable<provinceDTO>> GetAllProvinceAsync();
        Task<provinceDTO?> GetProvinceByIdAsync(Guid id);
        Task<provinceDTO> CreateProvinceAsync(provinceDTO province);
        Task<provinceDTO?> UpdateProvinceAsync(provinceDTO province);
        Task<bool> DeleteProvinceAsync(Guid id);

        //City
        Task<IEnumerable<cityDTO>> GetAllCityAsync();
        Task<cityDTO?> GetCityByIdAsync(Guid id);
        Task<cityDTO> CreateCityAsync(cityDTO city);
        Task<cityDTO?> UpdateCityAsync(cityDTO city);
        Task<bool> DeleteCityAsync(Guid id);

        //Bank
        Task<IEnumerable<bankDTO>> GetAllBankAsync();
        Task<bankDTO?> GetBankByIdAsync(Guid id);
        Task<bankDTO> CreateBankAsync(bankDTO bank);
        Task<bankDTO?> UpdateBankAsync(bankDTO bank);
        Task<bool> DeleteBankAsync(Guid id);
    }
}
