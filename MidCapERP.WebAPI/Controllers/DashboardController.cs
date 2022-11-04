using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;

namespace MidCapERP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public DashboardController(IUnitOfWorkBL unitOfWorkBL)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [HttpGet("OrderReceivableCount")]
        [Authorize(ApplicationIdentityConstants.Permissions.Dashboard.View)]
        public async Task<ApiResponse> OrderReceivableCount(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.OrderBL.GetOrderReceivableCount(cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "No Data found", result: data, statusCode: 404);
            }
            return new ApiResponse(message: "Data found", result: data, statusCode: 200);
        }

        [HttpGet("OrderApprovedCount")]
        [Authorize(ApplicationIdentityConstants.Permissions.Dashboard.View)]
        public async Task<ApiResponse> OrderApprovedCount(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.OrderBL.GetOrderApprovedCount(cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "No Data found", result: data, statusCode: 404);
            }
            return new ApiResponse(message: "Data found", result: data, statusCode: 200);
        }

        [HttpGet("OrderPendingApprovalCount")]
        [Authorize(ApplicationIdentityConstants.Permissions.Dashboard.View)]
        public async Task<ApiResponse> OrderPendingApprovalCount(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.OrderBL.GetOrderPendingApprovalCount(cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "No Data found", result: data, statusCode: 404);
            }
            return new ApiResponse(message: "Data found", result: data, statusCode: 200);
        }

        [HttpGet("OrderFollowUpCount")]
        [Authorize(ApplicationIdentityConstants.Permissions.Dashboard.View)]
        public async Task<ApiResponse> OrderFollowUpCount(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.OrderBL.GetOrderFollowUpCount(cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "No Data found", result: data, statusCode: 404);
            }
            return new ApiResponse(message: "Data found", result: data, statusCode: 200);
        }
    }
}