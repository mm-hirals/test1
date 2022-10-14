using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.CustomerAddresses
{
    public class CustomerAddressesApiRequestDto
    {
        [Required]
        public long CustomerId { get; set; }

        [DisplayName("Address Type")]
        public string AddressType { get; set; } = String.Empty;

        [DisplayName("Street1")]
        [MaxLength(200)]
        [MinLength(0, ErrorMessage = "Please enter 6 digits")]
        public string? Street1 { get; set; } = String.Empty;

        [DisplayName("Street2")]
        public string? Street2 { get; set; }

        [DisplayName("Landmark")]
        public string? Landmark { get; set; } = String.Empty;

        [DisplayName("Area")]
        public string? Area { get; set; } = String.Empty;

        [DisplayName("City")]
        public string? City { get; set; } = String.Empty;

        [DisplayName("State")]
        public string? State { get; set; } = String.Empty;

        [DisplayName("Pincode")]
        [MaxLength(6)]
        [MinLength(6, ErrorMessage = "Please enter 6 digits")]
        public string? ZipCode { get; set; } = String.Empty;

        [DisplayName("IsDefault")]
        public bool IsDefault { get; set; } = false;
    }
}