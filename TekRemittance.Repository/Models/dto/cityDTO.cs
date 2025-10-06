namespace TekRemittance.Web.Models.dto
{
    public class cityDTO
    {
        public Guid Id { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Guid CountryId { get; set; }
        public Guid ProvinceId { get; set; }
    }
}
