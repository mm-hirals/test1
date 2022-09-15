using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.Frames
{
    public class FrameResponseDto
    {
        [DisplayName("Frame Id")]
        public int FrameId { get; set; }

        [Display(Name = "Frame Type Name")]
        public string FrameTypeName { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Model No")]
        public string ModelNo { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        public int UnitId { get; set; }

        [Display(Name = "Unit Name")]
        public string UnitName { get; set; }

        [Display(Name = "Unit Price")]
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