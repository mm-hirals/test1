using MidCapERP.Dto.ProductImage;
using MidCapERP.Dto.ProductMaterial;

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

        public int SubjectId { get; set; }

        public ProductRequestDto ProductRequestDto { get; set; }

        public List<ProductImageRequestDto> ProductImageRequestDto { get; set; }

        public List<ProductMaterialRequestDto> ProductMaterialRequestDto { get; set; }
    }
}