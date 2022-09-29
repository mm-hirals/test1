using System.ComponentModel;

namespace MidCapERP.Dto.CustomersTypes
{
    public class CustomersTypesResponseDto
    {
        public int CustomerTypeId { get; set; }

        [DisplayName("Customer Type Name")]
        public string Name { get; set; }

        public string? GSTNo { get; set; }
        public long? RefferedBy { get; set; }
        public decimal Discount { get; set; }
        public int TenantID { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}