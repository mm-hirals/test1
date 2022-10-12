using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto.OrderCalculation;

namespace MidCapERP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public ProductController(IUnitOfWorkBL unitOfWorkBL)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [HttpGet("{id}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.View)]
        public async Task<ApiResponse> Get(int id, CancellationToken cancellationToken)
        {
            var productData = await _unitOfWorkBL.ProductBL.GetByIdAPI(id, cancellationToken);
            if (productData == null)
            {
                return new ApiResponse(message: "No Data found", result: productData, statusCode: 404);
            }
            return new ApiResponse(message: "Data Found", result: productData, statusCode: 200);
        }

        [HttpGet("detailsModelNo/{modelNo}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.View)]
        public async Task<ApiResponse> GetDetails(string modelNo, CancellationToken cancellationToken)
        {
            var productDetailsData = await _unitOfWorkBL.ProductBL.GetProductForDetailsByModuleNo(modelNo, cancellationToken);
            if (productDetailsData == null)
            {
                return new ApiResponse(message: "No Data found", result: productDetailsData, statusCode: 404);
            }
            return new ApiResponse(message: "Data Found", result: productDetailsData, statusCode: 200);
        }

        [HttpPost("GetPriceByDimensions")]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.View)]
        public async Task<ApiResponse> GetPriceByDimensions([FromBody] ProductDimensionsApiRequestDto orderCalculationApiRequestDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.ProductBL.GetPriceByDimensionsAPI(orderCalculationApiRequestDto, cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "No Data found", result: data, statusCode: 404);
            }
            return new ApiResponse(message: "Data found", result: data, statusCode: 200);
        }
    }
}