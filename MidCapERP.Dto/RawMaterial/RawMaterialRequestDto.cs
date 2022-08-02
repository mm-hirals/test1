using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.RawMaterial
{
    public class RawMaterialRequestDto
    {
        public int RawMaterialId { get; set; }

        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("Unit Name")]
        public int UnitId { get; set; }

        [DisplayName("Unit Price")]
        public decimal UnitPrice { get; set; }

        [DisplayName("Photo Upload")]
        [DataType(DataType.Upload)]
        public string? ImagePath { get; set; }

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