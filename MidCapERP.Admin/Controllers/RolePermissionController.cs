using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.Role;
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
        public async Task<IActionResult> Index(string Id, CancellationToken cancellationToken)
        {
            RoleRequestDto mRoleRequestDto = new RoleRequestDto();

            if (Id != null)
            {
                var allPermissions = ApplicationIdentityConstants.Permissions.GetAllPermissions();
                var rolePermissionRequestDto = await _unitOfWorkBL.RolePermissionBL.GetRolePermissions(Id, allPermissions, cancellationToken);

                var roleData = await _unitOfWorkBL.RoleBL.GetAllRoles(cancellationToken);
                var roleName = roleData.Where(x => x.Id == Id).Select(n => n.Name).FirstOrDefault();
                mRoleRequestDto.Id = Id;
                mRoleRequestDto.Name = roleName;
                mRoleRequestDto.ModulePermissionList.AddRange(rolePermissionRequestDto);
                return View(mRoleRequestDto);
            }
            else
                return View(mRoleRequestDto);
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