using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto.Company;
using MidCapERP.Dto.DataGrid;
using NToastNotify;

namespace MidCapERP.Admin.Controllers
{
    public class CompanyController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;
        private readonly IToastNotification _toastNotification;

        public CompanyController(IUnitOfWorkBL unitOfWorkBL, IToastNotification toastNotification, IStringLocalizer<BaseController> localizer) : base(localizer)
        {
            _unitOfWorkBL = unitOfWorkBL;
            _toastNotification = toastNotification;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Company.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Company.View)]
        public async Task<IActionResult> GetCompanyData([FromForm] DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.CompanyBL.GetFilterCompanyData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Company.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            return PartialView("_CompanyPartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Company.Create)]
        public async Task<IActionResult> Create(CompanyRequestDto lookupRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.CompanyBL.CreateCompany(lookupRequestDto, cancellationToken);

            _toastNotification.AddSuccessToastMessage("Data Saved Successfully!" + _localizer["hi"]);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Company.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            var company = await _unitOfWorkBL.CompanyBL.GetById(Id, cancellationToken);
            return PartialView("_CompanyPartial", company);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Company.Update)]
        public async Task<IActionResult> Update(int Id, CompanyRequestDto companyRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.CompanyBL.UpdateCompany(Id, companyRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Saved Successfully!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Company.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.CompanyBL.DeleteCompany(Id, cancellationToken);
            return RedirectToAction("Index");
        }
    }
}