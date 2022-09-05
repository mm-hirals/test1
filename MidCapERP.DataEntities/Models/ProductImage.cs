using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("ProductImages")]
    public class ProductImage
    {
        public long ProductImageID { get; set; }
        public long ProductId { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
    }
}