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

        [HttpGet("{phoneNumberOrEmailId}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.View)]
        public async Task<ApiResponse> Get(string phoneNumberOrEmailId, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.CustomersBL.GetCustomerByMobileNumberOrEmailId(phoneNumberOrEmailId, cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "No Data found", result: data, statusCode: 404);
            }
            return new ApiResponse(message: "Data found", result: data, statusCode: 200);
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

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.Create)]
        public async Task<ApiResponse> Post([FromBody] CustomersRequestDto customersRequestDto, CancellationToken cancellationToken)
        {
            ValidationRequest(customersRequestDto);
            var data = await _unitOfWorkBL.CustomersBL.CreateCustomers(customersRequestDto, cancellationToken);
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
        public async Task<ApiResponse> CustomerAddressGet(long customerId, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.CustomerAddressesBL.GetCustomerById(customerId, cancellationToken);
            if (data == null || data.Count() == 0)
            {
                return new ApiResponse(message: "Customer Address not found!", result: data, statusCode: 404);
            }
            return new ApiResponse(message: "Customer Address Found", result: data, statusCode: 200);
        }

        [HttpPost("CustomerAddress/{id}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.Create)]
        public async Task<ApiResponse> CreateOrEditCustomerAddress(int id, [FromBody] CustomerAddressesRequestDto customerAddressesRequestDto, CancellationToken cancellationToken)
        {
            ValidationRequest(customerAddressesRequestDto);
            if (id == 0 || id == null)
            {
                var data = await _unitOfWorkBL.CustomerAddressesBL.CreateCustomerAddresses(customerAddressesRequestDto, cancellationToken);
                if (data == null)
                {
                    return new ApiResponse(message: "Internal server error", result: data, statusCode: 500);
                }
                return new ApiResponse(message: "Data inserted successful", result: data, statusCode: 200);
            }
            else
            {
                var data = await _unitOfWorkBL.CustomerAddressesBL.UpdateCustomerAddresses(id, customerAddressesRequestDto, cancellationToken);
                if (data == null)
                {
                    return new ApiResponse(message: "Internal server error", result: data, statusCode: 500);
                }
                return new ApiResponse(message: "Data Update successful", result: data, statusCode: 200);
            }
        }

        #region Private Methods

        private void ValidationRequest(CustomersRequestDto customersRequestDto)
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

        #endregion Private Methods
    }
}