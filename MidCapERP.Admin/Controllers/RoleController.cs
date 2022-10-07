using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Role;
using MidCapERP.Dto.RolePermission;

namespace MidCapERP.Admin.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public RoleController(IUnitOfWorkBL unitOfWorkBL, IStringLocalizer<BaseController> localizer) : base(localizer)
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
        public async Task<IActionResult> GetRoleData([FromForm] RoleDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.RoleBL.GetFilterRoleData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Role.Create)]
        public async Task<IActionResult> CreateRole(RoleRequestDto roleRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.RoleBL.CreateRole(roleRequestDto, cancellationToken);
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
                var rolePermissionResponseDto = await _unitOfWorkBL.RolePermissionBL.GetRolePermissions(Id, allPermissions, cancellationToken);

                mRoleRequestDto = await _unitOfWorkBL.RoleBL.GetRoleNameID(Id, rolePermissionResponseDto, cancellationToken);
                return View(mRoleRequestDto);
            }
            else
                return View(mRoleRequestDto);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.RolePermission.Create)]
        public async Task<JsonResult> CreateRolePermission([FromForm] RolePermissionRequestDto rolePermissionResponseDto, CancellationToken cancellationToken)
        {
            try
            {
                if (rolePermissionResponseDto.IsChecked == "true")
                    await _unitOfWorkBL.RolePermissionBL.CreateRoleClaim(rolePermissionResponseDto, cancellationToken);
                else
                    await _unitOfWorkBL.RolePermissionBL.DeleteRoleClaim(rolePermissionResponseDto, cancellationToken);
                return Json("success");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        // Role Name Validation
        public async Task<bool> DuplicateRoleName(RoleRequestDto roleRequestDto, CancellationToken cancellationToken)
        {
            return await _unitOfWorkBL.RoleBL.ValidateRole(roleRequestDto, cancellationToken);
        }
    }
}