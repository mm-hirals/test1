using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto.Unit;

namespace MidCapERP.Admin.Controllers
{
    public class UnitController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public UnitController(IUnitOfWorkBL unitOfWorkBL, IStringLocalizer<BaseController> localizer) : base(localizer)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Unit.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Unit.View)]
        public async Task<IActionResult> GetUnitData([FromForm] UnitDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.UnitBL.GetFilterUnitData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Unit.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            return PartialView("_UnitPartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Unit.Create)]
        public async Task<IActionResult> Create(UnitRequestDto lookupsRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.UnitBL.CreateUnit(lookupsRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Unit.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            var lookups = await _unitOfWorkBL.UnitBL.GetById(Id, cancellationToken);
            return PartialView("_UnitPartial", lookups);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Unit.Update)]
        public async Task<IActionResult> Update(int Id, UnitRequestDto lookupsRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.UnitBL.UpdateUnit(Id, lookupsRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Unit.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.UnitBL.DeleteUnit(Id, cancellationToken);
            return RedirectToAction("Index");
        }

        // Unit Name Validation
        public async Task<bool> DuplicateUnitName(UnitRequestDto unitRequestDto, CancellationToken cancellationToken)
        {
            return await _unitOfWorkBL.UnitBL.ValidateUnitName(unitRequestDto, cancellationToken);
        }
    }
}