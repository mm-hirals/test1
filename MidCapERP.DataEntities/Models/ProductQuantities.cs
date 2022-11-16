using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("ProductQuantities")]
    public class ProductQuantities
    {
        [Key]
        public long ProductQuantityId { get; set; }

        public long ProductId { get; set; }
        public DateTime QuantityDate { get; set; }
        public int Quantity { get; set; }
        public long LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime LastModifiedUTCDate { get; set; }
    }
}