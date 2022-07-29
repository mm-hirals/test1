using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Constants;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Paging;
using MidCapERP.Dto.Unit;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class UnitBL : IUnitBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public UnitBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<UnitResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            var dataToReturn = _mapper.Map<List<UnitResponseDto>>(data.ToList());
            return dataToReturn;
        }

        public async Task<JsonRepsonse<UnitResponseDto>> GetFilterUnitData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var unitAllData = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            var unitResponseData = (from x in unitAllData
                                    join y in _unitOfWorkDA.LookupsDA.GetAll(cancellationToken).Result
                                         on new { x.LookupId } equals new { y.LookupId }
                                    where x.LookupId == (int)MasterPagesEnum.Unit
                                    select new UnitResponseDto()
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
                                    }).ToList();
            var unitData = new PagedList<UnitResponseDto>(unitResponseData, dataTableFilterDto.Start, dataTableFilterDto.PageSize);
            //var UnitResponseData = _mapper.Map<List<UnitResponseDto>>(UnitData);
            return new JsonRepsonse<UnitResponseDto>(dataTableFilterDto.Draw, unitData.TotalCount, unitData.TotalCount, unitResponseData);
        }

        public async Task<UnitResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken)
        {
            var data = await GetUnitById(Id, cancellationToken);
            return _mapper.Map<UnitResponseDto>(data);
        }

        public async Task<UnitRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var data = await GetUnitById(Id, cancellationToken);
            return _mapper.Map<UnitRequestDto>(data);
        }

        public async Task<UnitRequestDto> CreateUnit(UnitRequestDto model, CancellationToken cancellationToken)
        {
            var unitToInsert = _mapper.Map<LookupValues>(model);
            unitToInsert.IsDeleted = false;
            unitToInsert.CreatedBy = _currentUser.UserId;
            unitToInsert.CreatedDate = DateTime.Now;
            unitToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.LookupValuesDA.CreateLookupValue(unitToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<UnitRequestDto>(data);
            return _mappedUser;
        }

        public async Task<UnitRequestDto> UpdateUnit(int Id, UnitRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await GetUnitById(Id, cancellationToken);
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.LookupValuesDA.UpdateLookupValue(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<UnitRequestDto>(data);
            return _mappedUser;
        }

        private static void MapToDbObject(UnitRequestDto model, LookupValues oldData)
        {
            oldData.LookupValueName = model.LookupValueName;
            oldData.LookupValueId = model.LookupValueId;
        }

        public async Task<UnitRequestDto> DeleteUnit(int Id, CancellationToken cancellationToken)
        {
            var unitToUpdate = await GetUnitById(Id, cancellationToken);
            unitToUpdate.IsDeleted = true;
            unitToUpdate.UpdatedBy = _currentUser.UserId;
            unitToUpdate.UpdatedDate = DateTime.Now;
            unitToUpdate.UpdatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.LookupValuesDA.UpdateLookupValue(Id, unitToUpdate, cancellationToken);
            var _mappedUser = _mapper.Map<UnitRequestDto>(data);
            return _mappedUser;
        }

        #region PrivateMethods

        private async Task<LookupValues> GetUnitById(int Id, CancellationToken cancellationToken)
        {
            var unitDataById = await _unitOfWorkDA.LookupValuesDA.GetById(Id, cancellationToken);
            if (unitDataById == null)
            {
                throw new Exception("LookupValues not found");
            }
            return unitDataById;
        }

        #endregion PrivateMethods
    }
}