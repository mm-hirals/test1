using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto.Constants;
using MidCapERP.Dto.SearchResponse;

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
        /// Search by Product, Fabric and Polish for binding into the dropdown
        /// </summary>
        /// <param name="modelNo">Model No of Product or Fabric or Polish</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{modelNo}")]
        [Authorize(ApplicationIdentityConstants.Permissions.AppOrder.View)]
        public async Task<ApiResponse> Get(string modelNo, CancellationToken cancellationToken)
        {
            List<SearchResponse> productData = new List<SearchResponse>();
            productData.AddRange(await _unitOfWorkBL.ProductBL.GetProductForDropDownByModuleNo(modelNo, cancellationToken));
            productData.AddRange(await _unitOfWorkBL.FabricBL.GetFabricForDropDownByModuleNo(modelNo, cancellationToken));
            productData.AddRange(await _unitOfWorkBL.PolishBL.GetPolishForDropDownByModuleNo(modelNo, cancellationToken));
            if (productData != null && productData.Count > 0)
                return new ApiResponse(message: "Data Found", result: productData, statusCode: 200);
            else
                return new ApiResponse(message: "No Data Found", result: productData, statusCode: 404);
        }

        /// <summary>
        /// Get Product Details for Product, Fabric and Polish
        /// </summary>
        /// <param name="modelNo">Porduct ModelNo</param>
        /// <param name="subjectTypeId">Product OR Fabric OR Polish</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{modelNo}/{subjectTypeId}")]
        [Authorize(ApplicationIdentityConstants.Permissions.AppOrder.View)]
        public async Task<ApiResponse> GetDetails(string modelNo, int subjectTypeId, CancellationToken cancellationToken)
        {
            int productSubjectTypeId = await _unitOfWorkBL.ProductBL.GetProductSubjectTypeId(cancellationToken);
            int polishSubjectTypeId = await _unitOfWorkBL.ProductBL.GetPolishSubjectTypeId(cancellationToken);
            int fabricSubjectTypeId = await _unitOfWorkBL.ProductBL.GetFabricSubjectTypeId(cancellationToken);

            if (subjectTypeId == productSubjectTypeId)
            {
                var productData = await _unitOfWorkBL.ProductBL.GetProductForDetailsByModuleNo(modelNo, cancellationToken);
                if (productData == null)
                    return new ApiResponse(message: "No Data Found", result: productData, statusCode: 404);
                return new ApiResponse(message: "Data Found", result: productData, statusCode: 200);
            }
            else if (subjectTypeId == polishSubjectTypeId)
            {
                var polishData = await _unitOfWorkBL.PolishBL.GetPolishForDetailsByModuleNo(modelNo, cancellationToken);
                if (polishData == null)
                    return new ApiResponse(message: "No Data Found", result: polishData, statusCode: 200);
                return new ApiResponse(message: "Data Found", result: polishData, statusCode: 200);
            }
            else if (subjectTypeId == fabricSubjectTypeId)
            {
                var frabricData = await _unitOfWorkBL.FabricBL.GetFabricForDetailsByModuleNo(modelNo, cancellationToken);
                if (frabricData == null)
                    return new ApiResponse(message: "No Data Found", result: frabricData, statusCode: 200);
                return new ApiResponse(message: "Data Found", result: frabricData, statusCode: 200);
            }
            else
                return new ApiResponse(message: "No Data found", statusCode: 404);
        }
    }
}