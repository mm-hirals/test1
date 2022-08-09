using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Fabric;
using MidCapERP.Infrastructure.Constants;

namespace MidCapERP.Admin.Controllers
{
    public class FabricController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public FabricController(IUnitOfWorkBL unitOfWorkBL, IWebHostEnvironment hostingEnvironment)
        {
            _unitOfWorkBL = unitOfWorkBL;
            _hostingEnvironment = hostingEnvironment;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Fabric.View)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Fabric.View)]
        public async Task<IActionResult> GetFabricData([FromForm] DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.FabricBL.GetFilterFabricData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Fabric.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            await FillCompanyNameDropDown(cancellationToken);
            await FillUnitNameDropDown(cancellationToken);
            return PartialView("_FabricPartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Fabric.Create)]
        public async Task<IActionResult> Create(FabricRequestDto fabricRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.FabricBL.CreateFabric(fabricRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Fabric.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            await FillCompanyNameDropDown(cancellationToken);
            await FillUnitNameDropDown(cancellationToken);
            var fabric = await _unitOfWorkBL.FabricBL.GetById(Id, cancellationToken);
            return PartialView("_FabricPartial", fabric);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Fabric.Update)]
        public async Task<IActionResult> Update(int Id, FabricRequestDto fabricRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.FabricBL.UpdateFabric(Id, fabricRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Fabric.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.FabricBL.DeleteFabric(Id, cancellationToken);
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