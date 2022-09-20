using AutoWrapper.Extensions;
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

        [HttpPost("/Order")]
        [Authorize(ApplicationIdentityConstants.Permissions.Order.Create)]
        public async Task<ApiResponse> Post([FromBody] OrderRequestDto orderRequestDto, CancellationToken cancellationToken)
        {
            ValidationRequest(orderRequestDto);
            var data = await _unitOfWorkBL.OrderBL.CreateOrder(orderRequestDto, cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "Internal server error", result: data, statusCode: 500);
            }
            return new ApiResponse(message: "Data inserted successful", result: data, statusCode: 200);
        }

        #region Private Methods

        private void ValidationRequest(OrderRequestDto orderRequestDto)
        {
            if (!ModelState.IsValid)
            {
                throw new ApiException(ModelState.AllErrors());
            }
        }

        #endregion Private Methods
    }
}