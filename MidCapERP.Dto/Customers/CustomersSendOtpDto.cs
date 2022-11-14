﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace MidCapERP.Dto.Customers
{
    public class CustomersSendOtpDto
    {

        [JsonIgnore]
        public long CustomerId { get; set; }

        [DisplayName("Customer Type")]
        public int CustomerTypeId { get; set; }

        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }

        [DisplayName("Email Address")]
        public string? EmailId { get; set; }

        [DisplayName("Phone Number")]
        [Required]
        [MaxLength(10)]
        [MinLength(10, ErrorMessage = "Please enter 10 digits")]
        public string PhoneNumber { get; set; }

        [DisplayName("Alt. Phone Number")]
        public string? AltPhoneNumber { get; set; }

        [DisplayName("GST No")]
        public string? GSTNo { get; set; }

        public long RefferedBy { get; set; }

        public bool IsSubscribe { get; set; }

        public string? OTP { get; set; }

        public DateTime ExpiryTime { get; set; }
    }
}
