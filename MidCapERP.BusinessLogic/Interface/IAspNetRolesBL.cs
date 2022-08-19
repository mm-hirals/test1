using MidCapERP.Dto.AspNetRole;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IAspNetRolesBL
    {
        public Task<IEnumerable<AspNetRoleResponseDto>> GetAll(CancellationToken cancellationToken);
    }
}