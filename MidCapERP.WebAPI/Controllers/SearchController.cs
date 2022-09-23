using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto.MegaSearch;
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

        /// <summary>
        /// Search by Product, Fabric and Police for binding into the dropdown 
        /// </summary>
        /// <param name="modelNo">Model No of Product or Fabric or Police</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{modelNo}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.View)]
        public async Task<ApiResponse> Get(string modelNo, CancellationToken cancellationToken)
        {
            List<MegaSearchResponse> productData = new List<MegaSearchResponse>();
            productData.AddRange(await _unitOfWorkBL.ProductBL.GetProductForDropDownByModuleNo(modelNo, cancellationToken));
            productData.AddRange(await _unitOfWorkBL.FabricBL.GetFabricForDropDownByModuleNo(modelNo, cancellationToken));
            productData.AddRange(await _unitOfWorkBL.PolishBL.GetPolishForDropDownByModuleNo(modelNo, cancellationToken));
            if (productData != null && productData.Count > 0)
                return new ApiResponse(message: "Data Found", result: productData, statusCode: 200);
            else
                return new ApiResponse(message: "No Data Found", result: productData, statusCode: 404);
        }
    }
}