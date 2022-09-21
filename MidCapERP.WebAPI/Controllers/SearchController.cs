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
    public class SearchController : Controller
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public SearchController(IUnitOfWorkBL unitOfWorkBL)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [HttpGet("/Search/{modelNo}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.View)]
        public async Task<ApiResponse> Get(string modelNo, CancellationToken cancellationToken)
        {
            List<ProductForDorpDownByModuleNoResponseDto> productData = new List<ProductForDorpDownByModuleNoResponseDto>();
            productData.AddRange(await _unitOfWorkBL.ProductBL.GetProductForDropDownByModuleNo(modelNo, cancellationToken));
            productData.AddRange(await _unitOfWorkBL.FabricBL.GetFabricForDropDownByModuleNo(modelNo, cancellationToken));
            productData.AddRange(await _unitOfWorkBL.PolishBL.GetPolishForDropDownByModuleNo(modelNo, cancellationToken));
            if (productData != null && productData.Count > 0)
                return new ApiResponse(message: "Data Found", result: productData, statusCode: 200);
            else
                return new ApiResponse(message: "No Data Found", result: productData, statusCode: 404);
        }

        [HttpGet("/Search/{modelNo}/{productType}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.View)]
        public async Task<ApiResponse> GetDetails(string modelNo, string productType, CancellationToken cancellationToken)
        {
            if (productType == "Product")
            {
                IList<ProductForDetailsByModuleNoResponceDto> productData = new List<ProductForDetailsByModuleNoResponceDto>();
                productData = await _unitOfWorkBL.ProductBL.GetProductForDetailsByModuleNo(modelNo, cancellationToken);
                if(productData == null  || productData.Count == 0)
                    return new ApiResponse(message: "No Data Found", result: productData, statusCode: 404);
                return new ApiResponse(message: "Data Found", result: productData, statusCode: 200);
            }
            else if (productType == "Fabric")
            {
                var frabricData = await _unitOfWorkBL.FabricBL.GetFabricForDetailsByModuleNo(modelNo, cancellationToken);
                if(frabricData == null)
                    return new ApiResponse(message: "No Data Found", result: frabricData, statusCode: 200);
                return new ApiResponse(message: "Data Found", result: frabricData, statusCode: 200);
            }
            else if (productType == "Polish")
            {
                var polishData = await _unitOfWorkBL.PolishBL.GetPolishForDetailsByModuleNo(modelNo, cancellationToken);
                if(polishData == null)
                    return new ApiResponse(message: "No Data Found", result: polishData, statusCode: 200);
                return new ApiResponse(message: "Data Found", result: polishData, statusCode: 200);
            }
            else
                return new ApiResponse(message: "No Data found", statusCode: 404);
        }

        [HttpGet("/Search/Customer/{CustomerNameOrEmailOrMobileNo}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.View)]
        public async Task<ApiResponse> SearchCustomer(string CustomerNameOrEmailOrMobileNo, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.CustomersBL.SearchCustomer(CustomerNameOrEmailOrMobileNo, cancellationToken);
            if (data == null || data.Count() == 0)
            {
                return new ApiResponse(message: "Customer not found!", result: data, statusCode: 404);
            }
            return new ApiResponse(message: "Customer Found", result: data, statusCode: 200);
        }
    }
}