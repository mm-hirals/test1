using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.Status;
using MidCapERP.Infrastructure.Constants;
using NToastNotify;

namespace MidCapERP.Admin.Controllers
{
    public class StatusController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;
        private readonly IToastNotification _toastNotification;

        public StatusController(IUnitOfWorkBL unitOfWorkBL, IToastNotification toastNotification)
        {
            _unitOfWorkBL = unitOfWorkBL;
            _toastNotification = toastNotification;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Status.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await GetAllStatus(cancellationToken));
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Status.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            return PartialView("_StatusPartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Status.Create)]
        public async Task<IActionResult> Create(StatusRequestDto statusRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.StatusBL.CreateStatus(statusRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Status Saved Succesfully!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Status.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            var status = await _unitOfWorkBL.StatusBL.GetById(Id, cancellationToken);
            return PartialView("_StatusPartial", status);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Status.Update)]
        public async Task<IActionResult> Update(int Id, StatusRequestDto statusRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.StatusBL.UpdateStatus(Id, statusRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Status Saved Succesfully!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Status.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.StatusBL.DeleteStatus(Id, cancellationToken);
            return RedirectToAction("Index");
        }

        #region PrivateMethods

        private async Task<IEnumerable<StatusResponseDto>> GetAllStatus(CancellationToken cancellationToken)
        {
            return await _unitOfWorkBL.StatusBL.GetAll(cancellationToken);
        }

        #endregion PrivateMethods
    }
}