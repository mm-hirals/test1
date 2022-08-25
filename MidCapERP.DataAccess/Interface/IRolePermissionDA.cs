using Microsoft.AspNetCore.Identity;
using MidCapERP.DataEntities.Models;
using System.Security.Claims;

namespace MidCapERP.DataAccess.Interface
{
    public interface IRolePermissionDA
    {
        public Task<IList<Claim>> GetRoleClaimsByRole(ApplicationRole applicationRole, CancellationToken cancellationToken);

        public Task<IdentityResult> CreateRolePermission(ApplicationRole applicationRole, string claimValue, CancellationToken cancellationToken);

        public Task<IdentityResult> DeleteRolePermission(ApplicationRole applicationRole, string claimValue, CancellationToken cancellationToken);
    }
}