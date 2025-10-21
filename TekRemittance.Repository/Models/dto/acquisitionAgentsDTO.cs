using System;

namespace TekRemittance.Web.Models.dto
{
    public class acquisitionAgentsDTO
    {
        public Guid Id { get; set; }

        public string Code { get; set; }
        public string AgentName { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }
        public string? LogoUrl { get; set; }

        public string? Address { get; set; }
        public Guid CountryId { get; set; }
        public Guid ProvinceId { get; set; }
        public Guid CityId { get; set; }

        public TimeSpan CutOffTimeStart { get; set; }
        public TimeSpan CutOffTimeEnd { get; set; }
        public string RIN { get; set; }
        public string Process { get; set; }
        public string AcquisitionModes { get; set; }
        public string DisbursementModes { get; set; }
        public bool DirectIntegration { get; set; }
        public bool IsActive { get; set; }
       public string InquiryURL { get; set; }
        public string PaymentURL { get; set; }
        public string? UnlockURL { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
