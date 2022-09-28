using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MidCapERP.Admin.Middleware;

namespace MidCapERP.Admin.Controllers
{
    [TenantSelectionFilter]
    public class BaseController : Controller
    {
        public readonly IStringLocalizer<BaseController> _localizer;

        public BaseController(IStringLocalizer<BaseController> localizer)
        {
            _localizer = localizer;
        }
    }
}