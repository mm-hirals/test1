using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.Wood
{
    public class WoodResponseDto
    {
        [DisplayName("Wood Id")]
        public int WoodId { get; set; }

        [Display(Name = "Wood Type Name")]
        public string WoodTypeName { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Model No")]
        public string ModelNo { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

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