using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Entities
{
    public class PasswordPolicy
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NameOther { get; set; }
        public string Description { get; set; }
        public int CountHistory { get; set; }
        public int ExpiryDays { get; set; }
        public int NotifyDays { get; set; }
        public int AccountDisableDays { get; set; }
        public int InvalidLoginEntry { get; set; }
        public bool FirstReset { get; set; }
        public bool CyclicPassword { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }
}
