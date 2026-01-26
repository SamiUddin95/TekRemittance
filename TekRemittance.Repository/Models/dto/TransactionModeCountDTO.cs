using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Models.dto
{
    public class TransactionModeCountDTO
    {
        public int FTCount { get; set; }
        public int IBFTCount { get; set; }
        public int RTGSCount { get; set; }
        public int TotalCount { get; set; }
    }
}
