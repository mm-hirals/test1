using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("ProductMaterials")]
    public class ProductMaterial : BaseEntity
    {
        public long ProductMaterialID { get; set; }
        public long ProductId { get; set; }
        public int SubjectTypeId { get; set; }
        public int SubjectId { get; set; }
        public int Qty { get; set; }
        public decimal MaterialPrice { get; set; }
        public string Comments { get; set; }
    }
}