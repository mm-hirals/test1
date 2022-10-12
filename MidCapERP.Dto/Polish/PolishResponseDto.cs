using System.ComponentModel;

namespace MidCapERP.Dto.Polish
{
    public class PolishResponseDto
    {
        public int PolishId { get; set; }

        public string Title { get; set; }

        [DisplayName("Model No")]
        public string ModelNo { get; set; }

        public int CompanyId { get; set; }

        [DisplayName("Company Name")]
        public string CompanyName { get; set; }

        public int UnitId { get; set; }

        [DisplayName("Unit Name")]
        public string UnitName { get; set; }

        [DisplayName("Unit Price")]
        public decimal UnitPrice { get; set; }

        [DisplayName("Photo Upload")]
        public string? ImagePath { get; set; }

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