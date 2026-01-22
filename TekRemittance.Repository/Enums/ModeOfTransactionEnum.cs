using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Enums
{
    [Flags]
    public enum ModeOfTransactionEnum
    {
        
        FT = 1 << 0,   
        IBFT = 1 << 1,   
        RTGS = 1 << 2    
    }
}
