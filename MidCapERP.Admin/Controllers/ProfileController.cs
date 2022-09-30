using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto;
using MidCapERP.Dto.Tenant;
using MidCapERP.Dto.TenantBankDetail;

namespace MidCapERP.Admin.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;
        private readonly CurrentUser _currentUser;

        public ProfileController(IUnitOfWorkBL unitOfWorkBL, IStringLocalizer<BaseController> localizer, CurrentUser currentUser) : base(localizer)
        {
            _unitOfWorkBL = unitOfWorkBL;
            _currentUser = currentUser;
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Profile.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var Profile = await _unitOfWorkBL.TenantBL.GetById(_currentUser.TenantId, cancellationToken);
            return View("Index", Profile);
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Profile.View)]
        public async Task<IActionResult> GetFilterTenantBankDetailData([FromForm] TenantBankDetailDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.TenantBankDetailBL.GetFilterTenantBankDetailData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Profile.Update)]
        public async Task<IActionResult> UpdateTenant(TenantRequestDto tenantRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.TenantBL.UpdateTenant(tenantRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Profile.Create)]
        public async Task<IActionResult> CreateTenantBankDetail(CancellationToken cancellationToken)
        {
            return PartialView("_TenantBankDetailPartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Profile.Create)]
        public async Task<IActionResult> CreateTenantBankDetail(TenantBankDetailRequestDto tenantBankDetailRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.TenantBankDetailBL.CreateTenantBankDetail(tenantBankDetailRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateTenantBankDetail(int Id, CancellationToken cancellationToken)
        {
            var tenantBankDetail = await _unitOfWorkBL.TenantBankDetailBL.GetById(Id, cancellationToken);
            return PartialView("_TenantBankDetailPartial", tenantBankDetail);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Profile.Update)]
        public async Task<IActionResult> UpdateTenantBankDetail(int Id, TenantBankDetailRequestDto tenantBankDetailRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.TenantBankDetailBL.UpdateTenantBankDetail(Id, tenantBankDetailRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Profile.Delete)]
        public async Task<IActionResult> DeleteTenantBankDetail(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.TenantBankDetailBL.DeleteTenantBankDetail(Id, cancellationToken);
            return RedirectToAction("_TenantBankDetailPartial");
        }

    }
}