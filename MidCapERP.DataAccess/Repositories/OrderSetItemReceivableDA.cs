using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class OrderSetItemReceivableDA : IOrderSetItemReceivableDA
    {
        private readonly ISqlRepository<OrderSetItemReceivable> _orderSetItemReceivable;

        public OrderSetItemReceivableDA(ISqlRepository<OrderSetItemReceivable> orderSetItemReceivable)
        {
            _orderSetItemReceivable = orderSetItemReceivable;
        }

        public async Task<IQueryable<OrderSetItemReceivable>> GetAll(CancellationToken cancellationToken)
        {
            return await _orderSetItemReceivable.GetAsync(cancellationToken);
        }

        public async Task<OrderSetItemReceivable> GetById(long Id, CancellationToken cancellationToken)
        {
            return await _orderSetItemReceivable.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<OrderSetItemReceivable> CreateOrderSetItemReceivable(OrderSetItemReceivable model, CancellationToken cancellationToken)
        {
            return await _orderSetItemReceivable.InsertAsync(model, cancellationToken);
        }

        public async Task<OrderSetItemReceivable> UpdateOrderSetItemReceivable(OrderSetItemReceivable model, CancellationToken cancellationToken)
        {
            return await _orderSetItemReceivable.UpdateAsync(model, cancellationToken);
        }

        public async Task<OrderSetItemReceivable> DeleteOrderSetItemReceivable(OrderSetItemReceivable model, CancellationToken cancellationToken)
        {
            return await _orderSetItemReceivable.DeleteAsync(model, cancellationToken);
        }
    }
}