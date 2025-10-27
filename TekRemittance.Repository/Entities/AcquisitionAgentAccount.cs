using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Entities
{
   public class AcquisitionAgentAccount
    {
        public Guid Id { get; set; }
        public string AgentAccountName { get; set; }
        public int AccountNumber { get; set; }
        public string AgentName { get; set; }
        public bool Approve { get; set; }
        public string AccountTitle { get; set; }
        public string AccountType { get; set; }
        public bool IsActive { get; set; }


    }
}
