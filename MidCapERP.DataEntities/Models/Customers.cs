using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("Customers")]
    public class Customers : BaseEntity
    {
        [Key]
        public long CustomerId { get; set; }

        public int CustomerTypeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? EmailId { get; set; }
        public string PhoneNumber { get; set; }
        public string? AltPhoneNumber { get; set; }
        public string? GSTNo { get; set; }
        public long? RefferedBy { get; set; }
        public decimal Discount { get; set; }
        public int TenantId { get; set; }
        public bool IsSubscribe { get; set; }
    }
}