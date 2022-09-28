using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Constants;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.FrameType;
using MidCapERP.Dto.Paging;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class FrameTypeBL : IFrameTypeBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public FrameTypeBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<FrameTypeResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var frameTypeAllData = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            return _mapper.Map<List<FrameTypeResponseDto>>(frameTypeAllData.Where(x => x.LookupId == (int)MasterPagesEnum.FrameType).ToList());
        }

        public async Task<JsonRepsonse<FrameTypeResponseDto>> GetFilterFrameTypeData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var frameTypeAllData = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            var lookupsAllData = await _unitOfWorkDA.LookupsDA.GetAll(cancellationToken);
            var frameTypeResponseData = (from x in frameTypeAllData
                                         join y in lookupsAllData
                                         on new { x.LookupId } equals new { y.LookupId }
                                         where x.LookupId == (int)MasterPagesEnum.FrameType
                                         select new FrameTypeResponseDto()
                                         {
                                             LookupValueId = x.LookupValueId,
                                             LookupValueName = x.LookupValueName,
                                             LookupId = x.LookupId,
                                             LookupName = y.LookupName,
                                             IsDeleted = x.IsDeleted,
                                             CreatedBy = x.CreatedBy,
                                             CreatedDate = x.CreatedDate,
                                             CreatedUTCDate = x.CreatedUTCDate,
                                             UpdatedBy = x.UpdatedBy,
                                             UpdatedDate = x.UpdatedDate,
                                             UpdatedUTCDate = x.UpdatedUTCDate
                                         }).AsQueryable();
            var frameTypeData = new PagedList<FrameTypeResponseDto>(frameTypeResponseData, dataTableFilterDto);
            return new JsonRepsonse<FrameTypeResponseDto>(dataTableFilterDto.Draw, frameTypeData.TotalCount, frameTypeData.TotalCount, frameTypeData);
        }

        public async Task<FrameTypeResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken)
        {
            var frameTypedata = await GetFrameTypeById(Id, cancellationToken);
            return _mapper.Map<FrameTypeResponseDto>(frameTypedata);
        }

        public async Task<FrameTypeRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var frameTypedata = await GetFrameTypeById(Id, cancellationToken);
            return _mapper.Map<FrameTypeRequestDto>(frameTypedata);
        }

        public async Task<FrameTypeRequestDto> CreateFrameType(FrameTypeRequestDto model, CancellationToken cancellationToken)
        {
            var frameTypeToInsert = _mapper.Map<LookupValues>(model);
            frameTypeToInsert.IsDeleted = false;
            frameTypeToInsert.CreatedBy = _currentUser.UserId;
            frameTypeToInsert.CreatedDate = DateTime.Now;
            frameTypeToInsert.CreatedUTCDate = DateTime.UtcNow;
            var frameTypedata = await _unitOfWorkDA.LookupValuesDA.CreateLookupValue(frameTypeToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<FrameTypeRequestDto>(frameTypedata);
            return _mappedUser;
        }

        public async Task<FrameTypeRequestDto> UpdateFrameType(int Id, FrameTypeRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await GetFrameTypeById(Id, cancellationToken);
            UpdateFrameType(oldData);
            MapToDbObject(model, oldData);
            var frameTypedata = await _unitOfWorkDA.LookupValuesDA.UpdateLookupValue(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<FrameTypeRequestDto>(frameTypedata);
            return _mappedUser;
        }

        public async Task<FrameTypeRequestDto> DeleteFrameType(int Id, CancellationToken cancellationToken)
        {
            var frameTypeToUpdate = await GetFrameTypeById(Id, cancellationToken);
            frameTypeToUpdate.IsDeleted = true;
            UpdateFrameType(frameTypeToUpdate);
            var frameTypedata = await _unitOfWorkDA.LookupValuesDA.UpdateLookupValue(Id, frameTypeToUpdate, cancellationToken);
            var _mappedUser = _mapper.Map<FrameTypeRequestDto>(frameTypedata);
            return _mappedUser;
        }

        #region PrivateMethods

        private void UpdateFrameType(LookupValues data)
        {
            data.UpdatedBy = _currentUser.UserId;
            data.UpdatedDate = DateTime.Now;
            data.UpdatedUTCDate = DateTime.UtcNow;
        }

        private static void MapToDbObject(FrameTypeRequestDto model, LookupValues oldData)
        {
            oldData.LookupValueName = model.LookupValueName;
            oldData.LookupValueId = model.LookupValueId;
        }

        private async Task<LookupValues> GetFrameTypeById(int Id, CancellationToken cancellationToken)
        {
            var frameTypeDataById = await _unitOfWorkDA.LookupValuesDA.GetById(Id, cancellationToken);
            if (frameTypeDataById == null)
            {
                throw new Exception("FrameType not found");
            }
            return frameTypeDataById;
        }

        #endregion PrivateMethods
    }
}