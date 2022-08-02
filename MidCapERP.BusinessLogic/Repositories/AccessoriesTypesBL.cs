using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.AccessoriesTypes;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Paging;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class AccessoriesTypesBL : IAccessoriesTypesBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public AccessoriesTypesBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<AccessoriesTypesResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.AccessoriesTypesDA.GetAll(cancellationToken);
            var dataToReturn = _mapper.Map<List<AccessoriesTypesResponseDto>>(data.ToList());
            return dataToReturn;
        }


        public async Task<AccessoriesTypesResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken)
        {
            var data = await AccessoriesTypesGetById(Id, cancellationToken);
            return _mapper.Map<AccessoriesTypesResponseDto>(data);
        }

        public async Task<JsonRepsonse<AccessoriesTypesResponseDto>> GetFilterAccessoriesTypesData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var accessoriesTypesAllData = await _unitOfWorkDA.AccessoriesTypesDA.GetAll(cancellationToken);
            var lookupValueData = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            var companyResponseData = (from x in accessoriesTypesAllData
                                       join y in lookupValueData on new { CategoryId = x.CategoryId } equals new { CategoryId = y.LookupValueId }
                                       select new AccessoriesTypesResponseDto()
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

                                       }).ToList();
            var companyData = new PagedList<AccessoriesTypesResponseDto>(companyResponseData, dataTableFilterDto.Start, dataTableFilterDto.PageSize);
            //var companyResponseData = _mapper.Map<List<CompanyResponseDto>>(companyData);
            return new JsonRepsonse<AccessoriesTypesResponseDto>(dataTableFilterDto.Draw, companyData.TotalCount, companyData.TotalCount, companyData);
        }

        public async Task<AccessoriesTypesRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var data = await AccessoriesTypesGetById(Id, cancellationToken);
            return _mapper.Map<AccessoriesTypesRequestDto>(data);
        }

        public async Task<AccessoriesTypesRequestDto> CreateAccessoriesTypes(AccessoriesTypesRequestDto model, CancellationToken cancellationToken)
        {
            var AccessoriesTypesToInsert = _mapper.Map<AccessoriesTypes>(model);
            AccessoriesTypesToInsert.IsDeleted = false;
            AccessoriesTypesToInsert.TenantId = _currentUser.TenantId;
            AccessoriesTypesToInsert.CreatedBy = _currentUser.UserId;
            AccessoriesTypesToInsert.CreatedDate = DateTime.Now;
            AccessoriesTypesToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.AccessoriesTypesDA.CreateAccessoriesTypes(AccessoriesTypesToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<AccessoriesTypesRequestDto>(data);
            return _mappedUser;
        }

        public async Task<AccessoriesTypesRequestDto> UpdateAccessoriesTypes(int Id, AccessoriesTypesRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = AccessoriesTypesGetById(Id, cancellationToken).Result;
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.AccessoriesTypesDA.UpdateAccessoriesTypes(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<AccessoriesTypesRequestDto>(data);
            return _mappedUser;
        }
       

        private static void MapToDbObject(AccessoriesTypesRequestDto model, AccessoriesTypes oldData)
        {
            oldData.CategoryId = model.CategoryId;
            oldData.TypeName = model.TypeName;
            oldData.TenantId = model.TenantId;
        }

        public async Task<AccessoriesTypesRequestDto> DeleteAccessoriesTypes(int Id, CancellationToken cancellationToken)
        {
            var accessoriesTypesToUpdate = AccessoriesTypesGetById(Id, cancellationToken).Result;
            accessoriesTypesToUpdate.IsDeleted = true;
            accessoriesTypesToUpdate.UpdatedBy = _currentUser.UserId;
            accessoriesTypesToUpdate.UpdatedDate = DateTime.Now;
            accessoriesTypesToUpdate.UpdatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.AccessoriesTypesDA.UpdateAccessoriesTypes(Id, accessoriesTypesToUpdate, cancellationToken);
            var _mappedUser = _mapper.Map<AccessoriesTypesRequestDto>(data);
            return _mappedUser;
        }

        #region otherMethod

        private async Task<AccessoriesTypes> AccessoriesTypesGetById(int Id, CancellationToken cancellationToken)
        {
            var accessoriesTypesDataById = await _unitOfWorkDA.AccessoriesTypesDA.GetById(Id, cancellationToken);
            if (accessoriesTypesDataById == null)
            {
                throw new Exception("AccessoriesType not found");
            }
            return accessoriesTypesDataById;
        }

        #endregion otherMethod
    }
}