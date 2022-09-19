using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Infrastructure.Constants;

namespace MidCapERP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)] 
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public ProductController(IUnitOfWorkBL unitOfWorkBL)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [HttpGet("/Products/{id}")]
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

        [HttpGet("/Product/{modelNo}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.View)]
        public async Task<ApiResponse> Get(string modelNo, CancellationToken cancellationToken)
        {
            var productData = await _unitOfWorkBL.ProductBL.GetProductForDropDownByModuleNo(modelNo, cancellationToken);
            if (productData == null)
            {
                return new ApiResponse(message: "No Data found", result: productData, statusCode: 404);
            }
            return new ApiResponse(message: "Data Found", result: productData, statusCode: 200);
        }

        [HttpGet("/Product/detailsModelNo/{modelNo}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.View)]
        public async Task<ApiResponse> GetDetails(string modelNo, CancellationToken cancellationToken)
        {
            var productDetailsData = await _unitOfWorkBL.ProductBL.GetProductForDetailsByModuleNo(modelNo, cancellationToken);
            if (productDetailsData == null || productDetailsData.Count == 0)
            {
                return new ApiResponse(message: "No Data found", result: productDetailsData, statusCode: 404);
            }
            return new ApiResponse(message: "Data Found", result: productDetailsData, statusCode: 200);
        }
    }
}