using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto.Architect;
using MidCapERP.Dto.ArchitectAddresses;

using NToastNotify;

namespace MidCapERP.Admin.Controllers
{
    public class ArchitectController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public ArchitectController(IUnitOfWorkBL unitOfWorkBL, IStringLocalizer<BaseController> localizer) : base(localizer)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Architect.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Architect.View)]
        public async Task<IActionResult> GetArchitectsData([FromForm] ArchitectDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.ArchitectsBL.GetFilterArchitectsData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.ArchitectAddresses.View)]
        public async Task<IActionResult> GetArchitectAddressesData([FromForm] ArchitectAddressDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.ArchitectAddressesBL.GetFilterArchitectAddressesData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Architect.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            return PartialView("ArchitectEdit");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Architect.Create)]
        public async Task<IActionResult> Create(ArchitectRequestDto architectsRequestDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.ArchitectsBL.CreateArchitects(architectsRequestDto, cancellationToken);
            return RedirectToAction("Update", "Architect", new { id = data.CustomerId });
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.ArchitectAddresses.Create)]
        public async Task<IActionResult> CreateArchitectAddress(int customerId, CancellationToken cancellationToken)
        {
            ArchitectAddressesRequestDto dto = new();
            dto.CustomerId = customerId;
            return PartialView("_ArchitectAddressPartial", dto);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.ArchitectAddresses.Create)]
        public async Task<IActionResult> CreateArchitectAddress(ArchitectAddressesRequestDto architectsRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.ArchitectAddressesBL.CreateArchitectAddresses(architectsRequestDto, cancellationToken);
            return View("_ArchitectAddressPartial");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Architect.Update)]
        public async Task<IActionResult> Update(Int64 Id, CancellationToken cancellationToken)
        {
            var architects = await _unitOfWorkBL.ArchitectsBL.GetById(Id, cancellationToken);
            return View("ArchitectEdit", architects);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Architect.Update)]
        public async Task<IActionResult> Update(Int64 Id, ArchitectRequestDto architectsRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.ArchitectsBL.UpdateArchitects(Id, architectsRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.ArchitectAddresses.Update)]
        public async Task<IActionResult> UpdateArchitectAddresses(Int64 Id, CancellationToken cancellationToken)
        {
            var architectsAddress = await _unitOfWorkBL.ArchitectAddressesBL.GetById(Id, cancellationToken);
            return PartialView("_ArchitectAddressPartial", architectsAddress);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.ArchitectAddresses.Update)]
        public async Task<IActionResult> UpdateArchitectAddresses(Int64 Id, ArchitectAddressesRequestDto architectsAddressRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.ArchitectAddressesBL.UpdateArchitectAddresses(Id, architectsAddressRequestDto, cancellationToken);
            return RedirectToAction("ArchitectEdit");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.ArchitectAddresses.Delete)]
        public async Task<IActionResult> DeleteArchitectAddresses(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.ArchitectAddressesBL.DeleteArchitectAddresses(Id, cancellationToken);
            return RedirectToAction("ArchitectEdit");
        }

        [HttpPost]
        public async Task<JsonResult> MultipleSendArchitect(ArchitectsSendSMSDto model, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWorkBL.ArchitectsBL.SendSMSToArchitects(model, cancellationToken);
                return Json("success");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}