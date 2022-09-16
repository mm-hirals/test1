using MidCapERP.WebAPI.Middleware;
using MidCapERP.Infrastructure.Extensions;
using Serilog;
using static System.Net.Mime.MediaTypeNames;
using AutoWrapper;

namespace MidCapERP.WebAPI.Configuration
{
    public static class ConfigureMethod
    {
        public static void ConfigureWebApplication(this WebApplication app)
        {
            var configuration = app.Services.GetRequiredService<IConfiguration>();
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
            if (string.Equals(configuration["AppSettings:EnableSwagger"], "true", StringComparison.CurrentCultureIgnoreCase))
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //app.UseNToastNotify();

            //https://github.com/proudmonkey/AutoWrapper
            app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions { IsApiOnly = false, ApiVersion = "1.0", ShowApiVersion = true });

            // using static System.Net.Mime.MediaTypeNames;
            app.UseStatusCodePages(Text.Plain, "Status Code Page: {0}");

            //app.UseMiddleware<UseExceptionHandlerMiddleware>();
            app.UseExceptionHandlerMiddleware();
            app.UseRouting();

            app.UseCors(builder => builder
               .AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod());

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}