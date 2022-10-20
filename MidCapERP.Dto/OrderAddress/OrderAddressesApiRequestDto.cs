using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.OrderAddressesApi
{
    public class OrderAddressesApiRequestDto
    {
        public long OrderAddressId { get; set; }

        [Required]
        public long OrderId { get; set; }

        [Required]
        public long CustomerAddressId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string? EmailId { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string AddressType { get; set; }

        [Required]
        public string Street1 { get; set; }

        public string? Street2 { get; set; }
        public string? Landmark { get; set; }

        [Required]
        public string Area { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime CreatedUTCDate { get; set; }

        [Required]
        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}