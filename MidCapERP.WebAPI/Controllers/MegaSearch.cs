﻿using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
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
            productData.AddRange(await _unitOfWorkBL.CustomersBL.GetCustomerForDropDownByMobileNo(searchText, cancellationToken));
            productData.AddRange(await _unitOfWorkBL.OrderBL.GetOrderForDropDownByOrderNo(searchText, cancellationToken));
            if (productData != null && productData.Count > 0)
                return new ApiResponse(message: "Data Found", result: productData, statusCode: 200);
            else
                return new ApiResponse(message: "No Data Found", result: productData, statusCode: 404);
        }
    }
}