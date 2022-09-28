using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;

namespace MidCapERP.DataAccess.Repositories
{
    public class OrderDA : IOrderDA
    {
        private readonly ISqlRepository<Order> _order;
        private readonly ISqlRepository<OrderSet> _orderSet;
        private readonly ISqlRepository<OrderSetItem> _orderSetItem;
        private readonly CurrentUser _currentUser;

        public OrderDA(ISqlRepository<Order> order, ISqlRepository<OrderSet> orderSet, ISqlRepository<OrderSetItem> orderSetItem, CurrentUser currentUser)
        {
            _order = order;
            _orderSet = orderSet;
            _orderSetItem = orderSetItem;
            _currentUser = currentUser;
        }

        public async Task<IQueryable<Order>> GetAll(CancellationToken cancellationToken)
        {
            return await _order.GetAsync(cancellationToken, x => x.TenantId == _currentUser.TenantId);
        }

        public async Task<Order> GetById(long Id, CancellationToken cancellationToken)
        {
            return await _order.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<IQueryable<OrderSet>> GetAllOrderSet(CancellationToken cancellationToken)
        {
            return await _orderSet.GetAsync(cancellationToken, x => x.IsDeleted == false);
        }

        public async Task<IQueryable<OrderSetItem>> GetAllOrderSetItem(CancellationToken cancellationToken)
        {
            return await _orderSetItem.GetAsync(cancellationToken, x => x.IsDeleted == false);
        }

        public async Task<Order> CreateOrder(Order model, CancellationToken cancellationToken)
        {
            return await _order.InsertAsync(model, cancellationToken);
        }

        public async Task<Order> UpdateOrder(Int64 Id, Order model, CancellationToken cancellationToken)
        {
            return await _order.UpdateAsync(model, cancellationToken);
        }
    }
}