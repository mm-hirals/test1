using MidCapERP.Dto.CustomerAddresses;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MidCapERP.Dto.Customers
{
    public class CustomersRequestDto
    {
        public long CustomerId { get; set; }

        [DisplayName("Customer Type")]
        public int CustomerTypeId { get; set; }

        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }

        [DisplayName("Email Address")]
        [EmailAddress(ErrorMessage = "Please enter valid email address")]
        public string? EmailId { get; set; }

        [DisplayName("Phone Number")]
        [Required]
        [MaxLength(10)]
        [MinLength(10, ErrorMessage = "Please enter 10 digits")]
        public string PhoneNumber { get; set; }

        [DisplayName("Alt. Phone Number")]
        [MaxLength(10)]
        [MinLength(10, ErrorMessage = "Please enter 10 digits")]
        public string? AltPhoneNumber { get; set; }

        [DisplayName("GST No")]
        public string? GSTNo { get; set; }

        [DisplayName("Reffered Number")]
        public string? RefferedNumber { get; set; }

        [DisplayName("Reffered Name")]
        public string? RefferedName { get; set; }

        public CustomerAddressesRequestDto? CustomerAddressesRequestDto { get; set; }

        [JsonIgnore]
        public decimal Discount { get; set; }

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