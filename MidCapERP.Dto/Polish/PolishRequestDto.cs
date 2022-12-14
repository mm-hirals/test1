using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.Polish
{
    public class PolishRequestDto
    {
        public int PolishId { get; set; }
        public string Title { get; set; }

        [DisplayName("Model No")]
        public string ModelNo { get; set; }

        [DisplayName("Company Name")]
        public int CompanyId { get; set; }

        [DisplayName("Unit Name")]
        public int UnitId { get; set; }

        [DisplayName("Unit Price")]
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