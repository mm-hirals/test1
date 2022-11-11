using Microsoft.AspNetCore.Http;
using MidCapERP.Dto.ProductImage;
using MidCapERP.Dto.ProductMaterial;
using System.ComponentModel;

namespace MidCapERP.Dto.Product
{
    public class ProductMainRequestDto
    {
        public ProductMainRequestDto()
        {
            ProductRequestDto = new ProductRequestDto();
            ProductImageRequestDto = new List<ProductImageRequestDto>();
            ProductMaterialRequestDto = new List<ProductMaterialRequestDto>();
        }

        public Int64 ProductId { get; set; } = 0;

        [DisplayName("Cost Price")]
        public decimal CostPrice { get; set; }
        public bool isFixedPrice { get; set; }

        public string Status { get; set; }
        public ProductRequestDto ProductRequestDto { get; set; }
        public List<ProductImageRequestDto> ProductImageRequestDto { get; set; }
        public List<ProductMaterialRequestDto> ProductMaterialRequestDto { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}