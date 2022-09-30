using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public long OrderId { get; set; }

        public string OrderNo { get; set; }
        public long CustomerID { get; set; }
        public long? RefferedBy { get; set; }
        public decimal GrossTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal GSTTaxAmount { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? Comments { get; set; }
        public int Status { get; set; }
        public int TenantId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}