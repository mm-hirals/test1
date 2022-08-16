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
            await FillWoodDropDown(cancellationToken);
            await FillRawMaterialDropDown(cancellationToken);
            await FillAccessoriesDropDown(cancellationToken);
            await FillPolishDropDown(cancellationToken);
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

        private async Task FillWoodDropDown(CancellationToken cancellationToken)
        {
            var woodTypeData = await _unitOfWorkBL.WoodBL.GetAll(cancellationToken);
            var woodSelectedList = woodTypeData.Select(a =>
                                 new SelectListItem
                                 {
                                     Value = Convert.ToString(a.WoodId),
                                     Text = a.Title
                                 }).ToList();
            ViewBag.WoodSelectedList = woodSelectedList;
        }

        private async Task FillRawMaterialDropDown(CancellationToken cancellationToken)
        {
            var rawMaterialTypeData = await _unitOfWorkBL.RawMaterialBL.GetAll(cancellationToken);
            var rawMaterialSelectedList = rawMaterialTypeData.Select(a =>
                                 new SelectListItem
                                 {
                                     Value = Convert.ToString(a.RawMaterialId),
                                     Text = a.Title
                                 }).ToList();
            ViewBag.RawMaterialSelectedList = rawMaterialSelectedList;
        }

        private async Task FillAccessoriesDropDown(CancellationToken cancellationToken)
        {
            var accessoriesTypeData = await _unitOfWorkBL.AccessoriesBL.GetAll(cancellationToken);
            var accessoriesSelectedList = accessoriesTypeData.Select(a =>
                                 new SelectListItem
                                 {
                                     Value = Convert.ToString(a.AccessoriesId),
                                     Text = a.Title
                                 }).ToList();
            ViewBag.AccessoriesSelectedList = accessoriesSelectedList;
        }

        private async Task FillPolishDropDown(CancellationToken cancellationToken)
        {
            var polishTypeData = await _unitOfWorkBL.PolishBL.GetAll(cancellationToken);
            var polishSelectedList = polishTypeData.Select(a =>
                                 new SelectListItem
                                 {
                                     Value = Convert.ToString(a.PolishId),
                                     Text = a.Title
                                 }).ToList();
            ViewBag.PolishSelectedList = polishSelectedList;
        }

        #endregion Private Method
    }
}