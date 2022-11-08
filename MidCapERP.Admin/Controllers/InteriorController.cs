using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto.Interior;
using MidCapERP.Dto.InteriorAddresses;

namespace MidCapERP.Admin.Controllers
{
    public class InteriorController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public InteriorController(IUnitOfWorkBL unitOfWorkBL, IStringLocalizer<BaseController> localizer) : base(localizer)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Interior.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Interior.View)]
        public async Task<IActionResult> GetInteriorsData([FromForm] InteriorDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.InteriorsBL.GetFilterInteriorsData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.InteriorAddresses.View)]
        public async Task<IActionResult> GetInteriorAddressesData([FromForm] InteriorAddressDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.InteriorAddressesBL.GetFilterInteriorAddressesData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Interior.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            return PartialView("InteriorEdit");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Interior.Create)]
        public async Task<IActionResult> Create(InteriorRequestDto interiorsRequestDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.InteriorsBL.CreateInteriors(interiorsRequestDto, cancellationToken);
            return RedirectToAction("Update", "Interior", new { id = data.CustomerId });
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.InteriorAddresses.Create)]
        public async Task<IActionResult> CreateInteriorAddress(int customerId, CancellationToken cancellationToken)
        {
            InteriorAddressesRequestDto dto = new();
            dto.CustomerId = customerId;
            return PartialView("_InteriorAddressPartial", dto);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.InteriorAddresses.Create)]
        public async Task<IActionResult> CreateInteriorAddress(InteriorAddressesRequestDto interiorsRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.InteriorAddressesBL.CreateInteriorAddresses(interiorsRequestDto, cancellationToken);
            return View("_InteriorAddressPartial");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Interior.Update)]
        public async Task<IActionResult> Update(Int64 Id, CancellationToken cancellationToken)
        {
            var interiors = await _unitOfWorkBL.InteriorsBL.GetById(Id, cancellationToken);
            return View("InteriorEdit", interiors);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Interior.Update)]
        public async Task<IActionResult> Update(Int64 Id, InteriorRequestDto interiorsRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.InteriorsBL.UpdateInteriors(Id, interiorsRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.InteriorAddresses.Update)]
        public async Task<IActionResult> UpdateInteriorAddresses(Int64 Id, CancellationToken cancellationToken)
        {
            var interiorsAddress = await _unitOfWorkBL.InteriorAddressesBL.GetById(Id, cancellationToken);
            return PartialView("_InteriorAddressPartial", interiorsAddress);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.InteriorAddresses.Update)]
        public async Task<IActionResult> UpdateInteriorAddresses(Int64 Id, InteriorAddressesRequestDto interiorsAddressRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.InteriorAddressesBL.UpdateInteriorAddresses(Id, interiorsAddressRequestDto, cancellationToken);
            return RedirectToAction("InteriorEdit");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.InteriorAddresses.Delete)]
        public async Task<IActionResult> DeleteInteriorAddresses(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.InteriorAddressesBL.DeleteInteriorAddresses(Id, cancellationToken);
            return Json(true);
        }

        [HttpPost]
        public async Task<JsonResult> MultipleSendInterior(InteriorsSendSMSDto model, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWorkBL.InteriorsBL.SendSMSToInteriors(model, cancellationToken);
                return Json("success");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<bool> DuplicateInteriorPhoneNumber(InteriorRequestDto interiorRequestDto, CancellationToken cancellationToken)
        {
            return await _unitOfWorkBL.InteriorsBL.ValidateInteriorPhoneNumber(interiorRequestDto, cancellationToken);
        }
    }
}