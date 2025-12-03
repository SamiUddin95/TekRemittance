using System;
using TekRemittance.Repository.Entities;

namespace TekRemittance.Repository.DTOs
{
    public class BranchDTO
    {
        public Guid Id { get; set; }
        public Guid AgentId { get; set; }
        public string AgentName { get; set; }
        public Guid CountryId { get; set; }
        public Guid ProvinceId { get; set; }
        public Guid CityId { get; set; }
        public string Code { get; set; }
        public string AgentBranchName { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public AcquisitionModes AcquisitionModes { get; set; }
        public DisbursementModes DisbursementModes { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }
}
