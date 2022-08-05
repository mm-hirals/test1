using Microsoft.AspNetCore.Mvc;

namespace MidCapERP.Admin.Controllers
{
    public class FabricController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
