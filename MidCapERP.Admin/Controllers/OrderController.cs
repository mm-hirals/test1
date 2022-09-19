using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace MidCapERP.Admin.Controllers
{
    public class OrderController : BaseController
    {
        public OrderController(IStringLocalizer<BaseController> localizer) : base(localizer)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult OrderDetail()
        {
            return View();
        }
    }
}