using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.Customers
{
    public class CustomersRequestDto
    {
        public int CustomerId { get; set; }

        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }
        [DisplayName("Email Id")]
        [EmailAddress(ErrorMessage = "Please enter valid email address")]
        public string? EmailId { get; set; }
        [DisplayName("Phone Number")]
        [MaxLength(10)]
        [MinLength(10, ErrorMessage = "Please enter 10 digits")]
        public string PhoneNumber { get; set; }
        [DisplayName("AltPhone Number")]
        [MaxLength(10)]
        [MinLength(10, ErrorMessage = "Please enter 10 digits")]
        public string? AltPhoneNumber { get; set; }
        [DisplayName("Billing Street1")]
        public string BillingStreet1 { get; set; }
        [DisplayName("Billing Street2")]
        public string? BillingStreet2 { get; set; }
        [DisplayName("Billing Landmark")]
        public string? BillingLandmark { get; set; }
        [DisplayName("Billing Area")]
        public string BillingArea { get; set; }
        [DisplayName("Billing City")]
        public string BillingCity { get; set; }
        [DisplayName("Billing State")]
        public string BillingState { get; set; }
        [DisplayName("Billing Zip Code")]
        public string BillingZipCode { get; set; }
        [DisplayName("Shipping Street1")]
        public string ShippingStreet1 { get; set; }
        [DisplayName("Shipping Street2")]
        public string? ShippingStreet2 { get; set; }
        [DisplayName("Shipping Landmark")]
        public string? ShippingLandmark { get; set; }
        [DisplayName("Shipping Area")]
        public string ShippingArea { get; set; }
        [DisplayName("Shipping City")]
        public string ShippingCity { get; set; }
        [DisplayName("Shipping State")]
        public string ShippingState { get; set; }
        [DisplayName("Shipping Zip Code")]
        public string ShippingZipCode { get; set; }
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