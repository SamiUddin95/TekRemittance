using System;

namespace TekRemittance.Repository.Entities
{
    public class RemittanceInfo
    {
        public Guid Id { get; set; }
        public Guid AgentId { get; set; }
        public Guid TemplateId { get; set; }
        public Guid UploadId { get; set; }
        public int RowNumber { get; set; }
        public string DataJson { get; set; }
        public string? Error { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
