using System;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Interfaces
{
    public interface ITokenRevocationRepository
    {
        Task<bool> IsRevokedAsync(string jti);
        Task RevokeAsync(string jti, DateTime expiresAt);
    }
}
