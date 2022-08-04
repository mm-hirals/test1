using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.RawMaterial;
using MidCapERP.Infrastructure.Constants;
using NToastNotify;

namespace MidCapERP.Admin.Controllers
{
    public class RawMaterialController : Controller
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;
        private readonly IToastNotification _toastNotification;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public RawMaterialController(IUnitOfWorkBL unitOfWorkBL, IToastNotification toastNotification, IWebHostEnvironment hostingEnvironment)
        {
            _unitOfWorkBL = unitOfWorkBL;
            _toastNotification = toastNotification;
            _hostingEnvironment = hostingEnvironment;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.RawMaterial.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.RawMaterial.View)]
        public async Task<IActionResult> GetRawMaterialData([FromForm] DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.RawMaterialBL.GetFilterRawMaterialData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.RawMaterial.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            await FillUnitNameDropDown(cancellationToken);
            return PartialView("_RawMaterialPartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.RawMaterial.Create)]
        public async Task<IActionResult> Create(RawMaterialRequestDto rawMaterialRequestDto, CancellationToken cancellationToken)
        {
            rawMaterialRequestDto.ImagePath = StoreFile(rawMaterialRequestDto.ImagePath_File, cancellationToken);
            await _unitOfWorkBL.RawMaterialBL.CreateRawMaterial(rawMaterialRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Saved Successfully!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.RawMaterial.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            await FillUnitNameDropDown(cancellationToken);
            var lookups = await _unitOfWorkBL.RawMaterialBL.GetById(Id, cancellationToken);
            lookups.ImagePath = lookups.ImagePath;
            return PartialView("_RawMaterialPartial", lookups);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.RawMaterial.Update)]
        public async Task<IActionResult> Update(int Id, RawMaterialRequestDto rawMaterialRequestDto, CancellationToken cancellationToken)
        {
            if (rawMaterialRequestDto.ImagePath_File != null)
                rawMaterialRequestDto.ImagePath = StoreFile(rawMaterialRequestDto.ImagePath_File, cancellationToken);
            await _unitOfWorkBL.RawMaterialBL.UpdateRawMaterial(Id, rawMaterialRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Saved Successfully!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.RawMaterial.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.RawMaterialBL.DeleteRawMaterial(Id, cancellationToken);
            return RedirectToAction("Index");
        }

        #region Private Method

        private async Task FillUnitNameDropDown(CancellationToken cancellationToken)
        {
            var unitData = await _unitOfWorkBL.UnitBL.GetAll(cancellationToken);
            var data = unitData.Select(a => new SelectListItem
            {
                Value = Convert.ToString(a.LookupValueId),
                Text = a.LookupValueName
            }).ToList();
            ViewBag.UnitSelectItemList = data;
        }

        private string StoreFile(IFormFile file, CancellationToken cancellationToken)
        {
            string uploadedImagePath = string.Empty;
            string path = _hostingEnvironment.WebRootPath + @"\Files\RawMaterials\";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            using (var stream = new FileStream(path + fileName, FileMode.Create))
            {
                file.CopyTo(stream);
                uploadedImagePath = @"\Files\RawMaterials\" + fileName;
            }
            return uploadedImagePath;
        }

        #endregion Private Method
    }
}