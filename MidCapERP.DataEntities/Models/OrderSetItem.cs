using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("OrderSetItems")]
    public class OrderSetItem : BaseEntity
    {
        [Key]
        public long OrderSetItemId { get; set; }

        public long OrderId { get; set; }
        public long OrderSetId { get; set; }
        public int SubjectTypeId { get; set; }
        public long SubjectId { get; set; }
        public string ProductImage { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal? Depth { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public string Comment { get; set; }
        public int Status { get; set; }
    }
}