using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Enums;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Repository.Models.dto;
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
        public async Task<PagedResult<countryDTO>> GetAllCountriesAsync(int pageNumber = 1, int pageSize = 10, string? countryCode = null, string? countryName = null, StatusesEnums? status = null)
        {
            return await _repository.GetAllAsync(pageNumber, pageSize, countryCode, countryName, status);
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
        public async Task<PagedResult<bankDTO>> GetAllBankAsync(int pageNumber = 1, int pageSize = 10, string? bankCode = null, string? bankName = null, StatusesEnums? status = null)
        {
            return await _repository.GetAllBankAsync(pageNumber, pageSize,bankCode,bankName,status);
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
        public async Task<PagedResult<provinceDTO>> GetAllProvinceAsync(int pageNumber = 1, int pageSize = 10, string? provinceCode = null, string? provinceName = null, StatusesEnums? status = null)
        {
            return await _repository.GetAllProvinceAsync(pageNumber, pageSize,provinceCode,provinceName,status);
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
        public async Task<PagedResult<cityDTO>> GetAllCityAsync(int pageNumber = 1, int pageSize = 10, string? cityCode = null, string? cityName = null, StatusesEnums? status = null)
        {
            return await _repository.GetAllCityAsync(pageNumber, pageSize,cityCode,cityName,status);
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

        public async Task<PagedResult<AmlDataDTO>> GetAllAmlDataAsync(int pageNumber, int pageSize, string? cnic, string? accountName)
        {
            return await _repository.GetAllAsync(pageNumber, pageSize, cnic, accountName);
        }

        public async Task<AmlDataDTO?> GetAmlDataByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsyncAml(id);
        }

        public async Task<AmlDataDTO> CreateAmlDataAsync(AmlDataDTO dto)
        {
            return await _repository.AddAsync(dto);
        }

        public async Task<AmlDataDTO?> UpdateAmlDataAsync(AmlDataDTO dto)
        {
            var updated = await _repository.UpdateAsync(dto);

            if (updated == null) return null;

            return new AmlDataDTO
            {
                Id = updated.Id,
                CNIC = updated.CNIC,
                AccountName = updated.AccountName,
                Address = updated.Address,
                CreatedBy = updated.CreatedBy,
                CreatedOn = updated.CreatedOn,
                UpdatedBy = updated.UpdatedBy,
                UpdatedOn = updated.UpdatedOn
            };
        }

        public async Task<bool> DeleteAmlDataAsync(Guid id)
        {
            return await _repository.DeleteAsyncAml(id);
        }

        public async Task<List<AmlDataDTO>> ProcessAmlFileAsync(IFormFile file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            var extension = Path.GetExtension(file.FileName).ToLower();
            var amlDataList = new List<AmlDataDTO>();

            if (extension == ".csv")
            {
                using var reader = new StreamReader(file.OpenReadStream());
                bool skippedHeader = false;
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    
                    if (!skippedHeader)
                    {
                        skippedHeader = true;
                        continue;
                    }

                    var values = line.Split(',');
                    if (values.Length < 3) continue;

                    amlDataList.Add(new AmlDataDTO
                    {
                        CNIC = values[0].Trim(),
                        AccountName = values[1].Trim(),
                        Address = values[2].Trim()
                    });
                }
            }
            else if (extension == ".xlsx")
            {
                using var stream = file.OpenReadStream();
                using var wb = new ClosedXML.Excel.XLWorkbook(stream);
                var ws = wb.Worksheets.FirstOrDefault();
                if (ws == null) throw new InvalidOperationException("Worksheet not found");

                var table = ws.RangeUsed();
                if (table == null) throw new InvalidOperationException("Worksheet is empty");

                int startRow = table.RangeAddress.FirstAddress.RowNumber;
                int endRow = table.RangeAddress.LastAddress.RowNumber;

                bool skippedHeader = false;
                for (int r = startRow; r <= endRow; r++)
                {
                    if (!skippedHeader)
                    {
                        skippedHeader = true; 
                        continue;
                    }

                    var cnic = ws.Cell(r, 1).GetString().Trim();
                    var accountName = ws.Cell(r, 2).GetString().Trim();
                    var address = ws.Cell(r, 3).GetString().Trim();

                    amlDataList.Add(new AmlDataDTO
                    {
                        CNIC = cnic,
                        AccountName = accountName,
                        Address = address
                    });
                }
            }
            else
            {
                throw new ArgumentException("Only CSV or Excel files are allowed");
            }

           
            var createdRecords = await _repository.AddRangeAsync(amlDataList);
            return createdRecords;
        }



    }
}
