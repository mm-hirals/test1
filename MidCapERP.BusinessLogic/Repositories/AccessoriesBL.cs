using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Services.FileStorage;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Accessories;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Paging;


namespace MidCapERP.BusinessLogic.Repositories
{
    public class AccessoriesBL : IAccessoriesBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;
        private readonly IFileStorageService _fileStorageService;

        public AccessoriesBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser, IFileStorageService fileStorageService)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
            _fileStorageService = fileStorageService;
        }

        public async Task<IEnumerable<AccessoriesResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.AccessoriesDA.GetAll(cancellationToken);
            var dataToReturn = _mapper.Map<List<AccessoriesResponseDto>>(data.ToList());
            return dataToReturn;
        }

        public async Task<AccessoriesResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken)
        {
            var data = await AccessoriesGetById(Id, cancellationToken);
            return _mapper.Map<AccessoriesResponseDto>(data);
        }

        public async Task<JsonRepsonse<AccessoriesResponseDto>> GetFilterAccessoriesData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var accessoriesAllData = await _unitOfWorkDA.AccessoriesDA.GetAll(cancellationToken);
            var accessoriesTypesAllData = await _unitOfWorkDA.AccessoriesTypesDA.GetAll(cancellationToken);
            var cateagoryData = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            var unitData = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);

            var companyResponseData = (from x in accessoriesAllData
                                       join y in accessoriesTypesAllData on new { AccessoriesTypeId = x.AccessoriesTypeId } equals new { AccessoriesTypeId = y.AccessoriesTypeId }
                                       join z in cateagoryData on new { CategoryId = x.CategoryId } equals new { CategoryId = z.LookupValueId }
                                       join a in unitData on new { UnitId = x.UnitId } equals new { UnitId = a.LookupValueId }
                                       select new AccessoriesResponseDto()
                                       {
                                           AccessoriesId = x.AccessoriesId,
                                           CategoryId = x.CategoryId,
                                           CategoryName = z.LookupValueName,
                                           TypeName = y.TypeName,
                                           AccessoriesTypeName = y.TypeName,
                                           Title = x.Title,
                                           UnitId = x.UnitId,
                                           UnitName = a.LookupValueName,
                                           UnitPrice = Convert.ToInt32(x.UnitPrice),
                                           IsDeleted = x.IsDeleted,
                                           CreatedBy = x.CreatedBy,
                                           CreatedDate = x.CreatedDate,
                                           CreatedUTCDate = x.CreatedUTCDate,
                                           UpdatedBy = x.UpdatedBy,
                                           UpdatedDate = x.UpdatedDate,
                                           UpdatedUTCDate = x.UpdatedUTCDate
                                       }).ToList();
            var companyData = new PagedList<AccessoriesResponseDto>(companyResponseData, dataTableFilterDto.Start, dataTableFilterDto.PageSize);
            //var companyResponseData = _mapper.Map<List<CompanyResponseDto>>(companyData);
            return new JsonRepsonse<AccessoriesResponseDto>(dataTableFilterDto.Draw, companyData.TotalCount, companyData.TotalCount, companyData);
        }

        public async Task<AccessoriesRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var data = await AccessoriesGetById(Id, cancellationToken);
            return _mapper.Map<AccessoriesRequestDto>(data);
        }

        public async Task<AccessoriesRequestDto> CreateAccessories(AccessoriesRequestDto model, CancellationToken cancellationToken)
        {
            var AccessoriesToInsert = _mapper.Map<Accessories>(model);
            if (model.UploadImage != null)
                AccessoriesToInsert.ImagePath = await _fileStorageService.StoreFile(model.UploadImage, @"\Files\Accessories\");
            AccessoriesToInsert.IsDeleted = false;
            AccessoriesToInsert.TenantId = _currentUser.TenantId;
            AccessoriesToInsert.CreatedBy = _currentUser.UserId;
            AccessoriesToInsert.CreatedDate = DateTime.Now;
            AccessoriesToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.AccessoriesDA.CreateAccessories(AccessoriesToInsert, cancellationToken);

            var _mappedUser = _mapper.Map<AccessoriesRequestDto>(data);
            return _mappedUser;

        }

        private async Task<Accessories> AccessoriesGetById(int Id, CancellationToken cancellationToken)
        {
            var accessoriesDataById = await _unitOfWorkDA.AccessoriesDA.GetById(Id, cancellationToken);
            if (accessoriesDataById == null)
            {
                throw new Exception("Accessories not found");
            }
            return accessoriesDataById;
        }

        public async Task<AccessoriesRequestDto> UpdateAccessories(int Id, AccessoriesRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = AccessoriesGetById(Id, cancellationToken).Result;
            if (model.UploadImage != null)
                model.ImagePath = await _fileStorageService.StoreFile(model.UploadImage, @"\Files\Accessories\");
            UpdateAccessories(oldData);
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.AccessoriesDA.UpdateAccessories(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<AccessoriesRequestDto>(data);
            return _mappedUser;
        }

        private static void MapToDbObject(AccessoriesRequestDto model, Accessories oldData)
        {
            oldData.AccessoriesId = model.AccessoriesId;
            oldData.AccessoriesTypeId = model.AccessoriesTypeId;
            oldData.CategoryId = model.CategoryId;
            oldData.Title = model.Title;
            oldData.UnitId = model.UnitId;
            oldData.UnitPrice = model.UnitPrice;
            oldData.ImagePath = model.ImagePath;
        }

        public async Task<AccessoriesRequestDto> DeleteAccessories(int Id, CancellationToken cancellationToken)
        {
            var accessoriesToUpdate = AccessoriesGetById(Id, cancellationToken).Result;
            accessoriesToUpdate.IsDeleted = true;
            UpdateAccessories(accessoriesToUpdate);
            var data = await _unitOfWorkDA.AccessoriesDA.UpdateAccessories(Id, accessoriesToUpdate, cancellationToken);
            var _mappedUser = _mapper.Map<AccessoriesRequestDto>(data);
            return _mappedUser;
        }

        public void UpdateAccessories(Accessories oldData)
        {
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
        }
    }
}
