using MidCapERP.Dto.UserTenantMapping;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IUserTenantMappingBL
    {
        public Task<IEnumerable<UserTenantMappingResponseDto>> GetAll(CancellationToken cancellationToken);
    }
}