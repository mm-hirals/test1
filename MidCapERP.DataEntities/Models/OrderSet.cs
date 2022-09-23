using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("OrderSet")]
    public class OrderSet : BaseEntity
    {
        [Key]
        public Int64 OrderSetId { get; set; }

        public string OrderId { get; set; }
        public string SetName { set; get; }
        public decimal TotalAmount { set; get; }
    }
}