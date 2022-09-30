using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto.CustomerAddresses;
using MidCapERP.Dto.Customers;
using NToastNotify;

namespace MidCapERP.Admin.Controllers
{
    public class ArchitectController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;
        private readonly IToastNotification _toastNotification;

        public ArchitectController(IUnitOfWorkBL unitOfWorkBL, IToastNotification toastNotification, IStringLocalizer<BaseController> localizer) : base(localizer)
        {
            _unitOfWorkBL = unitOfWorkBL;
            _toastNotification = toastNotification;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Customer.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.View)]
        public async Task<IActionResult> GetArchitectsData([FromForm] CustomerDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.ArchitectsBL.GetFilterArchitectsData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.CustomerAddresses.View)]
        public async Task<IActionResult> GetArchitectAddressesData([FromForm] CustomerAddressDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.ArchitectAddressesBL.GetFilterArchitectAddressesData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            return PartialView("ArchitectEdit");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.Create)]
        public async Task<IActionResult> Create(CustomersRequestDto customersRequestDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.ArchitectsBL.CreateArchitects(customersRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Saved Successfully!");
            return RedirectToAction("Update", "Architect", new { id = data.CustomerId });
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.Update)]
        public async Task<IActionResult> Update(Int64 Id, CancellationToken cancellationToken)
        {
            var architects = await _unitOfWorkBL.ArchitectsBL.GetById(Id, cancellationToken);
            return View("ArchitectEdit", architects);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.Update)]
        public async Task<IActionResult> Update(Int64 Id, CustomersRequestDto customersRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.ArchitectsBL.UpdateArchitects(Id, customersRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Update Successfully!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.CustomerAddresses.Create)]
        public async Task<IActionResult> CreateArchitectAddress(int customerId, CancellationToken cancellationToken)
        {
            CustomerAddressesRequestDto dto = new();
            dto.CustomerId = customerId;
            return PartialView("_ArchitectAddressPartial", dto);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.CustomerAddresses.Create)]
        public async Task<IActionResult> CreateArchitectAddress(CustomerAddressesRequestDto customersRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.ArchitectAddressesBL.CreateArchitectAddresses(customersRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Saved Successfully!");
            return View("_ArchitectAddressPartial");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.CustomerAddresses.Update)]
        public async Task<IActionResult> UpdateArchitectAddresses(Int64 Id, CancellationToken cancellationToken)
        {
            var architectsAddress = await _unitOfWorkBL.ArchitectAddressesBL.GetById(Id, cancellationToken);
            return PartialView("_ArchitectAddressPartial", architectsAddress);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.CustomerAddresses.Update)]
        public async Task<IActionResult> UpdateArchitectAddresses(Int64 Id, CustomerAddressesRequestDto customersAddressRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.ArchitectAddressesBL.UpdateArchitectAddresses(Id, customersAddressRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Update Successfully!");
            return RedirectToAction("ArchitectEdit");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.CustomerAddresses.Delete)]
        public async Task<IActionResult> DeleteArchitectAddresses(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.ArchitectAddressesBL.DeleteArchitectAddresses(Id, cancellationToken);
            return RedirectToAction("ArchitectEdit");
        }

        [HttpPost]
        public async Task<JsonResult> MultipleSendArchitect(long?[] value_check)
        {
            try
            {
                return Json("success");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}