using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.Category;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Infrastructure.Constants;

namespace MidCapERP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public CategoryController(IUnitOfWorkBL unitOfWorkBL)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        //[HttpPost("/Search/")]
        //[Authorize(ApplicationIdentityConstants.Permissions.Category.View)]
        //public async Task<ApiResponse> Search([FromBody] DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        //{
        //    var data = await _unitOfWorkBL.CategoryBL.GetFilterCategoryData(dataTableFilterDto, cancellationToken);
        //    if (data == null)
        //    {
        //        return new ApiResponse(message: "No Data found", result: data, statusCode: 404);
        //    }
        //    return new ApiResponse(message: "Data found", result: data, statusCode: 200);
        //}

        //[HttpGet("/Search/{searchText}")]
        //[Authorize(ApplicationIdentityConstants.Permissions.Category.View)]
        //public async Task<ApiResponse> Get(string searchText, CancellationToken cancellationToken)
        //{
        //    var data = await _unitOfWorkBL.CategoryBL.GetCategorySearchByCategoryName(searchText, cancellationToken);
        //    if (data == null)
        //    {
        //        return new ApiResponse(message: "No Data found", result: data, statusCode: 404);
        //    }
        //    return new ApiResponse(message: "Data found", result: data, statusCode: 200);
        //}

        [HttpGet("{id}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Category.View)]
        public async Task<ApiResponse> Get(int id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.CategoryBL.GetById(id, cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "No Data found", result: data, statusCode: 404);
            }
            return new ApiResponse(message: "Data found", result: data, statusCode: 200);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Category.Create)]
        public async Task<ApiResponse> Post([FromBody] CategoryRequestDto categoryRequestDto, CancellationToken cancellationToken)
        {
            ValidateRequest(categoryRequestDto);
            var data = await _unitOfWorkBL.CategoryBL.CreateCategory(categoryRequestDto, cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "Internal server error", result: data, statusCode: 500);
            }
            return new ApiResponse(message: "Data inserted successful", result: data, statusCode: 200);
        }

        [HttpPut("{id}")]
        [Authorize(ApplicationIdentityConstants.Permissions.Category.Update)]
        public async Task<ApiResponse> Put(int id, [FromBody] CategoryRequestDto categoryRequestDto, CancellationToken cancellationToken)
        {
            ValidateRequest(categoryRequestDto);
            var data = await _unitOfWorkBL.CategoryBL.UpdateCategory(id, categoryRequestDto, cancellationToken);
            if (data == null)
            {
                return new ApiResponse(message: "Internal server error", result: data, statusCode: 500);
            }
            return new ApiResponse(message: "Data updated successful", result: data, statusCode: 200);
        }

        #region Private methods

        private void ValidateRequest(CategoryRequestDto categoryRequestDto)
        {
            if (!ModelState.IsValid)
            {
                throw new ApiException(ModelState.AllErrors());
            }
        }

        #endregion Private methods
    }
}