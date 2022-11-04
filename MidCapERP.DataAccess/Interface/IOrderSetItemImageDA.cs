using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IOrderSetItemImageDA
    {
        public Task<IQueryable<OrderSetItemImage>> GetAll(CancellationToken cancellationToken);

        public Task<OrderSetItemImage> GetById(long Id, CancellationToken cancellationToken);

        public Task<OrderSetItemImage> CreateOrderSetItemImage(OrderSetItemImage OrderSetItem, CancellationToken cancellationToken);

        public Task<OrderSetItemImage> UpdateOrderSetItemImage(OrderSetItemImage model, CancellationToken cancellationToken);

        public Task<OrderSetItemImage> DeleteOrderSetItemImage(OrderSetItemImage model, CancellationToken cancellationToken);
    }
}