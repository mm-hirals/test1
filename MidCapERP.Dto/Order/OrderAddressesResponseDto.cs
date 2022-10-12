﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidCapERP.Dto.Order
{
    public class OrderAddressesResponseDto
    {
        public long OrderAddressId { get; set; }
        public long? OrderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? EmailId { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressType { get; set; }
        public string Street1 { get; set; }
        public string? Street2 { get; set; }
        public string? Landmark { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string? GSTNo { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}
