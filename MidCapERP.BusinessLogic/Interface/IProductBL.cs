using MidCapERP.Dto.ActivityLogs;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.MegaSearch;
using MidCapERP.Dto.OrderCalculation;
using MidCapERP.Dto.Product;
using MidCapERP.Dto.ProductImage;
using MidCapERP.Dto.SearchResponse;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IProductBL
    {
        public Task<ProductRequestDto> GetById(Int64 Id, CancellationToken cancellationToken);

        public Task<ProductDetailResponseDto> GetProductDetailById(Int64 Id, CancellationToken cancellationToken);

        public Task<JsonRepsonse<ProductResponseDto>> GetFilterProductData(ProductDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<List<ProductImageRequestDto>> GetImageByProductId(long Id, CancellationToken cancellationToken);

        public Task<ProductMainRequestDto> GetMaterialByProductId(Int64 Id, CancellationToken cancellationToken);

        public Task<ProductForDetailsByModuleNoResponceDto> GetByIdAPI(Int64 Id, CancellationToken cancellationToken);

        public Task<IEnumerable<MegaSearchResponse>> GetProductMegaSearchForDropDownByModuleNo(string modelNo, CancellationToken cancellation);

        public Task<IEnumerable<SearchResponse>> GetProductForDropDownByModuleNo(string modelNo, CancellationToken cancellation);

        public Task<ProductForDetailsByModuleNoResponceDto> GetProductForDetailsByModuleNo(string modelNo, CancellationToken cancellation);

        public Task<ProductRequestDto> CreateProduct(ProductRequestDto model, CancellationToken cancellationToken);

        public Task<ProductRequestDto> UpdateProduct(ProductRequestDto model, CancellationToken cancellationToken);

        public Task<ProductRequestDto> UpdateProductDetail(ProductRequestDto model, CancellationToken cancellationToken);

        public Task UpdateProductStatus(ProductMainRequestDto model, CancellationToken cancellationToken);

        public Task<ProductRequestDto?> UpdateProductCost(ProductMainRequestDto model, CancellationToken cancellationToken);

        public Task<ProductImageRequestDto> CreateProductImages(ProductMainRequestDto model, CancellationToken cancellationToken);

        public Task UpdateProductImageMarkAsCover(int productImageId, bool IsCover, CancellationToken cancellationToken);

        public Task<ProductMainRequestDto> CreateProductMaterial(ProductMainRequestDto productMainRequestDto, CancellationToken cancellationToken);

        public Task<ProductRequestDto> DeleteProduct(int Id, CancellationToken cancellationToken);

        public Task DeleteProductImage(int productImageId, CancellationToken cancellationToken);

        public Task<int> GetRawMaterialSubjectTypeId(CancellationToken cancellationToken);

        public Task<int> GetPolishSubjectTypeId(CancellationToken cancellationToken);

        public Task<IEnumerable<ActivityLogsResponseDto>> GetProductActivityByProductId(Int64 productId, CancellationToken cancellationToken);

        public Task<JsonRepsonse<ActivityLogsResponseDto>> GetFilterProductActivityData(ProductActivityDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<int> GetProductSubjectTypeId(CancellationToken cancellationToken);

        public Task<int> GetFabricSubjectTypeId(CancellationToken cancellationToken);

        public Task<ProductDimensionsApiResponseDto> GetPriceByDimensionsAPI(ProductDimensionsApiRequestDto orderCalculationApiRequestDto, CancellationToken cancellationToken);

        public Task<List<ProductResponseDto>> PrintProductDetail(List<long> ProductList, CancellationToken cancellationToken);

        public Task<bool> ValidateModelNo(ProductRequestDto productRequestDto, CancellationToken cancellationToken);
    }
}