﻿using System.ComponentModel;

namespace MidCapERP.Dto.CustomerAddresses
{
    public class CustomerAddressesResponseDto
    {
        public Int64 CustomerAddressId { get; set; }
        public Int64 CustomerId { get; set; }
        public int AddressTypeId { get; set; }

        [DisplayName("Street1")]
        public string Street1 { get; set; }

        [DisplayName("Street2")]
        public string? Street2 { get; set; }

        [DisplayName("Landmark")]
        public string? Landmark { get; set; }

        [DisplayName("Area")]
        public string Area { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("State")]
        public string State { get; set; }

        [DisplayName("ZipCode")]
        public string ZipCode { get; set; }

        [DisplayName("IsDefault")]
        public bool IsDefault { get; set; }

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