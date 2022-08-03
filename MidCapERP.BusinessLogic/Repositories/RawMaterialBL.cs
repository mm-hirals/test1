using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Paging;
using MidCapERP.Dto.RawMaterial;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class RawMaterialBL : IRawMaterialBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public RawMaterialBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<RawMaterialResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.RawMaterialDA.GetAll(cancellationToken);
            var dataToReturn = _mapper.Map<List<RawMaterialResponseDto>>(data.ToList());
            return dataToReturn;
        }

        public async Task<JsonRepsonse<RawMaterialResponseDto>> GetFilterRawMaterialData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var RawMaterialAllData = await _unitOfWorkDA.RawMaterialDA.GetAll(cancellationToken);
            var RawMaterialResponseData = (from x in RawMaterialAllData
                                           join y in _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken).Result
                                                on new { LookupId = x.UnitId } equals new { LookupId = y.LookupValueId }
                                           select new RawMaterialResponseDto()
                                           {
                                               RawMaterialId = x.RawMaterialId,
                                               Title = x.Title,
                                               UnitName = y.LookupValueName,
                                               UnitPrice = x.UnitPrice
                                           }).ToList();
            var RawMaterialData = new PagedList<RawMaterialResponseDto>(RawMaterialResponseData, dataTableFilterDto.Start, dataTableFilterDto.PageSize);
            return new JsonRepsonse<RawMaterialResponseDto>(dataTableFilterDto.Draw, RawMaterialData.TotalCount, RawMaterialData.TotalCount, RawMaterialData);
        }

        public async Task<RawMaterialResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken)
        {
            var data = await GetRawMaterialById(Id, cancellationToken);
            return _mapper.Map<RawMaterialResponseDto>(data);
        }

        public async Task<RawMaterialRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var data = await GetRawMaterialById(Id, cancellationToken);
            return _mapper.Map<RawMaterialRequestDto>(data);
        }

        public async Task<RawMaterialRequestDto> CreateRawMaterial(RawMaterialRequestDto model, CancellationToken cancellationToken)
        {
            var RawMaterialToInsert = _mapper.Map<RawMaterial>(model);
            RawMaterialToInsert.IsDeleted = false;
            RawMaterialToInsert.TenantId = _currentUser.TenantId;
            RawMaterialToInsert.CreatedBy = _currentUser.UserId;
            RawMaterialToInsert.CreatedDate = DateTime.Now;
            RawMaterialToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.RawMaterialDA.CreateRawMaterial(RawMaterialToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<RawMaterialRequestDto>(data);
            return _mappedUser;
        }

        public async Task<RawMaterialRequestDto> UpdateRawMaterial(int Id, RawMaterialRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await GetRawMaterialById(Id, cancellationToken);
            UpdateData(oldData);
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.RawMaterialDA.UpdateRawMaterial(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<RawMaterialRequestDto>(data);
            return _mappedUser;
        }

        private static void MapToDbObject(RawMaterialRequestDto model, RawMaterial oldData)
        {
            oldData.Title = model.Title;
            oldData.UnitId = model.UnitId;
            oldData.UnitPrice = model.UnitPrice;
            oldData.ImagePath = model.ImagePath;
        }

        public async Task<RawMaterialRequestDto> DeleteRawMaterial(int Id, CancellationToken cancellationToken)
        {
            var RawMaterialToUpdate = await GetRawMaterialById(Id, cancellationToken);
            RawMaterialToUpdate.IsDeleted = true;
            UpdateData(RawMaterialToUpdate);
            var data = await _unitOfWorkDA.RawMaterialDA.UpdateRawMaterial(Id, RawMaterialToUpdate, cancellationToken);
            var _mappedUser = _mapper.Map<RawMaterialRequestDto>(data);
            return _mappedUser;
        }

        #region PrivateMethods

        private async Task<RawMaterial> GetRawMaterialById(int Id, CancellationToken cancellationToken)
        {
            var RawMaterialDataById = await _unitOfWorkDA.RawMaterialDA.GetById(Id, cancellationToken);
            if (RawMaterialDataById == null)
            {
                throw new Exception("RawMaterial not found");
            }
            return RawMaterialDataById;
        }

        private void UpdateData(RawMaterial oldData)
        {
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
        }

        #endregion PrivateMethods
    }
}