using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidCapERP.DataEntities.Models
{
    [Table("Categories")]
    public class Categories
    {
        [Key]
        public Int64 CategoryId { get; set; }
        
        public int CategoryTypeId { get; set; }
        public string? CategoryName { get; set; }
        public bool? IsFixedPrice { get; set; }
        public string? RSPPercentage { get; set; }
        public string? WSPPercentage { get; set; }
        public int TenantId { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }


    }
}
