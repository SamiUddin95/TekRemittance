using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekRemittance.Repository.Models.dto;

namespace TekRemittance.Repository.Interfaces
{
    public interface IDashboardsRepository
    {
       
        Task<object> GetDashboardDataAsync(string dateRange);


    }
}
