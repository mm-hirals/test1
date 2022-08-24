using MidCapERP.Dto.RolePermission;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IRolePermissionBL
    {
        public Task CreateRoleClaim(RolePermissionRequestDto rolePermissionRequestDto, CancellationToken cancellationToken);
    }
}