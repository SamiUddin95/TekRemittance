using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Models.dto
{
    public class AuditLogDTO
    {
        public Guid Id { get; set; }
        public string EntityName { get; set; }
        public Guid EntityId { get; set; }
        public string Action { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string PerformedBy { get; set; }
        public DateTime PerformedOn { get; set; }
    }
}
