using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class OrderSetItemImageDA : IOrderSetItemImageDA
    {
        private readonly ISqlRepository<OrderSetItemImage> _orderSetItemImage;

        public OrderSetItemImageDA(ISqlRepository<OrderSetItemImage> orderSetItemImage)
        {
            _orderSetItemImage = orderSetItemImage;
        }

        public async Task<IQueryable<OrderSetItemImage>> GetAll(CancellationToken cancellationToken)
        {
            return await _orderSetItemImage.GetAsync(cancellationToken);
        }

        public async Task<OrderSetItemImage> GetById(long Id, CancellationToken cancellationToken)
        {
            return await _orderSetItemImage.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<OrderSetItemImage> CreateOrderSetItemImage(OrderSetItemImage model, CancellationToken cancellationToken)
        {
            return await _orderSetItemImage.InsertAsync(model, cancellationToken);
        }

        public async Task<OrderSetItemImage> UpdateOrderSetItemImage(OrderSetItemImage model, CancellationToken cancellationToken)
        {
            return await _orderSetItemImage.UpdateAsync(model, cancellationToken);
        }

        public async Task<OrderSetItemImage> DeleteOrderSetItemImage(OrderSetItemImage model, CancellationToken cancellationToken)
        {
            return await _orderSetItemImage.DeleteAsync(model, cancellationToken);
        }
    }
}