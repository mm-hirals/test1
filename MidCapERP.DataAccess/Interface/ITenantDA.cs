using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface ITenantDA
    {
        public Task<IQueryable<Tenant>> GetAll(CancellationToken cancellationToken);
        public Task<Tenant> GetById(int Id, CancellationToken cancellationToken);
        public Task<Tenant> UpdateTenant(int Id, Tenant model, CancellationToken cancellationToken);
    }
}