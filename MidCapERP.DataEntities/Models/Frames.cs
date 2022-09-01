using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("Frames")]
    public class Frames : BaseEntity
    {
        [Key]
        public int FrameId { get; set; }

        public int FrameTypeId { get; set; }
        public string Title { get; set; }
        public string ModelNo { get; set; }
        public int CompanyId { get; set; }
        public int UnitId { get; set; }
        public decimal UnitPrice { get; set; }
        public string ImagePath { get; set; }
        public int TenantId { get; set; }
    }
}