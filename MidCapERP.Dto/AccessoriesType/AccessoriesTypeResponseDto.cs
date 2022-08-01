using System.ComponentModel;

namespace MidCapERP.Dto.AccessoriesTypes
{
    public class AccessoriesTypesResponseDto
    {
        public int AccessoriesTypeId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        [DisplayName("TypeName")]
        public string TypeName { get; set; }
        public int TenantId { get; set; }

        [DisplayName("IsCompleted")]
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