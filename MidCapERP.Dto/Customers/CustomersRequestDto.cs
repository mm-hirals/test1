using System.ComponentModel;

namespace MidCapERP.Dto.Customers
{
    public class CustomersRequestDto
    {
        public int CustomerId { get; set; }

        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }

        [DisplayName("Billing Address")]
        public string BillingAddress { get; set; }

        [DisplayName("Shipping Address")]
        public string ShippingAddress { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        public int? TenantId { get; set; }

        [DisplayName("Deleted")]
        public bool IsDeleted { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}