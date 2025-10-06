namespace TekRemittance.Web.Models.dto
{
    public class bankDTO
    {
        public Guid Id { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string? IMD { get; set; }
        public string? Website { get; set; }
        public string? Allases { get; set; }
        public int? PhoneNo { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
