using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Infrastructure.Constants;

namespace MidCapERP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardController : Controller
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public DashBoardController(IUnitOfWorkBL unitOfWorkBL)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [HttpGet("CustomerCount")]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.View)]
        public async Task<ApiResponse> CustomerCount(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.CustomersBL.GetCustomerCount(cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "No Data found", result: data, statusCode: 404);
            }
            return new ApiResponse(message: "Data found", result: data, statusCode: 200);
        }
    }
}