using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class OrderSetItemDA : IOrderSetItemDA
    {
        private readonly ISqlRepository<OrderSetItem> _orderSetItem;

        public OrderSetItemDA(ISqlRepository<OrderSetItem> orderSetItem)
        {
            _orderSetItem = orderSetItem;
        }

        public async Task<IQueryable<OrderSetItem>> GetAll(CancellationToken cancellationToken)
        {
            return await _orderSetItem.GetAsync(cancellationToken);
        }

        public async Task<OrderSetItem> GetById(long Id, CancellationToken cancellationToken)
        {
            return await _orderSetItem.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<OrderSetItem> CreateOrderSetItem(OrderSetItem model, CancellationToken cancellationToken)
        {
            return await _orderSetItem.InsertAsync(model, cancellationToken);
        }

        public async Task<OrderSetItem> UpdateOrderSetItem(OrderSetItem model, CancellationToken cancellationToken)
        {
            return await _orderSetItem.UpdateAsync(model, cancellationToken);
        }

        public async Task<OrderSetItem> DeleteOrderSetItem(OrderSetItem model, CancellationToken cancellationToken)
        {
            return await _orderSetItem.DeleteAsync(model, cancellationToken);
        }
    }
}