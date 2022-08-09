using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("Woods")]
    public class Woods : BaseEntity
    {
        [Key]
        public int WoodId { get; set; }

        public int WoodTypeId { get; set; }
        public string Title { get; set; }
        public string ModelNo { get; set; }
        public int CompanyId { get; set; }
        public int UnitId { get; set; }
        public decimal UnitPrice { get; set; }
        public string ImagePath { get; set; }
        public int TenantId { get; set; }
    }
}