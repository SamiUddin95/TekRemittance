using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Models.dto
{
    public class AgentRebateSharingResultDto
    {
        public int TotalTransactions { get; set; }

        public int EligibleTransactionCount { get; set; }

        public decimal TotalRebateSAR { get; set; }

        public decimal TotalRebatePKR { get; set; }

        public int TotalAgents { get; set; }


        public List<AgentRebateSharingItemDto> Agents { get; set; }
    }
}
