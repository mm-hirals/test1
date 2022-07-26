using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.LookupValues;
using MidCapERP.Infrastructure.Constants;

namespace MidCapERP.Admin.Controllers
{
    public class LookupValuesController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public LookupValuesController(IUnitOfWorkBL unitOfWorkBL)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.LookupValues.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await GetAllLookupValues(cancellationToken));
        }

        [HttpPost]
        public IActionResult GetLookupValuesData([FromForm] DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            try
            {
                var data = _unitOfWorkBL.LookupValuesBL.GetFilterLookupValuesData(dataTableFilterDto, cancellationToken);
                return Ok(data.Result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.LookupValues.Create)]
        public async Task<IActionResult> Create(int Id, CancellationToken cancellationToken)
        {
            return PartialView("_LookupValuesPartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.LookupValues.Create)]
        public async Task<IActionResult> Create(int Id, LookupValuesRequestDto lookupsRequestDto, CancellationToken cancellationToken)
        {
            var lookups = await _unitOfWorkBL.LookupValuesBL.CreateLookupValues(lookupsRequestDto, cancellationToken);
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
            var lookups = await _unitOfWorkBL.LookupValuesBL.UpdateLookupValues(Id, lookupsRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.LookupValues.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            var lookup = await _unitOfWorkBL.LookupValuesBL.DeleteLookupValues(Id, cancellationToken);
            return RedirectToAction("Index");
        }

        #region privateMethods

        private async Task<IEnumerable<LookupValuesResponseDto>> GetAllLookupValues(CancellationToken cancellationToken)
        {
            return await _unitOfWorkBL.LookupValuesBL.GetAll(cancellationToken);
        }

        #endregion privateMethods
    }
}