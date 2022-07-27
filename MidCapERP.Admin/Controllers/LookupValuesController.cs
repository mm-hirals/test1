using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.LookupValues;
using MidCapERP.Infrastructure.Constants;
using NToastNotify;

namespace MidCapERP.Admin.Controllers
{
    public class LookupValuesController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;
        private readonly IToastNotification _toastNotification;
        public LookupValuesController(IUnitOfWorkBL unitOfWorkBL, IToastNotification toastNotification)
        {
            _unitOfWorkBL = unitOfWorkBL;
            _toastNotification = toastNotification;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.LookupValues.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            _toastNotification.AddWarningToastMessage("This about Lookup values");
            return View(await GetAllLookupValues(cancellationToken));
        }

        [HttpPost]
        public IActionResult GetLookupValuesData([FromForm] DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = _unitOfWorkBL.LookupValuesBL.GetFilterLookupValuesData(dataTableFilterDto, cancellationToken);
            return Ok(data.Result);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.LookupValues.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            return PartialView("_LookupValuesPartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.LookupValues.Create)]
        public async Task<IActionResult> Create(LookupValuesRequestDto lookupsRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.LookupValuesBL.CreateLookupValues(lookupsRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Saved Successfully!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.LookupValues.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            var lookups = await _unitOfWorkBL.LookupValuesBL.GetById(Id, cancellationToken);
            return PartialView("_LookupValuesPartial", lookups);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.LookupValues.Update)]
        public async Task<IActionResult> Update(int Id, LookupValuesRequestDto lookupsRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.LookupValuesBL.UpdateLookupValues(Id, lookupsRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Saved Successfully!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.LookupValues.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.LookupValuesBL.DeleteLookupValues(Id, cancellationToken);
            return RedirectToAction("Index");
        }

        #region PrivateMethods

        private async Task<IEnumerable<LookupValuesResponseDto>> GetAllLookupValues(CancellationToken cancellationToken)
        {
            return await _unitOfWorkBL.LookupValuesBL.GetAll(cancellationToken);
        }

        #endregion PrivateMethods
    }
}