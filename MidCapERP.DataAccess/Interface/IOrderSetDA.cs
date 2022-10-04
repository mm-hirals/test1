using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IOrderSetDA
    {
        public Task<IQueryable<OrderSet>> GetAll(CancellationToken cancellationToken);

        public Task<OrderSet> GetById(long Id, CancellationToken cancellationToken);

        public Task<OrderSet> CreateOrderSet(OrderSet orderset, CancellationToken cancellationToken);

        public Task<OrderSet> UpdateOrderSet(OrderSet model, CancellationToken cancellationToken);

        public Task<OrderSet> DeleteOrderSet(OrderSet model, CancellationToken cancellationToken);
    }
}