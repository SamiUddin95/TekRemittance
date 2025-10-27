using System;
using TekRemittance.Repository.Entities;

namespace TekRemittance.Web.Models.dto
{
    public class agentFileTemplateFieldDTO
    {
        public Guid Id { get; set; }
        public Guid TemplateId { get; set; }

        public string? TemplateName { get; set; }
        public int FieldOrder { get; set; }
        public string FieldName { get; set; }
        public FieldType FieldType { get; set; }
        public bool Required { get; set; }
        public bool Enabled { get; set; }
        public int? StartIndex { get; set; }
        public int? Length { get; set; }
    }
}
