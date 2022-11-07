using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto.Category;
using MidCapERP.Dto.Constants;

namespace MidCapERP.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public CategoryController(IUnitOfWorkBL unitOfWorkBL, IStringLocalizer<BaseController> localizer) : base(localizer)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Category.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Category.View)]
        public async Task<IActionResult> GetCategoryData([FromForm] CategoryDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var categoryfilterdata = await _unitOfWorkBL.CategoryBL.GetFilterCategoryData(dataTableFilterDto, cancellationToken);
            return Ok(categoryfilterdata);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Category.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            BindCategoryType();
            return PartialView("_CategoryPartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Category.Create)]
        public async Task<IActionResult> Create(CategoryRequestDto categoryRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.CategoryBL.CreateCategory(categoryRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Category.Update)]
        public async Task<IActionResult> Update(long Id, CancellationToken cancellationToken)
        {
            var category = await _unitOfWorkBL.CategoryBL.GetById(Id, cancellationToken);
            BindCategoryType();
            return PartialView("_CategoryPartial", category);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Category.Update)]
        public async Task<IActionResult> Update(long Id, CategoryRequestDto categoryRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.CategoryBL.UpdateCategory(Id, categoryRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Category.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.CategoryBL.DeleteCategory(Id, cancellationToken);
            return RedirectToAction("Index");
        }

        public void BindCategoryType()
        {
            var categoryProductType = from ProductCategoryTypesEnum e in Enum.GetValues(typeof(ProductCategoryTypesEnum))
                                      select new
                                      {
                                          Value = (int)e,
                                          Text = e.ToString()
                                      };

            var categoryProductTypeList = categoryProductType.Select(a =>
                                new SelectListItem
                                {
                                    Value = Convert.ToString(a.Value),
                                    Text = a.Text
                                }).ToList();

            ViewBag.CategoryProductType = categoryProductTypeList;
        }
    }
}