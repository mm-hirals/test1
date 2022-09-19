using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace MidCapERP.Infrastructure.Localizer.JsonString
{
    public static class JsonStringServiceHandler
    {
        public static void SetupJsonStrinLocalizer(this IServiceCollection services)
        {
            services.AddLocalization();
            services.AddSingleton<LocalizationMiddleware>();
            services.AddDistributedMemoryCache();
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
        }
    }
}