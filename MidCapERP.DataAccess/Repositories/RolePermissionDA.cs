using Microsoft.AspNetCore.Identity;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;
using System.Security.Claims;

namespace MidCapERP.DataAccess.Repositories
{
    public class RolePermissionDA : IRolePermissionDA
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RolePermissionDA(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IList<Claim>> GetRoleClaimsByRole(ApplicationRole applicationRole, CancellationToken cancellationToken)
        {
            return await _roleManager.GetClaimsAsync(applicationRole);
        }

        public async Task<IdentityResult> CreateRolePermission(ApplicationRole applicationRole, string claimValue, CancellationToken cancellationToken)
        {
            return await _roleManager.AddClaimAsync(applicationRole, new Claim("Permission", claimValue));
        }

        public async Task<IdentityResult> DeleteRolePermission(ApplicationRole applicationRole, string claimValue, CancellationToken cancellationToken)
        {
            return await _roleManager.RemoveClaimAsync(applicationRole, new Claim("Permission", claimValue));
        }
    }
}