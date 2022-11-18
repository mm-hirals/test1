using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IOrderAnonymousDA
    {        
        public Task<OrderAnonymousView> CreateOrderAnonymousViews(OrderAnonymousView model, CancellationToken cancellationToken);
    }
}
