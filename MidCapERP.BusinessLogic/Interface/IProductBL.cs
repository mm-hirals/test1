using MidCapERP.Dto.Product;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IProductBL
    {
        public Task<IEnumerable<ProductResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<ProductRequestDto> CreateProduct(ProductRequestDto model, CancellationToken cancellationToken);
    }
}