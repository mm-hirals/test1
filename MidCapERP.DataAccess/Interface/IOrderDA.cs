using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IOrderDA
    {
        public Task<IQueryable<Order>> GetAll(CancellationToken cancellationToken);

        public Task<Order> GetById(long Id, CancellationToken cancellationToken);

        public Task<Order> CreateOrder(Order model, CancellationToken cancellationToken);

        public Task<Order> UpdateOrder(Order model, CancellationToken cancellationToken);

        public Task<Order> DeleteOrder(Order model, CancellationToken cancellationToken);

        public Task<string?> CreateOrderNo(string type, CancellationToken cancellationToken);
    }
}