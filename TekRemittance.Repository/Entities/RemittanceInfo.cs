using System;
using TekRemittance.Repository.Enums;

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
        public DateTime? UpdatedOn { get; set; }
        public string Status { get; set; }
        public string? AccountNumber { get; set; }
        public string? AccountTitle { get; set; }
        public string? Xpin { get; set; }
        public DateTime? Date { get; set; }
        public ModeOfTransactionEnum? ModeOfTransaction { get; set; }
        public string? LimitType { get; set; }
        
    }
}
