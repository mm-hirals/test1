using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.Product
{
    public class ProductRequestDto
    {
        public long ProductId { get; set; }

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

        [Required]
        public decimal Height { get; set; }

        [Required]
        public decimal Depth { get; set; }

        [DisplayName("Fabric")]
        public decimal? UsedFabric { get; set; }

        [DisplayName("Polish")]
        public decimal? UsedPolish { get; set; }

        [Required]
        [DisplayName("Only for wholesalers")]
        public bool IsVisibleToWholesalers { get; set; }

        [Required]
        [DisplayName("Total days to prepare")]
        public decimal TotalDaysToPrepare { get; set; }

        [Required]
        public string Features { get; set; }

        [Required]
        public string Comments { get; set; }

        //[Required]
        public decimal CostPrice { get; set; }

        //[Required]
        public decimal RetailerPrice { get; set; }

        //[Required]
        public decimal WholesalerPrice { get; set; }

        //[Required]
        public string CoverImage { get; set; }

        //[Required]
        [DisplayName("QR Image")]
        public string QRImage { get; set; }

        public int TenantId { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}