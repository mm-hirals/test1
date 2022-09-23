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

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Tenant.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            var tenant = await _unitOfWorkBL.TenantBL.GetById(1, cancellationToken);
            return View("Views/Account/ProfileMain.cshtml", tenant);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Tenant.Update)]
        public async Task<IActionResult> Update(int Id, TenantRequestDto tenantRequestDto, CancellationToken cancellationToken)
        {
            Id = tenantRequestDto.TenantId;
            await _unitOfWorkBL.TenantBL.UpdateTenant(Id, tenantRequestDto, cancellationToken);
            return RedirectToAction("Update");
        }

        [HttpGet]
        public async Task<IActionResult> GetTenantDetail(int Id, CancellationToken cancellationToken)
        {
            var tenant = await _unitOfWorkBL.TenantBL.GetById(1, cancellationToken);
            return PartialView("_TenantDetailPartial", tenant);
        }

        [HttpGet]
        public async Task<IActionResult> GetTenantBankDetail(int Id, CancellationToken cancellationToken)
        {
            var tenantBankDetail =await _unitOfWorkBL.TenantBankDetailBL.GetById(Id, cancellationToken);
            return PartialView("_TenantBankDetailPartial", tenantBankDetail);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.TenantBankDetail.Update)]
        public async Task<IActionResult> updateBankDetail(int Id, CancellationToken cancellationToken)
        {
            var tenantBankDetail = await _unitOfWorkBL.TenantBankDetailBL.GetById(Id, cancellationToken);
            return View("Views/Account/ProfileMain.cshtml", tenantBankDetail);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.TenantBankDetail.Update)]
        public async Task<IActionResult> updateBankDetail(int Id, TenantBankDetailRequestDto tenantBankDetailRequestDto, CancellationToken cancellationToken)
        {
            Id = tenantBankDetailRequestDto.TenantId;
            await _unitOfWorkBL.TenantBankDetailBL.UpdateTenantBankDetail(Id, tenantBankDetailRequestDto, cancellationToken);
            return RedirectToAction("Update", "Tenant");
        }

        #region PrivateMethod

        private void SetTenantCookie(string encValue)
        {
            Response.Cookies.Append(ApplicationIdentityConstants.TenantCookieName, encValue);
        }

        #endregion PrivateMethod
    }
}