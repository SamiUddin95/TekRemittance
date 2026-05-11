namespace TekRemittance.Repository.Models.DTOs
{
    public class DisbursementQueueDto
    {
        public Guid Id { get; set; }
        public string? AccountNumber { get; set; }
        public string? AccountTitle { get; set; }
        public string? BankCode { get; set; }
        public string? BankName { get; set; }
        public string? Status { get; set; }
        public DateTime? Date { get; set; }
        public int RowNumber { get; set; }
        public string DataJson { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}