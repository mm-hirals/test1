using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
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

        public async Task<UserTenantMappingRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var woodData = await UserTenantGetById(Id, cancellationToken);
            return _mapper.Map<UserTenantMappingRequestDto>(woodData);
        }

        public async Task<UserTenantMappingRequestDto> CreateUserTenant(UserTenantMappingRequestDto model, CancellationToken cancellationToken)
        {
            var userTenantToInsert = _mapper.Map<UserTenantMapping>(model);
            userTenantToInsert.IsDeleted = false;
            userTenantToInsert.CreatedBy = _currentUser.UserId;
            userTenantToInsert.CreatedDate = DateTime.Now;
            userTenantToInsert.CreatedUTCDate = DateTime.UtcNow;
            var userTenantData = await _unitOfWorkDA.UserTenantMappingDA.CreateUserTenant(userTenantToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<UserTenantMappingRequestDto>(userTenantData);
            return _mappedUser;
        }

        public async Task<UserTenantMappingRequestDto> UpdateUserTenant(int Id, UserTenantMappingRequestDto model, CancellationToken cancellationToken)
        {
            var userTenantToInsert = _mapper.Map<UserTenantMapping>(model);
            userTenantToInsert.IsDeleted = false;
            userTenantToInsert.CreatedBy = _currentUser.UserId;
            userTenantToInsert.CreatedDate = DateTime.Now;
            userTenantToInsert.CreatedUTCDate = DateTime.UtcNow;
            var userTenantData = await _unitOfWorkDA.UserTenantMappingDA.UpdateUserTenant(Id, userTenantToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<UserTenantMappingRequestDto>(userTenantData);
            return _mappedUser;
        }

        #region Private Method

        private async Task<UserTenantMapping> UserTenantGetById(int Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.UserTenantMappingDA.GetById(Id, cancellationToken);
            if (data == null)
            {
                throw new Exception("User Tenant not found");
            }
            return data;
        }

        #endregion Private Method
    }
}