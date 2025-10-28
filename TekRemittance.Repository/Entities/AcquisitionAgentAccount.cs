using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Entities
{
   public class AgentAccount
    {
        public Guid Id { get; set; }
        public string AccountNumber { get; set; }
        public Guid AgentId{ get; set; }
        public AcquisitionAgents AcquisitionAgents  { get; set; }
        public bool Approve { get; set; }
        public string AccountTitle { get; set; }
        public string AccountType { get; set; }
        public bool IsActive { get; set; }

    }
}
