using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Entities.Data;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Repository.Implementations
{
    public class BasicSetupRepository : IBasicSetupRepository
    {
        private readonly AppDbContext _context;

        public BasicSetupRepository(AppDbContext context)
        {
            _context = context;
        }

        #region Country
        public async Task<PagedResult<countryDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _context.Countries
                .AsNoTracking()
                .Select(c => new countryDTO
                {
                    Id = c.Id,
                    CountryCode = c.CountryCode,
                    CountryName = c.CountryName,
                    IsActive = c.IsActive,
                    CreatedBy = c.CreatedBy,
                    CreatedOn = c.CreatedOn,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedOn = c.UpdatedOn
                });

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(c => c.CountryName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<countryDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<countryDTO?> GetByIdAsync(Guid id)
        {
            return await _context.Countries
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new countryDTO
                {
                    Id = c.Id,
                    CountryCode = c.CountryCode,
                    CountryName = c.CountryName,
                    IsActive = c.IsActive,
                    CreatedBy = c.CreatedBy,
                    CreatedOn = c.CreatedOn,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedOn = c.UpdatedOn
                })
                .FirstOrDefaultAsync();
        }

        public async Task<countryDTO> AddAsync(countryDTO country)
        {
            // Duplicate Name Validation (case-insensitive, trimmed)
            var countryName = country.CountryName?.Trim() ?? string.Empty;
            if (await _context.Countries.AnyAsync(c => c.CountryName.ToLower() == countryName.ToLower()))
            {
                throw new ArgumentException("Country name already exists.");
            }
            var entity = new Country
            {
                Id = Guid.NewGuid(),
                CountryCode = country.CountryCode,
                CountryName = countryName,
                IsActive = country.IsActive,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "sami",
                UpdatedOn = DateTime.UtcNow,
                UpdatedBy = "sami"
            };

            await _context.Countries.AddAsync(entity);
            await _context.SaveChangesAsync();

            return new countryDTO
            {
                Id = entity.Id,
                CountryCode = entity.CountryCode,
                CountryName = entity.CountryName,
                IsActive = entity.IsActive,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                UpdatedBy = entity.UpdatedBy,
                UpdatedOn = entity.UpdatedOn
            };
        }

        public async Task<Country?> UpdateAsync(countryDTO country)
        {
            var existing = await _context.Countries.FirstOrDefaultAsync(c => c.Id == country.Id);
            if (existing == null) return null;

            // Duplicate Name Validation (case-insensitive, trimmed) excluding self
            var countryName = country.CountryName?.Trim() ?? string.Empty;
            if (await _context.Countries.AnyAsync(c => c.Id != country.Id && c.CountryName.ToLower() == countryName.ToLower()))
            {
                throw new ArgumentException("Country name already exists.");
            }

            existing.CountryCode = country.CountryCode;
            existing.CountryName = countryName;
            existing.IsActive = country.IsActive;
            existing.UpdatedBy = country.UpdatedBy;
            existing.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);
            if (existing == null) return false;

            _context.Countries.Remove(existing);
            await _context.SaveChangesAsync();

            return true;
        }
        #endregion

        #region Province
        public async Task<PagedResult<provinceDTO>> GetAllProvinceAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _context.Provinces
                .AsNoTracking()
                .Select(p => new provinceDTO
                {
                    Id = p.Id,
                    ProvinceCode = p.ProvinceCode,
                    ProvinceName = p.ProvinceName,
                    CountryId = p.CountryId,
                    IsActive = p.IsActive,
                    CreatedBy = p.CreatedBy,
                    CreatedOn = p.CreatedOn,
                    UpdatedBy = p.UpdatedBy,
                    UpdatedOn = p.UpdatedOn
                });

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(p => p.ProvinceName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<provinceDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<provinceDTO> GetProvinceByIdAsync(Guid id)
        {
            return await _context.Provinces
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new provinceDTO
                {
                    Id = c.Id,
                    ProvinceCode = c.ProvinceCode,
                    ProvinceName = c.ProvinceName,
                    CountryId = c.CountryId,
                    IsActive = c.IsActive
                    // map any other properties you need
                })
                .FirstOrDefaultAsync();
        }

        public async Task<provinceDTO> AddProvinceAsync(provinceDTO provincedto)
        {
            // Duplicate Name Validation within the same Country (case-insensitive, trimmed)
            var provinceName = provincedto.ProvinceName?.Trim() ?? string.Empty;
            if (await _context.Provinces.AnyAsync(p => p.CountryId == provincedto.CountryId && p.ProvinceName.ToLower() == provinceName.ToLower()))
            {
                throw new ArgumentException("Province name already exists in the selected country.");
            }

            var province = new Province
            {
                Id = Guid.NewGuid(),
                ProvinceCode = provincedto.ProvinceCode,
                ProvinceName = provinceName,
                CountryId = provincedto.CountryId,
                IsActive = provincedto.IsActive,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "sami",
                UpdatedOn = DateTime.UtcNow,
                UpdatedBy = "sami"
            };

            await _context.Provinces.AddAsync(province);
            await _context.SaveChangesAsync();

            return new provinceDTO
            {
                Id = province.Id,
                ProvinceCode = province.ProvinceCode,
                ProvinceName = province.ProvinceName,
                CountryId = province.CountryId,
                IsActive= province.IsActive,
                CreatedOn = province.CreatedOn
            }; ;
        }

        public async Task<Province?> UpdateProvinceAsync(provinceDTO province)
        {
            var existing = await _context.Provinces.FirstOrDefaultAsync(c => c.Id == province.Id);
            if (existing == null) return null;

            // Duplicate Name Validation within the same Country (case-insensitive, trimmed) excluding self
            var provinceName = province.ProvinceName?.Trim() ?? string.Empty;
            if (await _context.Provinces.AnyAsync(p => p.Id != province.Id && p.CountryId == province.CountryId && p.ProvinceName.ToLower() == provinceName.ToLower()))
            {
                throw new ArgumentException("Province name already exists in the selected country.");
            }

            existing.ProvinceCode = province.ProvinceCode;
            existing.ProvinceName = provinceName;
            existing.CountryId = province.CountryId;
            existing.IsActive = province.IsActive;
            existing.UpdatedBy = province.UpdatedBy;
            existing.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteProvinceAsync(Guid id)
        {
            var existing = await _context.Provinces.FirstOrDefaultAsync(c => c.Id == id);
            if (existing == null) return false;

            _context.Provinces.Remove(existing);
            await _context.SaveChangesAsync();

            return true;
        }
        #endregion

        #region City
        public async Task<PagedResult<cityDTO>> GetAllCityAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _context.Cities
                .AsNoTracking()
                .Select(c => new cityDTO
                {
                    Id = c.Id,
                    CityCode = c.CityCode,
                    CityName = c.CityName,
                    CountryId = c.CountryId,
                    ProvinceId = c.ProvinceId,
                    IsActive = c.IsActive,
                    CreatedBy = c.CreatedBy,
                    CreatedOn = c.CreatedOn,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedOn = c.UpdatedOn
                });

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(c => c.CityName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<cityDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<cityDTO?> GetCityByIdAsync(Guid id)
        {
            return await _context.Cities
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new cityDTO
                {
                    Id = c.Id,
                    CityCode = c.CityCode,
                    CityName = c.CityName,
                    CountryId = c.CountryId,
                    ProvinceId = c.ProvinceId,
                    IsActive = c.IsActive,
                    CreatedBy = c.CreatedBy,
                    CreatedOn = c.CreatedOn,
                    UpdatedBy = c.UpdatedBy,
                    UpdatedOn = c.UpdatedOn
                })
                .FirstOrDefaultAsync();
        }

        public async Task<cityDTO> AddCityAsync(cityDTO city)
        {
            // Duplicate Name Validation within the same Province (case-insensitive, trimmed)
            var cityName = city.CityName?.Trim() ?? string.Empty;
            if (await _context.Cities.AnyAsync(c => c.ProvinceId == city.ProvinceId && c.CityName.ToLower() == cityName.ToLower()))
            {
                throw new ArgumentException("City name already exists in the selected province.");
            }

            var entity = new City
            {
                Id = Guid.NewGuid(),
                CityCode = city.CityCode,
                CityName = cityName,
                CountryId = city.CountryId,
                ProvinceId = city.ProvinceId,
                IsActive = city.IsActive,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "sami",
                UpdatedOn = DateTime.UtcNow,
                UpdatedBy = "sami"
            };

            await _context.Cities.AddAsync(entity);
            await _context.SaveChangesAsync();

            return new cityDTO
            {
                Id = entity.Id,
                CityCode = entity.CityCode,
                CityName = entity.CityName,
                CountryId = entity.CountryId,
                ProvinceId = entity.ProvinceId,
                IsActive = entity.IsActive,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                UpdatedBy = entity.UpdatedBy,
                UpdatedOn = entity.UpdatedOn
            };
        }

        public async Task<City?> UpdateCityAsync(cityDTO city)
        {
            var existing = await _context.Cities.FirstOrDefaultAsync(c => c.Id == city.Id);
            if (existing == null) return null;

            // Duplicate Name Validation within the same Province (case-insensitive, trimmed) excluding self
            var cityName = city.CityName?.Trim() ?? string.Empty;
            if (await _context.Cities.AnyAsync(c => c.Id != city.Id && c.ProvinceId == city.ProvinceId && c.CityName.ToLower() == cityName.ToLower()))
            {
                throw new ArgumentException("City name already exists in the selected province.");
            }

            existing.CityCode = city.CityCode;
            existing.CityName = cityName;
            existing.CountryId = city.CountryId;
            existing.ProvinceId = city.ProvinceId;
            existing.IsActive = city.IsActive;
            existing.UpdatedBy = city.UpdatedBy;
            existing.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existing;
        }
        
        public async Task<bool> DeleteCityAsync(Guid id)
        {
            var existing = await _context.Cities.FirstOrDefaultAsync(c => c.Id == id);
            if (existing == null) return false;

            _context.Cities.Remove(existing);
            await _context.SaveChangesAsync();

            return true;
        }
        #endregion

        #region Bank
        public async Task<PagedResult<bankDTO>> GetAllBankAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _context.Banks
                .AsNoTracking()
                .Select(b => new bankDTO
                {
                    Id = b.Id,
                    BankCode = b.BankCode,
                    BankName = b.BankName,
                    IMD = b.IMD,
                    Website = b.Website,
                    Allases = b.Allases,
                    PhoneNo = b.PhoneNo,
                    Description = b.Description,
                    IsActive = b.IsActive,
                    CreatedBy = b.CreatedBy,
                    CreatedOn = b.CreatedOn,
                    UpdatedBy = b.UpdatedBy,
                    UpdatedOn = b.UpdatedOn
                });

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(b => b.BankName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<bankDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<bankDTO?> GetBankByIdAsync(Guid id)
        {
            return await _context.Banks
                .AsNoTracking()
                .Where(b => b.Id == id)
                .Select(b => new bankDTO
                {
                    Id = b.Id,
                    BankCode = b.BankCode,
                    BankName = b.BankName,
                    IMD = b.IMD,
                    Website = b.Website,
                    Allases = b.Allases,
                    PhoneNo = b.PhoneNo,
                    Description = b.Description,
                    IsActive = b.IsActive,
                    CreatedBy = b.CreatedBy,
                    CreatedOn = b.CreatedOn,
                    UpdatedBy = b.UpdatedBy,
                    UpdatedOn = b.UpdatedOn
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bankDTO> AddBankAsync(bankDTO bank)
        {
            // Duplicate Name Validation (case-insensitive, trimmed)
            var bankName = bank.BankName?.Trim() ?? string.Empty;
            if (await _context.Banks.AnyAsync(b => b.BankName.ToLower() == bankName.ToLower()))
            {
                throw new ArgumentException("Bank name already exists.");
            }

            var entity = new Bank
            {
                Id = Guid.NewGuid(),
                BankCode = bank.BankCode,
                BankName = bankName,
                IMD = bank.IMD,
                Website = bank.Website,
                Allases = bank.Allases,
                PhoneNo = bank.PhoneNo,
                Description = bank.Description,
                IsActive = bank.IsActive,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "sami",
                UpdatedOn = DateTime.UtcNow,
                UpdatedBy = "sami"
            };

            await _context.Banks.AddAsync(entity);
            await _context.SaveChangesAsync();

            return new bankDTO
            {
                Id = entity.Id,
                BankCode = entity.BankCode,
                BankName = entity.BankName,
                IMD = entity.IMD,
                Website = entity.Website,
                Allases = entity.Allases,
                PhoneNo = entity.PhoneNo,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                UpdatedBy = entity.UpdatedBy,
                UpdatedOn = entity.UpdatedOn
            };
        }

        public async Task<Bank?> UpdateBankAsync(bankDTO bank)
        {
            var existing = await _context.Banks.FirstOrDefaultAsync(b => b.Id == bank.Id);
            if (existing == null) return null;

            // Duplicate Name Validation (case-insensitive, trimmed) excluding self
            var bankName = bank.BankName?.Trim() ?? string.Empty;
            if (await _context.Banks.AnyAsync(b => b.Id != bank.Id && b.BankName.ToLower() == bankName.ToLower()))
            {
                throw new ArgumentException("Bank name already exists.");
            }

            existing.BankCode = bank.BankCode;
            existing.BankName = bankName;
            existing.IMD = bank.IMD;
            existing.Website = bank.Website;
            existing.Allases = bank.Allases;
            existing.PhoneNo = bank.PhoneNo;
            existing.Description = bank.Description;
            existing.IsActive = bank.IsActive;
            existing.UpdatedBy = bank.UpdatedBy;
            existing.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteBankAsync(Guid id)
        {
            var existing = await _context.Banks.FirstOrDefaultAsync(b => b.Id == id);
            if (existing == null) return false;

            _context.Banks.Remove(existing);
            await _context.SaveChangesAsync();

            return true;
        }
        #endregion
    }
}
