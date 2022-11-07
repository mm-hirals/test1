﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto.RawMaterial;

namespace MidCapERP.Admin.Controllers
{
    public class RawMaterialController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public RawMaterialController(IUnitOfWorkBL unitOfWorkBL, IStringLocalizer<BaseController> localizer) : base(localizer)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.RawMaterial.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.RawMaterial.View)]
        public async Task<IActionResult> GetRawMaterialData([FromForm] RawMaterialDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.RawMaterialBL.GetFilterRawMaterialData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.RawMaterial.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            await FillUnitNameDropDown(cancellationToken);
            return PartialView("_RawMaterialPartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.RawMaterial.Create)]
        public async Task<IActionResult> Create(RawMaterialRequestDto rawMaterialRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.RawMaterialBL.CreateRawMaterial(rawMaterialRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.RawMaterial.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            await FillUnitNameDropDown(cancellationToken);
            var lookups = await _unitOfWorkBL.RawMaterialBL.GetById(Id, cancellationToken);
            return PartialView("_RawMaterialPartial", lookups);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.RawMaterial.Update)]
        public async Task<IActionResult> Update(int Id, RawMaterialRequestDto rawMaterialRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.RawMaterialBL.UpdateRawMaterial(Id, rawMaterialRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.RawMaterial.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.RawMaterialBL.DeleteRawMaterial(Id, cancellationToken);
            return RedirectToAction("Index");
        }

        // Title Validation
        public async Task<bool> DuplicateRawMaterialTitle(RawMaterialRequestDto rawMaterialRequestDto, CancellationToken cancellationToken)
        {
            return await _unitOfWorkBL.RawMaterialBL.ValidateRawMaterialTitle(rawMaterialRequestDto, cancellationToken);
        }

        #region Private Method

        private async Task FillUnitNameDropDown(CancellationToken cancellationToken)
        {
            var unitData = await _unitOfWorkBL.UnitBL.GetAll(cancellationToken);
            var data = unitData.Select(a => new SelectListItem
            {
                Value = Convert.ToString(a.LookupValueId),
                Text = a.LookupValueName
            }).ToList();
            ViewBag.UnitSelectItemList = data;
        }

        #endregion Private Method
    }
}