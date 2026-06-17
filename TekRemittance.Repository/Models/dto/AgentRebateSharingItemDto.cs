using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Models.dto
{
    public class AgentRebateSharingItemDto
    {
        public Guid AgentId { get; set; }
        public string AgentName { get; set; }

        public decimal SharingPercentage { get; set; }

        public decimal AgentShare { get; set; }
        public string CountryName { get; set; }
        public int TotalTransactionCount { get; set; }
        public int EligibleTransactionCount { get; set; }
        public decimal RebateSAR { get; set; }
        public decimal RebatePKR { get; set; }




    }
}
