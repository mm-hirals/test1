using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Role;
using MidCapERP.Dto.RolePermission;
using MidCapERP.Infrastructure.Constants;

namespace MidCapERP.Admin.Controllers
{
    public class RoleController : Controller
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public RoleController(IUnitOfWorkBL unitOfWorkBL)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Role.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Role.View)]
        public async Task<IActionResult> GetRoleData([FromForm] DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.RoleBL.GetFilterRoleData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Role.Create)]
        public async Task<IActionResult> CreateRole(RoleRequestDto roleRequestDto, CancellationToken cancellationToken)
        {
            RoleRequestDto insertedRole = new RoleRequestDto();

            insertedRole = await _unitOfWorkBL.RoleBL.CreateRole(roleRequestDto, cancellationToken);
            var roleData = await _unitOfWorkBL.RoleBL.GetAllRoles(cancellationToken);
            var insertedRoleData = roleData.Where(x => x.Name == roleRequestDto.Name).FirstOrDefault();

            return RedirectToAction("RolePermission", "Role", new { id = insertedRoleData.Id });
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Role.Update)]
        public async Task<IActionResult> UpdateRole(RoleRequestDto roleRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.RoleBL.UpdateRole(roleRequestDto, cancellationToken);
            return RedirectToAction("Index", "Role");
        }

        [Authorize(ApplicationIdentityConstants.Permissions.RolePermission.View)]
        public async Task<IActionResult> RolePermission(string Id, CancellationToken cancellationToken)
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
        public async Task<IActionResult> CreateRolePermission([FromForm] RolePermissionRequestDto rolePermissionRequestDto, CancellationToken cancellationToken)
        {
            if (rolePermissionRequestDto.IsChecked == "true")
                await _unitOfWorkBL.RolePermissionBL.CreateRoleClaim(rolePermissionRequestDto, cancellationToken);
            else
                await _unitOfWorkBL.RolePermissionBL.DeleteRoleClaim(rolePermissionRequestDto, cancellationToken);
            return RedirectToAction("Index", "Role");
        }

        // Role Name Validation
        public async Task<bool> DuplicateRoleName(RoleRequestDto roleRequestDto, CancellationToken cancellationToken)
        {
            return await _unitOfWorkBL.RoleBL.ValidateRole(roleRequestDto, cancellationToken);
        }
    }
}