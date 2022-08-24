using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.RolePermission;
using MidCapERP.Infrastructure.Constants;
using NToastNotify;

namespace MidCapERP.Admin.Controllers
{
    public class RolePermissionController : Controller
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;
        private readonly IToastNotification _toastNotification;

        public RolePermissionController(IUnitOfWorkBL unitOfWorkBL, IToastNotification toastNotification)
        {
            _unitOfWorkBL = unitOfWorkBL;
            _toastNotification = toastNotification;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.RolePermission.View)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.RolePermission.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            await FillAspNetRoleDropDown(cancellationToken);
            return PartialView("_RolePermissionPartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.RolePermission.Create)]
        public async Task<IActionResult> Create(RolePermissionRequestDto rolePermissionRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.RolePermissionBL.CreateRoleClaim(rolePermissionRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Saved Successfully!");
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