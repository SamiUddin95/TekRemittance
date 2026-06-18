using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Models.dto
{
    public class ExchangeRebateRequestDTO
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public Guid? AgentId { get; set; }

        public decimal ExchangeRateSAR { get; set; }
        public decimal ExchangeRateUSD { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class AgentRebateDetailRequestDTO
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal ExchangeRateUSD { get; set; }
        public decimal ExchangeRateSAR { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
