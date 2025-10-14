using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities.Data;
using TekRemittance.Repository.Interfaces;

namespace TekRemittance.Repository.Implementations
{
    public class TokenRevocationRepository : ITokenRevocationRepository
    {
        private readonly AppDbContext _context;
        public TokenRevocationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsRevokedAsync(string jti)
        {
            return await _context.RevokedTokens.AsNoTracking().AnyAsync(r => r.Jti == jti);
        }

        public async Task RevokeAsync(string jti, DateTime expiresAt)
        {
            if (await IsRevokedAsync(jti)) return;
            _context.RevokedTokens.Add(new Repository.Entities.RevokedToken
            {
                Id = Guid.NewGuid(),
                Jti = jti,
                RevokedAt = DateTime.UtcNow,
                ExpiresAt = expiresAt
            });
            await _context.SaveChangesAsync();
        }
    }
}
