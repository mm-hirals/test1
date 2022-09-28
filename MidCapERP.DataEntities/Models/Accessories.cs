using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("Accessories")]
    public class Accessories : BaseEntity
    {
        [Key]
        public int AccessoriesId { get; set; }
        public int CategoryId { get; set; }
        public int AccessoriesTypeId { get; set; }
        public string Title { get; set; }
        public int UnitId { get; set; }
        public decimal UnitPrice { get; set; }
        public string? ImagePath { get; set; }
        public int TenantId { get; set; }
    }
}