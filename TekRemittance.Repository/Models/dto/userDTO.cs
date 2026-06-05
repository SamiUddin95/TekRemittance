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

        //public List<int> HubIds { get; set; } = new List<int>();
        //public List<int> BankBranchIds { get; set; } = new List<int>();
        //public List<string>? HubNames { get; set; }
        //public List<string>? BankBranchNames { get; set; }
        // IDs ki jagah Codes
        public List<string> HubCodes { get; set; } = new List<string>();
        public List<string> BankBranchCodes { get; set; } = new List<string>();

        // Response ke liye Names
        public List<string>? HubNames { get; set; }
        public List<string>? BankBranchNames { get; set; }

    }
}
