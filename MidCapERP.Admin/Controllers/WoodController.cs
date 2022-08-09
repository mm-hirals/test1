using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.Constants;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Wood;
using MidCapERP.Infrastructure.Constants;
using NToastNotify;

namespace MidCapERP.Admin.Controllers
{
    public class WoodController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;
        private readonly IToastNotification _toastNotification;

        public WoodController(IUnitOfWorkBL unitOfWorkBL, IToastNotification toastNotification)
        {
            _unitOfWorkBL = unitOfWorkBL;
            _toastNotification = toastNotification;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Wood.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Wood.View)]
        public async Task<IActionResult> GetWoodData([FromForm] DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var filterWooddata = await _unitOfWorkBL.WoodBL.GetFilterWoodData(dataTableFilterDto, cancellationToken);
            return Ok(filterWooddata);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Wood.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            await FillWoodTypeDropDown(cancellationToken);
            await FillCompanyDropDown(cancellationToken);
            await FillUnitDropDown(cancellationToken);
            return PartialView("_WoodPartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Wood.Create)]
        public async Task<IActionResult> Create(WoodRequestDto woodRequestDto, CancellationToken cancellationToken)
        {
            var wood = await _unitOfWorkBL.WoodBL.CreateWood(woodRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Created Successfully!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Wood.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            await FillWoodTypeDropDown(cancellationToken);
            await FillCompanyDropDown(cancellationToken);
            await FillUnitDropDown(cancellationToken);
            var wood = await _unitOfWorkBL.WoodBL.GetById(Id, cancellationToken);
            return PartialView("_WoodPartial", wood);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Wood.Update)]
        public async Task<IActionResult> Update(int Id, WoodRequestDto woodRequestDto, CancellationToken cancellationToken)
        {
            var wood = await _unitOfWorkBL.WoodBL.UpdateWood(Id, woodRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Update Successfully!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Wood.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.WoodBL.DeleteWood(Id, cancellationToken);
            return RedirectToAction("Index");
        }

        #region PrivateMethod

        private async Task FillWoodTypeDropDown(CancellationToken cancellationToken)
        {
            var woodTypeData = await _unitOfWorkBL.WoodTypeBL.GetAll(cancellationToken);
            var woodSelectedList = woodTypeData.Select(a =>
                                 new SelectListItem
                                 {
                                     Value = Convert.ToString(a.LookupValueId),
                                     Text = a.LookupValueName
                                 }).ToList();
            ViewBag.WoodSelectItemList = woodSelectedList;
        }

        private async Task FillCompanyDropDown(CancellationToken cancellationToken)
        {
            var companyData = await _unitOfWorkBL.CompanyBL.GetAll(cancellationToken);
            var companySelectedList = companyData.Select(a =>
                                 new SelectListItem
                                 {
                                     Value = Convert.ToString(a.LookupValueId),
                                     Text = a.LookupValueName
                                 }).ToList();
            ViewBag.CompanySelectItemList = companySelectedList;
        }

        private async Task FillUnitDropDown(CancellationToken cancellationToken)
        {
            var unitData = await _unitOfWorkBL.UnitBL.GetAll(cancellationToken);
            var unitSelectedList = unitData.Select(a =>
                                new SelectListItem
                                {
                                    Value = Convert.ToString(a.LookupValueId),
                                    Text = a.LookupValueName
                                }).ToList();
            ViewBag.UnitDataSelectedItemList = unitSelectedList;
        }

        #endregion PrivateMethod
    }
}