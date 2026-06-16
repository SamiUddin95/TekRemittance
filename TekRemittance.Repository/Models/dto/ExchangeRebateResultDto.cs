using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Models.dto
{
    public class ExchangeRebateResultDto
    {
        public int TotalTxns { get; set; }
        public decimal TotalRebateSAR { get; set; }
        public decimal TotalRebatePKR { get; set; }
        public decimal TotalPKR { get; set; }
        public decimal ExchangeRateSAR { get; set; }
        public decimal ExchangeRateUSD { get; set; }
        public List<ExchangeRebateItemDto> Items { get; set; } = new();
       
    }

    public class ExchangeRebateItemDto
    {
        public string XPIN { get; set; }
        public string BeneficiaryName { get; set; }
        public string BeneficiaryAccountNumber { get; set; }
        //public DateTime TransactionDate { get; set; }
        public string TransactionDate { get; set; }   // DD/MM/YYYY string

        public string RemitterName { get; set; }
        public string RemitterPassportNumber { get; set; }
        public string CountryCode { get; set; }
        public string AgentName { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string PostingTime { get; set; }        // DD/MM/YYYY string
        public decimal UsdLimit { get; set; }
        public decimal SarLimit { get; set; }
    }
}
