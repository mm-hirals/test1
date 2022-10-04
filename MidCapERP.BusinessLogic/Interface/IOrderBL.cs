using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.MegaSearch;
using MidCapERP.Dto.Order;
using MidCapERP.Dto.OrderCalculation;
using MidCapERP.Dto.OrderSetItem;
using MidCapERP.Dto.Product;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IOrderBL
    {
        public Task<IEnumerable<OrderResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<OrderResponseDto>> GetFilterOrderData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<OrderResponseDto> GetOrderDetailData(long Id, CancellationToken cancellationToken);

        public Task<IEnumerable<MegaSearchResponse>> GetOrderForDropDownByOrderNo(string orderNo, CancellationToken cancellation);

        public Task<OrderResponseDto> GetOrderForDetailsByOrderNo(string searchText, CancellationToken cancellation);

        public Task<OrderApiResponseDto> GetOrderDetailByOrderIdAPI(long id, CancellationToken cancellation);

        public Task<OrderApiResponseDto> CreateOrderAPI(OrderApiRequestDto model, CancellationToken cancellationToken);

        public Task<OrderApiResponseDto> UpdateOrderAPI(Int64 Id, OrderApiRequestDto model, CancellationToken cancellationToken);

        public Task<OrderApiResponseDto> UpdateOrderDiscountAmount(Int64 orderSetItemId, decimal discountPrice, CancellationToken cancellationToken);
        public Task<OrderCalculationApiResponseDto> CalculateProductDimensionPriceAPI(ProductRequestDto productData, int subjectTypeId, int subjectId, decimal width, decimal height, decimal depth, int quantity, CancellationToken cancellationToken);
        public Task DeleteOrder(OrderDeleteApiRequestDto orderDeleteApiRequestDto, CancellationToken cancellationToken);
    }
}