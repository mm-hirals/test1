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

        [Authorize(ApplicationIdentityConstants.Permissions.Statuses.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await GetAllStatuses(cancellationToken));
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Statuses.Create)]
        public async Task<IActionResult> Create(int Id, CancellationToken cancellationToken)
        {
            return PartialView("_StatusesPartial");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Statuses.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            var Statuses = await _unitOfWorkBL.StatusesBL.GetById(Id, cancellationToken);
            return PartialView("_StatusesPartial", Statuses);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Statuses.Create)]
        public async Task<IActionResult> Create(int Id, StatusesRequestDto StatusesRequestDto, CancellationToken cancellationToken)
        {
            var Statuses = await _unitOfWorkBL.StatusesBL.CreateStatuses(StatusesRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Statuses.Update)]
        public async Task<IActionResult> Update(int Id, StatusesRequestDto StatusesRequestDto, CancellationToken cancellationToken)
        {
            Id = StatusesRequestDto.StatusId;
            var Statuses = await _unitOfWorkBL.StatusesBL.UpdateStatuses(Id, StatusesRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Statuses.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            var Statuses = await _unitOfWorkBL.StatusesBL.DeleteStatuses(Id, cancellationToken);
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
