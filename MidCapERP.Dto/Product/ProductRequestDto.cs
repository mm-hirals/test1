using Microsoft.AspNetCore.Mvc;
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
        [Remote("DuplicateModelNo", "Product", AdditionalFields = nameof(ProductId), ErrorMessage = "Model no already exist. Please enter a different model no.")]
        public string ModelNo { get; set; }

        public decimal? Width { get; set; }

        [DisplayName("Width Numeric")]
        [StringLength(6)]
        public string? WidthNumeric { get; set; }

        public decimal? Height { get; set; }

        [DisplayName("Height Numeric")]
        [StringLength(6)]
        public string? HeightNumeric { get; set; }

        public decimal? Depth { get; set; }

        [DisplayName("Depth Numeric")]
        [StringLength(6)]
        public string? DepthNumeric { get; set; }

        public decimal? Diameter { get; set; }

        [DisplayName("Diameter Numeric")]
        [StringLength(6)]
        public string? DiameterNumeric { get; set; }

        [DisplayName("Fabric Needed")]
        public decimal? FabricNeeded { get; set; }

        [Required]
        [DisplayName("Only for wholesalers")]
        public bool IsVisibleToWholesalers { get; set; }

        [DisplayName("Total days to prepare")]
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