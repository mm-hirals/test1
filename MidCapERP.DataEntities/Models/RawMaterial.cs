using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("RawMaterials")]
    public class RawMaterial : BaseEntity
    {
        [Key]
        public int RawMaterialId { get; set; }

        public string Title { get; set; }
        public int UnitId { get; set; }
        public decimal UnitPrice { get; set; }
        public string? ImagePath { get; set; }
        public int TenantId { get; set; }
    }
}