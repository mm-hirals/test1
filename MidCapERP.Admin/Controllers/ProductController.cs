using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.DataGrid;
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

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.View)]
        public async Task<IActionResult> GetProductData([FromForm] DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.ProductBL.GetFilterProductData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            await FillCategoryDropDown(cancellationToken);
            await FillFrameDropDowns(cancellationToken);
            await FillRawMaterialDropDowns(cancellationToken);
            await FillPolishDropDowns(cancellationToken);
            await FillCushionDropDowns(cancellationToken);
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.Create)]
        public async Task<IActionResult> Create([FromForm] ProductMainRequestDto productMainRequestDto, CancellationToken cancellationToken)
        {
            productMainRequestDto.ProductMaterialRequestDto = productMainRequestDto.ProductMaterialRequestDto.Where(x => x.IsDeleted != true).ToList();
            await _unitOfWorkBL.ProductBL.CreateProduct(productMainRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            var getProductData = await _unitOfWorkBL.ProductBL.GetById(Id, cancellationToken);
            await FillCategoryDropDown(cancellationToken);
            await FillFrameDropDowns(cancellationToken);
            await FillRawMaterialDropDowns(cancellationToken);
            await FillPolishDropDowns(cancellationToken);
            await FillCushionDropDowns(cancellationToken);
            return View("Create", getProductData);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.Update)]
        public async Task<IActionResult> Update(int Id, ProductMainRequestDto PolishRequestDto, CancellationToken cancellationToken)
        {
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Polish.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
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

        private async Task FillFrameDropDowns(CancellationToken cancellationToken)
        {
            var frameTypeData = await _unitOfWorkBL.FrameBL.GetAll(cancellationToken);
            var frameSelectedList = frameTypeData.Select(a =>
                                 new SelectListItem
                                 {
                                     Value = Convert.ToString(a.FrameId),
                                     Text = a.Title
                                 }).ToList();
            ViewBag.FrameSelectedList = frameSelectedList;
        }

        private async Task FillRawMaterialDropDowns(CancellationToken cancellationToken)
        {
            var rawMaterialData = await _unitOfWorkBL.RawMaterialBL.GetAll(cancellationToken);
            var rawMaterialSelectedList = rawMaterialData.Select(a =>
                                 new SelectListItem
                                 {
                                     Value = Convert.ToString(a.RawMaterialId),
                                     Text = a.Title
                                 }).ToList();
            ViewBag.rawMaterialDropDownData = rawMaterialSelectedList;
        }

        private async Task FillPolishDropDowns(CancellationToken cancellationToken)
        {
            var polishData = await _unitOfWorkBL.PolishBL.GetAll(cancellationToken);
            var polishSelectedList = polishData.Select(a =>
                                 new SelectListItem
                                 {
                                     Value = Convert.ToString(a.PolishId),
                                     Text = a.Title
                                 }).ToList();
            ViewBag.polishDropDownData = polishSelectedList;
        }

        private async Task FillCushionDropDowns(CancellationToken cancellationToken)
        {
            var cushionData = await _unitOfWorkBL.FrameBL.GetAll(cancellationToken);
            var cushionSelectedList = cushionData.Select(a =>
                                new SelectListItem
                                {
                                    Value = Convert.ToString(a.FrameId),
                                    Text = a.Title
                                }).ToList();
            ViewBag.cushionDropDownData = cushionSelectedList;
        }

        #endregion Private Method
    }
}