﻿using Microsoft.AspNetCore.Identity;
using MidCapERP.DataEntities.Models;
using MidCapERP.Infrastructure.Constants;
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
            await roleManager.CreateAsync(new ApplicationRole(ApplicationIdentityConstants.Roles.Administrator));
            await roleManager.CreateAsync(new ApplicationRole(ApplicationIdentityConstants.Roles.StoreManager));
            await roleManager.CreateAsync(new ApplicationRole(ApplicationIdentityConstants.Roles.Supervisor));
            await roleManager.CreateAsync(new ApplicationRole(ApplicationIdentityConstants.Roles.SalesRepresentative));
            await roleManager.CreateAsync(new ApplicationRole(ApplicationIdentityConstants.Roles.Contractor));

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

            // Add new user and their role
            await userManager.CreateAsync(adminUser, ApplicationIdentityConstants.DefaultPassword);
            adminUser = await userManager.FindByNameAsync(adminUserName);
            await userManager.AddToRoleAsync(adminUser, ApplicationIdentityConstants.Roles.Administrator);

            var administratorRole = await roleManager.FindByNameAsync(ApplicationIdentityConstants.Roles.Administrator);

            //GIVE ALL PERMISSIONS TO THE ADMINISTRATOR USER
            await AddPermissionClaim(roleManager, administratorRole, "Users");
            await AddPermissionClaim(roleManager, administratorRole, "Role");
            await AddPermissionClaim(roleManager, administratorRole, "Dashboard");
            await AddPermissionClaim(roleManager, administratorRole, "Category");
            await AddPermissionClaim(roleManager, administratorRole, "Lookup");
            await AddPermissionClaim(roleManager, administratorRole, "Status");
            await AddPermissionClaim(roleManager, administratorRole, "Contractor");
            await AddPermissionClaim(roleManager, administratorRole, "SubjectType");
            await AddPermissionClaim(roleManager, administratorRole, "LookupValues");
            await AddPermissionClaim(roleManager, administratorRole, "ContractorCategoryMapping");
            await AddPermissionClaim(roleManager, administratorRole, "Customer");
            await AddPermissionClaim(roleManager, administratorRole, "ErrorLogs");
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