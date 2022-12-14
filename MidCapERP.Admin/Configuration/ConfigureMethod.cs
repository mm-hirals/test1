using MidCapERP.Admin.Middleware;
using MidCapERP.Infrastructure.Extensions;
using Serilog;

namespace MidCapERP.Admin.Configuration
{
    public static class ConfigureMethod
    {
        public static void ConfigureWebApplication(this WebApplication app)
        {
            // seed the default data
            app.Services.SeedIdentityDataAsync().Wait();

            // Set Common DateTime Format Globally
            app.ConfigureCulture();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Dashboard/Error");
                app.UseHsts();
            }
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseNToastNotify();

            //app.UseMiddleware<UseExceptionHandlerMiddleware>();
            app.UseExceptionHandlerMiddleware();
            app.UseRouting();

            // Localization implemented for message response
            app.UseLocalizationMiddleware();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Dashboard}/{action=Index}/{id?}");

            app.Run();
        }
    }
}