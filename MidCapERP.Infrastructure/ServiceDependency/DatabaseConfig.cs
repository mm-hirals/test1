using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MidCapERP.DataEntities;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.Infrastructure.ServiceDependency
{
    public static class DatabaseConfig
    {
        /// <summary>
        ///     Setup ASP.NET Core Identity DB, including connection string, Identity options, token providers, and token services, etc..
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void SetupIdentityDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            string dataBaseEnvironment = configuration.GetConnectionString("DataBaseEnvironment");

            if (dataBaseEnvironment == "MSSQL")
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("MidCapERP.DataEntities")));
            }

            if (dataBaseEnvironment == "MYSQL")
            {
                services.AddDbContextPool<ApplicationDbContext>(options =>
                {
                    options.UseMySql(
                        configuration.GetConnectionString("DefaultConnection"),
                        ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection")),
                        options => options.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: System.TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null)
                        );
                });
            }
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                    .AddDefaultTokenProviders()
                    .AddUserManager<UserManager<ApplicationUser>>()
                    .AddSignInManager<SignInManager<ApplicationUser>>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.Configure<IdentityOptions>(
                options =>
                {
                    //options.ClaimsIdentity.UserIdClaimType = TokenEnum.UserId.ToString();
                    //options.ClaimsIdentity.UserNameClaimType = TokenEnum.Name.ToString();
                    options.SignIn.RequireConfirmedEmail = true;
                    options.User.RequireUniqueEmail = true;
                    options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    // Identity : Default password settings
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;
                });

            // services required using Identity
            //services.AddScoped<ITokenService, TokenService>();
            //services.AddScoped(typeof(ISqlRepository<>), typeof(SqlDBRepository<>));
            //services.AddScoped<IUsersRepository, UsersRepository>();
            //services.AddScoped<IRolesRepository, RolesRepository>();
            //services.AddScoped<ILogRepository, LogRepository>();
            //services.AddScoped<ITenantRepository, TenantRepository>();
            //services.AddScoped<IMenuRepository, MenuRepository>();
            //services.AddScoped<ITemplateRepository, TemplateRepository>();
            //services.AddScoped<ITenantDataSourceRepository, TenantDataSourceRepository>();
            //services.AddScoped<IMenuMappingRepository, MenuMappingRepository>();
            //services.AddScoped<ICachingService, CachingService>();
        }
    }
}