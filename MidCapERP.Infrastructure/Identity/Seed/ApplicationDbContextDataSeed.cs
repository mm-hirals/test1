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
            await roleManager.CreateAsync(new ApplicationRole(ApplicationIdentityConstants.Roles.Administrator, 1));
            await roleManager.CreateAsync(new ApplicationRole(ApplicationIdentityConstants.Roles.StoreManager, 1));
            await roleManager.CreateAsync(new ApplicationRole(ApplicationIdentityConstants.Roles.Supervisor, 1));
            await roleManager.CreateAsync(new ApplicationRole(ApplicationIdentityConstants.Roles.SalesRepresentative, 1));
            await roleManager.CreateAsync(new ApplicationRole(ApplicationIdentityConstants.Roles.Contractor, 1));

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

            adminUser = await userManager.FindByNameAsync(adminUserName);
            // Add new user and their role
            if (adminUser == null)
            {
                await userManager.CreateAsync(adminUser, ApplicationIdentityConstants.DefaultPassword);
                await userManager.AddToRoleAsync(adminUser, ApplicationIdentityConstants.Roles.Administrator);
                var administratorRole = await roleManager.FindByNameAsync(ApplicationIdentityConstants.Roles.Administrator);
                //GIVE ALL PERMISSIONS TO THE ADMINISTRATOR USER
                await GrantPermissionToAdminUser(roleManager, administratorRole);
            }
        }

        private static async Task GrantPermissionToAdminUser(RoleManager<ApplicationRole> roleManager, ApplicationRole administratorRole)
        {
            var collection = new List<string>() { "Users", "Role", "Dashboard", "Lookup", "Status", "Contractor", "SubjectType", "LookupValues", "ContractorCategoryMapping", "Customer", "ErrorLogs", "Category", "Company", "Unit", "FrameType", "AccessoriesType", "RawMaterial", "Accessories", "Fabric", "Frame", "Polish", "RolePermission", "Product", "Order","Tenant","TenantBankDetail","Profile"
                };
            foreach (var item in collection)
            {
                await AddPermissionClaim(roleManager, administratorRole, item);
            }
        }

        /// <summary>
        /// Seed permissions to users in the Identity database.
        /// </summary>
        /// <param name="roleManager"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static async Task AddPermissionClaim(RoleManager<ApplicationRole> roleManager, ApplicationRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = ApplicationIdentityConstants.Permissions.GeneratePermissionsForModule(module);
            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == CustomClaimTypes.Permission && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, permission));
                }
            }
        }
    }
}