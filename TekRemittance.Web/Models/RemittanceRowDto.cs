using System;

namespace TekRemittance.Web.Models
{
    public class RemittanceRowDto
    {
        public int RowNumber { get; set; }
        public string? Error { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? AccountNumber { get; set; }
        public string? AccountTitle { get; set; }
        public string? Xpin { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
