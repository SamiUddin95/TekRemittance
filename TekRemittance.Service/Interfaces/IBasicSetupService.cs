using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Enums;
using TekRemittance.Repository.Models.dto;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Service.Interfaces
{
    public interface IBasicSetupService
    {
        //Country
        Task<PagedResult<countryDTO>> GetAllCountriesAsync(int pageNumber = 1, int pageSize = 10, string? countryCode = null, string? countryName = null, StatusesEnums? status = null);
        Task<countryDTO?> GetCountryByIdAsync(Guid id);
        Task<countryDTO> CreateCountryAsync(countryDTO country);
        Task<countryDTO?> UpdateCountryAsync(countryDTO country);
        Task<bool> DeleteCountryAsync(Guid id);

        //Province
        Task<PagedResult<provinceDTO>> GetAllProvinceAsync(int pageNumber = 1, int pageSize = 10, string? provinceCode = null, string? provinceName = null, StatusesEnums? status = null);
        Task<provinceDTO?> GetProvinceByIdAsync(Guid id);
        Task<provinceDTO> CreateProvinceAsync(provinceDTO province);
        Task<provinceDTO?> UpdateProvinceAsync(provinceDTO province);
        Task<bool> DeleteProvinceAsync(Guid id);

        //City
        Task<PagedResult<cityDTO>> GetAllCityAsync(int pageNumber = 1, int pageSize = 10, string? cityCode = null, string? cityName = null, StatusesEnums? status = null);
        Task<cityDTO?> GetCityByIdAsync(Guid id);
        Task<cityDTO> CreateCityAsync(cityDTO city);
        Task<cityDTO?> UpdateCityAsync(cityDTO city);
        Task<bool> DeleteCityAsync(Guid id);

        //Bank
        Task<PagedResult<bankDTO>> GetAllBankAsync(int pageNumber = 1, int pageSize = 10, string? bankCode = null, string? bankName = null, StatusesEnums? status = null);
        Task<bankDTO?> GetBankByIdAsync(Guid id);
        Task<bankDTO> CreateBankAsync(bankDTO bank);
        Task<bankDTO?> UpdateBankAsync(bankDTO bank);
        Task<bool> DeleteBankAsync(Guid id);


        //AmlData

        Task<PagedResult<AmlDataDTO>> GetAllAmlDataAsync(int pageNumber, int pageSize, string? cnic, string? accountName);
        Task<AmlDataDTO?> GetAmlDataByIdAsync(Guid id);
        Task<AmlDataDTO> CreateAmlDataAsync(AmlDataDTO dto);
        Task<AmlDataDTO?> UpdateAmlDataAsync(AmlDataDTO dto);
        Task<bool> DeleteAmlDataAsync(Guid id);
    }
}
