using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.LookupValues;
using MidCapERP.Dto.Paging;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class LookupValuesBL : ILookupValuesBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public LookupValuesBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<LookupValuesResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            var dataToReturn = _mapper.Map<List<LookupValuesResponseDto>>(data.ToList());
            return dataToReturn;
        }

        public async Task<JsonRepsonse<LookupValuesResponseDto>> GetFilterLookupValuesData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var customerData = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            var data = new PagedList<LookupValues>(customerData, dataTableFilterDto.Start, dataTableFilterDto.PageSize);
            var lookupValueResponseData = _mapper.Map<List<LookupValuesResponseDto>>(data);
            return new JsonRepsonse<LookupValuesResponseDto>(dataTableFilterDto.Draw, data.TotalCount, data.TotalCount, lookupValueResponseData);
        }

        public async Task<LookupValuesResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken)
        {
            var data = await LookupValuesGetById(Id, cancellationToken);
            return _mapper.Map<LookupValuesResponseDto>(data);
        }

        public async Task<LookupValuesRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var data = await LookupValuesGetById(Id, cancellationToken);
            return _mapper.Map<LookupValuesRequestDto>(data);
        }

        public async Task<LookupValuesRequestDto> CreateLookupValues(LookupValuesRequestDto model, CancellationToken cancellationToken)
        {
            var lookupToInsert = _mapper.Map<LookupValues>(model);
            lookupToInsert.IsDeleted = false;
            lookupToInsert.CreatedBy = _currentUser.UserId;
            lookupToInsert.CreatedDate = DateTime.Now;
            lookupToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.LookupValuesDA.CreateLookupValue(lookupToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<LookupValuesRequestDto>(data);
            return _mappedUser;
        }

        public async Task<LookupValuesRequestDto> UpdateLookupValues(int Id, LookupValuesRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await LookupValuesGetById(Id, cancellationToken);
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.LookupValuesDA.UpdateLookupValue(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<LookupValuesRequestDto>(data);
            return _mappedUser;
        }

        private static void MapToDbObject(LookupValuesRequestDto model, LookupValues oldData)
        {
            oldData.LookupValueName = model.LookupValueName;
            oldData.LookupValueId = model.LookupValueId;
        }

        public async Task<LookupValuesRequestDto> DeleteLookupValues(int Id, CancellationToken cancellationToken)
        {
            var lookupToUpdate = await LookupValuesGetById(Id, cancellationToken);
            lookupToUpdate.IsDeleted = true;
            lookupToUpdate.UpdatedDate = DateTime.Now;
            lookupToUpdate.UpdatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.LookupValuesDA.UpdateLookupValue(Id, lookupToUpdate, cancellationToken);
            var _mappedUser = _mapper.Map<LookupValuesRequestDto>(data);
            return _mappedUser;
        }

        #region PrivateMethods

        private async Task<LookupValues> LookupValuesGetById(int Id, CancellationToken cancellationToken)
        {
            var lookupValuesDataById = await _unitOfWorkDA.LookupValuesDA.GetById(Id, cancellationToken);
            if (lookupValuesDataById == null)
            {
                throw new Exception("LookupValues not found");
            }
            return lookupValuesDataById;
        }

        #endregion PrivateMethods
    }
}