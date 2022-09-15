using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Product;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IProductBL
    {
        public Task<IEnumerable<ProductResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<ProductResponseDto>> GetFilterProductData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<ProductMainRequestDto> GetById(Int64 Id, CancellationToken cancellationToken);

        public Task<ProductRequestDto> CreateProduct(ProductRequestDto model, CancellationToken cancellationToken);

        public Task<ProductRequestDto> UpdateProduct(int Id, ProductMainRequestDto model, CancellationToken cancellationToken);
    }
}