using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IOrderSetItemDA
    {
        public Task<IQueryable<OrderSetItem>> GetAll(CancellationToken cancellationToken);

        public Task<OrderSetItem> GetById(long Id, CancellationToken cancellationToken);

        public Task<OrderSetItem> CreateOrderSetItem(OrderSetItem OrderSetItem, CancellationToken cancellationToken);

        public Task<OrderSetItem> UpdateOrderSetItem(Int64 Id, OrderSetItem model, CancellationToken cancellationToken);
    }
}