using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.WoodType;
using MidCapERP.Infrastructure.Constants;
using NToastNotify;

namespace MidCapERP.Admin.Controllers
{
    public class WoodTypeController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;
        private readonly IToastNotification _toastNotification;

        public WoodTypeController(IUnitOfWorkBL unitOfWorkBL, IToastNotification toastNotification)
        {
            _unitOfWorkBL = unitOfWorkBL;
            _toastNotification = toastNotification;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.WoodType.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.WoodType.View)]
        public async Task<IActionResult> GetWoodTypeData([FromForm] DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var filterWoodTypedata = await _unitOfWorkBL.WoodTypeBL.GetFilterWoodTypeData(dataTableFilterDto, cancellationToken);
            return Ok(filterWoodTypedata);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.WoodType.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            return PartialView("_WoodTypePartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.WoodType.Create)]
        public async Task<IActionResult> Create(WoodTypeRequestDto woodTypeRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.WoodTypeBL.CreateWoodType(woodTypeRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Saved Successfully!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.WoodType.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            var woodType = await _unitOfWorkBL.WoodTypeBL.GetById(Id, cancellationToken);
            return PartialView("_WoodTypePartial", woodType);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.WoodType.Update)]
        public async Task<IActionResult> Update(int Id, WoodTypeRequestDto lookupsRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.WoodTypeBL.UpdateWoodType(Id, lookupsRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Updated Successfully!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.WoodType.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.WoodTypeBL.DeleteWoodType(Id, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Deleted Successfully!");
            return RedirectToAction("Index");
        }
    }
}