using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.ProductQuantities;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IProductQuantitiesBL
    {
        public Task<JsonRepsonse<ProductQuantitiesResponseDto>> GetFilterProductQuantitiesData(ProductQuantitiesDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<ProductQuantitiesRequestDto> GetById(long Id, CancellationToken cancellationToken);

        public Task<ProductQuantitiesRequestDto> UpdateProductQuantities(long Id, ProductQuantitiesRequestDto model, CancellationToken cancellationToken);
    }
}