using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.User;
using NToastNotify;

namespace MidCapERP.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;
        private readonly IToastNotification _toastNotification;

        public UserController(IUnitOfWorkBL unitOfWorkBL, IToastNotification toastNotification, IStringLocalizer<BaseController> localizer) : base(localizer)
        {
            _unitOfWorkBL = unitOfWorkBL;
            _toastNotification = toastNotification;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Users.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Users.View)]
        public async Task<IActionResult> GetUserData([FromForm] DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.UserBL.GetFilterUserData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Users.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            await FillAspNetRoleDropDown(cancellationToken);
            return PartialView("_UserPartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Users.Create)]
        public async Task<IActionResult> Create(UserRequestDto userRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.UserBL.CreateUser(userRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Saved Successfully!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Users.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            await FillAspNetRoleDropDown(cancellationToken);
            var user = await _unitOfWorkBL.UserBL.GetById(Id, cancellationToken);
            return PartialView("_UserPartial", user);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Users.Update)]
        public async Task<IActionResult> Update(int Id, UserRequestDto userRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.UserBL.UpdateUser(Id, userRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Update Successfully!");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Users.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.UserBL.DeleteUser(Id, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Deleted Successfully!");
            return RedirectToAction("Index");
        }

        #region Private Method

        private async Task FillAspNetRoleDropDown(CancellationToken cancellationToken)
        {
            var aspNetRoleData = await _unitOfWorkBL.UserBL.GetAllRoles(cancellationToken);
            var aspNetRoleDataSelectedList = aspNetRoleData.Select(x => new SelectListItem
            {
                Value = x.NormalizedName,
                Text = x.NormalizedName
            }).ToList();
            ViewBag.AspNetRoleSelectItemList = aspNetRoleDataSelectedList;
        }

        #endregion Private Method
    }
}