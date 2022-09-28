using AutoMapper;
using MidCapERP.BusinessLogic.Constants;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Services.FileStorage;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Constants;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.MegaSearch;
using MidCapERP.Dto.Paging;
using MidCapERP.Dto.Polish;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class PolishBL : IPolishBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;
        private readonly IFileStorageService _fileStorageService;

        public PolishBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser, IFileStorageService fileStorageService)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
            _fileStorageService = fileStorageService;
        }

        public async Task<IEnumerable<PolishResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.PolishDA.GetAll(cancellationToken);
            return _mapper.Map<List<PolishResponseDto>>(data.ToList());
        }

        public async Task<IList<MegaSearchResponse>> GetPolishForDropDownByModuleNo(string modelno, CancellationToken cancellationToken)
        {
            var polishAlldata = await _unitOfWorkDA.PolishDA.GetAll(cancellationToken);
            return polishAlldata.Where(x => x.ModelNo.StartsWith(modelno)).Select(x => new MegaSearchResponse(x.PolishId, x.Title, x.ModelNo, x.ImagePath, "Polish")).Take(10).ToList();
        }

        public async Task<PolishResponseDto> GetPolishForDetailsByModuleNo(string modelno, CancellationToken cancellationToken)
        {
            var polishAlldata = await _unitOfWorkDA.PolishDA.GetAll(cancellationToken);
            var data = polishAlldata.Where(x => x.ModelNo == modelno);
            return _mapper.Map<PolishResponseDto>(data);
        }

        public async Task<JsonRepsonse<PolishResponseDto>> GetFilterPolishData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var lookupCompanyId = await GetCompanyLookupId(cancellationToken);
            var lookupUnitId = await GetUnitLookupId(cancellationToken);
            var polishAllData = await _unitOfWorkDA.PolishDA.GetAll(cancellationToken);
            var lookupValuesAllData = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            var companyData = lookupValuesAllData.Where(x => x.LookupId == lookupCompanyId);
            var unitData = lookupValuesAllData.Where(x => x.LookupId == lookupUnitId);
            var polishResponseData = (from x in polishAllData
                                      join y in companyData on new { CompanyId = x.CompanyId } equals new { CompanyId = y.LookupValueId }
                                      join z in unitData on new { UnitId = x.UnitId } equals new { UnitId = z.LookupValueId }
                                      select new PolishResponseDto()
                                      {
                                          PolishId = x.PolishId,
                                          Title = x.Title,
                                          ModelNo = x.ModelNo,
                                          CompanyName = y.LookupValueName,
                                          UnitName = z.LookupValueName,
                                          UnitPrice = x.UnitPrice,
                                          ImagePath = x.ImagePath
                                      }).AsQueryable();
            var polishData = new PagedList<PolishResponseDto>(polishResponseData, dataTableFilterDto);
            return new JsonRepsonse<PolishResponseDto>(dataTableFilterDto.Draw, polishData.TotalCount, polishData.TotalCount, polishData);
        }

        public async Task<PolishResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken)
        {
            var data = await GetPolishById(Id, cancellationToken);
            return _mapper.Map<PolishResponseDto>(data);
        }

        public async Task<PolishRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var data = await GetPolishById(Id, cancellationToken);
            return _mapper.Map<PolishRequestDto>(data);
        }

        public async Task<PolishRequestDto> CreatePolish(PolishRequestDto model, CancellationToken cancellationToken)
        {
            var polishToInsert = _mapper.Map<Polish>(model);
            if (model.UploadImage != null)
                polishToInsert.ImagePath = await _fileStorageService.StoreFile(model.UploadImage, ApplicationFileStorageConstants.FilePaths.Polish);
            polishToInsert.IsDeleted = false;
            polishToInsert.TenantId = _currentUser.TenantId;
            polishToInsert.CreatedBy = _currentUser.UserId;
            polishToInsert.CreatedDate = DateTime.Now;
            polishToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.PolishDA.CreatePolish(polishToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<PolishRequestDto>(data);
            return _mappedUser;
        }

        public async Task<PolishRequestDto> UpdatePolish(int Id, PolishRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await GetPolishById(Id, cancellationToken);
            if (model.UploadImage != null)
                model.ImagePath = await _fileStorageService.StoreFile(model.UploadImage, ApplicationFileStorageConstants.FilePaths.Polish);
            UpdateData(oldData);
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.PolishDA.UpdatePolish(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<PolishRequestDto>(data);
            return _mappedUser;
        }

        public async Task<PolishRequestDto> DeletePolish(int Id, CancellationToken cancellationToken)
        {
            var polishToUpdate = await GetPolishById(Id, cancellationToken);
            polishToUpdate.IsDeleted = true;
            UpdateData(polishToUpdate);
            var data = await _unitOfWorkDA.PolishDA.UpdatePolish(Id, polishToUpdate, cancellationToken);
            var _mappedUser = _mapper.Map<PolishRequestDto>(data);
            return _mappedUser;
        }

        #region Private Method

        private async Task<Polish> GetPolishById(int Id, CancellationToken cancellationToken)
        {
            var polishDataById = await _unitOfWorkDA.PolishDA.GetById(Id, cancellationToken);
            if (polishDataById == null)
            {
                throw new Exception("Polish not found");
            }
            return polishDataById;
        }

        private void UpdateData(Polish oldData)
        {
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
        }

        private static void MapToDbObject(PolishRequestDto model, Polish oldData)
        {
            oldData.Title = model.Title;
            oldData.ModelNo = model.ModelNo;
            oldData.CompanyId = model.CompanyId;
            oldData.UnitId = model.UnitId;
            oldData.UnitPrice = model.UnitPrice;
            oldData.ImagePath = model.ImagePath;
        }

        private async Task<int> GetCompanyLookupId(CancellationToken cancellationToken)
        {
            var lookupsAllData = await _unitOfWorkDA.LookupsDA.GetAll(cancellationToken);
            var lookupId = lookupsAllData.Where(x => x.LookupName == nameof(MasterPagesEnum.Company)).Select(x => x.LookupId).FirstOrDefault();
            return lookupId;
        }

        private async Task<int> GetUnitLookupId(CancellationToken cancellationToken)
        {
            var lookupsAllData = await _unitOfWorkDA.LookupsDA.GetAll(cancellationToken);
            var lookupId = lookupsAllData.Where(x => x.LookupName == nameof(MasterPagesEnum.Unit)).Select(x => x.LookupId).FirstOrDefault();
            return lookupId;
        }

        #endregion Private Method
    }
}