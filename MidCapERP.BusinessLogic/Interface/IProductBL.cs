using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.MegaSearch;
using MidCapERP.Dto.Product;
using MidCapERP.Dto.ProductImage;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IProductBL
    {
        public Task<JsonRepsonse<ProductResponseDto>> GetFilterProductData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<ProductRequestDto> GetById(Int64 Id, CancellationToken cancellationToken);

        public Task<List<ProductImageRequestDto>> GetImageByProductId(long Id, CancellationToken cancellationToken);

        public Task<ProductMainRequestDto> GetMaterialByProductId(Int64 Id, CancellationToken cancellationToken);

        public Task<ProductRequestDto> GetByIdAPI(Int64 Id, CancellationToken cancellationToken);

        public Task<IEnumerable<MegaSearchResponse>> GetProductForDropDownByModuleNo(string modelNo, CancellationToken cancellation);

        public Task<IList<ProductForDetailsByModuleNoResponceDto>> GetProductForDetailsByModuleNo(string modelNo, CancellationToken cancellation);

        public Task<ProductRequestDto> CreateProduct(ProductRequestDto model, CancellationToken cancellationToken);

        public Task<ProductRequestDto> UpdateProduct(ProductRequestDto model, CancellationToken cancellationToken);

        public Task<ProductRequestDto> UpdateProductDetail(ProductRequestDto model, CancellationToken cancellationToken);

        public Task UpdateProductStatus(ProductMainRequestDto model, CancellationToken cancellationToken);

        public Task<ProductRequestDto?> UpdateProductCost(ProductMainRequestDto model, CancellationToken cancellationToken);

        public Task<ProductImageRequestDto> CreateProductImages(ProductMainRequestDto model, CancellationToken cancellationToken);

        public Task<ProductMainRequestDto> CreateProductMaterial(ProductMainRequestDto productMainRequestDto, CancellationToken cancellationToken);

        public Task<ProductRequestDto> DeleteProduct(int Id, CancellationToken cancellationToken);

        public Task DeleteProductImage(int productImageId, CancellationToken cancellationToken);

        public Task<int> GetRawMaterialSubjectTypeId(CancellationToken cancellationToken);

        public Task<int> GetPolishSubjectTypeId(CancellationToken cancellationToken);
    }
}