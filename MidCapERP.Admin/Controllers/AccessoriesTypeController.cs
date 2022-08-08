using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.AccessoriesType;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Infrastructure.Constants;
using NToastNotify;

namespace MidCapERP.Admin.Controllers
{
    public class AccessoriesTypeController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;
        private readonly IToastNotification _toastNotification;

        public AccessoriesTypeController(IUnitOfWorkBL unitOfWorkBL, IToastNotification toastNotification)
        {
            _unitOfWorkBL = unitOfWorkBL;
            _toastNotification = toastNotification;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.AccessoriesType.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.AccessoriesType.View)]
        public async Task<IActionResult> GetAccessoriesTypeData([FromForm] DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.AccessoriesTypeBL.GetFilterAccessoriesTypeData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.AccessoriesType.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            await FillCategoryDropDown(cancellationToken);
            return PartialView("_AccessoriesTypePartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.AccessoriesType.Create)]
        public async Task<IActionResult> Create(AccessoriesTypeRequestDto accessoriesTypesRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.AccessoriesTypeBL.CreateAccessoriesType(accessoriesTypesRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Saved Successfully!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.AccessoriesType.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            await FillCategoryDropDown(cancellationToken);
            var accessoriesTypes = await _unitOfWorkBL.AccessoriesTypeBL.GetById(Id, cancellationToken);
            return PartialView("_AccessoriesTypePartial", accessoriesTypes);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.AccessoriesType.Update)]
        public async Task<IActionResult> Update(int Id, AccessoriesTypeRequestDto accessoriesTypesRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.AccessoriesTypeBL.UpdateAccessoriesType(Id, accessoriesTypesRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data update Successfully!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.AccessoriesType.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.AccessoriesTypeBL.DeleteAccessoriesType(Id, cancellationToken);
            return RedirectToAction("Index");
        }

        #region Private Methods

        private async Task FillCategoryDropDown(CancellationToken cancellationToken)
        {
            var categoryData = await _unitOfWorkBL.CategoryBL.GetAll(cancellationToken);
            var categorySelectedList = categoryData.Select(a =>
                                 new SelectListItem
                                 {
                                     Value = Convert.ToString(a.LookupValueId),
                                     Text = a.LookupValueName
                                 }).ToList();
            ViewBag.CategorySelectItemList = categorySelectedList;
        }

        #endregion Private Methods
    }
}