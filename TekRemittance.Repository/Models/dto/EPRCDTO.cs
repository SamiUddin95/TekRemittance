using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Models.dto
{
    public class EPRCDTO
    {
        public long Id { get; set; }
        public string? TotalEPRCGenerated { get; set; }
        public string? EPRCVerified { get; set; }
        public string? NotVerfied { get; set; }
    }
}
