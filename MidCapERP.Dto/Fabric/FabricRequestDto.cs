using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.Fabric
{
    public class FabricRequestDto
    {
        public int FabricId { get; set; }
        [DisplayName("Title")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Minimum 2 characters, Maximum 150 characters")]
        public string Title { get; set; }

        [DisplayName("Model No")]
        public string ModelNo { get; set; }

        [DisplayName("Company Name")]
        public int CompanyId { get; set; }

        [DisplayName("Unit Name")]
        public int UnitId { get; set; }

        [DisplayName("Unit Price")]
        [Range(0, 999999, ErrorMessage = "Minimum 1 character, Maximum 6 characters")]
        public decimal UnitPrice { get; set; }

        [DisplayName("Retailers Price")]
        public decimal RetailerPrice { get; set; }

        [DisplayName("Wholesalers Price")]
        public decimal WholesalerPrice { get; set; }

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