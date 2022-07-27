using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.Customers;
using MidCapERP.Infrastructure.Constants;

namespace MidCapERP.Admin.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public CustomerController(IUnitOfWorkBL unitOfWorkBL)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Customer.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await _unitOfWorkBL.CustomersBL.GetAllCustomers(cancellationToken));
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            return PartialView("_CustomerPartial");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            var customers = await _unitOfWorkBL.CustomersBL.GetById(Id, cancellationToken);
            return PartialView("_CustomerPartial", customers);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            var customers = await _unitOfWorkBL.CustomersBL.DeleteCustomers(Id, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.Create)]
        public async Task<IActionResult> Create(CustomersRequestDto customersRequestDto, CancellationToken cancellationToken)
        {
            var customers = await _unitOfWorkBL.CustomersBL.CreateCustomers(customersRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.Update)]
        public async Task<IActionResult> Update(int Id, CustomersRequestDto customersRequestDto, CancellationToken cancellationToken)
        {
            var customers = await _unitOfWorkBL.CustomersBL.UpdateCustomers(Id, customersRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }
    }
}