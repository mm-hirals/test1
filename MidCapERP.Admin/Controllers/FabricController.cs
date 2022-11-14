using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto;
using MidCapERP.Dto.Fabric;

namespace MidCapERP.Admin.Controllers
{
    public class FabricController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;
        private readonly CurrentUser _currentUser;

        public FabricController(IUnitOfWorkBL unitOfWorkBL, CurrentUser currentUser, IStringLocalizer<BaseController> localizer) : base(localizer)
        {
            _unitOfWorkBL = unitOfWorkBL;
            _currentUser = currentUser;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.PortalFabric.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.PortalFabric.View)]
        public async Task<IActionResult> GetFabricData([FromForm] FabricDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.FabricBL.GetFilterFabricData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.PortalFabric.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            await FillCompanyNameDropDown(cancellationToken);
            await FillUnitNameDropDown(cancellationToken);
            await FillViewBags(cancellationToken);
            return PartialView("_FabricPartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.PortalFabric.Create)]
        public async Task<IActionResult> Create(FabricRequestDto fabricRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.FabricBL.CreateFabric(fabricRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.PortalFabric.Create)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            await FillCompanyNameDropDown(cancellationToken);
            await FillUnitNameDropDown(cancellationToken);
            await FillViewBags(cancellationToken);
            var fabric = await _unitOfWorkBL.FabricBL.GetById(Id, cancellationToken);
            return PartialView("_FabricPartial", fabric);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.PortalFabric.Create)]
        public async Task<IActionResult> Update(int Id, FabricRequestDto fabricRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.FabricBL.UpdateFabric(Id, fabricRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.PortalFabric.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.FabricBL.DeleteFabric(Id, cancellationToken);
            return RedirectToAction("Index");
        }

        // Model No Validation
        public async Task<bool> DuplicateModelNo(FabricRequestDto fabricRequestDto, CancellationToken cancellationToken)
        {
            return await _unitOfWorkBL.FabricBL.ValidateModelNo(fabricRequestDto, cancellationToken);
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

        private async Task FillViewBags(CancellationToken cancellationToken)
        {
            var tenantDetails = await _unitOfWorkBL.TenantBL.GetById(_currentUser.TenantId, cancellationToken);
            ViewBag.WholesalerSP = tenantDetails != null && tenantDetails?.FabricWSPPercentage > 0 ? tenantDetails.FabricWSPPercentage : 0;
            ViewBag.RetailerSP = tenantDetails != null && tenantDetails?.FabricRSPPercentage > 0 ? tenantDetails.FabricRSPPercentage : 0;
            ViewBag.RoundTo = tenantDetails != null && tenantDetails?.AmountRoundMultiple > 0 ? tenantDetails.AmountRoundMultiple : 0;
        }

        #endregion Private Method
    }
}