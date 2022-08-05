using Microsoft.AspNetCore.Mvc;

namespace MidCapERP.Admin.Controllers
{
    public class ProductController : BaseController
    {
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            return View();
        }
    }
}
