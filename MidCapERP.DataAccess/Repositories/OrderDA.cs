using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class OrderDA : IOrderDA
    {
        private readonly ISqlRepository<Order> _order;

        public OrderDA(ISqlRepository<Order> order)
        {
            _order = order;
        }

        public async Task<IQueryable<Order>> GetAll(CancellationToken cancellationToken)
        {
            return await _order.GetAsync(cancellationToken, x => x.IsDeleted == false);
        }

        public async Task<Order> CreateOrder(Order model, CancellationToken cancellationToken)
        {
            return await _order.InsertAsync(model, cancellationToken);
        }
    }
}