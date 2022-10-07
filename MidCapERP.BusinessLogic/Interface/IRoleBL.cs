using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Role;
using MidCapERP.Dto.RolePermission;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IRoleBL
    {
        public Task<IList<ApplicationRole>> GetAllRoles(CancellationToken cancellationToken);

        public Task<JsonRepsonse<RoleResponseDto>> GetFilterRoleData(RoleDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<RoleRequestDto> GetRoleNameID(string Id, List<RolePermissionResponseDto> rolePermissionResponseDto, CancellationToken cancellationToken);

        public Task<RoleRequestDto> CreateRole(RoleRequestDto roleRequestDto, CancellationToken cancellationToken);

        public Task<RoleRequestDto> UpdateRole(RoleRequestDto model, CancellationToken cancellationToken);

        public Task<bool> ValidateRole(RoleRequestDto roleRequestDto, CancellationToken cancellationToken);
    }
}