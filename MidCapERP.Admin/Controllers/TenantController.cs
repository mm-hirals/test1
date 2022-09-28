using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.Tenant;
using MidCapERP.Dto.TenantBankDetail;
using MidCapERP.Infrastructure.Constants;
using MidCapERP.Infrastructure.Identity.Models;

namespace MidCapERP.Admin.Controllers
{
    public class TenantController : Controller
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public TenantController(IUnitOfWorkBL unitOfWorkBL, IStringLocalizer<BaseController> localizer)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Tenant.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var userData = await _unitOfWorkBL.UserTenantMappingBL.GetAll(cancellationToken);
            var data = userData.Select(a => new SelectListItem
            {
                Value = Convert.ToString(a.TenantId),
                Text = a.TenantName
            }).ToList();
            ViewBag.FillUserDropDown = data;
            if (data.Count == 1)
            {
                SetTenantCookie(MagnusMinds.Utility.Encryption.Encrypt(data?.FirstOrDefault()?.Value, true, ApplicationIdentityConstants.EncryptionSecret));
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Tenant.View)]
        public async Task<IActionResult> Index([FromForm] SelectTenant selectTenant, CancellationToken cancellationToken)
        {
            var encValue = MagnusMinds.Utility.Encryption.Encrypt(selectTenant.TenantId, true, ApplicationIdentityConstants.EncryptionSecret);
            SetTenantCookie(encValue);
            return RedirectToAction("Index", "Dashboard");
        }

        

        #region PrivateMethod

        private void SetTenantCookie(string encValue)
        {
            Response.Cookies.Append(ApplicationIdentityConstants.TenantCookieName, encValue);
        }

        #endregion PrivateMethod             
    }
}