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
        private readonly ISqlRepository<fnGetOrderNumber> _getOrderNumber;

        public OrderDA(ISqlRepository<fnGetOrderNumber> getOrderNumber, ISqlRepository<Order> order, CurrentUser currentUser)
        {
            _order = order;
            _currentUser = currentUser;
            _getOrderNumber = getOrderNumber;
        }

        public async Task<IQueryable<Order>> GetAll(CancellationToken cancellationToken)
        {
            return await _order.GetAsync(cancellationToken, x => x.TenantId == _currentUser.TenantId);
        }

        public async Task<Order> GetById(long Id, CancellationToken cancellationToken)
        {
            return await _order.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<Order> CreateOrder(Order model, CancellationToken cancellationToken)
        {
            return await _order.InsertAsync(model, cancellationToken);
        }

        public async Task<Order> UpdateOrder(Order model, CancellationToken cancellationToken)
        {
            return await _order.UpdateAsync(model, cancellationToken);
        }

        public async Task<Order> DeleteOrder(Order model, CancellationToken cancellationToken)
        {
            return await _order.DeleteAsync(model, cancellationToken);
        }

        public async Task<string?> CreateOrderNo(string type, CancellationToken cancellationToken)
        {
            var orderData = await _getOrderNumber.GetWithRawSqlAsync($"SELECT OrderNo = dbo.GetOrderNumber('{type}') ", cancellationToken);
            return orderData.FirstOrDefault()?.OrderNo;
        }
    }
}