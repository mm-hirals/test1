using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;

namespace MidCapERP.Infrastructure.Localizer.JsonString
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IDistributedCache _cache;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public JsonStringLocalizerFactory(IDistributedCache cache, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _cache = cache;
        }

        public IStringLocalizer Create(Type resourceSource) => new JsonStringLocalizer(_cache, _webHostEnvironment);

        public IStringLocalizer Create(string baseName, string location) => new JsonStringLocalizer(_cache, _webHostEnvironment);
    }
}