using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Models.dto
{
    public class AgentPerformanceDTO
    {
        public string AgentName { get; set; }
        public int TotalTransactions { get; set; }
        public string TotalAmount { get; set; }
    }
}
