using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Models.dto
{
   public class AcquisitionAgentAccountDTO
    {
        public Guid Id { get; set; }
        public string AccountNumber { get; set; }
        public Guid AgentId { get; set; }
        public string? AgentName { get; set; }
        public bool Approve { get; set; }
        public string AccountTitle { get; set; }
        public string AccountType { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
