using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IOrderDA
    {
        public Task<IQueryable<Order>> GetAll(CancellationToken cancellationToken);

        public Task<Order> GetById(long Id, CancellationToken cancellationToken);

        public Task<IQueryable<OrderSet>> GetAllOrderSet(CancellationToken cancellationToken);

        public Task<IQueryable<OrderSetItem>> GetAllOrderSetItem(CancellationToken cancellationToken);

        public Task<Order> CreateOrder(Order model, CancellationToken cancellationToken);

        public Task<Order> UpdateOrder(Int64 Id, Order model, CancellationToken cancellationToken);
    }
}