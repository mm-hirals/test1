using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;

namespace MidCapERP.DataAccess.Repositories
{
    public class OrderAddressDA : IOrderAddressDA
    {
        private readonly ISqlRepository<OrderAddress> _orderAddressDA;
        private readonly CurrentUser _currentUser;

        public OrderAddressDA(ISqlRepository<OrderAddress> orderAddress, CurrentUser currentUser)
        {
            _orderAddressDA = orderAddress;
            _currentUser = currentUser;
        }

        public async Task<IQueryable<OrderAddress>> GetOrderAddressesByOrderId(long OrderId, CancellationToken cancellationToken)
        {
            return await _orderAddressDA.GetAsync(cancellationToken, x => x.OrderId == OrderId);
        }

        public async Task<OrderAddress> CreateOrderAddress(OrderAddress model, CancellationToken cancellationToken)
        {
            return await _orderAddressDA.InsertAsync(model, cancellationToken);
        }

        public async Task<OrderAddress> UpdateOrderAddress(Int64 Id, OrderAddress model, CancellationToken cancellationToken)
        {
            return await _orderAddressDA.UpdateAsync(model, cancellationToken);
        }
    }
}