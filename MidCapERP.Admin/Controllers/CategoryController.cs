using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.Categories;
using MidCapERP.Infrastructure.Constants;

namespace MidCapERP.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public CategoryController(IUnitOfWorkBL unitOfWorkBL)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Category.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var categories = await _unitOfWorkBL.CategoriesBL.GetAll(cancellationToken);
            return View(categories);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Category.Create)]
        [Authorize(ApplicationIdentityConstants.Permissions.Category.Update)]
        public async Task<IActionResult> CreateOrUpdate(int Id, CancellationToken cancellationToken)
        {
            if (Id == 0)
            {
                return PartialView("_CategoryPartial");
            }
            else
            {
                var categories = await _unitOfWorkBL.CategoriesBL.GetById(Id, cancellationToken);
                return PartialView("_CategoryPartial", categories);
            }
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Category.Create)]
        [Authorize(ApplicationIdentityConstants.Permissions.Category.Update)]
        public async Task<IActionResult> CreateOrUpdate(int Id, CategoriesRequestDto categoriesRequestDto, CancellationToken cancellationToken)
        {
            if (Id == 0)
            {
                var categories = await _unitOfWorkBL.CategoriesBL.CreateCategory(categoriesRequestDto, cancellationToken);
            }
            else
            {
                var categories = await _unitOfWorkBL.CategoriesBL.UpdateCategory(Id, categoriesRequestDto, cancellationToken);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Category.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            var categories = await _unitOfWorkBL.CategoriesBL.DeleteCategory(Id, cancellationToken);
            return RedirectToAction("Index");
        }
    }
}