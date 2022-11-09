﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.InteriorAddresses
{
    public class InteriorAddressesRequestDto
    {
        public long CustomerAddressId { get; set; }

        [Required]
        public long CustomerId { get; set; }

        [DisplayName("Address Type")]
        public string AddressType { get; set; } = String.Empty;

        [DisplayName("Street1")]
        [MaxLength(200)]
        [MinLength(0, ErrorMessage = "Please enter 6 digits")]
        [Required]
        public string? Street1 { get; set; } = String.Empty;

        [DisplayName("Street2")]
        public string? Street2 { get; set; }

        [DisplayName("Landmark")]
        public string? Landmark { get; set; } = String.Empty;

        [DisplayName("Area")]
        [Required]
        public string? Area { get; set; } = String.Empty;

        [DisplayName("City")]
        [Required]
        public string? City { get; set; } = String.Empty;

        [DisplayName("State")]
        [Required]
        public string? State { get; set; } = String.Empty;

        [DisplayName("Pincode")]
        [MaxLength(6)]
        [MinLength(6, ErrorMessage = "Please enter 6 digits")]
        [Required]
        public string? ZipCode { get; set; } = String.Empty;

        [DisplayName("IsDefault")]
        public bool IsDefault { get; set; } = false;

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