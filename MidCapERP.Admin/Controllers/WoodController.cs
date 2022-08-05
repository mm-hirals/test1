using Microsoft.AspNetCore.Mvc;

namespace MidCapERP.Admin.Controllers
{
    public class WoodController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
