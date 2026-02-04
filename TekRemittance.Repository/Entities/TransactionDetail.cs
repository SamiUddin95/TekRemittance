using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Entities
{
    public class TransactionDetail
    {
        public long Id { get; set; }
        public string? TransactionNumber { get; set; }
        public string? TransactionType { get; set; }
        public string? BankName  { get; set;}
        public string AgentName { get; set; }
        public string? Amount { get; set; }
        public string? AccountNo { get; set; }
        public DateTime? Date { get; set; }
        public string? Status { get; set; }
        public string? ChannelType { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
