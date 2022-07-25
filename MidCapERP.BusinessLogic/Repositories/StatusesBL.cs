using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Statuses;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class StatusesBL : IStatusesBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;

        public StatusesBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StatusesResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.StatusesDA.GetAll(cancellationToken);
            var DataToReturn = _mapper.Map<List<StatusesResponseDto>>(data.ToList());
            return DataToReturn;
        }


        public async Task<StatusesRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var data = StatusesGetById(Id, cancellationToken).Result;
            return _mapper.Map<StatusesRequestDto>(data);
        }

        public async Task<StatusesRequestDto> CreateStatuses(StatusesRequestDto model, CancellationToken cancellationToken)
        {
            var StatusesToInsert = _mapper.Map<Statuses>(model);
            StatusesToInsert.IsDeleted = false;
            StatusesToInsert.CreatedBy = 1;
            StatusesToInsert.CreatedDate = DateTime.Now;
            StatusesToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.StatusesDA.CreateStatuses(StatusesToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<StatusesRequestDto>(data);
            return _mappedUser;
        }

        public async Task<StatusesRequestDto> UpdateStatuses(int Id, StatusesRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = StatusesGetById(Id, cancellationToken).Result;
            oldData.UpdatedBy = 1;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.StatusesDA.UpdateStatuses(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<StatusesRequestDto>(data);
            return _mappedUser;
        }

        private static void MapToDbObject(StatusesRequestDto model, Statuses oldData)
        {
            oldData.StatusTitle = model.StatusTitle;
            oldData.StatusDescription = model.StatusDescription;
            oldData.StatusOrder = model.StatusOrder;
            oldData.TenantId = model.TenantId;
            oldData.IsCompleted = model.IsCompleted;
            oldData.IsDeleted = model.IsDeleted;

        }

        public async Task<StatusesRequestDto> DeleteStatuses(int Id, CancellationToken cancellationToken)
        {
            var StatusesToUpdate = StatusesGetById(Id, cancellationToken).Result;
            StatusesToUpdate.IsDeleted = true;
            StatusesToUpdate.UpdatedDate = DateTime.Now;
            StatusesToUpdate.UpdatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.StatusesDA.UpdateStatuses(Id, StatusesToUpdate, cancellationToken);
            var _mappedUser = _mapper.Map<StatusesRequestDto>(data);
            return _mappedUser;
        }

        #region otherMethod
        private async Task<Statuses> StatusesGetById(int Id, CancellationToken cancellationToken)
        {
            var StatusesDataById = await _unitOfWorkDA.StatusesDA.GetById(Id, cancellationToken);
            if (StatusesDataById == null)
            {
                throw new Exception("Status not found");
            }
            return StatusesDataById;
        }
        #endregion
    }
}
