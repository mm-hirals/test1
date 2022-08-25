using MidCapERP.WebAPI.Middleware;
using MidCapERP.Infrastructure.Extensions;
using Serilog;
using static System.Net.Mime.MediaTypeNames;

namespace MidCapERP.WebAPI.Configuration
{
    public static class ConfigureMethod
    {
        public static void ConfigureWebApplication(this WebApplication app)
        {
            // seed the default data
            app.Services.SeedIdentityDataAsync().Wait();

            // Set Common DateTime Format Globally
            //app.ConfigureCulture();

            // Configure the HTTP request pipeline.
            //if (!app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseExceptionHandler("/Dashboard/Error");
                app.UseHsts();
            }
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI();

            //app.UseNToastNotify();

            // using static System.Net.Mime.MediaTypeNames;
            app.UseStatusCodePages(Text.Plain, "Status Code Page: {0}");

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