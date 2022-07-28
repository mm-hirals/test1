using MagnusMinds.Utility.EmailService;
using MidCapERP.Infrastructure.ServiceDependency;
using Serilog;

namespace MidCapERP.Admin.Configuration
{
    public static class BuilderMethod
    {
        public static void ConfigureBuilder(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();
            builder.Host.UseSerilog(((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration)));

            ConfigurationManager configuration = builder.Configuration;
            builder.Services.ConfigureEmail(configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
            builder.Services.SetupControllers();
            builder.Services.SetupIdentityDatabase(configuration);
            builder.Services.SetupDIServices(configuration);
            builder.Services.AddRazorPages().AddNToastNotifyToastr();
        }
    }
}