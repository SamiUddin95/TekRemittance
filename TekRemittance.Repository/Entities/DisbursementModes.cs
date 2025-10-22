using System;

namespace TekRemittance.Repository.Entities
{
    [Flags]
    public enum DisbursementModes
    {
        None = 0,
        IsCOTCAllow = 1 << 0,
        IsDirectCreditAllow = 1 << 1,
        IsOtherCreditAllow = 1 << 2,
        IsRemitterSMSAllow = 1 << 3
    }
}
