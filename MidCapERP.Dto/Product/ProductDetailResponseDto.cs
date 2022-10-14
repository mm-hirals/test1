using MidCapERP.Dto.ProductImage;
using MidCapERP.Dto.Tenant;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.Product
{
    public class ProductDetailResponseDto
    {
        public ProductDetailResponseDto()
        {
            ProductImageResponseDto = new List<ProductImageResponseDto>();
            TenantResponseDto = new TenantResponseDto();
        }

        public Int64 ProductId { get; set; }

        [DisplayName("Category")]
        public int CategoryId { get; set; }

        [DisplayName("Product Title")]
        public string ProductTitle { get; set; }

        [DisplayName("Model No")]
        public string ModelNo { get; set; }

        [DisplayName("Width")]
        public string Width { get; set; }

        [DisplayName("Height")]
        public string Height { get; set; }

        [DisplayName("Depth")]
        public string Depth { get; set; }

        [DisplayName("Fabric Needed")]
        [StringLength(3, MinimumLength = 1, ErrorMessage = "The Minimum 1 Number, Maximum 3 Number.")]
        public decimal? FabricNeeded { get; set; }

        [DisplayName("Total days to prepare")]
        public decimal TotalDaysToPrepare { get; set; }

        public string? Features { get; set; }

        public string? Comments { get; set; }

        [DisplayName("Price")]
        public decimal RetailerPrice { get; set; }

        [DisplayName("QR Image")]
        public string? QRImage { get; set; }

        public string? Polish { get; set; }

        public List<ProductImageResponseDto> ProductImageResponseDto { get; set; }
        public TenantResponseDto TenantResponseDto { get; set; }
    }
}