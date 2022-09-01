using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface ITenantDA
    {
        public Task<IQueryable<Tenant>> GetAll(CancellationToken cancellationToken);
    }
}