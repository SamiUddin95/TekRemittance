using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;

namespace TekRemittance.Repository.Models.dto
{
   public class AgentFileUploadDTO
    {
        
            public Guid Id { get; set; }
            public Guid AgentId { get; set; }

            public Guid TemplateId { get; set; }

            public string TemplateName { get; set; }   
            public string FileName { get; set; }
            public string? StoragePath { get; set; }
            public UploadStatus Status { get; set; }
            public string? ErrorMessage { get; set; }
            public int RowCount { get; set; }
            public DateTime? ProcessedAt { get; set; }
            public DateTime CreatedOn { get; set; }
        

    }
}
