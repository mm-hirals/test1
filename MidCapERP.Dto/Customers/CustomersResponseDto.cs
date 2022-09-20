using System.ComponentModel;

namespace MidCapERP.Dto.Customers
{
    public class CustomersResponseDto
    {
        public long CustomerId { get; set; }
        
        [DisplayName("Customer Type")]
        public int CustomerTypeId { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Email Address")]
        public string EmailId { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [DisplayName("Alt.Phone Number")]
        public string AltPhoneNumber { get; set; }
        [DisplayName("GST No")]
        public string? GSTNo { get; set; }
        public long? RefferedBy { get; set; }
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