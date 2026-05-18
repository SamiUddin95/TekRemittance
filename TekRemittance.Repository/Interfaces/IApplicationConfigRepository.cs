using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Interfaces
{
    public interface IApplicationConfigRepository
    {
        Task<string?> GetValueByKeyAsync(string key);
        Task<bool> UpdateValueByKeyAsync(string key, string value, string updatedBy);
    }
}
