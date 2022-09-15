using Microsoft.AspNetCore.Identity;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IRoleDA
    {
        public Task<IQueryable<ApplicationRole>> GetRoles(CancellationToken cancellationToken);

        public Task<IdentityResult> CreateRole(ApplicationRole model);

        public Task<IdentityResult> UpdateRole(ApplicationRole model);
    }
}