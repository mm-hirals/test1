using Microsoft.AspNetCore.Identity;
using MidCapERP.Core.Constants;
using MidCapERP.DataEntities.Models;
using System.Security.Claims;

namespace MidCapERP.Infrastructure.Identity.Seed
{
    public class ApplicationDbContextDataSeed
    {
        /// <summary>
        ///     Seed users and roles in the Identity database.
        /// </summary>
        /// <param name="userManager">ASP.NET Core Identity User Manager</param>
        /// <param name="roleManager">ASP.NET Core Identity Role Manager</param>
        /// <returns></returns>
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            // Add roles supported
            await roleManager.CreateAsync(new ApplicationRole(ApplicationIdentityConstants.Roles.SuperAdmin, 0));

            // New admin user
            string adminUserName = "kparmar@magnusminds.net";
            var adminUser = new ApplicationUser
            {
                UserName = adminUserName,
                Email = adminUserName,
                IsActive = true,
                EmailConfirmed = true,
                FirstName = "Kishan",
                LastName = "Parmar"
            };

            var checkAdminUser = await userManager.FindByNameAsync(adminUserName);
            // Add new user and their role
            if (checkAdminUser == null)
            {
                await userManager.CreateAsync(adminUser, ApplicationIdentityConstants.DefaultPassword);
                await userManager.AddToRoleAsync(adminUser, ApplicationIdentityConstants.Roles.SuperAdmin);
            }
            var administratorRole = await roleManager.FindByNameAsync(ApplicationIdentityConstants.Roles.SuperAdmin);
            await GrantPermissionToAdminUser(roleManager, administratorRole);
        }

        private static async Task GrantPermissionToAdminUser(RoleManager<ApplicationRole> roleManager, ApplicationRole administratorRole)
        {
            var allClaims = await roleManager.GetClaimsAsync(administratorRole);
            var allPermissions = ApplicationIdentityConstants.Permissions.GetAllPermissions();
            foreach (var permission in allPermissions)
                if (!allClaims.Any(a => a.Type == CustomClaimTypes.Permission && a.Value == permission))
                    await roleManager.AddClaimAsync(administratorRole, new Claim(CustomClaimTypes.Permission, permission));
        }
    }
}