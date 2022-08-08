using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.AccessoriesType
{
    public class AccessoriesTypeRequestDto
    {
        [Required]
        public int AccessoriesTypeId { get; set; }

        [DisplayName("Category Name")]
        public int CategoryId { get; set; }

        [DisplayName("Accessory Type Name")]
        public string AccessoryTypeName { get; set; }

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