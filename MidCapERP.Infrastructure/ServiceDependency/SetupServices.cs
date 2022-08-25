﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MidCapERP.BusinessLogic.Extention;
using MidCapERP.DataAccess.Extention;
using MidCapERP.Dto;
using MidCapERP.Infrastructure.Identity.Authorization;
using MidCapERP.Infrastructure.Identity.Models;
using MidCapERP.Infrastructure.Services.Email;
using MidCapERP.Infrastructure.Services.Token;
using System.Text;
using static MidCapERP.Infrastructure.Constants.ApplicationIdentityConstants;

namespace MidCapERP.Infrastructure.ServiceDependency
{
    public static class SetupServices
    {
        public static void SetupDIServices(this IServiceCollection services, IConfiguration configuration, string AuthenticationScheme = "Cookies")
        {
            services.Configure<TokenConfiguration>(configuration.GetSection("token"));
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailHelper, EmailHelper>();
            services.AddScoped<CurrentUser>();
            services.SetAuthorization(configuration, AuthenticationScheme);
            services.SetupUnitOfWorkDA();
            services.SetupUnitOfWorkBL();
            services.SetupAutoMapper();
        }

        public static void SetAuthorization(this IServiceCollection services, IConfiguration configuration, string AuthenticationScheme)
        {
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();


            TokenConfiguration tokenConfiguration = new TokenConfiguration();
            (configuration.GetSection("token")).Bind(tokenConfiguration);
            byte[] secret = Encoding.ASCII.GetBytes(tokenConfiguration.Secret);


            //if (AuthenticationScheme == "Cookies")
            {
                services.AddAuthentication()
                    .AddCookie(options =>
                    {
                        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                        options.SlidingExpiration = true;
                        options.AccessDeniedPath = "/Forbidden/";
                        options.LoginPath = "/Authorize/Login";
                    }).AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.SaveToken = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(secret),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };

                    });
            }
            if (AuthenticationScheme == "Bearer")
            {
                //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme,options =>
                //{
                //    options.AddPolicy(JwtBearerDefaults.AuthenticationScheme,
                //        new AuthorizationPolicyBuilder()
                //        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                //        .RequireAuthenticatedUser()
                //        .Build());
                //});
                //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                //    .AddCookie(options =>
                //    {
                //        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                //        options.SlidingExpiration = true;
                //        options.AccessDeniedPath = "/Forbidden/";
                //        options.LoginPath = "/Authorize/Login";
                //    });
            }
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