using Microsoft.AspNetCore.Localization;
using MidCapERP.Admin.Middleware;
using MidCapERP.Infrastructure.Extensions;
using Serilog;
using System.Globalization;

namespace MidCapERP.Admin.Configuration
{
    public static class ConfigureMethod
    {
        public static void ConfigureWebApplication(this WebApplication app)
        {
            // seed the default data
            app.Services.SeedIdentityDataAsync().Wait();

            // Set Common DateTime Format Globally
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            var dateformat = new DateTimeFormatInfo
            {
                ShortDatePattern = "dd/MM/yyyy",
                LongDatePattern = "dd/MM/yyyy hh:mm:ss tt"
            };
            culture.DateTimeFormat = dateformat;
            var supportedCultures = new[] { culture };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //app.UseMiddleware<UseExceptionHandlerMiddleware>();
            app.UseExceptionHandlerMiddleware();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}