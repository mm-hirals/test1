﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Role;
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
        [Authorize(ApplicationIdentityConstants.Permissions.RolePermission.Create)]
        public async Task<IActionResult> CreateRole(RoleRequestDto roleRequestDto, CancellationToken cancellationToken)
        {
            RoleRequestDto insertedRole = new RoleRequestDto();

            insertedRole = await _unitOfWorkBL.RoleBL.CreateRole(roleRequestDto, cancellationToken);
            var roleData = await _unitOfWorkBL.RoleBL.GetAllRoles(cancellationToken);
            var insertedRoleData = roleData.Where(x => x.Name == roleRequestDto.Name).FirstOrDefault();

            return RedirectToAction("Index", "RolePermission", new { id = insertedRoleData.Id });
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.RolePermission.Update)]
        public async Task<IActionResult> Update(RoleRequestDto roleRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.RoleBL.UpdateRole(roleRequestDto, cancellationToken);
            return RedirectToAction("Index", "Role");
        }

        public bool DuplicateRoleName(RoleRequestDto roleRequestDto, CancellationToken cancellationToken)
        {
            var existData = _unitOfWorkBL.RoleBL.GetAllRoles(cancellationToken).Result.Where(x => x.Name == roleRequestDto.Name).FirstOrDefault();
            if (existData == null)
                return true;
            else
                return false;
        }
    }
}