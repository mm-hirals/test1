using System.ComponentModel;

namespace MidCapERP.Dto.Customers
{
    public class CustomersApiResponseDto
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
        
        [DisplayName("Alt Phone Number")]
        public string AltPhoneNumber { get; set; }

        [DisplayName("GST No")]
        public string? GSTNo { get; set; }

        public CustomersApiResponseDto? Reffered { get; set; }

        public bool IsSubscribe { get; set; }
    }
}