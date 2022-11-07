using System.ComponentModel;

namespace MidCapERP.Dto.Customers
{
    public class CustomerApiDropDownResponceDto
    {
        [DisplayName("Reffered By")]
        public long RefferedById { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public string CustomerType { get; set; }
        public string MobileNo { get; set; }
    }
}