using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto.Order;
using MidCapERP.Dto.OrderCalculation;

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

        [HttpGet("OrderStatus/{status}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Order.View)]
        public async Task<ApiResponse> GetOrderbyStatus(string status, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.OrderBL.GetOrderForDetailsByStatus(status, cancellationToken);
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
        public async Task<ApiResponse> Put(Int64 id, [FromBody] OrderApiRequestDto orderRequestApiDto, CancellationToken cancellationToken)
        {
            ValidationRequest(orderRequestApiDto);
            var data = await _unitOfWorkBL.OrderBL.UpdateOrderAPI(id, orderRequestApiDto, cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "Internal server error", result: data, statusCode: 500);
            }
            return new ApiResponse(message: "Data updated successful", result: data, statusCode: 200);
        }

        [HttpPatch("{id}/{advanceAmount}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Order.Update)]
        public async Task<ApiResponse> UpdateOrderAdvanceAmount(Int64 id, decimal advanceAmount, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.OrderBL.UpdateOrderAdvanceAmountAPI(id, advanceAmount, cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "No Data found", result: data, statusCode: 404);
            }
            return new ApiResponse(message: "Data updated successful", result: data, statusCode: 200);
        }

        [HttpPatch("SendForApproval/{id}/{comments}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Order.Update)]
        public async Task<ApiResponse> UpdateOrderSendForApproval(Int64 id, string comments, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.OrderBL.UpdateOrderSendForApproval(id, comments, cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "No Data found", result: data, statusCode: 404);
            }
            return new ApiResponse(message: "Data updated successful", result: data, statusCode: 200);
        }

        [HttpDelete]
        [Authorize(ApplicationIdentityConstants.Permissions.Order.Delete)]
        public async Task<ApiResponse> DeleteOrder([FromBody] OrderDeleteApiRequestDto orderDeleteApiRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.OrderBL.DeleteOrderAPI(orderDeleteApiRequestDto, cancellationToken);
            if (orderDeleteApiRequestDto.DeleteType != (int)OrderDeleteTypeEnum.Order)
            {
                return await Get(orderDeleteApiRequestDto.OrderId, cancellationToken);
            }
            return new ApiResponse(message: "Data deleted successful", result: null, statusCode: 200);
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