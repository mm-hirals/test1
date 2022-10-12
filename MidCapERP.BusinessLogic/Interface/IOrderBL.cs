using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.MegaSearch;
using MidCapERP.Dto.Order;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IOrderBL
    {
        public Task<IEnumerable<OrderResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<OrderResponseDto>> GetFilterOrderData(OrderDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<OrderResponseDto> GetOrderDetailData(long Id, CancellationToken cancellationToken);

        public Task<IEnumerable<OrderStatusApiResponseDto>> GetOrderForDetailsByStatus(string status, CancellationToken cancellationToken);

        public Task<OrderResponseDto> GetOrderSetDetailData(long Id, CancellationToken cancellationToken);

        public Task<IEnumerable<MegaSearchResponse>> GetOrderForDropDownByOrderNo(string orderNo, CancellationToken cancellation);

        public Task<OrderResponseDto> GetOrderForDetailsByOrderNo(string searchText, CancellationToken cancellation);

        public Task<OrderApiResponseDto> GetOrderDetailByOrderIdAPI(long id, CancellationToken cancellation);

        public Task<OrderApiResponseDto> CreateOrderAPI(OrderApiRequestDto model, CancellationToken cancellationToken);

        public Task<OrderApiResponseDto> UpdateOrderAPI(Int64 Id, OrderApiRequestDto model, CancellationToken cancellationToken);

        public Task<OrderApiResponseDto> UpdateOrderDiscountAmountAPI(Int64 orderSetItemId, decimal discountPrice, CancellationToken cancellationToken);

        public Task DeleteOrderAPI(OrderDeleteApiRequestDto orderDeleteApiRequestDto, CancellationToken cancellationToken);
    }
}