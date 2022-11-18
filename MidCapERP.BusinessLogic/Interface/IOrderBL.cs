using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.MegaSearch;
using MidCapERP.Dto.Order;
using MidCapERP.Dto.OrderSetItem;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IOrderBL
    {
        public Task<IEnumerable<OrderResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<OrderResponseDto> GetById(Int64 Id, CancellationToken cancellationToken);

        public Task<JsonRepsonse<OrderResponseDto>> GetFilterOrderData(OrderDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<OrderResponseDto> GetOrderDetailData(long Id, CancellationToken cancellationToken);

        public Task<IEnumerable<OrderStatusApiResponseDto>> GetOrderForDetailsByStatus(string status, CancellationToken cancellationToken);

        public Task<OrderResponseDto> GetOrderSetDetailData(long Id, CancellationToken cancellationToken);

        public Task<IEnumerable<MegaSearchResponse>> GetOrderForDropDownByOrderNo(string orderNo, CancellationToken cancellation);

        public Task<OrderResponseDto> GetOrderForDetailsByOrderNo(string searchText, CancellationToken cancellation);

        public Task<OrderApiResponseDto> GetOrderDetailByOrderIdAPI(long id, CancellationToken cancellation);

        public Task<OrderApiResponseDto> CreateOrderAPI(OrderApiRequestDto model, CancellationToken cancellationToken);

        public Task<OrderApiResponseDto> UpdateOrderAPI(Int64 Id, OrderApiRequestDto model, CancellationToken cancellationToken);

        public Task<OrderApiResponseDto> UpdateOrderAdvanceAmountAPI(Int64 orderSetItemId, decimal discountPrice, CancellationToken cancellationToken);

        public Task<OrderApiResponseDto> UpdateOrderSendForApproval(OrderUpdateStatusAPI model, CancellationToken cancellationToken);

        public Task<OrderStatusResponseDto> UpdateOrderApprovedOrDeclinedAPI(OrderUpdateApproveOrDeclineAPI model, CancellationToken cancellationToken);

        public Task<OrderApiResponseDto> GetOrderReceivableMaterial(Int64 orderId, Int64 orderSetItemId, CancellationToken cancellationToken);

        public Task<OrderMaterialReceiveResponseDto> GetOrderReceivedMaterial(Int64 orderId, Int64 orderSetItemId, CancellationToken cancellationToken);

        public Task<OrderApiResponseDto> UpdateOrderReceiveMaterial(OrderUpdateReceiveMaterialAPI model, CancellationToken cancellationToken);

        public Task DeleteOrderAPI(OrderDeleteApiRequestDto orderDeleteApiRequestDto, CancellationToken cancellationToken);

        public Task<OrderSetItem> UpdateOrderSetItemDiscount(OrderSetItemRequestDto orderSetItemRequestDto, CancellationToken cancellationToken);

        public Task<Int64> GetOrderReceivableCount(CancellationToken cancellationToken);

        public Task<Int64> GetOrderApprovedCount(CancellationToken cancellationToken);

        public Task<Int64> GetOrderPendingApprovalCount(CancellationToken cancellationToken);

        public Task<Int64> GetOrderFollowUpCount(CancellationToken cancellationToken);

        public Task<OrderResponseDto> GetOrderDetailsAnonymous(string orderNo, CancellationToken cancellationToken);

        public Task<OrderStatusResponseDto> ApproveOrderStatus(long Id, CancellationToken cancellationToken);

        public Task DeclineOrderStatus(long Id, CancellationToken cancellationToken);

        public Task ShareOrderWithCustomer(long Id, CancellationToken cancellationToken);
    }
}