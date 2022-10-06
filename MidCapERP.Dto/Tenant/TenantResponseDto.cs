﻿using MidCapERP.Dto.TenantBankDetail;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.Tenant
{
    public class TenantResponseDto
    {
        public int TenantId { get; set; }

        [Required]
        [DisplayName("Organization Name")]
        public string TenantName { get; set; }

        [Required]
        [DisplayName("Owner First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Owner Last Name")]
        public string LastName { get; set; }

        [Required]
        [DisplayName("Email Id")]
        public string EmailId { get; set; }

        [Required]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [DisplayName("Logo")]
        public string? LogoPath { get; set; }

        [Required]
        [DisplayName("Street Address1")]
        public string StreetAddress1 { get; set; }

        [Required]
        [DisplayName("Street Address2")]
        public string? StreetAddress2 { get; set; }

        [DisplayName("Landmark")]
        public string? Landmark { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        [Required]
        [DisplayName("State")]
        public string State { get; set; }

        [Required]
        [DisplayName("Pincode")]
        public string Pincode { get; set; }

        [Required]
        [DisplayName("GST No")]
        public string GSTNo { get; set; }

        [Required]
        [DisplayName("Retailer SP %")]
        public decimal? ProductRSPPercentage { get; set; }

        [Required]
        [DisplayName("WholeSaler SP %")]
        public decimal? ProductWSPPercentage { get; set; }

        [Required]
        [DisplayName("Architect Discount %")]
        public decimal? ArchitectDiscount { get; set; }

        [Required]
        [DisplayName("Fabric Retailer SP %")]
        public decimal? FabricRSPPercentage { get; set; }

        [Required]
        [DisplayName("Fabric WholeSaler SP %")]
        public decimal? FabricWSPPercentage { get; set; }

        [Required]
        [DisplayName("Amount Rounding off Multiple")]
        public int? AmountRoundMultiple { get; set; }

        public string IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedUTCDate { get; set; }
        public string? UpdatedBy { get; set; }
        public string? UpdatedDate { get; set; }
        public string? UpdatedUTCDate { get; set; }
        public string? WebsiteURL { get; set; }
        public string? TwitterURL { get; set; }
        public string? FacebookURL { get; set; }
        public string? InstagramURL { get; set; }

        public List<TenantBankDetailResponseDto> BankDetail { get; set; }
    }
}