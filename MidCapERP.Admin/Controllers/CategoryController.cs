using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto.Category;
using MidCapERP.Dto.DataGrid;
using NToastNotify;

namespace MidCapERP.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;
        private readonly IToastNotification _toastNotification;

        public CategoryController(IUnitOfWorkBL unitOfWorkBL, IToastNotification toastNotification, IStringLocalizer<BaseController> localizer) : base(localizer)
        {
            _unitOfWorkBL = unitOfWorkBL;
            _toastNotification = toastNotification;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Category.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Category.View)]
        public async Task<IActionResult> GetCategoryData([FromForm] DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var categoryfilterdata = await _unitOfWorkBL.CategoryBL.GetFilterCategoryData(dataTableFilterDto, cancellationToken);
            return Ok(categoryfilterdata);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Category.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            return PartialView("_CategoryPartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Category.Create)]
        public async Task<IActionResult> Create(CategoryRequestDto categoryRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.CategoryBL.CreateCategory(categoryRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Saved Successfully!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Category.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            var category = await _unitOfWorkBL.CategoryBL.GetById(Id, cancellationToken);
            return PartialView("_CategoryPartial", category);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Category.Update)]
        public async Task<IActionResult> Update(int Id, CategoryRequestDto categoryRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.CategoryBL.UpdateCategory(Id, categoryRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data updated Successfully!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Category.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.CategoryBL.DeleteCategory(Id, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Deleted Successfully!");
            return RedirectToAction("Index");
        }
    }
}