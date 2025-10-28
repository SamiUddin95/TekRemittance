using System;
using TekRemittance.Repository.Entities;

namespace TekRemittance.Web.Models.dto
{
    public class agentFileTemplateDTO
    {
        public Guid Id { get; set; }
        public Guid AgentId { get; set; }
        public string Name { get; set; }
        public string? SheetName { get; set; }
        public FileFormat Format { get; set; }
        public bool IsFixedLength { get; set; }
        public bool DelimiterEnabled { get; set; }
        public string? Delimiter { get; set; }
        public bool IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string AgentName { get; set; }
    }
}
