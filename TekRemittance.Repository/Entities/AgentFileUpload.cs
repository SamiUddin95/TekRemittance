using System;

namespace TekRemittance.Repository.Entities
{
    public class AgentFileUpload
    {
        public Guid Id { get; set; }
        public Guid AgentId { get; set; }
        public AcquisitionAgents Agent { get; set; }
        public Guid TemplateId { get; set; }
        public AgentFileTemplate Template { get; set; }

        public string FileName { get; set; }
        public string? StoragePath { get; set; }
        public UploadStatus Status { get; set; }
        public string? ErrorMessage { get; set; }
        public int RowCount { get; set; }
        public DateTime? ProcessedAt { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
