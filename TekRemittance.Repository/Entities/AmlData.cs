using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Entities
{
    public class AmlData
    {
        public Guid Id { get; set; }
        public string? CNIC { get; set; }
        public string? AccountName { get; set; }
        public string? Address { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }


    }
}
