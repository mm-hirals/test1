﻿using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.Customers;
using MidCapERP.Infrastructure.Constants;

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

        [HttpGet("/Customer/{phoneNumberOrEmailId}")]
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

        [HttpPost("/Customer")]
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

        [HttpGet("/Customer/CheckCustomer")]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.View)]
        public async Task<ApiResponse> CheckCustomers(string phoneNumberOrEmail, CancellationToken cancellationToken)
        {
            bool data = await _unitOfWorkBL.CustomersBL.CheckCustomerExistOrNot(phoneNumberOrEmail, cancellationToken);
            if (data == null || data == false)
            {
                return new ApiResponse(message: "False", result: data, statusCode: 404);
            }
            return new ApiResponse(message: "True", result: data, statusCode: 200);
        }

        #region Private Methods

        private void ValidationRequest(CustomersRequestDto customersRequestDto)
        {
            if (!ModelState.IsValid)
            {
                throw new ApiException(ModelState.AllErrors());
            }
        }

        #endregion Private Methods
    }
}