using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.Lookups;
using MidCapERP.Infrastructure.Constants;
using NToastNotify;

namespace MidCapERP.Admin.Controllers
{
    public class LookupController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;
        private readonly IToastNotification _toastNotification;

        public LookupController(IUnitOfWorkBL unitOfWorkBL, IToastNotification toastNotification)
        {
            _unitOfWorkBL = unitOfWorkBL;
            _toastNotification = toastNotification;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Lookup.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await GetAllLookup(cancellationToken));
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Lookup.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            return PartialView("_LookupPartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Lookup.Create)]
        public async Task<IActionResult> Create(LookupsRequestDto lookupsRequestDto, CancellationToken cancellationToken)
        {
            var lookups = await _unitOfWorkBL.LookupsBL.CreateLookup(lookupsRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Saved Successfully!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Lookup.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            var lookups = await _unitOfWorkBL.LookupsBL.GetById(Id, cancellationToken);
            return PartialView("_LookupPartial", lookups);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Lookup.Update)]
        public async Task<IActionResult> Update(int Id, LookupsRequestDto lookupsRequestDto, CancellationToken cancellationToken)
        {
            Id = lookupsRequestDto.LookupId;
            var lookups = await _unitOfWorkBL.LookupsBL.UpdateLookup(Id, lookupsRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Saved Successfully!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Lookup.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            var lookup = await _unitOfWorkBL.LookupsBL.DeleteLookup(Id, cancellationToken);
            return RedirectToAction("Index");
        }

        #region privateMethods
        private async Task<IEnumerable<LookupsResponseDto>> GetAllLookup(CancellationToken cancellationToken)
        {
            return await _unitOfWorkBL.LookupsBL.GetAll(cancellationToken);
        }
        #endregion
    }
}
