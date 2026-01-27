using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Models.dto
{
   public class RemitttanceInfosStatusDTO
    {
        public Guid Id { get; set; }
        public Guid AgentId { get; set; }
        public string AgentName { get; set; }
        public Guid TemplateId { get; set; }
        public Guid UploadId { get; set; }
        public int RowNumber { get; set; }
        public string DataJson { get; set; }
        public string? Error { get; set; }
        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }
        public string Status { get; set; }

        public string LimitMessage { get; set; }
    }
}
