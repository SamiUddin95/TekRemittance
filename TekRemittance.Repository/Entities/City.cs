using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Entities
{
    public class City
    {
        public Guid Id { get; set; }

        public string CityCode { get; set; }

        public string CityName { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        // Foreign Key
        public Guid CountryId { get; set; }

        public Country Country { get; set; }

        public Guid ProvinceId { get; set; }

        public Province Province { get; set; }
    }
}
