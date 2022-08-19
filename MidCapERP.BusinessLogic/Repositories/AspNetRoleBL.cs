using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.Dto;
using MidCapERP.Dto.AspNetRole;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class AspNetRoleBL : IAspNetRolesBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public AspNetRoleBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<AspNetRoleResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var aspNetRoleData = await _unitOfWorkDA.AspNetRoleDA.GetAll(cancellationToken);
            return _mapper.Map<List<AspNetRoleResponseDto>>(aspNetRoleData.Where(x => x.TenantId == _currentUser.TenantId).ToList());
        }
    }
}