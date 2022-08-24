using Microsoft.AspNetCore.Identity;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.RolePermission;

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

        public async Task CreateRoleClaim(RolePermissionRequestDto model, CancellationToken cancellationToken)
        {
            var applicationRole = await _roleManager.FindByNameAsync(model.AspNetRole);
            await _unitOfWorkDA.RolePermissionDA.CreateRolePermission(applicationRole, model.claimValue, cancellationToken);
        }
    }
}