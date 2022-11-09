using Microsoft.AspNetCore.Mvc;
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
        [StringLength(50, MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z]+[a-zA-Z\s]*$", ErrorMessage = "The First Name is Not Valid Please Enter Valid First Name.")]
        [Required]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [StringLength(50, MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z]+[a-zA-Z\s]*$", ErrorMessage = "The Last Name is Not Valid Please Enter Valid Last Name.")]
        [Required]
        public string LastName { get; set; }

        [DisplayName("Email Address")]
        [EmailAddress(ErrorMessage = "Please enter valid email address.")]
        public string? EmailId { get; set; }

        [DisplayName("Phone Number")]
        [Required]
        [MaxLength(10)]
        [MinLength(10, ErrorMessage = "Please enter 10 digits.")]
        [Remote("DuplicateCustomerPhoneNumber", "Customer", AdditionalFields = nameof(CustomerId), ErrorMessage = "Phone Number already exist. Please enter a different Phone Number.")]
        public string PhoneNumber { get; set; }

        [DisplayName("Alt. Phone Number")]
        [MaxLength(10)]
        [MinLength(10, ErrorMessage = "Please enter 10 digits.")]
        public string? AltPhoneNumber { get; set; }

        [DisplayName("GST No")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The Minimum 2 characters, Maximum 15 characters.")]
        public string? GSTNo { get; set; }

        [DisplayName("Reffered By")]
        public long? RefferedBy { get; set; }

        public CustomerAddressesRequestDto? CustomerAddressesRequestDto { get; set; }

        [DisplayName("Commission(%)")]
        [JsonIgnore]
        public decimal? Discount { get; set; }

        [DisplayName("Subscribe Newsletters/Greetings")]
        public bool IsSubscribe { get; set; }

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