using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Entities.Data;
using TekRemittance.Repository.Interfaces;

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
        public async Task<IEnumerable<Country>> GetAllAsync()
        {
            return await _context.Countries.AsNoTracking().ToListAsync();
        }

        public async Task<Country> GetByIdAsync(Guid id)
        {
            return await _context.Countries.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Country> AddAsync(Country country)
        {
            country.Id = Guid.NewGuid();
            country.CreatedOn = DateTime.UtcNow;

            await _context.Countries.AddAsync(country);
            await _context.SaveChangesAsync();

            return country;
        }

        public async Task<Country?> UpdateAsync(Country country)
        {
            var existing = await _context.Countries.FirstOrDefaultAsync(c => c.Id == country.Id);
            if (existing == null) return null;

            existing.CountryCode = country.CountryCode;
            existing.CountryName = country.CountryName;
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
        public async Task<IEnumerable<Province>> GetAllProvinceAsync()
        {
            return await _context.Provinces.AsNoTracking().ToListAsync();
        }

        public async Task<Province> GetProvinceByIdAsync(Guid id)
        {
            return await _context.Provinces.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Province> AddProvinceAsync(Province province)
        {
            province.Id = Guid.NewGuid();
            province.CreatedOn = DateTime.UtcNow;

            await _context.Provinces.AddAsync(province);
            await _context.SaveChangesAsync();

            return province;
        }

        public async Task<Province?> UpdateProvinceAsync(Province province)
        {
            var existing = await _context.Provinces.FirstOrDefaultAsync(c => c.Id == province.Id);
            if (existing == null) return null;

            existing.ProvinceCode = province.ProvinceCode;
            existing.ProvinceName = province.ProvinceName;
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
        public async Task<IEnumerable<City>> GetAllCityAsync()
        {
            return await _context.Cities.AsNoTracking().ToListAsync();
        }

        public async Task<City> GetCityByIdAsync(Guid id)
        {
            return await _context.Cities.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<City> AddCityAsync(City city)
        {
            city.Id = Guid.NewGuid();
            city.CreatedOn = DateTime.UtcNow;

            await _context.Cities.AddAsync(city);
            await _context.SaveChangesAsync();

            return city;
        }

        public async Task<City?> UpdateCityAsync(City city)
        {
            var existing = await _context.Cities.FirstOrDefaultAsync(c => c.Id == city.Id);
            if (existing == null) return null;

            existing.CityCode = city.CityCode;
            existing.CityName = city.CityName;
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
            var existing = await _context.Provinces.FirstOrDefaultAsync(c => c.Id == id);
            if (existing == null) return false;

            _context.Provinces.Remove(existing);
            await _context.SaveChangesAsync();

            return true;
        }
        #endregion
    }
}
