﻿using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto.Order;

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
        public async Task<ApiResponse> Get(long id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.OrderBL.GetOrderDetailByOrderIdAPI(id, cancellationToken);
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
            var data = await _unitOfWorkBL.OrderBL.CreateOrderAPI(orderRequestApiDto, cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "Internal server error", result: data, statusCode: 500);
            }
            return new ApiResponse(message: "Data inserted successful", result: data, statusCode: 200);
        }

        [HttpPut("{id}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Order.Update)]
        public async Task<ApiResponse> Put(int id, [FromBody] OrderApiRequestDto orderRequestApiDto, CancellationToken cancellationToken)
        {
            ValidationRequest(orderRequestApiDto);
            var data = await _unitOfWorkBL.OrderBL.UpdateOrderAPI(id, orderRequestApiDto, cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "Internal server error", result: data, statusCode: 500);
            }
            return new ApiResponse(message: "Data updated successful", result: data, statusCode: 200);
        }

        [HttpPost("DeleteOrder")]
        [Authorize(ApplicationIdentityConstants.Permissions.Order.Delete)]
        public async Task<ApiResponse> DeleteOrder([FromBody] OrderDeleteApiRequestDto orderDeleteApiRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.OrderBL.DeleteOrder(orderDeleteApiRequestDto, cancellationToken);
            if (orderDeleteApiRequestDto.DeleteType != (int)OrderDeleteTypeEnum.Order)
            {
                return await Get(orderDeleteApiRequestDto.OrderId, cancellationToken);
            }
            return new ApiResponse(message: "Data deleted successful", result: null, statusCode: 200);
        }

        [HttpGet("{orderSetItemId}/{discountPrice}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Order.Update)]
        public async Task<ApiResponse> UpdateOrderDiscountAmount(Int64 orderSetItemId, decimal discountPrice, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.OrderBL.UpdateOrderDiscountAmount(orderSetItemId, discountPrice, cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "No Data found", result: data, statusCode: 404);
            }
            return new ApiResponse(message: "Data updated successful", result: data, statusCode: 200);
        }

        /// <summary>
        /// Calculate product price based on dimension 
        /// </summary>
        /// <param name="subjectTypeId"></param>
        /// <param name="subjectId"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="depth"></param>
        /// <param name="quantity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{subjectTypeId}/{subjectId}/{width}/{height}/{depth}/{quantity}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Order.Update)]
        public async Task<ApiResponse> CalculateProductDimensionPrice(int subjectTypeId, int subjectId, decimal width, decimal height, decimal depth, int quantity, CancellationToken cancellationToken)
        {
            // Get product by productId
            var productData = await _unitOfWorkBL.ProductBL.GetByIdAPI(subjectId, cancellationToken);
            if (productData == null)
            {
                return new ApiResponse(message: "No Data found", result: productData, statusCode: 404);
            }

            // Calculate product dimension price
            var data = await _unitOfWorkBL.OrderBL.CalculateProductDimensionPriceAPI(productData, subjectTypeId, subjectId, width, height, depth, quantity, cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "Internal server error", result: data, statusCode: 500);
            }
            return new ApiResponse(message: "Data found", result: data, statusCode: 200);
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