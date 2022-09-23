using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IOrderSetItemDA
    {
        public Task<OrderSetItem> CreateOrder(OrderSetItem OrderSetItem, CancellationToken cancellationToken);
    }
}