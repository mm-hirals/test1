using Microsoft.AspNetCore.Mvc;

namespace MidCapERP.Admin.Controllers
{
    public class OrderController : BaseController
    {
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
