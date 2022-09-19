using Microsoft.AspNetCore.Localization;
using MidCapERP.Infrastructure.Localizer.JsonString;
using System.Globalization;

namespace MidCapERP.Admin.Middleware
{
    public static class UseJsonStringLocalizer
    {
        /// <summary>
        /// Use exception handler to handle the exception
        /// </summary>
        /// <param name="app"></param>
        public static void UseLocalizationMiddleware(this IApplicationBuilder app)
        {
            var options = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(new CultureInfo("en-US"))
            };
            app.UseRequestLocalization(options);
            app.UseStaticFiles();
            app.UseMiddleware<LocalizationMiddleware>();
        }
    }
}