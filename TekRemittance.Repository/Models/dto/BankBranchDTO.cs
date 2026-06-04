using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Models.dto
{
    public class BankBranchDTO
    {
        public int Id { get; set; }
        public string? Code { get; set; }

        public string? Name { get; set; }
        public bool IsActive { get; set; }

        //public int HubId { get; set; }
        public string? HubCode { get; set; }

        public bool IsDeleted { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
