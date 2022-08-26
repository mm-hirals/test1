using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Role;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IRoleBL
    {
        public Task<IList<ApplicationRole>> GetAllRoles(CancellationToken cancellationToken);

        public Task<JsonRepsonse<RoleResponseDto>> GetFilterRoleData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<RoleRequestDto> CreateRole(RoleRequestDto roleRequestDto, CancellationToken cancellationToken);

        public Task<RoleRequestDto> UpdateRole(RoleRequestDto model, CancellationToken cancellationToken);
    }
}