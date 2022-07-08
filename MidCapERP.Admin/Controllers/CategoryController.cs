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
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var categories = await _unitOfWorkBL.CategoriesBL.GetAll(cancellationToken);
            return View(categories);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Category.Create)]
        public async Task<IActionResult> Create(CategoriesRequestDto categoriesRequestDto, CancellationToken cancellationToken)
        {
            var categories = await _unitOfWorkBL.CategoriesBL.CreateCategory(categoriesRequestDto, cancellationToken);
            return View(categories);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Category.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            var categories = await _unitOfWorkBL.CategoriesBL.GetById(Id, cancellationToken);
            return View(categories);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Category.Update)]
        public async Task<IActionResult> Update(int Id, CategoriesRequestDto categoriesRequestDto, CancellationToken cancellationToken)
        {
            var categories = await _unitOfWorkBL.CategoriesBL.UpdateCategory(Id, categoriesRequestDto, cancellationToken);
            return View(categories);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Category.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            var categories = await _unitOfWorkBL.CategoriesBL.DeleteCategory(Id, cancellationToken);
            return View(categories);
        }
    }
}