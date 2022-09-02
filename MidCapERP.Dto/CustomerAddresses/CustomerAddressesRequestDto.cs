using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.CustomerAddresses
{
    public class CustomerAddressesRequestDto
    {
        public long CustomerAddressId { get; set; }
        public long CustomerId { get; set; }

        [DisplayName("Address Type")]
        public string AddressType { get; set; }

        [DisplayName("Street1")]
        [MaxLength(200)]
        [MinLength(0, ErrorMessage = "Please enter 6 digits")]
        public string Street1 { get; set; }

        [DisplayName("Street2")]
        public string? Street2 { get; set; }

        [DisplayName("Landmark")]
        public string? Landmark { get; set; }

        [DisplayName("Area")]
        public string Area { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("State")]
        public string State { get; set; }

        [DisplayName("ZipCode")]
        [MaxLength(6)]
        [MinLength(6, ErrorMessage = "Please enter 6 digits")]
        public string ZipCode { get; set; }

        [DisplayName("IsDefault")]
        public bool IsDefault { get; set; }

        public int TenantID { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}