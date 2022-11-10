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
            //List<ModuleResponseDto> ModuleResponseDto = new List<ModuleResponseDto>();
            foreach (var item in allPermissions)
            {
                RolePermissionResponseDto moduleResponseDtoChild = RolePermissionResponseDto.FirstOrDefault(p => p.ApplicationType == item.Split(".")[1]);

                if (moduleResponseDtoChild == null)
                {
                    moduleResponseDtoChild = new RolePermissionResponseDto();
                    moduleResponseDtoChild.ApplicationType = item.Split(".")[1];
                    moduleResponseDtoChild.RolePermissionList = new List<PermissiongResponseDto>();
                    RolePermissionResponseDto.Add(moduleResponseDtoChild);
                }

                PermissiongResponseDto permissionResponseDto = new PermissiongResponseDto();
                permissionResponseDto.Id = item.Split(".")[2] + item.Split(".")[3];
                permissionResponseDto.Module = item.Split(".")[2];
                permissionResponseDto.PermissionType = item.Split(".")[3];
                permissionResponseDto.Permission = item;

                if (allClaimsByRole.Any(x => x.Value == item))
                {
                    permissionResponseDto.IsChecked = "checked";
                }

                moduleResponseDtoChild.RolePermissionList.Add(permissionResponseDto);

                //RolePermissionResponseDto.Add(ModuleResponseDto);
                ////RolePermissionResponseDto permissionRequestDto = new RolePermissionResponseDto();
                ////permissionRequestDto.ApplicationType = item.Split(".")[1];

                ////ModuleResponseDto moduleResponseDto = new ModuleResponseDto();
                ////moduleResponseDto.Module = item.Split(".")[2];

                ////PermissiongResponseDto permissionResponseDto = new PermissiongResponseDto();
                ////permissionResponseDto.Id = item.Split(".")[2] + item.Split(".")[3];
                ////permissionResponseDto.PermissionType = item.Split(".")[3];
                ////permissionResponseDto.Permission = item;
                ////if (allClaimsByRole.Any(x => x.Value == item))
                ////{
                ////    permissionResponseDto.IsChecked = "checked";
                ////}
                ////moduleResponseDto.ModulePermissionList.Add(permissionResponseDto);

                ////permissionRequestDto.ModuleResponseList.Add(moduleResponseDto);
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