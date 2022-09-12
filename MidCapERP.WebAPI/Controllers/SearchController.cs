using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.Product;
using MidCapERP.Infrastructure.Constants;

namespace MidCapERP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : Controller
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public SearchController(IUnitOfWorkBL unitOfWorkBL)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [HttpGet("/Search/{modelNo}/productType")]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.View)]
        [Authorize(ApplicationIdentityConstants.Permissions.Fabric.View)]
        [Authorize(ApplicationIdentityConstants.Permissions.Polish.View)]
        public async Task<ApiResponse> Get(string modelNo, string productType, CancellationToken cancellationToken)
        {
            IList<ProductForDorpDownByModuleNoResponseDto> productData = new List<ProductForDorpDownByModuleNoResponseDto>();
            if (productType.ToUpper() == "PRD")
            {
                productData = await _unitOfWorkBL.ProductBL.GetProductForDropDownByModuleNo(modelNo, cancellationToken);
                if (productData == null)
                {
                    return new ApiResponse(message: "No Data found", result: productData, statusCode: 404);
                }
            }
            else if (productType.ToUpper() == "FBR")
            {
                productData = await _unitOfWorkBL.FabricBL.GetFabricForDropDownByModuleNo(modelNo, cancellationToken);
                if (productData == null)
                {
                    return new ApiResponse(message: "No Data found", result: productData, statusCode: 404);
                }
            }
            else if (productType.ToUpper() == "PLS")
            {
                productData = await _unitOfWorkBL.PolishBL.GetPolishForDropDownByModuleNo(modelNo, cancellationToken);
                if (productData == null)
                {
                    return new ApiResponse(message: "No Data found", result: productData, statusCode: 404);
                }
            }
            else
            {
                return new ApiResponse(message: "No Data found");
            }
            return new ApiResponse(message: "Data Found", result: productData, statusCode: 200);
        }

        [HttpGet("/Search/DetailsModelNo/productTypes")]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.View)]
        [Authorize(ApplicationIdentityConstants.Permissions.Fabric.View)]
        [Authorize(ApplicationIdentityConstants.Permissions.Polish.View)]
        public async Task<ApiResponse> GetDetails(string modelNo, string productType, CancellationToken cancellationToken)
        {
            IList<ProductForDetailsByModuleNoResponceDto> productData = new List<ProductForDetailsByModuleNoResponceDto>();
            if (productType.ToUpper() == "PRD")
            {
                productData = await _unitOfWorkBL.ProductBL.GetProductForDetailsByModuleNo(modelNo, cancellationToken);
                if (modelNo == null || productData.Count == 0)
                {
                    return new ApiResponse(message: "No Data found", result: productData, statusCode: 404);
                }
            }
            else if (productType.ToUpper() == "FBR")
            {
                var frabricData = await _unitOfWorkBL.FabricBL.GetFabricForDetailsByModuleNo(modelNo, cancellationToken);
                if (frabricData == null)
                {
                    return new ApiResponse(message: "No Data found", result: frabricData, statusCode: 404);
                }
                return new ApiResponse(message: "Data Found", result: frabricData, statusCode: 200);
            }
            else if (productType.ToUpper() == "PLS")
            {
                var polishData = await _unitOfWorkBL.PolishBL.GetPolishForDetailsByModuleNo(modelNo, cancellationToken);
                if (polishData == null)
                {
                    return new ApiResponse(message: "No Data found", result: polishData, statusCode: 404);
                }
                return new ApiResponse(message: "Data Found", result: polishData, statusCode: 200);
            }
            else
            {
                return new ApiResponse(message: "No Data found");
            }
            return new ApiResponse(message: "Data Found", result: productData, statusCode: 200);
        }
    }
}