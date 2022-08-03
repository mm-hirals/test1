﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.Accessories;
using MidCapERP.Dto.Constants;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Infrastructure.Constants;
using NToastNotify;

namespace MidCapERP.Admin.Controllers
{
    public class AccessoriesController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;
        private readonly IToastNotification _toastNotification;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AccessoriesController(IUnitOfWorkBL unitOfWorkBL, IToastNotification toastNotification, IWebHostEnvironment hostingEnvironment)
        {
            _unitOfWorkBL = unitOfWorkBL;
            _toastNotification = toastNotification;
            _toastNotification = toastNotification;
            _hostingEnvironment = hostingEnvironment;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Accessories.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Accessories.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var categoryData = await _unitOfWorkBL.CategoryBL.GetAll(cancellationToken);
            var categorySelectedList = categoryData.ToList().Select(a =>
                                  new SelectListItem
                                  {
                                      Value = Convert.ToString(a.LookupValueId),
                                      Text = a.LookupValueName
                                  }).ToList();

            var accessoriesTypesData = await _unitOfWorkBL.AccessoriesTypesBL.GetAll(cancellationToken);
            var accessoriesTypesSelectedList = accessoriesTypesData.ToList().Select(a =>
                                  new SelectListItem
                                  {
                                      Value = Convert.ToString(a.AccessoriesTypeId),
                                      Text = a.TypeName
                                  }).ToList();

            var unitData = await _unitOfWorkBL.UnitBL.GetAll(cancellationToken);
            var unitDataSelectedList = unitData.Where(x => x.LookupId == (int)MasterPagesEnum.Unit).ToList().Select(a =>
                                  new SelectListItem
                                  {
                                      Value = Convert.ToString(a.LookupValueId),
                                      Text = a.LookupValueName
                                  }).ToList();

            ViewBag.CategorySelectItemList = categorySelectedList;
            ViewBag.AccessoriesSelectItemList = accessoriesTypesSelectedList;
            ViewBag.unitDataSelectedList = unitDataSelectedList;

            return PartialView("_AccessoriesPartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Accessories.Create)]
        public async Task<IActionResult> Create(AccessoriesRequestDto accessoriesRequestDto, CancellationToken cancellationToken)
        {
            accessoriesRequestDto.ImagePath = StoreFile(accessoriesRequestDto.ImagePath_File, cancellationToken);
            var accessories = await _unitOfWorkBL.AccessoriesBL.CreateAccessories(accessoriesRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Created Successfully!");
            return RedirectToAction("Index");

        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Accessories.View)]
        public async Task<IActionResult> GetAccessories([FromForm] DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.AccessoriesBL.GetFilterAccessoriesData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }


        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Accessories.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            var categoryData = await _unitOfWorkBL.CategoryBL.GetAll(cancellationToken);
            var categorySelectedList = categoryData.ToList().Select(a =>
                                  new SelectListItem
                                  {
                                      Value = Convert.ToString(a.LookupValueId),
                                      Text = a.LookupValueName
                                  }).ToList();

            var accessoriesTypesData = await _unitOfWorkBL.AccessoriesTypesBL.GetAll(cancellationToken);
            var accessoriesTypesSelectedList = accessoriesTypesData.ToList().Select(a =>
                                  new SelectListItem
                                  {
                                      Value = Convert.ToString(a.AccessoriesTypeId),
                                      Text = a.TypeName
                                  }).ToList();


            var unitData = await _unitOfWorkBL.UnitBL.GetAll(cancellationToken);
            var unitDataSelectedList = unitData.Where(x => x.LookupId == (int)MasterPagesEnum.Unit).ToList().Select(a =>
                                  new SelectListItem
                                  {
                                      Value = Convert.ToString(a.LookupValueId),
                                      Text = a.LookupValueName
                                  }).ToList();

            ViewBag.CategorySelectItemList = categorySelectedList;
            ViewBag.AccessoriesSelectItemList = accessoriesTypesSelectedList;
            ViewBag.unitDataSelectedList = unitDataSelectedList;

            var accessories = await _unitOfWorkBL.AccessoriesBL.GetById(Id, cancellationToken);
            return PartialView("_AccessoriesPartial", accessories);
        }


        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Accessories.Update)]
        public async Task<IActionResult> Update(int Id, AccessoriesRequestDto accessoriesRequestDto, CancellationToken cancellationToken)
        {

            if (accessoriesRequestDto.ImagePath_File != null)
                accessoriesRequestDto.ImagePath = StoreFile(accessoriesRequestDto.ImagePath_File, cancellationToken);
            var accessories = await _unitOfWorkBL.AccessoriesBL.UpdateAccessories(Id, accessoriesRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Update Successfully!");
            return RedirectToAction("Index");

        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Accessories.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.AccessoriesBL.DeleteAccessories(Id, cancellationToken);
            return RedirectToAction("Index");
        }

        #region Private Method
        private string StoreFile(IFormFile file, CancellationToken cancellationToken)
        {

            string uploadedImagePath = string.Empty;
            string path = _hostingEnvironment.WebRootPath + @"\Files\Accessories\";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            using (var stream = new FileStream(path + fileName, FileMode.Create))
            {
                file.CopyTo(stream);
                uploadedImagePath = @"\Files\Accessories\" + fileName;
            }
            return uploadedImagePath;

        }
        #endregion
    }
}