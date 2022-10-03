using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.MegaSearch;
using MidCapERP.Dto.Order;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IOrderBL
    {
        public Task<IEnumerable<OrderResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<OrderResponseDto>> GetFilterOrderData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<OrderResponseDto> GetOrderDetailData(long Id, CancellationToken cancellationToken);

        public Task<IEnumerable<MegaSearchResponse>> GetOrderForDropDownByOrderNo(string orderNo, CancellationToken cancellation);

        public Task<OrderResponseDto> GetOrderForDetailsByOrderNo(string searchText, CancellationToken cancellation);

        public Task<OrderApiResponseDto> GetOrderDetailByOrderIdAPI(int id, CancellationToken cancellation);

        public Task<OrderApiResponseDto> CreateOrderAPI(OrderApiRequestDto model, CancellationToken cancellationToken);

        public Task<OrderApiResponseDto> UpdateOrderAPI(Int64 Id, OrderApiRequestDto model, CancellationToken cancellationToken);
    }
}