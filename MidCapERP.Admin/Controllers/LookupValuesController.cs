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
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.LookupValues.View)]
        public async Task<IActionResult> GetLookupValuesDataAsync([FromForm] DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.LookupValuesBL.GetFilterLookupValuesData(dataTableFilterDto, cancellationToken);
            return Ok(data);
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
    }
}