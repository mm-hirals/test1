using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IUserTenantMappingDA
    {
        public Task<IQueryable<UserTenantMapping>> GetAll(CancellationToken cancellationToken);

        public Task<UserTenantMapping> GetById(int Id, CancellationToken cancellationToken);

        public Task<UserTenantMapping> CreateUserTenant(UserTenantMapping model, CancellationToken cancellationToken);

        public Task<UserTenantMapping> UpdateUserTenant(int Id, UserTenantMapping model, CancellationToken cancellationToken);
    }
}