using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Service.Interfaces;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<userDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _repo.GetAllAsync(pageNumber, pageSize);
        }

        public async Task<userDTO?> GetByIdAsync(Guid id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<userDTO> CreateAsync(userDTO user, string password)
        {
            var hash = HashPassword(password);
            return await _repo.AddAsync(user, hash);
        }

        public async Task<userDTO?> UpdateAsync(userDTO user)
        {
            return await _repo.UpdateAsync(user);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<userDTO?> ValidateCredentialsAsync(string loginName, string password)
        {
            var auth = await _repo.GetAuthByLoginAsync(loginName);
            if (auth == null) return null;
            var hash = HashPassword(password);
            if (!string.Equals(auth.Value.PasswordHash, hash, StringComparison.Ordinal)) return null;
            if (!auth.Value.IsActive) return null;
            return await _repo.GetByIdAsync(auth.Value.Id);
        }

        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            var sb = new StringBuilder();
            foreach (var b in bytes) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        public async Task<bool> UpdateIsSuperviseAsync(Guid id, bool isSupervise)
        {
            return await _repo.UpdateIsSuperviseAsync(id, isSupervise);
        }

        public async Task<bool> UpdateNameAndPasswordAsync(Guid id, string name, string password)
        {
            var hash = HashPassword(password);
            return await _repo.UpdateNameAndPasswordAsync(id, name, hash);
        }
    }
}
