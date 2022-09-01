using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("ProductImages")]
    public class ProductImage : BaseEntity
    {
        public long ProductImageID { get; set; }
        public long ProductId { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
    }
}