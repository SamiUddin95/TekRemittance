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
        public string? NIFTBranchCode { get; set; }
        public string? Name { get; set; }
        public int HubId { get; set; }
        //public string? HubName { get; set; }
        public bool IsDeleted { get; set; }
        public string? Email1 { get; set; }
        public string? Email2 { get; set; }
        public string? Email3 { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
