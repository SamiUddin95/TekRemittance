using System.Threading.Tasks;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Service.Interfaces
{
    public interface ILicenseService
    {
        Task<LicenseStatusDTO> GetLicenseStatusAsync();
        Task<bool> IsLicenseValidAsync();
        Task<bool> UpdateLicenseAsync(string encryptedKey, string updatedBy);
    }
}