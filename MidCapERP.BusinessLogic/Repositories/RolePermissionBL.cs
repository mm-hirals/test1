using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.RolePermission;
using System.Security.Claims;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class RolePermissionBL : IRolePermissionBL
    {
        private readonly IUnitOfWorkDA _unitOfWorkDA;
        private readonly CurrentUser _currentUser;
        private readonly IMapper _mapper;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RolePermissionBL(IUnitOfWorkDA unitOfWorkDA, CurrentUser currentUser, IMapper mapper, RoleManager<ApplicationRole> roleManager)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _currentUser = currentUser;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<IList<Claim>> GetAllRoleClaimsByRole(string applicationRole, CancellationToken cancellationToken)
        {
            var roleByName = await _roleManager.FindByNameAsync(applicationRole);
            var roleClaims = await _unitOfWorkDA.RolePermissionDA.GetRoleClaimsByRole(roleByName, cancellationToken);
            return roleClaims;
        }

        public async Task<List<RolePermissionRequestDto>> GetRolePermissions(List<string> allPermissions, CancellationToken cancellationToken)
        {
            var allClaimsByRole = await GetAllRoleClaimsByRole("Administrator", cancellationToken);
            List<RolePermissionRequestDto> listOfRolePermission = new List<RolePermissionRequestDto>();

            foreach (var item in allPermissions)
            {
                RolePermissionRequestDto permissionRequestDto = new RolePermissionRequestDto();
                permissionRequestDto.Id = item.Split(".")[1] + item.Split(".")[2];
                permissionRequestDto.Module = item.Split(".")[1];
                permissionRequestDto.PermissionType = item.Split(".")[2];
                permissionRequestDto.Permission = item;

                if (allClaimsByRole.Any(x => x.Value == item))
                {
                    permissionRequestDto.IsChecked = "checked";
                }

                listOfRolePermission.Add(permissionRequestDto);
            }

            List<RolePermissionRequestDto> rolePermissionRequestDto = new List<RolePermissionRequestDto>();
            foreach (string listModule in listOfRolePermission.Select(p => p.Module).Distinct())
            {
                rolePermissionRequestDto.Add(new RolePermissionRequestDto() { Module = listModule, ModulePermissionList = listOfRolePermission.Where(x => x.Module == listModule).ToList() });
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