using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("Fabrics")]
    public class Fabric : BaseEntity
    {
        public int FabricId { get; set; }
        public string Title { get; set; }
        public string ModelNo { get; set; }
        public int CompanyId { get; set; }
        public int UnitId { get; set; }
        public decimal UnitPrice { get; set; }
        public string? ImagePath { get; set; }
        public int TenantId { get; set; }
    }
}