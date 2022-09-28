using Microsoft.AspNetCore.Localization;
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
            app.UseMiddleware<LocalizationMiddleware>();
        }
    }
}