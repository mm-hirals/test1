using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.CustomerAddresses;
using MidCapERP.Dto.Customers;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Infrastructure.Constants;
using NToastNotify;

namespace MidCapERP.Admin.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;
        private readonly IToastNotification _toastNotification;

        public CustomerController(IUnitOfWorkBL unitOfWorkBL, IToastNotification toastNotification)
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
        public async Task<IActionResult> GetCustomersData([FromForm] DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.CustomersBL.GetFilterCustomersData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.CustomerAddresses.View)]
        public async Task<IActionResult> GetCustomerAddressesData([FromForm] DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.CustomerAddressesBL.GetFilterCustomerAddressesData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            return PartialView("CustomerEdit");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.Create)]
        public async Task<IActionResult> Create(CustomersRequestDto customersRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.CustomersBL.CreateCustomers(customersRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Saved Successfully!");
            return View("CustomerEdit");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.CustomerAddresses.Create)]
        public async Task<IActionResult> CreateCustomerAddress(CancellationToken cancellationToken)
        {
            return PartialView("_CustomerAddressPartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.CustomerAddresses.Create)]
        public async Task<IActionResult> CreateCustomerAddress(CustomerAddressesRequestDto customersRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.CustomerAddressesBL.CreateCustomerAddresses(customersRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Saved Successfully!");
            return View("CustomerEdit");
        }

        //Customer Update
        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            var customers = await _unitOfWorkBL.CustomersBL.GetById(Id, cancellationToken);
            return View("CustomerEdit", customers);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.Update)]
        public async Task<IActionResult> Update(int Id, CustomersRequestDto customersRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.CustomersBL.UpdateCustomers(Id, customersRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Saved Successfully!");
            return RedirectToAction("Index");
        }

        //CustomerAddress Update
        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.CustomerAddresses.Update)]
        public async Task<IActionResult> UpdateCustomerAddresses(int Id, CancellationToken cancellationToken)
        {
            var customersAddress = await _unitOfWorkBL.CustomerAddressesBL.GetById(Id, cancellationToken);
            return View("_CustomerAddressPartial", customersAddress);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.CustomerAddresses.Update)]
        public async Task<IActionResult> UpdateCustomerAddresses(int Id, CustomerAddressesRequestDto customersAddressRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.CustomerAddressesBL.UpdateCustomerAddresses(Id, customersAddressRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Update Successfully!");
            return RedirectToAction("CustomerEdit");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.CustomersBL.DeleteCustomers(Id, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.CustomerAddresses.Delete)]
        public async Task<IActionResult> DeleteCustomerAddresses(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.CustomerAddressesBL.DeleteCustomerAddresses(Id, cancellationToken);
            return RedirectToAction("CustomerEdit");
        }
    }
}