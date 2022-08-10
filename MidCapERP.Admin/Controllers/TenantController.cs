using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Infrastructure.Constants;
using MidCapERP.Infrastructure.Identity.Models;

namespace MidCapERP.Admin.Controllers
{
    public class TenantController : Controller
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public TenantController(IUnitOfWorkBL unitOfWorkBL)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var userData = await _unitOfWorkBL.UserTenantMappingBL.GetAll(cancellationToken);
            var data = userData.Select(a => new SelectListItem
            {
                Value = Convert.ToString(a.TenantId),
                Text = a.TenantName
            }).ToList();
            ViewBag.FillUserDropDown = data;
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