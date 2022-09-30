using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto.Product;

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

        //[HttpGet("{modelNo}")]
        //[Authorize(ApplicationIdentityConstants.Permissions.Product.View)]
        //public async Task<ApiResponse> Get(string modelNo, CancellationToken cancellationToken)
        //{
        //    var productData = await _unitOfWorkBL.ProductBL.GetProductForDropDownByModuleNo(modelNo, cancellationToken);
        //    if (productData == null)
        //    {
        //        return new ApiResponse(message: "No Data found", result: productData, statusCode: 404);
        //    }
        //    return new ApiResponse(message: "Data Found", result: productData, statusCode: 200);
        //}

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

        /// <summary>
        /// Get Product Details for Product, Fabric and Polish
        /// </summary>
        /// <param name="modelNo">Porduct ModelNo</param>
        /// <param name="productType">Product OR Fabric OR Polish</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{modelNo}/{productType}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.View)]
        public async Task<ApiResponse> GetDetails(string modelNo, string productType, CancellationToken cancellationToken)
        {
            if (productType == "product")
            {
                var productData = await _unitOfWorkBL.ProductBL.GetProductForDetailsByModuleNo(modelNo, cancellationToken);
                if (productData == null)
                    return new ApiResponse(message: "No Data Found", result: productData, statusCode: 404);
                return new ApiResponse(message: "Data Found", result: productData, statusCode: 200);
            }
            else if (productType == "fabric")
            {
                var frabricData = await _unitOfWorkBL.FabricBL.GetFabricForDetailsByModuleNo(modelNo, cancellationToken);
                if (frabricData == null)
                    return new ApiResponse(message: "No Data Found", result: frabricData, statusCode: 200);
                return new ApiResponse(message: "Data Found", result: frabricData, statusCode: 200);
            }
            else if (productType == "polish")
            {
                var polishData = await _unitOfWorkBL.PolishBL.GetPolishForDetailsByModuleNo(modelNo, cancellationToken);
                if (polishData == null)
                    return new ApiResponse(message: "No Data Found", result: polishData, statusCode: 200);
                return new ApiResponse(message: "Data Found", result: polishData, statusCode: 200);
            }
            else
                return new ApiResponse(message: "No Data found", statusCode: 404);
        }
    }
}