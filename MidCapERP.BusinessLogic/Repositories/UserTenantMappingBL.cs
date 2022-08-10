using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.Dto;
using MidCapERP.Dto.UserTenantMapping;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class UserTenantMappingBL : IUserTenantMappingBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public UserTenantMappingBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<UserTenantMappingResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var userData = await _unitOfWorkDA.UserTenantMappingDA.GetAll(cancellationToken);
            var tenantAllData = await _unitOfWorkDA.TenantDA.GetAll(cancellationToken);
            var userTenantMappingData = (from x in userData
                                         join y in tenantAllData
                                         on new { TenantId = x.TenantId } equals new { TenantId = y.TenantId }
                                         where x.UserId == _currentUser.UserId
                                         select new UserTenantMappingResponseDto()
                                         {
                                             UserTenantMappingId = x.UserTenantMappingId,
                                             UserId = x.UserId,
                                             TenantId = x.TenantId,
                                             TenantName = y.TenantName
                                         }).AsQueryable();
            return _mapper.Map<List<UserTenantMappingResponseDto>>(userTenantMappingData);
        }
    }
}