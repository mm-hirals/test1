using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Infrastructure.Constants;
using MidCapERP.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using MidCapERP.Dto.Tenant;

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
        public async Task<IActionResult> Index([FromForm] SelectTenant selectTenant, CancellationToken cancellationToken)
        {
            var encValue = MagnusMinds.Utility.Encryption.Encrypt(selectTenant.TenantId, true, ApplicationIdentityConstants.EncryptionSecret);
            SetTenantCookie(encValue);
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            var tenant = await _unitOfWorkBL.TenantBL.GetById(1, cancellationToken);
            return View("Views/Account/Profile.cshtml",tenant);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Tenant.Update)]
        public async Task<IActionResult> Update(int Id, TenantRequestDto tenantRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.TenantBL.UpdateTenant(Id, tenantRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        #region PrivateMethod

        private void SetTenantCookie(string encValue)
        {
            Response.Cookies.Append(ApplicationIdentityConstants.TenantCookieName, encValue);
        }

        #endregion PrivateMethod
    }
}