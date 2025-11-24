using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Entities.Data;
using TekRemittance.Service.Interfaces;

namespace TekRemittance.Service.Implementations
{
    public class PermissionHelperService : IPermissionHelperService
    {
        private readonly AppDbContext _context;

        public PermissionHelperService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetUserPermissionsAsync(Guid userId)
        {
            return await _context.UserGroups
                .Where(ug => ug.UserId == userId)
                .SelectMany(ug => ug.Group.GroupPermissions)
                .Where(gp => gp.Permission.IsActive && gp.Group.IsActive)
                .Select(gp => gp.Permission.Category)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<string>> GetModulePermissionsAsync(string module)
        {
            return await _context.Permissions
                .Where(p => p.IsActive && p.Category.StartsWith(module + "."))
                .Select(p => p.Name)
                .ToListAsync();
        }

        public async Task SeedDefaultPermissionsAsync()
        {
            var permissions = new List<string>
            {
                // BasicSetup Module
                "BasicSetup.Countries.Read",
                "BasicSetup.Countries.Create", 
                "BasicSetup.Countries.Edit",
                "BasicSetup.Countries.Delete",
                "BasicSetup.Provinces.Read",
                "BasicSetup.Provinces.Create",
                "BasicSetup.Provinces.Edit", 
                "BasicSetup.Provinces.Delete",
                "BasicSetup.Cities.Read",
                "BasicSetup.Cities.Create",
                "BasicSetup.Cities.Edit",
                "BasicSetup.Cities.Delete",
                "BasicSetup.Banks.Read",
                "BasicSetup.Banks.Create",
                "BasicSetup.Banks.Edit",
                "BasicSetup.Banks.Delete",

                // Users Module
                "Users.Read",
                "Users.Create",
                "Users.Edit", 
                "Users.Delete",
                "Users.Authorize",

                // Groups Module
                "Groups.Read",
                "Groups.Create",
                "Groups.Edit",
                "Groups.Delete",
                "Groups.ManageUsers",
                "Groups.ManagePermissions",

                // Permissions Module
                "Permissions.Read",
                "Permissions.Create",
                "Permissions.Edit",
                "Permissions.Delete",

                // Agents Module
                "Agents.Read",
                "Agents.Create",
                "Agents.Edit",
                "Agents.Delete",

                // Remittance Module
                "Remittance.Upload",
                "Remittance.Process",
                "Remittance.View",
                "Remittance.Delete",

                // Disbursement Module
                "Disbursement.Read",
                "Disbursement.Create",
                "Disbursement.Edit",
                "Disbursement.Delete",

                // Reports Module
                "Reports.View",
                "Reports.Export",

                // Audit Module
                "Audit.View"
            };

            foreach (var permissionName in permissions)
            {
                var exists = await _context.Permissions
                    .AnyAsync(p => p.Category == permissionName);

                if (!exists)
                {
                    _context.Permissions.Add(new Permission
                    {
                        Id = Guid.NewGuid(),
                        Name = permissionName,
                        Category = permissionName,
                        Module = permissionName.Split('.')[0],
                        Description = GetPermissionDescription(permissionName),
                        IsActive = true,
                        CreatedBy = "system",
                        CreatedOn = DateTime.UtcNow
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        private string GetPermissionDescription(string permissionName)
        {
            var parts = permissionName.Split('.');
            if (parts.Length >= 2)
            {
                var module = parts[0];
                var action = parts[^1]; // Last part
                var entity = parts.Length > 2 ? parts[1] : module;
                
                return $"{action} access to {entity} in {module} module";
            }
            return permissionName;
        }
    }
}
