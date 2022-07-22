using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("Customers")]
    public class Customers : BaseEntity
    {
        [Key]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string PhoneNumber { get; set; }
        public int? TenantId { get; set; }
    }
}
