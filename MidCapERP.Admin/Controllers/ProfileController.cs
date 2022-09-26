using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto;
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

        //public async Task<IActionResult> Update(CancellationToken cancellationToken)
        //{
        //    var Profile = await _unitOfWorkBL.TenantBL.UpdateTenant(_currentUser.TenantId, cancellationToken);
        //    Profile.BankDetail = await _unitOfWorkBL.TenantBankDetailBL.UpdateTenantBankDetail(_currentUser.TenantId, cancellationToken);
        //    return View("Index", Profile);
        //}

    }
}