using MidCapERP.Dto.UserTenantMapping;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IUserTenantMappingBL
    {
        public Task<IEnumerable<UserTenantMappingResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<UserTenantMappingRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<UserTenantMappingRequestDto> CreateUserTenant(UserTenantMappingRequestDto model, CancellationToken cancellationToken);

        public Task<UserTenantMappingRequestDto> UpdateUserTenant(int Id, UserTenantMappingRequestDto model, CancellationToken cancellationToken);
    }
}