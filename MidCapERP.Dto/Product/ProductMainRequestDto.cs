using Microsoft.AspNetCore.Http;
using MidCapERP.Dto.ProductMaterial;

namespace MidCapERP.Dto.Product
{
    public class ProductMainRequestDto
    {
        public ProductMainRequestDto()
        {
            ProductRequestDto = new ProductRequestDto();
            ProductMaterialRequestDto = new List<ProductMaterialRequestDto>();
        }

        public ProductRequestDto ProductRequestDto { get; set; }

        public List<ProductMaterialRequestDto> ProductMaterialRequestDto { get; set; }

        public List<IFormFile> Files { get; set; }
    }
}