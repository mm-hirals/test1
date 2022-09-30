using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
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
            var data = await _unitOfWorkBL.CustomersBL.GetFilterArchitectsData(dataTableFilterDto, cancellationToken);
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
            var data = await _unitOfWorkBL.CustomersBL.CreateCustomers(customersRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Saved Successfully!");
            return RedirectToAction("Update", "Architect", new { id = data.CustomerId });
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.Update)]
        public async Task<IActionResult> Update(Int64 Id, CancellationToken cancellationToken)
        {
            var architects = await _unitOfWorkBL.CustomersBL.GetById(Id, cancellationToken);
            return View("ArchitectEdit", architects);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.Update)]
        public async Task<IActionResult> Update(Int64 Id, CustomersRequestDto customersRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.CustomersBL.UpdateCustomers(Id, customersRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Update Successfully!");
            return RedirectToAction("Index");
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