using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Models.dto
{
    public class DisbursementSuccessPercentageDTO
    {
        public int TotalCount { get; set; }
        public int SuccessCount { get; set; }
        public decimal SuccessPercentage { get; set; }
    }
}
