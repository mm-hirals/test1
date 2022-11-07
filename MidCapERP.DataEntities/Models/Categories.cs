using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("Categories")]
    public class Categories
    {
        [Key]
        public long CategoryId { get; set; }
        public long CategoryTypeId { get; set; }
        public string CategoryName { get; set; }
        public bool IsFixedPrice { get; set; }
        public decimal RSPPercentage { get; set; }
        public decimal WSPPercentage { get; set; }
        public long TenantId { get; set; }
        public bool IsDeleted { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}
