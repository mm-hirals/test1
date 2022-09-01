using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class UserTenantMappingDA : IUserTenantMappingDA
    {
        private readonly ISqlRepository<UserTenantMapping> _userTenantMapping;

        public UserTenantMappingDA(ISqlRepository<UserTenantMapping> userTenantMapping)
        {
            _userTenantMapping = userTenantMapping;
        }

        public async Task<IQueryable<UserTenantMapping>> GetAll(CancellationToken cancellationToken)
        {
            return await _userTenantMapping.GetAsync(cancellationToken, x => x.IsDeleted == false);
        }

        public async Task<UserTenantMapping> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _userTenantMapping.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<UserTenantMapping> CreateUserTenant(UserTenantMapping model, CancellationToken cancellationToken)
        {
            return await _userTenantMapping.InsertAsync(model, cancellationToken);
        }

        public async Task<UserTenantMapping> UpdateUserTenant(int Id, UserTenantMapping model, CancellationToken cancellationToken)
        {
            return await _userTenantMapping.UpdateAsync(model, cancellationToken);
        }
    }
}