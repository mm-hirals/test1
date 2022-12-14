using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.AccessoriesType;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Paging;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class AccessoriesTypeBL : IAccessoriesTypeBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public AccessoriesTypeBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<AccessoriesTypeResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.AccessoriesTypeDA.GetAll(cancellationToken);
            var dataToReturn = _mapper.Map<List<AccessoriesTypeResponseDto>>(data.ToList());
            return dataToReturn;
        }

        public async Task<JsonRepsonse<AccessoriesTypeResponseDto>> GetFilterAccessoriesTypeData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var accessoriesTypesAllData = await _unitOfWorkDA.AccessoriesTypeDA.GetAll(cancellationToken);
            var lookupValueData = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            var accessoriesTypeResponseData = (from x in accessoriesTypesAllData
                                               join y in lookupValueData on new { CategoryId = x.CategoryId } equals new { CategoryId = y.LookupValueId }
                                               select new AccessoriesTypeResponseDto()
                                               {
                                                   AccessoriesTypeId = x.AccessoriesTypeId,
                                                   CategoryId = x.CategoryId,
                                                   CategoryName = y.LookupValueName,
                                                   TypeName = x.TypeName,
                                                   IsDeleted = x.IsDeleted,
                                                   CreatedBy = x.CreatedBy,
                                                   CreatedDate = x.CreatedDate,
                                                   CreatedUTCDate = x.CreatedUTCDate,
                                                   UpdatedBy = x.UpdatedBy,
                                                   UpdatedDate = x.UpdatedDate,
                                                   UpdatedUTCDate = x.UpdatedUTCDate
                                               }).AsQueryable();
            var companyData = new PagedList<AccessoriesTypeResponseDto>(accessoriesTypeResponseData, dataTableFilterDto);
            return new JsonRepsonse<AccessoriesTypeResponseDto>(dataTableFilterDto.Draw, companyData.TotalCount, companyData.TotalCount, companyData);
        }

        public async Task<AccessoriesTypeResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken)
        {
            var data = await AccessoriesTypeGetById(Id, cancellationToken);
            return _mapper.Map<AccessoriesTypeResponseDto>(data);
        }

        public async Task<AccessoriesTypeRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var data = await AccessoriesTypeGetById(Id, cancellationToken);
            return _mapper.Map<AccessoriesTypeRequestDto>(data);
        }

        public async Task<AccessoriesTypeRequestDto> CreateAccessoriesType(AccessoriesTypeRequestDto model, CancellationToken cancellationToken)
        {
            var accessoriesTypeToInsert = _mapper.Map<AccessoriesType>(model);
            accessoriesTypeToInsert.IsDeleted = false;
            accessoriesTypeToInsert.TenantId = _currentUser.TenantId;
            accessoriesTypeToInsert.CreatedBy = _currentUser.UserId;
            accessoriesTypeToInsert.CreatedDate = DateTime.Now;
            accessoriesTypeToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.AccessoriesTypeDA.CreateAccessoriesType(accessoriesTypeToInsert, cancellationToken);
            return _mapper.Map<AccessoriesTypeRequestDto>(data);

        }

        public async Task<AccessoriesTypeRequestDto> UpdateAccessoriesType(int Id, AccessoriesTypeRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await AccessoriesTypeGetById(Id, cancellationToken);
            UpdateAccessoriesType(oldData);
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.AccessoriesTypeDA.UpdateAccessoriesType(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<AccessoriesTypeRequestDto>(data);
            return _mappedUser;
        }

        public async Task<AccessoriesTypeRequestDto> DeleteAccessoriesType(int Id, CancellationToken cancellationToken)
        {
            var accessoriesTypesToUpdate = await AccessoriesTypeGetById(Id, cancellationToken);
            accessoriesTypesToUpdate.IsDeleted = true;
            UpdateAccessoriesType(accessoriesTypesToUpdate);
            var data = await _unitOfWorkDA.AccessoriesTypeDA.UpdateAccessoriesType(Id, accessoriesTypesToUpdate, cancellationToken);
            var _mappedUser = _mapper.Map<AccessoriesTypeRequestDto>(data);
            return _mappedUser;
        }

        public void UpdateAccessoriesType(AccessoriesType oldData)
        {
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
        }

        #region PrivateMethods

        private static void MapToDbObject(AccessoriesTypeRequestDto model, AccessoriesType oldData)
        {
            oldData.CategoryId = model.CategoryId;
            oldData.TypeName = model.TypeName;
            oldData.TenantId = model.TenantId;
        }

        private async Task<AccessoriesType> AccessoriesTypeGetById(int Id, CancellationToken cancellationToken)
        {
            var accessoriesTypesDataById = await _unitOfWorkDA.AccessoriesTypeDA.GetById(Id, cancellationToken);
            if (accessoriesTypesDataById == null)
            {
                throw new Exception("AccessoriesType not found");
            }
            return accessoriesTypesDataById;
        }

        #endregion PrivateMethods
    }
}