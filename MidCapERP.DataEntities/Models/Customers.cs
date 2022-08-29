using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("Customers")]
    public class Customers : BaseEntity
    {
        [Key]
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string? EmailId { get; set; }
        public string PhoneNumber { get; set; }
        public string? AltPhoneNumber { get; set; }
        public string? GSTNo { get; set; }
        public string? RefferedBy { get; set; }
        public string? RefferedContactNo { get; set; }
        public int TenantId { get; set; }
    }
}