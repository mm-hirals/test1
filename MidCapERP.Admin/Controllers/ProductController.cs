using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Product;
using MidCapERP.Infrastructure.Constants;

namespace MidCapERP.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public ProductController(IUnitOfWorkBL unitOfWorkBL, IStringLocalizer<BaseController> localizer) : base(localizer)
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
            return View("ProductMain");
        }

        [HttpGet]
        public async Task<IActionResult> CreateProductBasicDetail(int productId, CancellationToken cancellationToken)
        {
            await FillCategoryDropDown(cancellationToken);
            if (productId > 0)
            {
                var getProductInfoById = await _unitOfWorkBL.ProductBL.GetById(productId, cancellationToken);
                return PartialView("_productPartial", getProductInfoById);
            }
            else
                return PartialView("_productPartial");
        }

        [HttpGet]
        public async Task<IActionResult> CreateProductDetail(int productId, CancellationToken cancellationToken)
        {
            if (productId > 0)
            {
                var getProductInfoById = await _unitOfWorkBL.ProductBL.GetById(productId, cancellationToken);
                return PartialView("_productDetailPartial", getProductInfoById);
            }
            else
                return PartialView("_productDetailPartial");
        }

        [HttpGet]
        public async Task<IActionResult> CreateProductImage(int productId, CancellationToken cancellationToken)
        {
            if (productId > 0)
            {
                var getImageById = await _unitOfWorkBL.ProductBL.GetImageByProductId(productId, cancellationToken);
                ProductMainRequestDto prodMainDto = new ProductMainRequestDto();
                prodMainDto.ProductId = productId;
                prodMainDto.ProductImageRequestDto = getImageById;
                return PartialView("_productImagePartial", prodMainDto);
            }
            else
                return PartialView("_productImagePartial");
        }

        [HttpGet]
        public async Task<IActionResult> CreateProductMaterial(int productId, CancellationToken cancellationToken)
        {
            await FillRawMaterialDropDowns(cancellationToken);
            await FillPolishDropDowns(cancellationToken);

            if (productId > 0)
            {
                var productMaterial = await _unitOfWorkBL.ProductBL.GetMaterialByProductId(productId, cancellationToken);
                return PartialView("_productMaterialPartial", productMaterial);
            }
            else
                return PartialView("_productMaterialPartial");
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductMaterial(ProductMainRequestDto productMainRequestDto, CancellationToken cancellationToken)
        {
            if (productMainRequestDto.ProductMaterialRequestDto.Count > 0)
            {
                await _unitOfWorkBL.ProductBL.CreateProductMaterial(productMainRequestDto, cancellationToken);
                return RedirectToAction("CreateProductMaterial", "Product", new { productId = productMainRequestDto.ProductId });
            }

            throw new Exception("Please Add Material");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            ProductMainRequestDto prdMainDto = new ProductMainRequestDto();
            prdMainDto.ProductId = Id;
            return View("ProductMain", prdMainDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductBasicDetail(int productId, ProductRequestDto model, CancellationToken cancellationToken)
        {
            await FillCategoryDropDown(cancellationToken);
            if (productId > 0)
            {
                var updateProduct = await _unitOfWorkBL.ProductBL.UpdateProduct(model, cancellationToken);
                return RedirectToAction("Update", "Product", new { Id = updateProduct.ProductId });
            }
            else
            {
                var createProduct = await _unitOfWorkBL.ProductBL.CreateProduct(model, cancellationToken);
                return RedirectToAction("Update", "Product", new { Id = createProduct.ProductId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductDetail(ProductRequestDto model, CancellationToken cancellationToken)
        {
            if (model.ProductId > 0)
            {
                await _unitOfWorkBL.ProductBL.UpdateProductDetail(model, cancellationToken);
                return RedirectToAction("CreateProductDetail", "Product", new { productId = model.ProductId });
            }

            throw new Exception("Product Not Found!");
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductImage(Int64 productId, ProductMainRequestDto model, CancellationToken cancellationToken)
        {
            if (productId > 0)
            {
                await _unitOfWorkBL.ProductBL.CreateProductImages(model, cancellationToken);
                return PartialView("ProductMain");
            }
            else
            {
                throw new Exception("Please Provide ProductId.");
            }
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.ProductBL.DeleteProduct(Id, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.Create)]
        public async Task<JsonResult> UpdateProductStatus([FromForm] ProductMainRequestDto productMainRequestDto, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWorkBL.ProductBL.UpdateProductStatus(productMainRequestDto, cancellationToken);
                return Json("success");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Product.View)]
        public async Task<IActionResult> Detail(int Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.ProductBL.GetById(Id, cancellationToken);
            return View("Detail", data);
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
                                           orderby x.Title
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
                                      orderby x.Title
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