using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.Statuses;
using MidCapERP.Infrastructure.Constants;

namespace MidCapERP.Admin.Controllers
{
    public class StatusesController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public StatusesController(IUnitOfWorkBL unitOfWorkBL)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Status.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await GetAllStatuses(cancellationToken));
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Status.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            return PartialView("_StatusesPartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Status.Create)]
        public async Task<IActionResult> Create(StatusesRequestDto statusesRequestDto, CancellationToken cancellationToken)
        {
            var statuses = await _unitOfWorkBL.StatusesBL.CreateStatuses(statusesRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Status.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            var statuses = await _unitOfWorkBL.StatusesBL.GetById(Id, cancellationToken);
            return PartialView("_StatusesPartial", statuses);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Status.Update)]
        public async Task<IActionResult> Update(StatusesRequestDto statusesRequestDto, CancellationToken cancellationToken)
        {
            int Id = statusesRequestDto.StatusId;
            var statuses = await _unitOfWorkBL.StatusesBL.UpdateStatuses(Id, statusesRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Status.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            var statuses = await _unitOfWorkBL.StatusesBL.DeleteStatuses(Id, cancellationToken);
            return RedirectToAction("Index", "Statuses");
        }

        #region privateMethods
        private async Task<IEnumerable<StatusesResponseDto>> GetAllStatuses(CancellationToken cancellationToken)
        {
            return await _unitOfWorkBL.StatusesBL.GetAll(cancellationToken);
        }
        #endregion

    }
}
