using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.Product
{
    public class ProductRequestDto
    {
        public Int64 ProductId { get; set; }

        [DisplayName("Product Category")]
        public int? CategoryId { get; set; }

        [Required]
        [DisplayName("Product Title")]
        public string ProductTitle { get; set; }

        [Required]
        [DisplayName("Model No")]
        public string ModelNo { get; set; }

        [Required]
        public decimal Width { get; set; }

        [DisplayName("Width Numeric")]
        public string WidthNumeric { get; set; }

        [Required]
        public decimal Height { get; set; }

        [DisplayName("Height Numeric")]
        public string HeightNumeric { get; set; }

        [Required]
        public decimal Depth { get; set; }

        [DisplayName("Depth Numeric")]
        public string DepthNumeric { get; set; }

        [DisplayName("Fabric")]
        public decimal? UsedFabric { get; set; }

        [Required]
        [DisplayName("Only for wholesalers")]
        public bool IsVisibleToWholesalers { get; set; }

        [DisplayName("Total days to prepare")]
        public decimal TotalDaysToPrepare { get; set; }

        public string? Features { get; set; }

        public string? Comments { get; set; }

        [Required]
        [DisplayName("Cost Price")]
        public decimal CostPrice { get; set; }

        [Required]
        [DisplayName("Retailer Price")]
        public decimal RetailerPrice { get; set; }

        [Required]
        [DisplayName("Wholesaler Price")]
        public decimal WholesalerPrice { get; set; }

        public string? CoverImage { get; set; }
        public IFormFile? UploadImage { get; set; }

        [DisplayName("QR Image")]
        public string? QRImage { get; set; }

        public int TenantId { get; set; }
        public byte? Status { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public string? UpdatedByName { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
        public string HostURL { get; set; }
    }
}