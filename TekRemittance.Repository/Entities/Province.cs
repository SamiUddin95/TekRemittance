using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Entities
{
    public class Province
    {
        public Guid Id { get; set; }

        public string ProvinceCode { get; set; }

        public string ProvinceName { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        // Foreign Key
        public Guid CountryId { get; set; }

        public Country Country { get; set; }

    }
}
