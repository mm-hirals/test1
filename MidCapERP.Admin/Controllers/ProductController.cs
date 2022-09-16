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
                var getProductInfoById = await _unitOfWorkBL.ProductBL.GetById(productId, cancellationToken);
                ProductMainRequestDto productMain = new ProductMainRequestDto();
                productMain.ProductId = productId;
                productMain.CostPrice = getProductInfoById.CostPrice;
                productMain.WholesalerPrice = getProductInfoById.WholesalerPrice;
                productMain.RetailerPrice = getProductInfoById.RetailerPrice;
                productMain.ProductMaterialRequestDto = productMaterial;
                return PartialView("_productMaterialPartial", productMain);
            }
            else
                return PartialView("_productMaterialPartial");
        }

        [HttpPost]
        public async Task<IActionResult> SaveProductMaterial(ProductMainRequestDto productMainRequestDto, CancellationToken cancellationToken)
        {
            if (productMainRequestDto.ProductMaterialRequestDto.Count > 0)
            {
                await _unitOfWorkBL.ProductBL.CreateProductMaterial(productMainRequestDto.ProductId, productMainRequestDto.ProductMaterialRequestDto, cancellationToken);
                await _unitOfWorkBL.ProductBL.UpdateProductCost(productMainRequestDto.ProductId, productMainRequestDto, cancellationToken);

                return RedirectToAction("CreateProductMaterial", "Product", new { productId = productMainRequestDto.ProductId });
            }

            return null;
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
                var updateProduct = await _unitOfWorkBL.ProductBL.UpdateProduct(productId, model, cancellationToken);
                return RedirectToAction("Update", "Product", new { Id = updateProduct.ProductId });
            }
            else
            {
                var createProduct = await _unitOfWorkBL.ProductBL.CreateProduct(model, cancellationToken);
                var getProductById = await _unitOfWorkBL.ProductBL.GetById(createProduct.ProductId, cancellationToken);
                return RedirectToAction("Update", "Product", new { Id = getProductById.ProductId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductDetail(int productId, ProductRequestDto model, CancellationToken cancellationToken)
        {
            if (productId > 0)
            {
                var updateProductDetail = await _unitOfWorkBL.ProductBL.UpdateProductDetail(productId, model, cancellationToken);
                return PartialView("ProductMain", updateProductDetail);
            }
            else
            {
                return PartialView("ProductMain", model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductImage(int productId, ProductMainRequestDto model, CancellationToken cancellationToken)
        {
            if (productId > 0)
            {
                var saveImage = await _unitOfWorkBL.ProductBL.SaveImages(productId, model, cancellationToken);
                return PartialView("ProductMain");
            }
            else
            {
                return PartialView("ProductMain", model);
            }
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