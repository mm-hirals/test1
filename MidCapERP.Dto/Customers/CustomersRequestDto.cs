using System.ComponentModel;

namespace MidCapERP.Dto.Customers
{
    public class CustomersRequestDto
    {
        public int CustomerId { get; set; }

        [DisplayName("Customer Name")]
		public string CustomerName { get; set; }
		public string EmailId { get; set; }
		public string PhoneNumber { get; set; }
		public string AltPhoneNumber { get; set; }
		public string BillingStreet1 { get; set; }
		public string BillingStreet2 { get; set; }
		public string BillingLandmark { get; set; }
		public string BillingArea { get; set; }
		public string BillingCity { get; set; }
		public string BillingState { get; set; }
		public string BillingZipCode { get; set; }
		public string ShippingStreet1 { get; set; }
		public string ShippingStreet2 { get; set; }
		public string ShippingLandmark { get; set; }
		public string ShippingArea { get; set; }
		public string ShippingCity { get; set; }
		public string ShippingState { get; set; }
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