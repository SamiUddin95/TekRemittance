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
    }
}
