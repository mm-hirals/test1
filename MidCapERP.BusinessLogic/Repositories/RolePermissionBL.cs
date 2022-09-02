using Microsoft.AspNetCore.Identity;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.RolePermission;
using System.Security.Claims;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class RolePermissionBL : IRolePermissionBL
    {
        private readonly IUnitOfWorkDA _unitOfWorkDA;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RolePermissionBL(IUnitOfWorkDA unitOfWorkDA, RoleManager<ApplicationRole> roleManager)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _roleManager = roleManager;
        }

        public async Task<IList<Claim>> GetAllRoleClaimsByRole(string applicationRole, CancellationToken cancellationToken)
        {
            var roleByName = await _roleManager.FindByIdAsync(applicationRole);
            var roleClaims = await _unitOfWorkDA.RolePermissionDA.GetRoleClaimsByRole(roleByName, cancellationToken);
            return roleClaims;
        }

        public async Task<List<RolePermissionRequestDto>> GetRolePermissions(string Id, List<string> allPermissions, CancellationToken cancellationToken)
        {
            var allClaimsByRole = await GetAllRoleClaimsByRole(Id, cancellationToken);
            List<RolePermissionRequestDto> rolePermissionRequestDto = new List<RolePermissionRequestDto>();

            foreach (var item in allPermissions)
            {
                RolePermissionRequestDto permissionRequestDto = new RolePermissionRequestDto();
                permissionRequestDto.Module = item.Split(".")[1];

                var rolePermission = rolePermissionRequestDto.FirstOrDefault(p => p.Module == permissionRequestDto.Module);
                if (rolePermission == null)
                {
                    permissionRequestDto.ModulePermissionList = new List<RolePermissionRequestDto>();
                    rolePermissionRequestDto.Add(permissionRequestDto);
                    rolePermission = rolePermissionRequestDto.FirstOrDefault(p => p.Module == permissionRequestDto.Module);
                }

                permissionRequestDto.Id = item.Split(".")[1] + item.Split(".")[2];
                permissionRequestDto.PermissionType = item.Split(".")[2];
                permissionRequestDto.Permission = item;
                if (allClaimsByRole.Any(x => x.Value == item))
                {
                    permissionRequestDto.IsChecked = "checked";
                }
                rolePermission.ModulePermissionList.Add(permissionRequestDto);
            }

            return rolePermissionRequestDto;
        }

        public async Task CreateRoleClaim(RolePermissionRequestDto model, CancellationToken cancellationToken)
        {
            var applicationRole = await _roleManager.FindByNameAsync(model.AspNetRoleName);
            await _unitOfWorkDA.RolePermissionDA.CreateRolePermission(applicationRole, model.Permission, cancellationToken);
        }

        public async Task DeleteRoleClaim(RolePermissionRequestDto model, CancellationToken cancellationToken)
        {
            var applicationRole = await _roleManager.FindByNameAsync(model.AspNetRoleName);
            await _unitOfWorkDA.RolePermissionDA.DeleteRolePermission(applicationRole, model.Permission, cancellationToken);
        }
    }
}