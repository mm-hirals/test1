using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Polish;

namespace MidCapERP.Admin.Controllers
{
    public class PolishController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public PolishController(IUnitOfWorkBL unitOfWorkBL, IStringLocalizer<BaseController> localizer) : base(localizer)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Polish.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Polish.View)]
        public async Task<IActionResult> GetPolishData([FromForm] DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.PolishBL.GetFilterPolishData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Polish.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            await FillCompanyNameDropDown(cancellationToken);
            await FillUnitNameDropDown(cancellationToken);
            return PartialView("_PolishPartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Polish.Create)]
        public async Task<IActionResult> Create(PolishRequestDto PolishRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.PolishBL.CreatePolish(PolishRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Polish.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            await FillCompanyNameDropDown(cancellationToken);
            await FillUnitNameDropDown(cancellationToken);
            var polish = await _unitOfWorkBL.PolishBL.GetById(Id, cancellationToken);
            return PartialView("_PolishPartial", polish);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Polish.Update)]
        public async Task<IActionResult> Update(int Id, PolishRequestDto PolishRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.PolishBL.UpdatePolish(Id, PolishRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Polish.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.PolishBL.DeletePolish(Id, cancellationToken);
            return RedirectToAction("Index");
        }

        #region Private Method

        private async Task FillCompanyNameDropDown(CancellationToken cancellationToken)
        {
            var companyData = await _unitOfWorkBL.CompanyBL.GetAll(cancellationToken);
            var companyDataSelectedList = companyData.Select(x => new SelectListItem
            {
                Value = Convert.ToString(x.LookupValueId),
                Text = x.LookupValueName
            }).ToList();
            ViewBag.CompanySelectItemList = companyDataSelectedList;
        }

        private async Task FillUnitNameDropDown(CancellationToken cancellationToken)
        {
            var unitData = await _unitOfWorkBL.UnitBL.GetAll(cancellationToken);
            var unitDataSelectedList = unitData.Select(x => new SelectListItem
            {
                Value = Convert.ToString(x.LookupValueId),
                Text = x.LookupValueName
            }).ToList();
            ViewBag.UnitSelectItemList = unitDataSelectedList;
        }

        #endregion Private Method
    }
}