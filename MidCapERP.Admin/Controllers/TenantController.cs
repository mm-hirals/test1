using Microsoft.AspNetCore.Mvc;
using MidCapERP.Infrastructure.Constants;
using MidCapERP.Infrastructure.Identity.Models;

namespace MidCapERP.Admin.Controllers
{
    public class TenantController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] SelectTenant selectTenant, CancellationToken cancellationToken)
        {
            var encValue = MagnusMinds.Utility.Encryption.Encrypt(selectTenant.TenantId, true, ApplicationIdentityConstants.EncryptionSecret);
            Response.Cookies.Append(ApplicationIdentityConstants.TenantCookieName, encValue);
            return RedirectToAction("Index", "Dashboard");
        }
    }
}