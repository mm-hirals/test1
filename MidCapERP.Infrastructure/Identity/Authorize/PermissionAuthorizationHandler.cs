using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MidCapERP.Core.Constants;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.Infrastructure.Identity.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public PermissionAuthorizationHandler(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null)
            {
                return;
            }
            /// IF USER NOT FOUND THEN UNCOMMENT BELOW LINE AND METHOD TO GET USER BY USERID
            // var user = await _userManager.FindByIdAsync(GetUserId(context.User, TokenEnum.UserId.ToString()));
            var user = await _userManager.GetUserAsync(context.User);
            if (user == null)
            {
                return;
            }
            var userRoleNames = await _userManager.GetRolesAsync(user);
            var userRoles = _roleManager.Roles.Where(x => userRoleNames.Contains(x.Name)).ToList();

            foreach (var role in userRoles)
            {
                var roleClaims = _roleManager.GetClaimsAsync(role).Result;
                var permissions = roleClaims.Where(x => x.Type == CustomClaimTypes.Permission &&
                                                        x.Value == requirement.Permission &&
                                                        x.Issuer == "LOCAL AUTHORITY")
                                            .Select(x => x.Value);
                if (permissions.Any())
                {
                    context.Succeed(requirement);
                    return;
                }
                else
                {
                    //throw new ForbiddenAccessException();
                }
            }
        }

        /// IF USER NOT FOUND THEN UNCOMMENT BELOW LINE AND METHOD TO GET USER BY USERID
        //private string GetUserId(ClaimsPrincipal principal, string claimName)
        //{
        //    return principal.FindFirstValue(claimName);
        //}
    }
}