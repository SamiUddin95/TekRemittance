using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekRemittance.Repository.Enums;

namespace TekRemittance.Repository.Models.dto
{
    public class RecentTransactionDTO
    {
        public string AgentName { get; set; }
        public string XPIN { get; set; }
        public DateTime? Date { get; set; }
        public string AccountNumber { get; set; }
        public string AccountTitle { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public ModeOfTransactionEnum? ModeOfTransaction { get; set; }
    }
}
