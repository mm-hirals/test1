﻿using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Constants;
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
            var data = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            var dataToReturn = _mapper.Map<List<RawMaterialResponseDto>>(data.ToList());
            return dataToReturn;
        }

        public async Task<JsonRepsonse<RawMaterialResponseDto>> GetFilterRawMaterialData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var RawMaterialAllData = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            var RawMaterialResponseData = (from x in RawMaterialAllData
                                        join y in _unitOfWorkDA.LookupsDA.GetAll(cancellationToken).Result
                                             on new { x.LookupId } equals new { y.LookupId }
                                        select new RawMaterialResponseDto()
                                        {
                                           
                                            UnitName = y.LookupName,
                                            IsDeleted = x.IsDeleted,

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
            var RawMaterialToInsert = _mapper.Map<LookupValues>(model);
            RawMaterialToInsert.IsDeleted = false;
            RawMaterialToInsert.CreatedBy = _currentUser.UserId;
            RawMaterialToInsert.CreatedDate = DateTime.Now;
            RawMaterialToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.LookupValuesDA.CreateLookupValue(RawMaterialToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<RawMaterialRequestDto>(data);
            return _mappedUser;
        }

        public async Task<RawMaterialRequestDto> UpdateRawMaterial(int Id, RawMaterialRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await GetRawMaterialById(Id, cancellationToken);
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.LookupValuesDA.UpdateLookupValue(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<RawMaterialRequestDto>(data);
            return _mappedUser;
        }

        private static void MapToDbObject(RawMaterialRequestDto model, LookupValues oldData)
        {
            //oldData.LookupValueName = model.LookupValueName;
            //oldData.LookupValueId = model.LookupValueId;
        }

        public async Task<RawMaterialRequestDto> DeleteRawMaterial(int Id, CancellationToken cancellationToken)
        {
            var RawMaterialToUpdate = await GetRawMaterialById(Id, cancellationToken);
            RawMaterialToUpdate.IsDeleted = true;
            RawMaterialToUpdate.UpdatedBy = _currentUser.UserId;
            RawMaterialToUpdate.UpdatedDate = DateTime.Now;
            RawMaterialToUpdate.UpdatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.LookupValuesDA.UpdateLookupValue(Id, RawMaterialToUpdate, cancellationToken);
            var _mappedUser = _mapper.Map<RawMaterialRequestDto>(data);
            return _mappedUser;
        }

        #region PrivateMethods

        private async Task<LookupValues> GetRawMaterialById(int Id, CancellationToken cancellationToken)
        {
            var RawMaterialDataById = await _unitOfWorkDA.LookupValuesDA.GetById(Id, cancellationToken);
            if (RawMaterialDataById == null)
            {
                throw new Exception("LookupValues not found");
            }
            return RawMaterialDataById;
        }

        public Task<JsonRepsonse<RawMaterialResponseDto>> GetFilterCategoryData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion PrivateMethods
    }
}
