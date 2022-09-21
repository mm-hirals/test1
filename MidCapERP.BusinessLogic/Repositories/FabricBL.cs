using AutoMapper;
using MidCapERP.BusinessLogic.Constants;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Services.FileStorage;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Constants;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Fabric;
using MidCapERP.Dto.Paging;
using MidCapERP.Dto.Product;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class FabricBL : IFabricBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;
        private readonly IFileStorageService _fileStorageService;

        public FabricBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser, IFileStorageService fileStorageService)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
            _fileStorageService = fileStorageService;
        }

        public async Task<IEnumerable<FabricResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.FabricDA.GetAll(cancellationToken);
            return _mapper.Map<List<FabricResponseDto>>(data.ToList());
        }

        public async Task<IEnumerable<ProductForDorpDownByModuleNoResponseDto>> GetFabricForDropDownByModuleNo(string modelno, CancellationToken cancellationToken)
        {
            var frabricAlldata = await _unitOfWorkDA.FabricDA.GetAll(cancellationToken);
            return frabricAlldata.Where(x => x.ModelNo.StartsWith(modelno)).Select(x => new ProductForDorpDownByModuleNoResponseDto(x.FabricId, x.Title, x.ModelNo, x.ImagePath, "Fabric")).ToList();
        }

        public async Task<FabricResponseDto> GetFabricForDetailsByModuleNo(string modelno, CancellationToken cancellationToken)
        {
            var frabricAlldata = await _unitOfWorkDA.FabricDA.GetAll(cancellationToken);
            var data = frabricAlldata.Where(x => x.ModelNo == modelno);
            return _mapper.Map<FabricResponseDto>(data);
        }

        public async Task<JsonRepsonse<FabricResponseDto>> GetFilterFabricData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var fabricAllData = await _unitOfWorkDA.FabricDA.GetAll(cancellationToken);
            var lookupValuesAllData = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            var companyData = lookupValuesAllData.Where(x => x.LookupId == (int)MasterPagesEnum.Company);
            var unitData = lookupValuesAllData.Where(x => x.LookupId == (int)MasterPagesEnum.Unit);
            var fabricResponseData = (from x in fabricAllData
                                      join y in companyData on new { CompanyId = x.CompanyId } equals new { CompanyId = y.LookupValueId }
                                      join z in unitData on new { UnitId = x.UnitId } equals new { UnitId = z.LookupValueId }
                                      select new FabricResponseDto()
                                      {
                                          FabricId = x.FabricId,
                                          Title = x.Title,
                                          ModelNo = x.ModelNo,
                                          CompanyName = y.LookupValueName,
                                          UnitName = z.LookupValueName,
                                          UnitPrice = x.UnitPrice,
                                          ImagePath = x.ImagePath
                                      }).AsQueryable();
            var fabricData = new PagedList<FabricResponseDto>(fabricResponseData, dataTableFilterDto);
            return new JsonRepsonse<FabricResponseDto>(dataTableFilterDto.Draw, fabricData.TotalCount, fabricData.TotalCount, fabricData);
        }

        public async Task<FabricResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken)
        {
            var data = await GetFabricById(Id, cancellationToken);
            return _mapper.Map<FabricResponseDto>(data);
        }

        public async Task<FabricRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var data = await GetFabricById(Id, cancellationToken);
            return _mapper.Map<FabricRequestDto>(data);
        }

        public async Task<FabricRequestDto> CreateFabric(FabricRequestDto model, CancellationToken cancellationToken)
        {
            var FabricToInsert = _mapper.Map<Fabrics>(model);
            if (model.UploadImage != null)
                FabricToInsert.ImagePath = await _fileStorageService.StoreFile(model.UploadImage, ApplicationFileStorageConstants.FilePaths.Fabrics);
            FabricToInsert.IsDeleted = false;
            FabricToInsert.TenantId = _currentUser.TenantId;
            FabricToInsert.CreatedBy = _currentUser.UserId;
            FabricToInsert.CreatedDate = DateTime.Now;
            FabricToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.FabricDA.CreateFabric(FabricToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<FabricRequestDto>(data);
            return _mappedUser;
        }

        public async Task<FabricRequestDto> UpdateFabric(int Id, FabricRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await GetFabricById(Id, cancellationToken);
            if (model.UploadImage != null)
                model.ImagePath = await _fileStorageService.StoreFile(model.UploadImage, ApplicationFileStorageConstants.FilePaths.Fabrics);
            UpdateData(oldData);
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.FabricDA.UpdateFabric(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<FabricRequestDto>(data);
            return _mappedUser;
        }

        public async Task<FabricRequestDto> DeleteFabric(int Id, CancellationToken cancellationToken)
        {
            var fabricToUpdate = await GetFabricById(Id, cancellationToken);
            fabricToUpdate.IsDeleted = true;
            UpdateData(fabricToUpdate);
            var data = await _unitOfWorkDA.FabricDA.UpdateFabric(Id, fabricToUpdate, cancellationToken);
            var _mappedUser = _mapper.Map<FabricRequestDto>(data);
            return _mappedUser;
        }

        #region Private Method

        private async Task<Fabrics> GetFabricById(int Id, CancellationToken cancellationToken)
        {
            var fabricDataById = await _unitOfWorkDA.FabricDA.GetById(Id, cancellationToken);
            if (fabricDataById == null)
            {
                throw new Exception("Fabric not found");
            }
            return fabricDataById;
        }

        private void UpdateData(Fabrics oldData)
        {
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
        }

        private static void MapToDbObject(FabricRequestDto model, Fabrics oldData)
        {
            oldData.Title = model.Title;
            oldData.ModelNo = model.ModelNo;
            oldData.CompanyId = model.CompanyId;
            oldData.UnitId = model.UnitId;
            oldData.UnitPrice = model.UnitPrice;
            oldData.ImagePath = model.ImagePath;
        }

        #endregion Private Method
    }
}