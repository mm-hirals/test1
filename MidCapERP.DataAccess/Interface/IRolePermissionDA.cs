using Microsoft.AspNetCore.Identity;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IRolePermissionDA
    {
        public Task<IdentityResult> CreateRolePermission(ApplicationRole applicationRole, string claimValue, CancellationToken cancellationToken);
    }
}