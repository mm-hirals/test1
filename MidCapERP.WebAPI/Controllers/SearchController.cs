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

        [HttpGet("/Search/modelNo/{modelNo}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.View)]
        public async Task<ApiResponse> Get(string modelNo, CancellationToken cancellationToken)
        {
            List<ProductForDorpDownByModuleNoResponseDto> productData = new List<ProductForDorpDownByModuleNoResponseDto>();
            productData.AddRange(await _unitOfWorkBL.ProductBL.GetProductForDropDownByModuleNo(modelNo, cancellationToken));
            productData.AddRange(await _unitOfWorkBL.FabricBL.GetFabricForDropDownByModuleNo(modelNo, cancellationToken));
            productData.AddRange(await _unitOfWorkBL.PolishBL.GetPolishForDropDownByModuleNo(modelNo, cancellationToken));
            if (productData != null)
                return new ApiResponse(message: "Data Found", result: productData, statusCode: 200);
            else
                return new ApiResponse(message: "No Data Found", result: productData, statusCode: 200);
        }

        [HttpGet("/Search/DetailsModelNo/{modelNo}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.View)]
        public async Task<ApiResponse> GetDetails(string modelNo, string productType, CancellationToken cancellationToken)
        {
            if (productType == "Product")
            {
                IList<ProductForDetailsByModuleNoResponceDto> productData = new List<ProductForDetailsByModuleNoResponceDto>();
                productData = await _unitOfWorkBL.ProductBL.GetProductForDetailsByModuleNo(modelNo, cancellationToken);
                return new ApiResponse(message: "Data Found", result: productData, statusCode: 200);
            }
            else if (productType == "Fabric")
            {
                var frabricData = await _unitOfWorkBL.FabricBL.GetFabricForDetailsByModuleNo(modelNo, cancellationToken);
                return new ApiResponse(message: "Data Found", result: frabricData, statusCode: 200);
            }
            else if (productType == "Polish")
            {
                var polishData = await _unitOfWorkBL.PolishBL.GetPolishForDetailsByModuleNo(modelNo, cancellationToken);
                return new ApiResponse(message: "Data Found", result: polishData, statusCode: 200);
            }
            else
                return new ApiResponse(message: "No Data found", statusCode: 404);
        }
    }
}