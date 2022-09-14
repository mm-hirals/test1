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
            await FillRawMaterialDropDowns(cancellationToken);
            await FillPolishDropDowns(cancellationToken);
            return View("Create");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.Create)]
        public async Task<IActionResult> Create(ProductRequestDto productRequestDto, CancellationToken cancellationToken)
        {
            //productRequestDto.ProductMaterialRequestDto = productRequestDto.ProductMaterialRequestDto.Where(x => x.IsDeleted != true).ToList();
            await _unitOfWorkBL.ProductBL.CreateProduct(productRequestDto, cancellationToken);
            var getAllProductData = await _unitOfWorkBL.ProductBL.GetAll(cancellationToken);
            var insertedProductData = getAllProductData.Where(x => x.ProductId == productRequestDto.ProductId);

            //return RedirectToAction("RolePermission", "Role", new { id = insertedProductData. });
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            var getProductData = await _unitOfWorkBL.ProductBL.GetById(Id, cancellationToken);
            await FillCategoryDropDown(cancellationToken);
            await FillRawMaterialDropDowns(cancellationToken);
            await FillPolishDropDowns(cancellationToken);
            return View("Create", getProductData);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.Update)]
        public async Task<IActionResult> Update(int Id, [FromForm] ProductMainRequestDto productMainRequestDto, CancellationToken cancellationToken)
        {
            productMainRequestDto.ProductMaterialRequestDto = productMainRequestDto.ProductMaterialRequestDto.Where(x => !x.IsDeleted).ToList();
            await _unitOfWorkBL.ProductBL.UpdateProduct(Id, productMainRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.Create)]
        public async Task<IActionResult> CreateProductDetail(ProductRequestDto productRequestDto, CancellationToken cancellationToken)
        {
            //await _unitOfWorkBL.ProductBL.CreateProduct(productRequestDto, cancellationToken);
            return RedirectToAction("Index");
            //var insertedProductData = roleData.Where(x => x.Name == roleRequestDto.Name).FirstOrDefault();

            //return RedirectToAction("RolePermission", "Role", new { id = insertedRoleData.Id });
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.Delete)]
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

        private async Task FillRawMaterialDropDowns(CancellationToken cancellationToken)
        {
            var rawMaterialData = await _unitOfWorkBL.RawMaterialBL.GetAll(cancellationToken);
            var unitData = await _unitOfWorkBL.UnitBL.GetAll(cancellationToken);
            var rawMaterialSelectedList = (from x in rawMaterialData
                                           join y in unitData on x.UnitId equals y.LookupValueId
                                           select new ProductMaterialListItem
                                           {
                                               Value = Convert.ToString(x.RawMaterialId),
                                               Text = x.Title,
                                               UnitPrice = x.UnitPrice,
                                               UnitName = y.LookupValueName
                                           }).ToList();
            ViewBag.RawMaterialDropDownData = rawMaterialSelectedList;
        }

        private async Task FillPolishDropDowns(CancellationToken cancellationToken)
        {
            var polishData = await _unitOfWorkBL.PolishBL.GetAll(cancellationToken);
            var unitData = await _unitOfWorkBL.UnitBL.GetAll(cancellationToken);
            var polishSelectedList = (from x in polishData
                                      join y in unitData on x.UnitId equals y.LookupValueId
                                      select new ProductMaterialListItem
                                      {
                                          Value = Convert.ToString(x.PolishId),
                                          Text = x.Title,
                                          UnitPrice = x.UnitPrice,
                                          UnitName = y.LookupValueName
                                      }).ToList();
            ViewBag.PolishDropDownData = polishSelectedList;
        }

        #endregion Private Method
    }
}