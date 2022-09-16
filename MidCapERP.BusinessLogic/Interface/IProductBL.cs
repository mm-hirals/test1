using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Product;
using MidCapERP.Dto.ProductMaterial;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IProductBL
    {
        public Task<IEnumerable<ProductResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<ProductResponseDto>> GetFilterProductData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<ProductRequestDto> GetById(Int64 Id, CancellationToken cancellationToken);

        public Task<List<ProductMaterialRequestDto>> GetMaterialByProductId(Int64 Id, CancellationToken cancellationToken);

        public Task<ProductRequestDto> GetByIdAPI(Int64 Id, CancellationToken cancellationToken);

        public Task<IEnumerable<ProductForDorpDownByModuleNoResponseDto>> GetProductForDropDownByModuleNo(string modelNo, CancellationToken cancellation);

        public Task<IList<ProductForDetailsByModuleNoResponceDto>> GetProductForDetailsByModuleNo(string modelNo, CancellationToken cancellation);

        public Task<ProductRequestDto> CreateProduct(ProductRequestDto model, CancellationToken cancellationToken);

        public Task<ProductRequestDto> CreateProductDetail(ProductRequestDto model, CancellationToken cancellationToken);

        public Task<ProductRequestDto> UpdateProduct(int Id, ProductRequestDto model, CancellationToken cancellationToken);

        public Task<ProductRequestDto> UpdateProductDetail(int Id, ProductRequestDto model, CancellationToken cancellationToken);

        public Task<List<ProductMaterialRequestDto>> CreateProductMaterial(int productId, List<ProductMaterialRequestDto> productMaterialRequestList, CancellationToken cancellationToken);
        public Task<ProductRequestDto?> UpdateProductCost(int Id, ProductMainRequestDto model, CancellationToken cancellationToken);
    }
}