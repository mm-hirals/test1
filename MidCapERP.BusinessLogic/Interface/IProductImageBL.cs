using MidCapERP.Dto.ProductImage;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IProductImageBL
    {
        public Task<ProductImageRequestDto> CreateProductImage(List<ProductImageRequestDto> model, CancellationToken cancellationToken);
    }
}