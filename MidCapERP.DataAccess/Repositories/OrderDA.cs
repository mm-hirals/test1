using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;

namespace MidCapERP.DataAccess.Repositories
{
    public class OrderDA : IOrderDA
    {
        private readonly ISqlRepository<Order> _order;
        private readonly CurrentUser _currentUser;

        public OrderDA(ISqlRepository<Order> order, CurrentUser currentUser)
        {
            _order = order;
            _currentUser = currentUser;
        }

        public async Task<IQueryable<Order>> GetAll(CancellationToken cancellationToken)
        {
            return await _order.GetAsync(cancellationToken, x => x.TenantId == _currentUser.TenantId);
        }

        public async Task<Order> CreateOrder(Order model, CancellationToken cancellationToken)
        {
            return await _order.InsertAsync(model, cancellationToken);
        }
    }
}