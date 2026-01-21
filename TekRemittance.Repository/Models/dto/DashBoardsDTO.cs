using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Models.dto
{
    public class DashBoardsDTO
    {
        public decimal AmlAmount { get; set; }
        public decimal RejectAmount { get; set; }
        public decimal RepairAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
