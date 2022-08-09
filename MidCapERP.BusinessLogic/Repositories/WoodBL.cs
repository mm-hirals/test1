using AutoMapper;
using MidCapERP.BusinessLogic.Constants;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Services.FileStorage;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Constants;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Paging;
using MidCapERP.Dto.Wood;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class WoodBL : IWoodBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;
        private readonly IFileStorageService _fileStorageService;

        public WoodBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser, IFileStorageService fileStorageService)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
            _fileStorageService = fileStorageService;
        }

        public async Task<IEnumerable<WoodResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.WoodDA.GetAll(cancellationToken);
            return _mapper.Map<List<WoodResponseDto>>(data.ToList());
        }

        public async Task<JsonRepsonse<WoodResponseDto>> GetFilterWoodData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var woodAllData = await _unitOfWorkDA.WoodDA.GetAll(cancellationToken);
            var lookupValueAll = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            var woodTypeData = lookupValueAll.Where(x => x.LookupId == (int)MasterPagesEnum.WoodType);
            var companyData = lookupValueAll.Where(x => x.LookupId == (int)MasterPagesEnum.Company);
            var unitData = lookupValueAll.Where(x => x.LookupId == (int)MasterPagesEnum.Unit);
            var woodResponseData = (from x in woodAllData
                                    join y in woodTypeData on new { WoodTypeId = x.WoodTypeId } equals new { WoodTypeId = y.LookupValueId }
                                    join z in companyData on new { CompanyId = x.CompanyId } equals new { CompanyId = z.LookupValueId }
                                    join a in unitData on new { UnitId = x.UnitId } equals new { UnitId = a.LookupValueId }
                                    select new WoodResponseDto()
                                    {
                                        WoodId = x.WoodId,
                                        WoodTypeName = y.LookupValueName,
                                        Title = x.Title,
                                        ModelNo = x.ModelNo,
                                        CompanyName = z.LookupValueName,
                                        UnitName = a.LookupValueName,
                                        UnitPrice = x.UnitPrice,
                                        IsDeleted = x.IsDeleted,
                                        CreatedBy = x.CreatedBy,
                                        CreatedDate = x.CreatedDate,
                                        CreatedUTCDate = x.CreatedUTCDate,
                                        UpdatedBy = x.UpdatedBy,
                                        UpdatedDate = x.UpdatedDate,
                                        UpdatedUTCDate = x.UpdatedUTCDate
                                    }).AsQueryable();
            var woodData = new PagedList<WoodResponseDto>(woodResponseData, dataTableFilterDto);
            return new JsonRepsonse<WoodResponseDto>(dataTableFilterDto.Draw, woodData.TotalCount, woodData.TotalCount, woodData);
        }

        public async Task<WoodResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken)
        {
            var data = await WoodGetById(Id, cancellationToken);
            return _mapper.Map<WoodResponseDto>(data);
        }

        public async Task<WoodRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var data = await WoodGetById(Id, cancellationToken);
            return _mapper.Map<WoodRequestDto>(data);
        }

        public async Task<WoodRequestDto> CreateWood(WoodRequestDto model, CancellationToken cancellationToken)
        {
            var woodToInsert = _mapper.Map<Wood>(model);
            if (model.UploadImage != null)
                woodToInsert.ImagePath = await _fileStorageService.StoreFile(model.UploadImage, ApplicationFileStorageConstants.FilePaths.Woods);
            woodToInsert.IsDeleted = false;
            woodToInsert.TenantId = _currentUser.TenantId;
            woodToInsert.CreatedBy = _currentUser.UserId;
            woodToInsert.CreatedDate = DateTime.Now;
            woodToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.WoodDA.CreateWood(woodToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<WoodRequestDto>(data);
            return _mappedUser;
        }

        public async Task<WoodRequestDto> UpdateWood(int Id, WoodRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await WoodGetById(Id, cancellationToken);
            if (model.UploadImage != null)
                model.ImagePath = await _fileStorageService.StoreFile(model.UploadImage, ApplicationFileStorageConstants.FilePaths.Woods);
            UpdateWood(oldData);
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.WoodDA.UpdateWood(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<WoodRequestDto>(data);
            return _mappedUser;
        }

        public async Task<WoodRequestDto> DeleteWood(int Id, CancellationToken cancellationToken)
        {
            var woodToUpdate = await WoodGetById(Id, cancellationToken);
            woodToUpdate.IsDeleted = true;
            UpdateWood(woodToUpdate);
            var data = await _unitOfWorkDA.WoodDA.UpdateWood(Id, woodToUpdate, cancellationToken);
            var _mapped = _mapper.Map<WoodRequestDto>(data);
            return _mapped;
        }

        #region Private Method

        private void UpdateWood(Wood oldData)
        {
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
        }

        private static void MapToDbObject(WoodRequestDto model, Wood oldData)
        {
            oldData.WoodTypeId = model.WoodTypeId;
            oldData.Title = model.Title;
            oldData.ModelNo = model.ModelNo;
            oldData.CompanyId = model.CompanyId;
            oldData.UnitId = model.UnitId;
            oldData.UnitPrice = model.UnitPrice;
            oldData.ImagePath = model.ImagePath;
        }

        private async Task<Wood> WoodGetById(int Id, CancellationToken cancellationToken)
        {
            var woodDataById = await _unitOfWorkDA.WoodDA.GetById(Id, cancellationToken);
            if (woodDataById == null)
            {
                throw new Exception("Wood not found");
            }
            return woodDataById;
        }

        #endregion Private Method
    }
}