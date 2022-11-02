using MagnusMinds.Utility.EmailService;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using MidCapERP.Admin.Middleware.TagHelper;
using MidCapERP.Infrastructure.ServiceDependency;
using Serilog;

namespace MidCapERP.Admin.Configuration
{
    public static class BuilderMethod
    {
        public static void ConfigureBuilder(this WebApplicationBuilder builder)
        {
            ConfigurationManager configuration = builder.Configuration;
            builder.Services.ConfigureEmail(configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
            builder.Services.SetupControllers();
            builder.Services.SetupIdentityDatabase(configuration);
            builder.Services.SetupDIServices(configuration);
            builder.Services.AddRazorPages().AddNToastNotifyToastr();
            builder.Services.AddSingleton<ITagHelperInitializer<ScriptTagHelper>, AppendVersionTagHelperInitializer>();
            builder.Services.AddSingleton<ITagHelperInitializer<LinkTagHelper>, AppendVersionTagHelperInitializer>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            builder.Services.AddRazorTemplating();

            /// Put this down at the last thing.
            Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();
            builder.Host.UseSerilog(((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration)));
        }
    }
}