using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IOrderSetItemReceivableDA
    {
        public Task<IQueryable<OrderSetItemReceivable>> GetAll(CancellationToken cancellationToken);

        public Task<OrderSetItemReceivable> GetById(long Id, CancellationToken cancellationToken);

        public Task<OrderSetItemReceivable> CreateOrderSetItemReceivable(OrderSetItemReceivable OrderSetItem, CancellationToken cancellationToken);

        public Task<OrderSetItemReceivable> UpdateOrderSetItemReceivable(OrderSetItemReceivable model, CancellationToken cancellationToken);

        public Task<OrderSetItemReceivable> DeleteOrderSetItemReceivable(OrderSetItemReceivable model, CancellationToken cancellationToken);
    }
}