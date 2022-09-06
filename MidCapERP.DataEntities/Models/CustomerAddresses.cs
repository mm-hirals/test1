using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("CustomerAddresses")]
    public class CustomerAddresses : BaseEntity
    {
        [Key]
        public long CustomerAddressId { get; set; }

        public long CustomerId { get; set; }
        public string AddressType { get; set; }
        public string Street1 { get; set; }
        public string? Street2 { get; set; }
        public string? Landmark { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public bool IsDefault { get; set; }
    }
}