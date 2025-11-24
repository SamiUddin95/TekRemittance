using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TekRemittance.Service.Interfaces
{
    public interface IPermissionHelperService
    {
        Task<List<string>> GetUserPermissionsAsync(Guid userId);
        Task SeedDefaultPermissionsAsync();
        Task<List<string>> GetModulePermissionsAsync(string module);
    }
}
