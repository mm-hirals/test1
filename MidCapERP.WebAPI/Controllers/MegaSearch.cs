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
    public class MegaSearchController : Controller
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public MegaSearchController(IUnitOfWorkBL unitOfWorkBL)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [HttpGet("{searchText}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.View)]
        public async Task<ApiResponse> MegaSearch(string searchText, CancellationToken cancellationToken)
        {
            List<Object> productData = new List<Object>();
            productData.AddRange(await _unitOfWorkBL.ProductBL.GetProductForDropDownByModuleNo(searchText, cancellationToken));
            productData.AddRange(await _unitOfWorkBL.FabricBL.GetFabricForDropDownByModuleNo(searchText, cancellationToken));
            productData.AddRange(await _unitOfWorkBL.PolishBL.GetPolishForDropDownByModuleNo(searchText, cancellationToken));
            productData.AddRange(await _unitOfWorkBL.CustomersBL.GetCustomerForDropDownByMobileNo(searchText, cancellationToken));
            productData.AddRange(await _unitOfWorkBL.OrderBL.GetOrderForDropDownByOrderNo(searchText, cancellationToken));
            if (productData != null && productData.Count > 0)
                return new ApiResponse(message: "Data Found", result: productData, statusCode: 200);
            else
                return new ApiResponse(message: "No Data Found", result: productData, statusCode: 404);
        }

        [HttpGet("{searchText}/{Type}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.View)]
        public async Task<ApiResponse> MegaSearch(string searchText, string Type, CancellationToken cancellationToken)
        {
            if (Type == "Product")
            {
                IList<ProductForDetailsByModuleNoResponceDto> productData = new List<ProductForDetailsByModuleNoResponceDto>();
                productData = await _unitOfWorkBL.ProductBL.GetProductForDetailsByModuleNo(searchText, cancellationToken);
                if (productData == null || productData.Count == 0)
                    return new ApiResponse(message: "No Data Found", result: productData, statusCode: 404);
                return new ApiResponse(message: "Data Found", result: productData, statusCode: 200);
            }
            else if (Type == "Fabric")
            {
                var frabricData = await _unitOfWorkBL.FabricBL.GetFabricForDetailsByModuleNo(searchText, cancellationToken);
                if (frabricData == null)
                    return new ApiResponse(message: "No Data Found", result: frabricData, statusCode: 404);
                return new ApiResponse(message: "Data Found", result: frabricData, statusCode: 200);
            }
            else if (Type == "Polish")
            {
                var polishData = await _unitOfWorkBL.PolishBL.GetPolishForDetailsByModuleNo(searchText, cancellationToken);
                if (polishData == null)
                    return new ApiResponse(message: "No Data Found", result: polishData, statusCode: 404);
                return new ApiResponse(message: "Data Found", result: polishData, statusCode: 200);
            }
            else if (Type == "Customer")
            {
                var customerData = await _unitOfWorkBL.CustomersBL.GetCustomerForDetailsByMobileNo(searchText, cancellationToken);
                if (customerData == null)
                    return new ApiResponse(message: "No Data Found", result: customerData, statusCode: 404);
                return new ApiResponse(message: "Data Found", result: customerData, statusCode: 200);
            }
            else if (Type == "Order")
            {
                var orderData = await _unitOfWorkBL.OrderBL.GetOrderForDetailsByOrderNo(searchText, cancellationToken);
                if (orderData == null)
                    return new ApiResponse(message: "No Data Found", result: orderData, statusCode: 404);
                return new ApiResponse(message: "Data Found", result: orderData, statusCode: 200);
            }
            else
                return new ApiResponse(message: "No Data found", statusCode: 404);
        }
    }
}