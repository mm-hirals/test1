using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.Frame
{
    public class FrameRequestDto
    {
        [DisplayName("Frame Id")]
        public int FrameId { get; set; }

        [DisplayName("Frame Type Name")]
        public int FrameTypeId { get; set; }

        [Required]
        [DisplayName("Title")]
        public string Title { get; set; }

        [Required]
        [DisplayName("Model No")]
        public string ModelNo { get; set; }

        [DisplayName("Company Name")]
        public int CompanyId { get; set; }

        [DisplayName("Unit Name")]
        public int UnitId { get; set; }

        [DisplayName("Unit Price")]
        public decimal UnitPrice { get; set; }

        public string? ImagePath { get; set; }

        [Display(Name = "Photo Upload")]
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