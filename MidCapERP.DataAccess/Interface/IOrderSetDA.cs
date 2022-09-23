using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IOrderSetDA
    {
        public Task<OrderSet> CreateOrder(OrderSet orderset, CancellationToken cancellationToken);
    }
}