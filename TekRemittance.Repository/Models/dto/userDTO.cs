using System;

namespace TekRemittance.Web.Models.dto
{
    public class userDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? EmployeeId { get; set; }
        public decimal Limit { get; set; }
        public string? LoginName { get; set; }
        public bool IsActive { get; set; }
        public string? password { get; set; }

        public bool IsSupervise { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public string UserType { get; set; }

    }
}
