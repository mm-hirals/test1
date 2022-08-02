using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.RawMaterial;
using MidCapERP.Infrastructure.Constants;
using NToastNotify;

namespace MidCapERP.Admin.Controllers
{
    public class RawMaterialController : Controller
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;
        private readonly IToastNotification _toastNotification;

        public RawMaterialController(IUnitOfWorkBL unitOfWorkBL, IToastNotification toastNotification)
        {
            _unitOfWorkBL = unitOfWorkBL;
            _toastNotification = toastNotification;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.RawMaterial.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.RawMaterial.View)]
        public async Task<IActionResult> GetRawMaterialData([FromForm] DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.RawMaterialBL.GetFilterRawMaterialData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.RawMaterial.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var unitData = await _unitOfWorkBL.UnitBL.GetAll(cancellationToken);
            var data = unitData.Select(a => new SelectListItem
            {
                Value = Convert.ToString(a.LookupValueId),
                Text = a.LookupValueName
            }).ToList();
            ViewBag.UnitSelectItemList = data;

            return PartialView("_RawMaterialPartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.RawMaterial.Create)]
        public async Task<IActionResult> Create(RawMaterialRequestDto RawMaterialRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.RawMaterialBL.CreateRawMaterial(RawMaterialRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Saved Successfully!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.RawMaterial.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            var unitData = await _unitOfWorkBL.UnitBL.GetAll(cancellationToken);
            var data = unitData.Select(a => new SelectListItem
            {
                Value = Convert.ToString(a.LookupValueId),
                Text = a.LookupValueName
            }).ToList();
            ViewBag.UnitSelectItemList = data;

            var lookups = await _unitOfWorkBL.RawMaterialBL.GetById(Id, cancellationToken);
            return PartialView("_RawMaterialPartial", lookups);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.RawMaterial.Update)]
        public async Task<IActionResult> Update(int Id, RawMaterialRequestDto RawMaterialRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.RawMaterialBL.UpdateRawMaterial(Id, RawMaterialRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Saved Successfully!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.RawMaterial.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.RawMaterialBL.DeleteRawMaterial(Id, cancellationToken);
            return RedirectToAction("Index");
        }
    }
}