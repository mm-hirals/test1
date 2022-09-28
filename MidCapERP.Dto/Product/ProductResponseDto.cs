using System.ComponentModel;

namespace MidCapERP.Dto.Product
{
    public class ProductResponseDto
    {
        public Int64 ProductId { get; set; }
        public int? CategoryId { get; set; }

        [DisplayName("Category Name")]
        public string? CategoryName { get; set; }

        [DisplayName("Product Title")]
        public string ProductTitle { get; set; }

        [DisplayName("Model No")]
        public string ModelNo { get; set; }

        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Depth { get; set; }
        public decimal? UsedFabric { get; set; }
        public bool IsVisibleToWholesalers { get; set; }
        public decimal TotalDaysToPrepare { get; set; }
        public string? Features { get; set; }
        public string? Comments { get; set; }

        [DisplayName("Cost Price")]
        public decimal CostPrice { get; set; }

        [DisplayName("Retailer Price")]
        public decimal RetailerPrice { get; set; }

        [DisplayName("Wholesaler Price")]
        public decimal WholesalerPrice { get; set; }

        public string? CoverImage { get; set; }
        public string? QRImage { get; set; }
        public int TenantId { get; set; }

        [DisplayName("Publish")]
        public byte? Status { get; set; }

        public int CreatedBy { get; set; }

        [DisplayName("Created By")]
        public string CreatedByName { get; set; }

        public DateTime CreatedDate { get; set; }

        [DisplayName("Created Date")]
        public string CreatedDateFormat => CreatedDate.ToLongDateString();

        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }

        [DisplayName("Last Modified By")]
        public string? UpdatedByName { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [DisplayName("Last Modified On")]
        public string UpdatedDateFormat { get; set; }

        public DateTime? UpdatedUTCDate { get; set; }
    }
}