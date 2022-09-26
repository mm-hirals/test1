using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("OrderSets")]
    public class OrderSet : BaseEntity
    {
        [Key]
        public long OrderSetId { get; set; }

        public long OrderId { get; set; }
        public string SetName { set; get; }
        public decimal TotalAmount { set; get; }
    }
}