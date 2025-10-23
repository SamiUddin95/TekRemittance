using System;

namespace TekRemittance.Repository.Entities
{
    public class AgentFileTemplateField
    {
        public Guid Id { get; set; }
        public Guid TemplateId { get; set; }
        public AgentFileTemplate Template { get; set; }

        public int FieldOrder { get; set; }
        public string FieldName { get; set; }
        public FieldType FieldType { get; set; }
        public bool Required { get; set; }
        public bool Enabled { get; set; }

        public int? StartIndex { get; set; }
        public int? Length { get; set; }
    }
}
