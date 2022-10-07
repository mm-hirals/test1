﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.Polish
{
    public class PolishRequestDto
    {
        public int PolishId { get; set; }

        [StringLength(150, MinimumLength = 2, ErrorMessage = "Minimum 10 characters, Maximum 150 characters")]
        [Required]
        public string Title { get; set; }

        [DisplayName("Model No")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Minimum 2 characters, Maximum 50 characters")]
        public string ModelNo { get; set; }

        [DisplayName("Company Name")]
        public int CompanyId { get; set; }

        [DisplayName("Unit Name")]
        public int UnitId { get; set; }

        [DisplayName("Unit Price")]
        [StringLength(8, MinimumLength = 2, ErrorMessage = "Minimum 2 characters, Maximum 8 characters")]
        public decimal UnitPrice { get; set; }

        public string? ImagePath { get; set; }

        [DisplayName("Photo Upload")]
        [DataType(DataType.Upload)]
        public IFormFile? UploadImage { get; set; }

        public int TenantId { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}