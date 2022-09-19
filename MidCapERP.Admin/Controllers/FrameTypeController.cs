using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.FrameType;
using MidCapERP.Infrastructure.Constants;
using NToastNotify;

namespace MidCapERP.Admin.Controllers
{
    public class FrameTypeController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;
        private readonly IToastNotification _toastNotification;

        public FrameTypeController(IUnitOfWorkBL unitOfWorkBL, IToastNotification toastNotification, IStringLocalizer<BaseController> localizer) : base(localizer)
        {
            _unitOfWorkBL = unitOfWorkBL;
            _toastNotification = toastNotification;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.FrameType.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.FrameType.View)]
        public async Task<IActionResult> GetFrameTypeData([FromForm] DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var filterFrameTypedata = await _unitOfWorkBL.FrameTypeBL.GetFilterFrameTypeData(dataTableFilterDto, cancellationToken);
            return Ok(filterFrameTypedata);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.FrameType.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            return PartialView("_FrameTypePartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.FrameType.Create)]
        public async Task<IActionResult> Create(FrameTypeRequestDto frameTypeRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.FrameTypeBL.CreateFrameType(frameTypeRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Saved Successfully!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.FrameType.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            var frameType = await _unitOfWorkBL.FrameTypeBL.GetById(Id, cancellationToken);
            return PartialView("_FrameTypePartial", frameType);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.FrameType.Update)]
        public async Task<IActionResult> Update(int Id, FrameTypeRequestDto lookupsRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.FrameTypeBL.UpdateFrameType(Id, lookupsRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Updated Successfully!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.FrameType.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.FrameTypeBL.DeleteFrameType(Id, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Deleted Successfully!");
            return RedirectToAction("Index");
        }
    }
}