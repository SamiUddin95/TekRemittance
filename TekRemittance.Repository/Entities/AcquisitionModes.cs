using System;

namespace TekRemittance.Repository.Entities
{
    [Flags]
    public enum AcquisitionModes
    {
        None = 0,
        IsOnLineAllow = 1 << 0,
        IsFileUploadAllow = 1 << 1,
        IsFTPAllow = 1 << 2,
        IsEmailUploadAllow = 1 << 3,
        IsWebServiceAllow = 1 << 4,
        IsBeneficiarySMSAllow = 1 << 5,
        IsActive = 1 << 6
    }
}
