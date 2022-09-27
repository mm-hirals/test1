using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;

namespace MidCapERP.DataAccess.Repositories
{
    public class OrderSetItemDA : IOrderSetItemDA
    {
        private readonly ISqlRepository<OrderSetItem> _orderSetItem;
        private readonly CurrentUser _currentUser;

        public OrderSetItemDA(ISqlRepository<OrderSetItem> orderSetItem, CurrentUser currentUser)
        {
            _orderSetItem = orderSetItem;
            _currentUser = currentUser;
        }

        public async Task<IQueryable<OrderSetItem>> GetAll(CancellationToken cancellationToken)
        {
            return await _orderSetItem.GetAsync(cancellationToken);
        }

        public async Task<OrderSetItem> CreateOrderSetItem(OrderSetItem model, CancellationToken cancellationToken)
        {
            return await _orderSetItem.InsertAsync(model, cancellationToken);
        }
    }
}