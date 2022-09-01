using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Constants;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Paging;
using MidCapERP.Dto.WoodType;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class WoodTypeBL : IWoodTypeBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public WoodTypeBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<WoodTypeResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var woodTypeAllData = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            return _mapper.Map<List<WoodTypeResponseDto>>(woodTypeAllData.Where(x => x.LookupId == (int)MasterPagesEnum.FrameType).ToList());
        }

        public async Task<JsonRepsonse<WoodTypeResponseDto>> GetFilterWoodTypeData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var woodTypeAllData = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            var lookupsAllData = await _unitOfWorkDA.LookupsDA.GetAll(cancellationToken);
            var woodTypeResponseData = (from x in woodTypeAllData
                                        join y in lookupsAllData
                                        on new { x.LookupId } equals new { y.LookupId }
                                        where x.LookupId == (int)MasterPagesEnum.FrameType
                                        select new WoodTypeResponseDto()
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
            var woodTypeData = new PagedList<WoodTypeResponseDto>(woodTypeResponseData, dataTableFilterDto);
            return new JsonRepsonse<WoodTypeResponseDto>(dataTableFilterDto.Draw, woodTypeData.TotalCount, woodTypeData.TotalCount, woodTypeData);
        }

        public async Task<WoodTypeResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken)
        {
            var woodTypedata = await GetWoodTypeById(Id, cancellationToken);
            return _mapper.Map<WoodTypeResponseDto>(woodTypedata);
        }

        public async Task<WoodTypeRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var woodTypedata = await GetWoodTypeById(Id, cancellationToken);
            return _mapper.Map<WoodTypeRequestDto>(woodTypedata);
        }

        public async Task<WoodTypeRequestDto> CreateWoodType(WoodTypeRequestDto model, CancellationToken cancellationToken)
        {
            var woodTypeToInsert = _mapper.Map<LookupValues>(model);
            woodTypeToInsert.IsDeleted = false;
            woodTypeToInsert.CreatedBy = _currentUser.UserId;
            woodTypeToInsert.CreatedDate = DateTime.Now;
            woodTypeToInsert.CreatedUTCDate = DateTime.UtcNow;
            var woodTypedata = await _unitOfWorkDA.LookupValuesDA.CreateLookupValue(woodTypeToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<WoodTypeRequestDto>(woodTypedata);
            return _mappedUser;
        }

        public async Task<WoodTypeRequestDto> UpdateWoodType(int Id, WoodTypeRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await GetWoodTypeById(Id, cancellationToken);
            UpdateWoodType(oldData);
            MapToDbObject(model, oldData);
            var woodTypedata = await _unitOfWorkDA.LookupValuesDA.UpdateLookupValue(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<WoodTypeRequestDto>(woodTypedata);
            return _mappedUser;
        }

        public async Task<WoodTypeRequestDto> DeleteWoodType(int Id, CancellationToken cancellationToken)
        {
            var woodTypeToUpdate = await GetWoodTypeById(Id, cancellationToken);
            woodTypeToUpdate.IsDeleted = true;
            UpdateWoodType(woodTypeToUpdate);
            var woodTypedata = await _unitOfWorkDA.LookupValuesDA.UpdateLookupValue(Id, woodTypeToUpdate, cancellationToken);
            var _mappedUser = _mapper.Map<WoodTypeRequestDto>(woodTypedata);
            return _mappedUser;
        }

        #region PrivateMethods

        private void UpdateWoodType(LookupValues data)
        {
            data.UpdatedBy = _currentUser.UserId;
            data.UpdatedDate = DateTime.Now;
            data.UpdatedUTCDate = DateTime.UtcNow;
        }

        private static void MapToDbObject(WoodTypeRequestDto model, LookupValues oldData)
        {
            oldData.LookupValueName = model.LookupValueName;
            oldData.LookupValueId = model.LookupValueId;
        }

        private async Task<LookupValues> GetWoodTypeById(int Id, CancellationToken cancellationToken)
        {
            var woodTypeDataById = await _unitOfWorkDA.LookupValuesDA.GetById(Id, cancellationToken);
            if (woodTypeDataById == null)
            {
                throw new Exception("WoodType not found");
            }
            return woodTypeDataById;
        }

        #endregion PrivateMethods
    }
}