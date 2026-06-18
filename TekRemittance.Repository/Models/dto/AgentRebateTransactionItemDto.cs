using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Repository.Models.dto
{
    public class AgentRebateTransactionItemDto
    {
        public string xpin { get; set; }   
        public string Date { get; set; } 
        public string Beneficiary { get; set; }
        public decimal AmountPKR { get; set; }
        public decimal RebatePKR { get; set; }
        public decimal agentshare { get; set; }


        
    }

    public class AgentRebateSharingDetailResultDto
    {
        public Guid AgentId { get; set; }
        public string AgentName { get; set; }
        public string CountryName { get; set; }
        public decimal SharingPercent { get; set; }

        public PagedResult<AgentRebateTransactionItemDto> Transactions { get; set; }

        public decimal TotalAmountPKR { get; set; }
        public decimal TotalRebatePKR { get; set; }
        public decimal TotalAgentSharePKR { get; set; }
    }
}
