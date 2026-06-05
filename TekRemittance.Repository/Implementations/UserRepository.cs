using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Entities.Data;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Repository.Models.dto;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;


        public UserRepository (AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<userDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? name = null, string? employeeId = null, string? loginName = null)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _context.Users.AsNoTracking();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(u => u.Name.Contains(name));
            if (!string.IsNullOrEmpty(employeeId))
                query = query.Where(u => u.EmployeeId.Contains(employeeId));
            if (!string.IsNullOrEmpty(loginName))
                query = query.Where(u => u.LoginName.Contains(loginName));

            var totalCount = await query.CountAsync();

            var rawItems = await query
                .OrderByDescending(u => u.UpdatedOn ?? u.CreatedOn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var allHubCodes = rawItems
                .SelectMany(u => JsonSerializer.Deserialize<List<string>>(u.HubCodes ?? "[]") ?? new List<string>())
                .Distinct().ToList();

            var allBranchCodes = rawItems
                .SelectMany(u => JsonSerializer.Deserialize<List<string>>(u.BankBranchCodes ?? "[]") ?? new List<string>())
                .Distinct().ToList();

            var allHubs = await _context.Hub
                .Where(h => allHubCodes.Contains(h.Code) && !h.IsDeleted)
                .Select(h => new CodeNameDTO { Code = h.Code, Name = h.Name })
                .ToListAsync();

            var allBranches = await _context.BankBranches
                .Where(b => allBranchCodes.Contains(b.Code) && !b.IsDeleted)
                .Select(b => new CodeNameDTO { Code = b.Code, Name = b.Name })
                .ToListAsync();

            var items = rawItems.Select(u =>
            {
                var hubCodes = JsonSerializer.Deserialize<List<string>>(u.HubCodes ?? "[]") ?? new List<string>();
                var branchCodes = JsonSerializer.Deserialize<List<string>>(u.BankBranchCodes ?? "[]") ?? new List<string>();

                return new userDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Phone = u.Phone,
                    EmployeeId = u.EmployeeId,
                    Limit = u.Limit,
                    LoginName = u.LoginName,
                    IsSupervise = u.IsSupervise,
                    IsActive = u.IsActive,
                    CreatedBy = u.CreatedBy,
                    CreatedOn = u.CreatedOn,
                    UpdatedBy = u.UpdatedBy,
                    UpdatedOn = u.UpdatedOn,
                    UserType = u.UserType,
                    Hubs = allHubs.Where(h => hubCodes.Contains(h.Code)).ToList(),
                    BankBranches = allBranches.Where(b => branchCodes.Contains(b.Code)).ToList()
                };
            }).ToList();

            return new PagedResult<userDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }


        public async Task<userDTO?> GetByIdAsync(Guid id)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            if (user == null) return null;

            var hubCodes = JsonSerializer.Deserialize<List<string>>(user.HubCodes ?? "[]") ?? new List<string>();
            var branchCodes = JsonSerializer.Deserialize<List<string>>(user.BankBranchCodes ?? "[]") ?? new List<string>();

            var hubs = await _context.Hub
          .Where(h => hubCodes.Contains(h.Code) && !h.IsDeleted)
          .Select(h => new CodeNameDTO { Code = h.Code, Name = h.Name })
          .ToListAsync();

            var branches = await _context.BankBranches
                .Where(b => branchCodes.Contains(b.Code) && !b.IsDeleted)
                .Select(b => new CodeNameDTO { Code = b.Code, Name = b.Name })
                .ToListAsync();

            return new userDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                EmployeeId = user.EmployeeId,
                Limit = user.Limit,
                LoginName = user.LoginName,
                IsActive = user.IsActive,
                IsSupervise = user.IsSupervise,
                password = user.PasswordHash,
                CreatedBy = user.CreatedBy,
                CreatedOn = user.CreatedOn,
                UpdatedBy = user.UpdatedBy,
                UpdatedOn = user.UpdatedOn,
                UserType = user.UserType,
                Hubs = hubs,
                BankBranches = branches
            };
        }

        public async Task<userDTO> AddAsync(userDTO dto, string passwordHash)
        {
            // Duplicate LoginName Validation (case-insensitive, trimmed)
            var loginName = dto.LoginName?.Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(loginName))
            {
                if (await _context.Users.AnyAsync(u => u.LoginName != null && u.LoginName.ToLower() == loginName.ToLower()))
                {
                    throw new ArgumentException("Login name already exists.");
                }
            }
            var entity = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                EmployeeId = dto.EmployeeId,
                Limit = dto.Limit,
                LoginName = loginName!,
                PasswordHash = passwordHash,
                IsActive = dto.IsActive,
                CreatedBy = dto.CreatedBy ?? "system",
                CreatedOn = DateTime.UtcNow,
                UpdatedBy = dto.UpdatedBy ?? "system",
                UpdatedOn = DateTime.UtcNow,
                IsSupervise  = false,
                UserType=dto.UserType,
                HubCodes = JsonSerializer.Serialize(dto.Hubs.Select(h => h.Code).ToList()),
                BankBranchCodes = JsonSerializer.Serialize(dto.BankBranches.Select(b => b.Code).ToList())
            };
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();

            return new userDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                Phone = entity.Phone,
                EmployeeId = entity.EmployeeId,
                Limit = entity.Limit,
                LoginName = entity.LoginName,
                IsActive = entity.IsActive,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                UpdatedBy = entity.UpdatedBy,
                UpdatedOn = entity.UpdatedOn,
                IsSupervise = entity.IsSupervise,
                UserType=entity.UserType,
                Hubs = dto.Hubs,
                BankBranches = dto.BankBranches

            };
        }

        public async Task<userDTO?> UpdateAsync(userDTO dto)
        {
            var existing = await _context.Users.FirstOrDefaultAsync(u => u.Id == dto.Id);
            if (existing == null) return null;

            // Duplicate LoginName Validation (case-insensitive, trimmed) excluding self
            var loginName = dto.LoginName?.Trim() ?? string.Empty;
            if (!string.IsNullOrEmpty(loginName))
            {
                if (await _context.Users.AnyAsync(u => u.Id != dto.Id && u.LoginName != null && u.LoginName.ToLower() == loginName.ToLower()))
                {
                    throw new ArgumentException("Login name already exists.");
                }
            }

            existing.Name = dto.Name;
            existing.Email = dto.Email;
            existing.Phone = dto.Phone;
            existing.EmployeeId = dto.EmployeeId;
            existing.Limit = dto.Limit;
            existing.LoginName = loginName;
            existing.IsActive = dto.IsActive;
            existing.UpdatedBy = dto.UpdatedBy;
            existing.UpdatedOn = DateTime.UtcNow;
            existing.IsSupervise = false;
            existing.UserType = dto.UserType;
            existing.HubCodes = JsonSerializer.Serialize(dto.Hubs.Select(h => h.Code).ToList());
            existing.BankBranchCodes = JsonSerializer.Serialize(dto.BankBranches.Select(b => b.Code).ToList());


            await _context.SaveChangesAsync();

            return new userDTO
            {
                Id = existing.Id,
                Name = existing.Name,
                Email = existing.Email,
                Phone = existing.Phone,
                EmployeeId = existing.EmployeeId,
                Limit = existing.Limit,
                LoginName = existing.LoginName,
                IsActive = existing.IsActive,
                CreatedBy = existing.CreatedBy,
                CreatedOn = existing.CreatedOn,
                UpdatedBy = existing.UpdatedBy,
                UpdatedOn = existing.UpdatedOn,
                IsSupervise = existing.IsSupervise,
                UserType=existing.UserType,
                Hubs = dto.Hubs,
                BankBranches = dto.BankBranches
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (existing == null) return false;
            _context.Users.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(Guid Id, string PasswordHash, bool IsActive)?> GetAuthByLoginAsync(string loginName)
        {   
            var data = await _context.Users.AsNoTracking()
                .Where(u => u.LoginName == loginName)
                .Select(u => new { u.Id, u.PasswordHash, u.IsActive, u.IsSupervise })
                .FirstOrDefaultAsync();
            if (data == null) return null;
            return (data.Id, data.PasswordHash!, data.IsActive);
        }

        public async Task<bool> UpdateIsSuperviseAsync(Guid id, bool isSupervise)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return false;
            user.IsSupervise = isSupervise;
            user.UpdatedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateNameAndPasswordAsync(Guid id, string name, string passwordHash)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return false;
            if (!string.IsNullOrWhiteSpace(name))
            {
                user.Name = name;
            }
            user.PasswordHash = passwordHash;
            user.UpdatedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

      
        private string GeneratePlainPassword(int length = 8)
        {
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string special = "!@#$%^&*()_?";

            string all = upper + lower + digits + special;
            var random = new Random();
            var password = new char[length];

            password[0] = upper[random.Next(upper.Length)];
            password[1] = lower[random.Next(lower.Length)];
            password[2] = digits[random.Next(digits.Length)];
            password[3] = special[random.Next(special.Length)];

            for (int i = 4; i < length; i++)
            {
                password[i] = all[random.Next(all.Length)];
            }

            return new string(password.OrderBy(x => random.Next()).ToArray());
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
        public async Task<(bool Success, string Message, string NewPassword)> ForgetPasswordAsync(ForgetPasswordDTO dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.LoginName == dto.LoginName && u.Email == dto.Email);

            if (user == null)
                return (false, "Invalid username or email.", null);

            var plainPassword = GeneratePlainPassword(8);

            var hashedPassword = ComputeSha256Hash(plainPassword);

            user.PasswordHash = hashedPassword;

            await _context.SaveChangesAsync();

            return (true, "Email sent successfully.", plainPassword);
        }   

        public async Task<PagedResult<userUnAuthorizeDTO>> GetUnAuthorizeUser(int pageNumber = 1, int pageSize = 10, string? name = null, string? employeeId = null, string? loginName = null)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _context.Users.Where(u => u.IsSupervise == false).AsNoTracking();

            var querys = _context.Users.AsNoTracking();
            if (!string.IsNullOrEmpty(name))
                query = query.Where(u => u.Name.Contains(name));
            if (!string.IsNullOrEmpty(employeeId))
                query = query.Where(u => u.EmployeeId.Contains(employeeId));
            if (!string.IsNullOrEmpty(loginName))
                query = query.Where(u => u.LoginName.Contains(loginName));

            var totalcount = await query.CountAsync();

            var items = await query
        .OrderBy(u => u.UpdatedOn??u.CreatedOn)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .Select(u => new userUnAuthorizeDTO
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            Phone = u.Phone,
            EmployeeId = u.EmployeeId,
            Limit = u.Limit,
            LoginName = u.LoginName,
            IsSupervise = u.IsSupervise,
            IsActive = u.IsActive,
            CreatedBy = u.CreatedBy,
            CreatedOn = u.CreatedOn,
            UpdatedBy = u.UpdatedBy,
            UpdatedOn = u.UpdatedOn
        })
        .ToListAsync();

            return new PagedResult<userUnAuthorizeDTO>
            {
                Items = items,
                TotalCount = totalcount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

        }


    }
}
