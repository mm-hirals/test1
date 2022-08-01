using System.ComponentModel.DataAnnotations;

namespace MidCapERP.DataEntities.Models
{
    public class RawMaterial : BaseEntity
    {
        [Key]
        public int RawMaterialId { get; set; }

        public string Title { get; set; }
        public int UnitId { get; set; }
        public double UnitPrice { get; set; }
        public string ImagePath { get; set; }
        public int TenantId { get; set; }
    }
}
