using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.Product;
using MidCapERP.Infrastructure.Constants;

namespace MidCapERP.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public ProductController(IUnitOfWorkBL unitOfWorkBL)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Product.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            await FillCategoryDropDown(cancellationToken);
            return View("Create");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.Create)]
        public async Task<IActionResult> Create(ProductRequestDto productRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.ProductBL.CreateProduct(productRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        #region Private Method

        private async Task FillCategoryDropDown(CancellationToken cancellationToken)
        {
            var categoryTypeData = await _unitOfWorkBL.CategoryBL.GetAll(cancellationToken);
            var categorySelectedList = categoryTypeData.Select(a =>
                                 new SelectListItem
                                 {
                                     Value = Convert.ToString(a.LookupValueId),
                                     Text = a.LookupValueName
                                 }).ToList();
            ViewBag.CategorySelectedList = categorySelectedList;
        }

        #endregion Private Method
    }
}