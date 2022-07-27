using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Status;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class StatusBL : IStatusBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;

        public StatusBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StatusResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.StatusDA.GetAll(cancellationToken);
            var DataToReturn = _mapper.Map<List<StatusResponseDto>>(data.ToList());
            return DataToReturn;
        }

        public async Task<StatusRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var data = StatusGetById(Id, cancellationToken).Result;
            return _mapper.Map<StatusRequestDto>(data);
        }

        public async Task<StatusRequestDto> CreateStatus(StatusRequestDto model, CancellationToken cancellationToken)
        {
            var StatusToInsert = _mapper.Map<Statuses>(model);
            StatusToInsert.IsDeleted = false;
            StatusToInsert.CreatedBy = 1;
            StatusToInsert.CreatedDate = DateTime.Now;
            StatusToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.StatusDA.CreateStatus(StatusToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<StatusRequestDto>(data);
            return _mappedUser;
        }

        public async Task<StatusRequestDto> UpdateStatus(int Id, StatusRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = StatusGetById(Id, cancellationToken).Result;
            oldData.UpdatedBy = 1;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.StatusDA.UpdateStatus(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<StatusRequestDto>(data);
            return _mappedUser;
        }

        private static void MapToDbObject(StatusRequestDto model, Statuses oldData)
        {
            oldData.StatusTitle = model.StatusTitle;
            oldData.StatusDescription = model.StatusDescription;
            oldData.StatusOrder = model.StatusOrder;
            oldData.TenantId = model.TenantId;
            oldData.IsCompleted = model.IsCompleted;
            oldData.IsDeleted = model.IsDeleted;
        }

        public async Task<StatusRequestDto> DeleteStatus(int Id, CancellationToken cancellationToken)
        {
            var StatusToUpdate = StatusGetById(Id, cancellationToken).Result;
            StatusToUpdate.IsDeleted = true;
            StatusToUpdate.UpdatedDate = DateTime.Now;
            StatusToUpdate.UpdatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.StatusDA.UpdateStatus(Id, StatusToUpdate, cancellationToken);
            var _mappedUser = _mapper.Map<StatusRequestDto>(data);
            return _mappedUser;
        }

        #region otherMethod

        private async Task<Statuses> StatusGetById(int Id, CancellationToken cancellationToken)
        {
            var StatusDataById = await _unitOfWorkDA.StatusDA.GetById(Id, cancellationToken);
            if (StatusDataById == null)
            {
                throw new Exception("Status not found");
            }
            return StatusDataById;
        }

        #endregion otherMethod
    }
}