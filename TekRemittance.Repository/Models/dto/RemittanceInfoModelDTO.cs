using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Models.dto
{
   public class RemittanceInfoModelDTO
    {
        public Guid? UserId { get; set; }
        public string Xpin { get; set; }

    }
}
