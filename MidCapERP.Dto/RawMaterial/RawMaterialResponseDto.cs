using System.ComponentModel;

namespace MidCapERP.Dto.RawMaterial
{
    public class RawMaterialResponseDto
    {
        public int RawMaterialId { get; set; }

        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("Unit Id")]
        public int UnitId { get; set; }

        [DisplayName("Unit Name")]
        public string UnitName { get; set; }

        [DisplayName("Unit Price")]
        public decimal UnitPrice { get; set; }

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