using AutoMapper;
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
    public class ProfileController : Controller
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;
        private readonly CurrentUser _currentUser;

        public ProfileController(IUnitOfWorkBL unitOfWorkBL, IStringLocalizer<BaseController> localizer, CurrentUser currentUser)
        {
            _unitOfWorkBL = unitOfWorkBL;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var Profile = await _unitOfWorkBL.TenantBL.GetById(_currentUser.TenantId, cancellationToken);
            Profile.BankDetail = await _unitOfWorkBL.TenantBankDetailBL.GetById(_currentUser.TenantId, cancellationToken);
            return View("Index", Profile);
        }
        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.TenantBankDetail.View)]
        public async Task<IActionResult> GetFilterTenantBankDetailData([FromForm] TenantBankDetailDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.TenantBankDetailBL.GetFilterTenantBankDetailData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }
        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Tenant.Update)]
        public async Task<IActionResult> UpdateTenant(int Id, CancellationToken cancellationToken)
        {
            var tenant = await _unitOfWorkBL.TenantBL.GetById(Id, cancellationToken);
            return View("Index");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Tenant.Update)]
        public async Task<IActionResult> UpdateTenant(int Id, TenantRequestDto tenantRequestDto, CancellationToken cancellationToken)
        {
            Id = tenantRequestDto.TenantId;
            await _unitOfWorkBL.TenantBL.UpdateTenant(Id, tenantRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        //[HttpGet]
        //[Authorize(ApplicationIdentityConstants.Permissions.TenantBankDetail.Create)]
        //public async Task<IActionResult> CreateTenantBankDetail(int TenantId, CancellationToken cancellationToken)
        //{
        //    TenantBankDetailRequestDto dto = new();
        //    dto.TenantId = TenantId;
        //    return View("Index");
        //}

        //[HttpPost]
        //[Authorize(ApplicationIdentityConstants.Permissions.TenantBankDetail.Create)]
        //public async Task<IActionResult> CreateTenantBankDetail(TenantBankDetailRequestDto tenantBankDetailRequestDto, CancellationToken cancellationToken)
        //{
        //    await _unitOfWorkBL.TenantBankDetailBL.CreateTenantBankDetail(tenantBankDetailRequestDto, cancellationToken);
        //    return View("Index");
        //}
        //[HttpGet]
        //[Authorize(ApplicationIdentityConstants.Permissions.TenantBankDetail.Update)]
        //public async Task<IActionResult> UpdateTenantBankDetail(int Id, CancellationToken cancellationToken)
        //{
          
        //    var customers = await _unitOfWorkBL.TenantBankDetailBL.GetById(Id, cancellationToken);
        //    return View("Index");
        //}

        //[HttpPost]
        //[Authorize(ApplicationIdentityConstants.Permissions.TenantBankDetail.Update)]
        //public async Task<IActionResult> UpdateTenantBankDetail(int Id, TenantBankDetailRequestDto tenantBankDetailRequestDto, CancellationToken cancellationToken)
        //{
        //    await _unitOfWorkBL.TenantBankDetailBL.UpdateTenantBankDetail(Id, tenantBankDetailRequestDto, cancellationToken);
        //    return RedirectToAction("Index");
        //}

    }
}