using AutoMapper;
using MidCapERP.BusinessLogic.Constants;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Services.FileStorage;
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
        private readonly IFileStorageService _fileStorageService;

        public RawMaterialBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser, IFileStorageService fileStorageService)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
            _fileStorageService = fileStorageService;
        }

        public async Task<IEnumerable<RawMaterialResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var rawMaterialAllData = await _unitOfWorkDA.RawMaterialDA.GetAll(cancellationToken);
            return _mapper.Map<List<RawMaterialResponseDto>>(rawMaterialAllData.ToList());
        }

        public async Task<JsonRepsonse<RawMaterialResponseDto>> GetFilterRawMaterialData(RawMaterialDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var rawMaterialAllData = await _unitOfWorkDA.RawMaterialDA.GetAll(cancellationToken);
            var lookupValuesAllData = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            var rawMaterialResponseData = (from x in rawMaterialAllData
                                           join y in lookupValuesAllData
                                           on new { LookupId = x.UnitId } equals new { LookupId = y.LookupValueId }
                                           select new RawMaterialResponseDto()
                                           {
                                               RawMaterialId = x.RawMaterialId,
                                               Title = x.Title,
                                               UnitName = y.LookupValueName,
                                               UnitPrice = x.UnitPrice,
                                               ImagePath = x.ImagePath
                                           }).AsQueryable();
            var rawMaterialFilteredData = FilterRawMaterialData(dataTableFilterDto, rawMaterialResponseData);
            var rawMaterialData = new PagedList<RawMaterialResponseDto>(rawMaterialFilteredData, dataTableFilterDto);
            return new JsonRepsonse<RawMaterialResponseDto>(dataTableFilterDto.Draw, rawMaterialData.TotalCount, rawMaterialData.TotalCount, rawMaterialData);
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
            var rawMaterialToInsert = _mapper.Map<RawMaterial>(model);
            if (model.UploadImage != null)
                rawMaterialToInsert.ImagePath = await _fileStorageService.StoreFile(model.UploadImage, ApplicationFileStorageConstants.FilePaths.RawMaterials);
            rawMaterialToInsert.IsDeleted = false;
            rawMaterialToInsert.TenantId = _currentUser.TenantId;
            rawMaterialToInsert.CreatedBy = _currentUser.UserId;
            rawMaterialToInsert.CreatedDate = DateTime.Now;
            rawMaterialToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.RawMaterialDA.CreateRawMaterial(rawMaterialToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<RawMaterialRequestDto>(data);
            return _mappedUser;
        }

        public async Task<RawMaterialRequestDto> UpdateRawMaterial(int Id, RawMaterialRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await GetRawMaterialById(Id, cancellationToken);
            if (model.UploadImage != null)
                model.ImagePath = await _fileStorageService.StoreFile(model.UploadImage, ApplicationFileStorageConstants.FilePaths.RawMaterials);
            UpdateData(oldData);
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.RawMaterialDA.UpdateRawMaterial(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<RawMaterialRequestDto>(data);
            return _mappedUser;
        }

        public async Task<RawMaterialRequestDto> DeleteRawMaterial(int Id, CancellationToken cancellationToken)
        {
            var rawMaterialToUpdate = await GetRawMaterialById(Id, cancellationToken);
            rawMaterialToUpdate.IsDeleted = true;
            UpdateData(rawMaterialToUpdate);
            var data = await _unitOfWorkDA.RawMaterialDA.UpdateRawMaterial(Id, rawMaterialToUpdate, cancellationToken);
            var _mappedUser = _mapper.Map<RawMaterialRequestDto>(data);
            return _mappedUser;
        }

        public async Task<bool> ValidateRawMaterialTitle(RawMaterialRequestDto rawMaterialRequestDto, CancellationToken cancellationToken)
        {
            var getAllRawMaterial = await _unitOfWorkDA.RawMaterialDA.GetAll(cancellationToken);

            if (rawMaterialRequestDto.RawMaterialId > 0)
            {
                var getRawMaterialById = getAllRawMaterial.First(c => c.RawMaterialId == rawMaterialRequestDto.RawMaterialId);
                if (getRawMaterialById.Title.Trim() == rawMaterialRequestDto.Title.Trim())
                {
                    return true;
                }
                else
                {
                    return !getAllRawMaterial.Any(c => c.Title.Trim() == rawMaterialRequestDto.Title.Trim() && c.RawMaterialId != rawMaterialRequestDto.RawMaterialId);
                }
            }
            else
            {
                return !getAllRawMaterial.Any(c => c.Title.Trim() == rawMaterialRequestDto.Title.Trim());
            }
        }

        #region PrivateMethods

        private static void MapToDbObject(RawMaterialRequestDto model, RawMaterial oldData)
        {
            oldData.Title = model.Title;
            oldData.UnitId = model.UnitId;
            oldData.UnitPrice = model.UnitPrice;
            oldData.ImagePath = model.ImagePath;
        }

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

        private static IQueryable<RawMaterialResponseDto> FilterRawMaterialData(RawMaterialDataTableFilterDto rawMaterialDataTableFilterDto, IQueryable<RawMaterialResponseDto> rawMaterialResponseDto)
        {
            if (rawMaterialDataTableFilterDto != null)
            {
                if (!string.IsNullOrEmpty(rawMaterialDataTableFilterDto.Title))
                {
                    rawMaterialResponseDto = rawMaterialResponseDto.Where(p => p.Title.StartsWith(rawMaterialDataTableFilterDto.Title));
                }

                if (!string.IsNullOrEmpty(rawMaterialDataTableFilterDto.UnitName))
                {
                    rawMaterialResponseDto = rawMaterialResponseDto.Where(p => p.UnitName.StartsWith(rawMaterialDataTableFilterDto.UnitName));
                }
            }
            return rawMaterialResponseDto;
        }

        #endregion PrivateMethods
    }
}