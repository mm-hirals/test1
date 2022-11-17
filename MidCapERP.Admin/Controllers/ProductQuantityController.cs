using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto.ProductQuantities;

namespace MidCapERP.Admin.Controllers
{
    public class ProductQuantityController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public ProductQuantityController(IUnitOfWorkBL unitOfWorkBL, IStringLocalizer<BaseController> localizer) : base(localizer)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.PortalProduct.QuantityView)]
        public IActionResult Index(CancellationToken cancellationToken)
        {
            FillCategoryDropDown(cancellationToken);
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.PortalProduct.QuantityView)]
        public async Task<IActionResult> GetProductQuantitiesData([FromForm] ProductQuantitiesDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.ProductQuantitiesBL.GetFilterProductQuantitiesData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.PortalProduct.QuantityCreate)]
        public async Task<IActionResult> Update(long Id, CancellationToken cancellationToken)
        {
            var productQuantity = await _unitOfWorkBL.ProductQuantitiesBL.GetById(Id, cancellationToken);
            return PartialView("_ProductQuantityPartial", productQuantity);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.PortalProduct.QuantityCreate)]
        public async Task<IActionResult> Update(long Id, ProductQuantitiesRequestDto productQuantitiesRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.ProductQuantitiesBL.UpdateProductQuantities(Id, productQuantitiesRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        #region Private Method

        private async void FillCategoryDropDown(CancellationToken cancellationToken)
        {
            var categoryTypeData = await _unitOfWorkBL.CategoryBL.GetAll(cancellationToken);
            var categorySelectedList = categoryTypeData.Select(a =>
                                 new SelectListItem
                                 {
                                     Value = Convert.ToString(a.CategoryId),
                                     Text = a.CategoryName
                                 }).ToList();
            ViewBag.CategorySelectedList = categorySelectedList;
        }

        #endregion Private Method
    }
}