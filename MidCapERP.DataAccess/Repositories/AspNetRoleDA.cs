using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class AspNetRoleDA : IAspNetRoleDA
    {
        private readonly ISqlRepository<ApplicationRole> _roles;

        public AspNetRoleDA(ISqlRepository<ApplicationRole> roles)
        {
            _roles = roles;
        }

        public async Task<IQueryable<ApplicationRole>> GetAll(CancellationToken cancellationToken)
        {
            return await _roles.GetAsync(cancellationToken);
        }
    }
}