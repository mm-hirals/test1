using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MidCapERP.BusinessLogic.Extention;
using MidCapERP.DataAccess.Extention;
using MidCapERP.Dto;
using MidCapERP.Infrastructure.Identity.Authorization;
using MidCapERP.Infrastructure.Identity.Models;
using MidCapERP.Infrastructure.Services.Email;
using MidCapERP.Infrastructure.Services.Token;
using static MidCapERP.Infrastructure.Constants.ApplicationIdentityConstants;

namespace MidCapERP.Infrastructure.ServiceDependency
{
    public static class SetupServices
    {
        public static void SetupDIServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TokenConfiguration>(configuration.GetSection("token"));
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailHelper, EmailHelper>();
            services.AddScoped<CurrentUser>();
            services.SetAuthorization();
            services.SetupUnitOfWorkDA();
            services.SetupUnitOfWorkBL();
            services.SetupAutoMapper();
        }

        public static void SetAuthorization(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    options.SlidingExpiration = true;
                    options.AccessDeniedPath = "/Forbidden/";
                    options.LoginPath = "/Authorize/Login";
                });
            services.AddAuthorization(
                options =>
                {
                    foreach (var item in Permissions.GetAllPermissions())
                    {
                        options.AddPolicy(item, builder => { builder.AddRequirements(new PermissionRequirement(item)); });
                    }
                    options.AddPolicy(
                        JwtBearerDefaults.AuthenticationScheme,
                                           new AuthorizationPolicyBuilder()
                                               .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                                               .RequireAuthenticatedUser()
                                               .Build());
                });
        }
    }
}