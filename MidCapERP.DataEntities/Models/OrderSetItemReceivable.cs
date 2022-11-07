using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("OrderSetItemReceivables")]
    public class OrderSetItemReceivable
    {
        [Key]
        public long OrderSetItemReceivableId { get; set; }

        public long OrderSetItemId { get; set; }
        public DateTime ReceivedDate { get; set; }
        public decimal ProvidedMaterial { get; set; }
        public string ReceivedFrom { get; set; }
        public long ReceivedBy { get; set; }
        public string? Comment { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}