using System.ComponentModel;

namespace MidCapERP.Dto.Customers
{
    public class CustomerApiDropDownResponceDto
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
    }
}