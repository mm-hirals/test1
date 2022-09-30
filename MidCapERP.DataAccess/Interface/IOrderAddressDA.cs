﻿using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IOrderAddressDA
    {
        public Task<OrderAddress> CreateOrderAddress(OrderAddress model, CancellationToken cancellationToken);

        public Task<OrderAddress> UpdateOrderAddress(Int64 Id, OrderAddress model, CancellationToken cancellationToken);
    }
}