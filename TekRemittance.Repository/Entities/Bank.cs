using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Entities
{
    public class Bank
    {
        public Guid Id { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string? IMD {  get; set; }
        public string? Website { get; set; }
        public string? Allases { get; set; }
        public string? PhoneNo { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }
}
    