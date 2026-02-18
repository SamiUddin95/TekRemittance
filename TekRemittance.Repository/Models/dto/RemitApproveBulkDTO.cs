using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekRemittance.Repository.Enums;

namespace TekRemittance.Repository.Models.dto
{
    public  class RemitApproveBulkDTO
    {
        public List<string> Xpins { get; set; } = new();
        public Guid? UserId { get; set; }
        public ModeOfTransactionEnum? ModeOfTransaction { get; set; }
    }
}
