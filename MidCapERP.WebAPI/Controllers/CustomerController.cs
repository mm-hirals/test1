using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto.CustomerAddresses;
using MidCapERP.Dto.Customers;

namespace MidCapERP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public CustomerController(IUnitOfWorkBL unitOfWorkBL)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [HttpGet("{id}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.View)]
        public async Task<ApiResponse> Get(Int64 id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.CustomersBL.GetById(id, cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "No Data found", result: data, statusCode: 404);
            }
            return new ApiResponse(message: "Data found", result: data, statusCode: 200);
        }

        [HttpPut("{id}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.Update)]
        public async Task<ApiResponse> Put(int id, [FromBody] CustomerApiRequestDto customerApiRequestDto, CancellationToken cancellationToken)
        {
            ValidationRequest(customerApiRequestDto);
            var data = await _unitOfWorkBL.CustomersBL.UpdateCustomerApi(id, customerApiRequestDto, cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "Internal server error", result: data, statusCode: 500);
            }
            return new ApiResponse(message: "Data updated successful", result: data, statusCode: 200);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.Create)]
        public async Task<ApiResponse> Post([FromBody] CustomerApiRequestDto customerApiRequestDto, CancellationToken cancellationToken)
        {
            ValidationRequest(customerApiRequestDto);
            var data = await _unitOfWorkBL.CustomersBL.CreateCustomerApi(customerApiRequestDto, cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "Internal server error", result: data, statusCode: 500);
            }
            return new ApiResponse(message: "Data inserted successful", result: data, statusCode: 200);
        }

        [HttpGet("CheckCustomer")]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.View)]
        public async Task<ApiResponse> CheckCustomers(string phoneNumberOrEmail, CancellationToken cancellationToken)
        {
            bool data = await _unitOfWorkBL.CustomersBL.CheckCustomerExistOrNot(phoneNumberOrEmail, cancellationToken);
            if (data == null || data == false)
            {
                return new ApiResponse(message: "Customer not found!", result: data, statusCode: 404);
            }
            return new ApiResponse(message: "Customer Found", result: data, statusCode: 200);
        }

        [HttpGet("CustomerAddress/{customerId}")]
        [Authorize(ApplicationIdentityConstants.Permissions.CustomerAddresses.View)]
        public async Task<ApiResponse> GetCustomerAddress(long customerId, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.CustomerAddressesBL.GetCustomerById(customerId, cancellationToken);
            if (data == null || data.Count() == 0)
            {
                return new ApiResponse(message: "Customer Address not found!", result: data, statusCode: 404);
            }
            return new ApiResponse(message: "Customer Address Found", result: data, statusCode: 200);
        }

        [HttpGet("GetCustomerAddress/{customerAddressId}")]
        [Authorize(ApplicationIdentityConstants.Permissions.CustomerAddresses.View)]
        public async Task<ApiResponse> GetCustomerAddressById(long customerAddressId, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.CustomerAddressesBL.GetCustomerAddressById(customerAddressId, cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "Customer Address not found!", result: data, statusCode: 404);
            }
            return new ApiResponse(message: "Customer Address Found", result: data, statusCode: 200);
        }

        [HttpPut("CustomerAddress/{customerAddressId}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.Update)]
        public async Task<ApiResponse> UpdateCustomerAddress(int customerAddressId, [FromBody] CustomerAddressesApiRequestDto customerAddressesRequestDto, CancellationToken cancellationToken)
        {
            ValidationRequest(customerAddressesRequestDto);
            var data = await _unitOfWorkBL.CustomerAddressesBL.UpdateCustomerApiAddresses(customerAddressId, customerAddressesRequestDto, cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "Internal server error", result: data, statusCode: 500);
            }
            return new ApiResponse(message: "Data Update successful", result: data, statusCode: 200);
        }

        [HttpPost("CustomerAddress")]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.Create)]
        public async Task<ApiResponse> CreateCustomerAddress([FromBody] CustomerAddressesApiRequestDto customerAddressesRequestDto, CancellationToken cancellationToken)
        {
            ValidationRequest(customerAddressesRequestDto);
            var data = await _unitOfWorkBL.CustomerAddressesBL.CreateCustomerApiAddresses(customerAddressesRequestDto, cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "Internal server error", result: data, statusCode: 500);
            }
            return new ApiResponse(message: "Data inserted successful", result: data, statusCode: 200);
        }

        [HttpDelete("CustomerAddress/{customerAddressId}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.Update)]
        public async Task<ApiResponse> DeleteCustomerAddress(int customerAddressId, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.CustomerAddressesBL.DeleteCustomerAddresses(customerAddressId, cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "Internal server error", result: data, statusCode: 500);
            }
            return new ApiResponse(message: "Data Update successful", result: data, statusCode: 200);
        }

        [HttpGet("Search/{CustomerNameOrEmailOrMobileNo}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.View)]
        public async Task<ApiResponse> SearchCustomer(string CustomerNameOrEmailOrMobileNo, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.CustomersBL.SearchCustomer(CustomerNameOrEmailOrMobileNo, cancellationToken);
            if (data == null || data.Count() == 0)
            {
                return new ApiResponse(message: "Customer not found!", result: data, statusCode: 404);
            }
            return new ApiResponse(message: "Customer Found", result: data, statusCode: 200);
        }

        [HttpGet("SearchForCustomerDropdown/{searchText}")]
        [Authorize((ApplicationIdentityConstants.Permissions.Customer.View))]
        public async Task<ApiResponse> SearchDropDownRefferedBy(string searchText, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.CustomersBL.GetSearchCustomerForDropDownNameOrPhoneNumber(searchText, cancellationToken);
            if (data == null || data.Count() == 0)
            {
                return new ApiResponse(message: "No Data found", result: data, statusCode: 404);
            }
            return new ApiResponse(message: "Data found", result: data, statusCode: 200);
        }

        #region Private Methods

        private void ValidationRequest(CustomerApiRequestDto customersRequestDto)
        {
            if (!ModelState.IsValid)
            {
                throw new ApiException(ModelState.AllErrors());
            }
        }

        private void ValidationRequest(CustomerAddressesRequestDto customersRequestDto)
        {
            if (!ModelState.IsValid)
            {
                throw new ApiException(ModelState.AllErrors());
            }
        }

        private void ValidationRequest(CustomerAddressesApiRequestDto customersRequestDto)
        {
            if (!ModelState.IsValid)
            {
                throw new ApiException(ModelState.AllErrors());
            }
        }

        #endregion Private Methods
    }
}