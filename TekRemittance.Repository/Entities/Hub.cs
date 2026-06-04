using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Entities
{
    public class Hub
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }
        [MaxLength(128)]
        public string? Code { get; set; }
        [MaxLength(256)]
        public string? Name { get; set; }
        public bool IsActive { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }
}
