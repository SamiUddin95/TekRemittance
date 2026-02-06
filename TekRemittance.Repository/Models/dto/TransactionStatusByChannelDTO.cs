using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Models.dto
{
    public class TransactionStatusByChannelDTO
    {
        public string TransactionType { get; set; }
        public decimal IncomingPercentage { get; set; }
        public decimal OutgoingPercentage { get; set; }
        public decimal PendingPercentage { get; set; }
    }
}
