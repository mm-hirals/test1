using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("Products")]
    public class Products : BaseEntity
    {
        [Key]
        public int ProductId { get; set; }

        public int? CategoryId { get; set; }
        public string ProductTitle { get; set; }
        public string ProductCode { get; set; }
        public decimal Price { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Depth { get; set; }
        public bool IsVisibleToWholesalers { get; set; }
        public decimal TotalDaysToPrepare { get; set; }
        public int StoreQty { get; set; }
        public string? Comments { get; set; }
        public int? TenantId { get; set; }
    }
}