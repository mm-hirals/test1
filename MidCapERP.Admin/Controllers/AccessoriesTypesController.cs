using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.AccessoriesTypes;
using MidCapERP.Dto.Constants;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Infrastructure.Constants;
using NToastNotify;

namespace MidCapERP.Admin.Controllers
{
    public class AccessoriesTypesController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;
        private readonly IToastNotification _toastNotification;

        public AccessoriesTypesController(IUnitOfWorkBL unitOfWorkBL, IToastNotification toastNotification)
        {
            _unitOfWorkBL = unitOfWorkBL;
            _toastNotification = toastNotification;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.AccessoriesTypes.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }


        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.AccessoriesTypes.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var categoryData = await _unitOfWorkBL.CategoryBL.GetAll(cancellationToken);
            var data = categoryData.Where(x => x.LookupId == (int)MasterPagesEnum.Category).ToList().Select(a =>
                                  new SelectListItem
                                  {
                                      Value = Convert.ToString(a.LookupId),
                                      Text = a.LookupValueName
                                  }).ToList();

            ViewBag.CategorySelectItemList = data;

            return PartialView("_AccessoriesTypesPartial");
        }


        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.AccessoriesTypes.Create)]
        public async Task<IActionResult> Create(AccessoriesTypesRequestDto accessoriesTypesRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.AccessoriesTypesBL.CreateAccessoriesTypes(accessoriesTypesRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Saved Successfully!");
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.AccessoriesTypes.View)]
        public async Task<IActionResult> GetAccessoriesTypesData([FromForm] DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.AccessoriesTypesBL.GetFilterAccessoriesTypesData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }


        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.AccessoriesTypes.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            var categoryData = await _unitOfWorkBL.CategoryBL.GetAll(cancellationToken);
            var data = categoryData.Where(x => x.LookupId == (int)MasterPagesEnum.Category).ToList().Select(a =>
                               new SelectListItem
                               {
                                   Value = Convert.ToString(a.LookupId),
                                   Text = a.LookupValueName
                               }).ToList();

            ViewBag.CategorySelectItemList = data;
            var accessoriesTypes = await _unitOfWorkBL.AccessoriesTypesBL.GetById(Id, cancellationToken);
            return PartialView("_AccessoriesTypesPartial", accessoriesTypes);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.AccessoriesTypes.Update)]
        public async Task<IActionResult> Update(int Id, AccessoriesTypesRequestDto accessoriesTypesRequestDto, CancellationToken cancellationToken)
        {

            await _unitOfWorkBL.AccessoriesTypesBL.UpdateAccessoriesTypes(Id, accessoriesTypesRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data update Successfully!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.AccessoriesTypes.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.AccessoriesTypesBL.DeleteAccessoriesTypes(Id, cancellationToken);
            return RedirectToAction("Index");
        }
    }
}