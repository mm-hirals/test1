using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace MidCapERP.Admin.Configuration
{
    public static class SetupCultureInfo
    {
        public static void ConfigureCulture(this WebApplication app)
        {
            var _configuration = app.Services.GetRequiredService<IConfiguration>();

            // Set Common DateTime Format Globally
            var culture = CultureInfo.CreateSpecificCulture(_configuration["AppSettings:CultureInfo:SpecificCulture"]);
            var dateformat = new DateTimeFormatInfo
            {
                ShortDatePattern = _configuration["AppSettings:CultureInfo:ShortDatePattern"],
                LongDatePattern = _configuration["AppSettings:CultureInfo:LongDatePattern"]
            };
            culture.DateTimeFormat = dateformat;
            var supportedCultures = new[] { culture };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
        }
    }
}