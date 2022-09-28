using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Frames;
using NToastNotify;

namespace MidCapERP.Admin.Controllers
{
    public class FrameController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;
        private readonly IToastNotification _toastNotification;

        public FrameController(IUnitOfWorkBL unitOfWorkBL, IToastNotification toastNotification, IStringLocalizer<BaseController> localizer) : base(localizer)
        {
            _unitOfWorkBL = unitOfWorkBL;
            _toastNotification = toastNotification;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Frame.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Frame.View)]
        public async Task<IActionResult> GetFrameData([FromForm] DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var filterFramedata = await _unitOfWorkBL.FrameBL.GetFilterFrameData(dataTableFilterDto, cancellationToken);
            return Ok(filterFramedata);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Frame.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            await FillFrameTypeDropDown(cancellationToken);
            await FillCompanyDropDown(cancellationToken);
            await FillUnitDropDown(cancellationToken);
            return PartialView("_FramePartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Frame.Create)]
        public async Task<IActionResult> Create(FrameRequestDto frameRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.FrameBL.CreateFrame(frameRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Created Successfully!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Frame.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            await FillFrameTypeDropDown(cancellationToken);
            await FillCompanyDropDown(cancellationToken);
            await FillUnitDropDown(cancellationToken);
            var frame = await _unitOfWorkBL.FrameBL.GetById(Id, cancellationToken);
            return PartialView("_FramePartial", frame);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Frame.Update)]
        public async Task<IActionResult> Update(int Id, FrameRequestDto frameRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.FrameBL.UpdateFrame(Id, frameRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Update Successfully!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Frame.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.FrameBL.DeleteFrame(Id, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Deleted Successfully!");
            return RedirectToAction("Index");
        }

        #region PrivateMethod

        private async Task FillFrameTypeDropDown(CancellationToken cancellationToken)
        {
            var frameTypeData = await _unitOfWorkBL.FrameTypeBL.GetAll(cancellationToken);
            var frameSelectedList = frameTypeData.Select(a =>
                                 new SelectListItem
                                 {
                                     Value = Convert.ToString(a.LookupValueId),
                                     Text = a.LookupValueName
                                 }).ToList();
            ViewBag.FrameSelectItemList = frameSelectedList;
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