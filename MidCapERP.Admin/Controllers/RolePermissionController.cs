using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.RolePermission;
using MidCapERP.Infrastructure.Constants;

namespace MidCapERP.Admin.Controllers
{
    public class RolePermissionController : Controller
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public RolePermissionController(IUnitOfWorkBL unitOfWorkBL)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.RolePermission.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var allPermissions = ApplicationIdentityConstants.Permissions.GetAllPermissions();
            var rolePermissionRequestDto = await _unitOfWorkBL.RolePermissionBL.GetRolePermissions(allPermissions, cancellationToken);

            ViewBag.RoleName = "Administrator";
            return View(rolePermissionRequestDto);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.RolePermission.Create)]
        public async Task<IActionResult> Create([FromForm] RolePermissionRequestDto rolePermissionRequestDto, CancellationToken cancellationToken)
        {
            if (rolePermissionRequestDto.IsChecked == "true")
                await _unitOfWorkBL.RolePermissionBL.CreateRoleClaim(rolePermissionRequestDto, cancellationToken);
            else
                await _unitOfWorkBL.RolePermissionBL.DeleteRoleClaim(rolePermissionRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }
    }
}