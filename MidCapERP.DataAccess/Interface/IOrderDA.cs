using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IOrderDA
    {
        public Task<IQueryable<Order>> GetAll(CancellationToken cancellationToken);

        public Task<Order> CreateOrder(Order model, CancellationToken cancellationToken);
    }
}