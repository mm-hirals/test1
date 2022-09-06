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

        public async Task<List<RolePermissionResponseDto>> GetRolePermissions(string Id, List<string> allPermissions, CancellationToken cancellationToken)
        {
            var allClaimsByRole = await GetAllRoleClaimsByRole(Id, cancellationToken);
            List<RolePermissionResponseDto> RolePermissionResponseDto = new List<RolePermissionResponseDto>();

            foreach (var item in allPermissions)
            {
                RolePermissionResponseDto permissionRequestDto = new RolePermissionResponseDto();
                permissionRequestDto.Module = item.Split(".")[1];

                var rolePermission = RolePermissionResponseDto.FirstOrDefault(p => p.Module == permissionRequestDto.Module);
                if (rolePermission == null)
                {
                    permissionRequestDto.ModulePermissionList = new List<PermissiongResponseDto>();
                    RolePermissionResponseDto.Add(permissionRequestDto);
                    rolePermission = RolePermissionResponseDto.FirstOrDefault(p => p.Module == permissionRequestDto.Module);
                }

                PermissiongResponseDto permissionResponseDto = new PermissiongResponseDto();
                permissionResponseDto.Id = item.Split(".")[1] + item.Split(".")[2];
                permissionResponseDto.PermissionType = item.Split(".")[2];
                permissionResponseDto.Permission = item;
                if (allClaimsByRole.Any(x => x.Value == item))
                {
                    permissionResponseDto.IsChecked = "checked";
                }
                rolePermission.ModulePermissionList.Add(permissionResponseDto);
            }

            return RolePermissionResponseDto;
        }

        public async Task CreateRoleClaim(RolePermissionRequestDto rolePermissionRequestDto, CancellationToken cancellationToken)
        {
            var applicationRole = await _roleManager.FindByIdAsync(rolePermissionRequestDto.RoleId);
            await _unitOfWorkDA.RolePermissionDA.CreateRolePermission(applicationRole, rolePermissionRequestDto.Permission, cancellationToken);
        }

        public async Task DeleteRoleClaim(RolePermissionRequestDto model, CancellationToken cancellationToken)
        {
            var applicationRole = await _roleManager.FindByIdAsync(model.RoleId);
            await _unitOfWorkDA.RolePermissionDA.DeleteRolePermission(applicationRole, model.Permission, cancellationToken);
        }
    }
}