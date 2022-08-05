using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.Accessories
{
    public class AccessoriesRequestDto
    {
        [DisplayName("AccessoriesId")]
        public int AccessoriesId { get; set; }

        [DisplayName("Type Name")]
        public string TypeName { get; set; }

        public string CategoryName { get; set; }

        [DisplayName("CategoryId")]
        public int CategoryId { get; set; }

        [DisplayName("AccessoriesTypeId")]
        public int AccessoriesTypeId { get; set; }

        [DisplayName("AccessoriesType Name")]
        public string AccessoriesTypeName { get; set; }

        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("Unit Name")]
        public string UnitName { get; set; }

        [DisplayName("Unit Id")]
        public int UnitId { get; set; }

        [DisplayName("Unit Price")]
        public int UnitPrice { get; set; }

        public string? ImagePath { get; set; }

        [Display(Name = "Photo Upload")]
        [DataType(DataType.Upload)]
        public IFormFile? UploadImage { get; set; }
        public int TenantId { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}
