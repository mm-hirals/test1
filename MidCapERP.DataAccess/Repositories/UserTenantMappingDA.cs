using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class UserTenantMappingDA : IUserTenantMappingDA
    {
        private readonly ISqlRepository<UserTenantMapping> _UserTenantMapping;

        public UserTenantMappingDA(ISqlRepository<UserTenantMapping> userTenantMapping)
        {
            _UserTenantMapping = userTenantMapping;
        }

        public async Task<IQueryable<UserTenantMapping>> GetAll(CancellationToken cancellationToken)
        {
            return await _UserTenantMapping.GetAsync(cancellationToken, x => x.IsDeleted == false);
        }

        public async Task<UserTenantMapping> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _UserTenantMapping.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<UserTenantMapping> CreateUserTenant(UserTenantMapping model, CancellationToken cancellationToken)
        {
            return await _UserTenantMapping.InsertAsync(model, cancellationToken);
        }

        public async Task<UserTenantMapping> UpdateUserTenant(int Id, UserTenantMapping model, CancellationToken cancellationToken)
        {
            return await _UserTenantMapping.UpdateAsync(model, cancellationToken);
        }
    }
}