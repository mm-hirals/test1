using MidCapERP.Dto.Order;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IOrderBL
    {
        public Task<IEnumerable<OrderResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<OrderRequestDto> CreateOrder(OrderRequestDto model, CancellationToken cancellationToken);

        public Task<IEnumerable<OrderForDorpDownByOrderNoResponseDto>> GetCustomerForDropDownByOrderNo(string orderNo, CancellationToken cancellation);

        public Task<OrderResponseDto> GetOrderForDetailsByOrderNo(string searchText, CancellationToken cancellation);
    }
}