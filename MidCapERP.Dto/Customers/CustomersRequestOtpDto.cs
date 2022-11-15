using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace MidCapERP.Dto.Customers
{
    public class CustomersRequestOtpDto
    {
        [JsonIgnore]
        public long CustomerId { get; set; }
        public string PhoneNumber { get; set; }
        public string? OTP { get; set; }
    }
}
