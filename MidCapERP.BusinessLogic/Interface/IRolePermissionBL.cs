using MidCapERP.Dto.RolePermission;
using System.Security.Claims;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IRolePermissionBL
    {
        public Task<IList<Claim>> GetAllRoleClaimsByRole(string applicationRole, CancellationToken cancellationToken);

        public Task<List<RolePermissionRequestDto>> GetRolePermissions(string Id, List<string> allPermissions, CancellationToken cancellationToken);

        public Task CreateRoleClaim(RolePermissionRequestDto rolePermissionRequestDto, CancellationToken cancellationToken);

        public Task DeleteRoleClaim(RolePermissionRequestDto model, CancellationToken cancellationToken);
    }
}