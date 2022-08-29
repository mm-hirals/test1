using System.ComponentModel;

namespace MidCapERP.Dto.Customers
{
    public class CustomersResponseDto
    {
        public int CustomerId { get; set; }

        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }

        [DisplayName("Email Address")]
        public string EmailId { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [DisplayName("Alternate Phone Number")]
        public string AltPhoneNumber { get; set; }

        public string? GSTNo { get; set; }
        public string? RefferedBy { get; set; }
        public string? RefferedContactNo { get; set; }
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