using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("OrderSetItemImages")]
    public class OrderSetItemImage
    {
        [Key]
        public long OrderSetItemImageId { get; set; }

        public long OrderSetItemId { get; set; }
        public string? DrawImage { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
    }
}