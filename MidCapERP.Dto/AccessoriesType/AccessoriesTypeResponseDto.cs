using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.AccessoriesType
{
    public class AccessoriesTypeResponseDto
    {
        [Required]
        public int AccessoriesTypeId { get; set; }
        public int CategoryId { get; set; }
        [DisplayName("Category Name")]
        public string CategoryName { get; set; }
        [DisplayName("Type Name")]
        public string TypeName { get; set; }
        public int TenantId { get; set; }

        [DisplayName("Is Completed?")]
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