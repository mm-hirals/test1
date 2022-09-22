using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.TenantBankDetail;
using MidCapERP.Infrastructure.Constants;

namespace MidCapERP.Admin.Controllers
{
    public class TenantBankDetailController : Controller
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public TenantBankDetailController(IUnitOfWorkBL unitOfWorkBL, IStringLocalizer<BaseController> localizer)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.TenantBankDetail.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            var TenantBankDetail = await _unitOfWorkBL.TenantBankDetailBL.GetById(Id, cancellationToken);
            return View("Views/Account/ProfileMain.cshtml", TenantBankDetail);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.TenantBankDetail.Update)]
        public async Task<IActionResult> Update(int Id, TenantBankDetailRequestDto tenantBankDetailRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.TenantBankDetailBL.UpdateTenantBankDetail(Id, tenantBankDetailRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

 
    }
}

