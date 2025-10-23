using System;
using TekRemittance.Repository.Entities;

namespace TekRemittance.Web.Models.dto
{
    public class acquisitionAgentDTO
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
        public RinType RIN { get; set; }
        public ProcessType Process { get; set; }
        public AcquisitionModes AcquisitionModes { get; set; }
        public DisbursementModes DisbursementModes { get; set; }
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
