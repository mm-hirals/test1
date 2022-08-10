using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IUserTenantMappingDA
    {
        public Task<IQueryable<UserTenantMapping>> GetAll(CancellationToken cancellationToken);
    }
}