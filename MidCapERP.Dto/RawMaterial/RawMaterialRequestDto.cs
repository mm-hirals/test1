using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.RawMaterial
{
    public class RawMaterialRequestDto
    {
        public int RawMaterialId { get; set; }

        [DisplayName("Title")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Minimum 2 characters, Maximum 150 characters")]
        [Remote("DuplicateRawMaterialTitle", "RawMaterial", AdditionalFields = nameof(RawMaterialId), ErrorMessage = "Title already exist. Please enter a different Title.")]
        public string Title { get; set; }

        [DisplayName("Unit Name")]
        public int UnitId { get; set; }

        [DisplayName("Unit Price")]
        [Range(0, 999999, ErrorMessage = "Minimum 1 character, Maximum 6 characters")]
        public decimal UnitPrice { get; set; }

        public string? ImagePath { get; set; }

        [DisplayName("Photo Upload")]
        [DataType(DataType.Upload)]
        public IFormFile? UploadImage { get; set; }

        public int TenantId { get; set; }

        [DisplayName("IsDeleted")]
        public bool IsDeleted { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}