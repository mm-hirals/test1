using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class OrderAnonymousDA : IOrderAnonymousDA
    {
        private readonly ISqlRepository<OrderAnonymousView> _orderAnonymousViews;

        public OrderAnonymousDA(ISqlRepository<OrderAnonymousView> orderAnonymousViews)
        {
            _orderAnonymousViews = orderAnonymousViews;
        }

        public async Task<OrderAnonymousView> CreateOrderAnonymousViews(OrderAnonymousView model, CancellationToken cancellationToken)
        {
            return await _orderAnonymousViews.InsertAsync(model, cancellationToken);
        }
    }
}
