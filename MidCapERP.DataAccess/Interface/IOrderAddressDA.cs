﻿using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IOrderAddressDA
    {
        public Task<IQueryable<OrderAddress>> GetOrderAddressesByOrderId(long OrderId, CancellationToken cancellationToken);

        public Task<OrderAddress> GetById(long Id, CancellationToken cancellationToken);

        public Task<OrderAddress> CreateOrderAddress(OrderAddress model, CancellationToken cancellationToken);

        public Task<OrderAddress> UpdateOrderAddress(Int64 Id, OrderAddress model, CancellationToken cancellationToken);

        public Task<OrderAddress> DeleteOrderAddress(Int64 Id, OrderAddress model, CancellationToken cancellationToken);
    }
}