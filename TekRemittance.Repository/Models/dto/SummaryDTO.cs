using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Models.dto
{
    public class SummaryDTO
    {
        public int TotalCount { get; set; }
        public string TotalAmount { get; set; }
        public decimal SuccessPercentage { get; set; }
        public decimal FailurePercentage { get; set; }
    }
}
