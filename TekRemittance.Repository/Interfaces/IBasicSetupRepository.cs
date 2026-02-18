using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Enums;
using TekRemittance.Repository.Models.dto;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Repository.Interfaces
{
    public interface IBasicSetupRepository
    {
        //Country
        Task<PagedResult<countryDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? countryCode = null, string? countryName = null, StatusesEnums? status = null);
        Task<countryDTO?> GetByIdAsync(Guid id);
        Task<countryDTO> AddAsync(countryDTO country);
        Task<Country?> UpdateAsync(countryDTO country);
        Task<bool> DeleteAsync(Guid id);

        //Province
        Task<PagedResult<provinceDTO>> GetAllProvinceAsync(int pageNumber = 1, int pageSize = 10, string? provinceCode = null, string? provinceName = null, StatusesEnums? status = null);
        Task<provinceDTO?> GetProvinceByIdAsync(Guid id);
        Task<provinceDTO> AddProvinceAsync(provinceDTO province);
        Task<Province?> UpdateProvinceAsync(provinceDTO province);
        Task<bool> DeleteProvinceAsync(Guid id);

        //City
        Task<PagedResult<cityDTO>> GetAllCityAsync(int pageNumber = 1, int pageSize = 10, string? cityCode = null, string? cityName = null, StatusesEnums? status = null);
        Task<cityDTO?> GetCityByIdAsync(Guid id);
        Task<cityDTO> AddCityAsync(cityDTO city);
        Task<City?> UpdateCityAsync(cityDTO city);
        Task<bool> DeleteCityAsync(Guid id);

        //Bank
        Task<PagedResult<bankDTO>> GetAllBankAsync(int pageNumber = 1, int pageSize = 10, string? bankCode = null, string? bankName = null, StatusesEnums? status = null);
        Task<bankDTO?> GetBankByIdAsync(Guid id);
        Task<bankDTO> AddBankAsync(bankDTO bank);
        Task<Bank?> UpdateBankAsync(bankDTO bank);
        Task<bool> DeleteBankAsync(Guid id);


        Task<PagedResult<AmlDataDTO>> GetAllAsync(int pageNumber, int pageSize, string? cnic, string? accountName);
        Task<AmlDataDTO?> GetByIdAsyncAml(Guid id);
        Task<AmlDataDTO> AddAsync(AmlDataDTO dto);
        Task<AmlData?> UpdateAsync(AmlDataDTO dto);
        Task<bool> DeleteAsyncAml(Guid id);

        //Task<AmlDataDTO> AddAsync(AmlDataDTO dto);
        Task<List<AmlDataDTO>> AddRangeAsync(List<AmlDataDTO> dtos);
    }
}