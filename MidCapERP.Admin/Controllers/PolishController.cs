using Microsoft.AspNetCore.Mvc;

namespace MidCapERP.Admin.Controllers
{
    public class PolishController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
