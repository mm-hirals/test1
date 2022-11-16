using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidCapERP.DataEntities.Models
{
    [Table("CustomerVisits")]
    public class CustomerVisits
    {
        [Key]
        public long CustomerVisitId { get; set; }
        public long CustomerId { get; set; }
        public string? Comment { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
    }
}
