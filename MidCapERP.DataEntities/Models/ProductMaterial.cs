using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("ProductMaterials")]
    public class ProductMaterial
    {
        [Key]
        public long ProductMaterialID { get; set; }

        public long ProductId { get; set; }
        public int SubjectTypeId { get; set; }
        public int SubjectId { get; set; }
        public int Qty { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}