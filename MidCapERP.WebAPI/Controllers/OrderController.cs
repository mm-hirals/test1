﻿using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.Order;
using MidCapERP.Infrastructure.Constants;

namespace MidCapERP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public OrderController(IUnitOfWorkBL unitOfWorkBL)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [HttpGet("{id}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Order.View)]
        public async Task<ApiResponse> Get(int id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.OrderBL.GetOrderAll(id, cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "No Data found", result: data, statusCode: 404);
            }
            return new ApiResponse(message: "Data found", result: data, statusCode: 200);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Order.Create)]
        public async Task<ApiResponse> Post([FromBody] OrderApiRequestDto orderRequestApiDto, CancellationToken cancellationToken)
        {
            ValidationRequest(orderRequestApiDto);
            var data = await _unitOfWorkBL.OrderBL.CreateOrder(orderRequestApiDto, cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "Internal server error", result: data, statusCode: 500);
            }
            return new ApiResponse(message: "Data inserted successful", result: data, statusCode: 200);
        }

        [HttpPut("{id}/{orderSetId}/{orderSetItemId}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Order.Update)]
        public async Task<ApiResponse> Put(int id, [FromBody] OrderApiRequestDto orderRequestApiDto, CancellationToken cancellationToken)
        {
            ValidationRequest(orderRequestApiDto);
            var data = await _unitOfWorkBL.OrderBL.UpdateOrderApi(id, orderRequestApiDto, cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "Internal server error", result: data, statusCode: 500);
            }
            return new ApiResponse(message: "Data updated successful", result: data, statusCode: 200);
        }

        #region Private Methods

        private void ValidationRequest(OrderApiRequestDto orderRequestDto)
        {
            if (!ModelState.IsValid)
            {
                throw new ApiException(ModelState.AllErrors());
            }
        }

        #endregion Private Methods
    }
}