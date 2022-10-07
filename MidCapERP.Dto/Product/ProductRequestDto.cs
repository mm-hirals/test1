using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.Product
{
    public class ProductRequestDto
    {
        public Int64 ProductId { get; set; }

        [DisplayName("Product Category")]
        public int CategoryId { get; set; }

        [Required]
        [DisplayName("Product Title")]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Maximum 150 characters.")]
        public string ProductTitle { get; set; }

        [Required]
        [DisplayName("Model No")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Maximum 20 characters.")]
        public string ModelNo { get; set; }

        [Required]
        public decimal Width { get; set; }

        [DisplayName("Width Numeric")]
        [Required(ErrorMessage = "The Width field is required.")]
        [RegularExpression("^\\d+$", ErrorMessage = "The Width Not Valid.")]
        [StringLength(3, MinimumLength = 1, ErrorMessage = "The Minimum 1 Number, Maximum 3 Number.")]
        public string WidthNumeric { get; set; }

        [Required]
        public decimal Height { get; set; }

        [DisplayName("Height Numeric")]
        [Required(ErrorMessage = "The Height field is required.")]
        [RegularExpression("^\\d+$", ErrorMessage = "The Height Not Valid.")]
        [StringLength(3, MinimumLength = 1, ErrorMessage = "The Minimum 1 Number, Maximum 3 Number.")]
        public string HeightNumeric { get; set; }

        [Required]
        public decimal Depth { get; set; }

        [DisplayName("Depth Numeric")]
        [Required(ErrorMessage = "The Depth field is required.")]
        [RegularExpression("^\\d+$", ErrorMessage = "The Depth Not Valid.")]
        [StringLength(3, MinimumLength = 1, ErrorMessage = "The Minimum 1 Number, Maximum 3 Number.")]
        public string DepthNumeric { get; set; }

        [DisplayName("Fabric")]
        [StringLength(3, MinimumLength = 1, ErrorMessage = "The Minimum 1 Number, Maximum 3 Number.")]
        public decimal? FabricNeeded { get; set; }

        [Required]
        [DisplayName("Only for wholesalers")]
        public bool IsVisibleToWholesalers { get; set; }

        [DisplayName("Total days to prepare")]
        [RegularExpression("(0)|\\d{1,3}", ErrorMessage = "The Day Not Valid.")]
        public decimal TotalDaysToPrepare { get; set; }

        [StringLength(2000, MinimumLength = 1, ErrorMessage = "The Minimum 1 Number, Maximum 2000 Number.")]
        public string? Features { get; set; }

        [StringLength(2000, MinimumLength = 1, ErrorMessage = "The Minimum 1 Number, Maximum 2000 Number.")]
        public string? Comments { get; set; }

        [Required]
        [DisplayName("Cost Price")]
        public decimal CostPrice { get; set; }

        [DisplayName("QR Image")]
        public string? QRImage { get; set; }

        public int TenantId { get; set; }
        public int Status { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public string? UpdatedByName { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}