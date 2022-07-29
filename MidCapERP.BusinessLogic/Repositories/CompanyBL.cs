using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Company;
using MidCapERP.Dto.Constants;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Paging;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class CompanyBL : ICompanyBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public CompanyBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<CompanyResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            var dataToReturn = _mapper.Map<List<CompanyResponseDto>>(data.ToList());
            return dataToReturn;
        }

        public async Task<JsonRepsonse<CompanyResponseDto>> GetFilterCompanyData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var companyAllData = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            var companyResponseData = (from x in companyAllData
                                       join y in _unitOfWorkDA.LookupsDA.GetAll(cancellationToken).Result
                                            on new { x.LookupId } equals new { y.LookupId }
                                       where x.LookupId == (int)MasterPagesEnum.Company
                                       select new CompanyResponseDto()
                                       {
                                           LookupValueId = x.LookupValueId,
                                           LookupValueName = x.LookupValueName,
                                           LookupId = x.LookupId,
                                           LookupName = y.LookupName,
                                           IsDeleted = x.IsDeleted,
                                           CreatedBy = x.CreatedBy,
                                           CreatedDate = x.CreatedDate,
                                           CreatedUTCDate = x.CreatedUTCDate,
                                           UpdatedBy = x.UpdatedBy,
                                           UpdatedDate = x.UpdatedDate,
                                           UpdatedUTCDate = x.UpdatedUTCDate
                                       }).ToList();
            var companyData = new PagedList<CompanyResponseDto>(companyResponseData, dataTableFilterDto.Start, dataTableFilterDto.PageSize);
            //var companyResponseData = _mapper.Map<List<CompanyResponseDto>>(companyData);
            return new JsonRepsonse<CompanyResponseDto>(dataTableFilterDto.Draw, companyData.TotalCount, companyData.TotalCount, companyResponseData);
        }

        public async Task<CompanyResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken)
        {
            var data = await GetCompanyById(Id, cancellationToken);
            return _mapper.Map<CompanyResponseDto>(data);
        }

        public async Task<CompanyRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var data = await GetCompanyById(Id, cancellationToken);
            return _mapper.Map<CompanyRequestDto>(data);
        }

        public async Task<CompanyRequestDto> CreateCompany(CompanyRequestDto model, CancellationToken cancellationToken)
        {
            var companyToInsert = _mapper.Map<LookupValues>(model);
            companyToInsert.IsDeleted = false;
            companyToInsert.CreatedBy = _currentUser.UserId;
            companyToInsert.CreatedDate = DateTime.Now;
            companyToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.LookupValuesDA.CreateLookupValue(companyToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<CompanyRequestDto>(data);
            return _mappedUser;
        }

        public async Task<CompanyRequestDto> UpdateCompany(int Id, CompanyRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await GetCompanyById(Id, cancellationToken);
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.LookupValuesDA.UpdateLookupValue(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<CompanyRequestDto>(data);
            return _mappedUser;
        }

        private static void MapToDbObject(CompanyRequestDto model, LookupValues oldData)
        {
            oldData.LookupValueName = model.LookupValueName;
            oldData.LookupValueId = model.LookupValueId;
        }

        public async Task<CompanyRequestDto> DeleteCompany(int Id, CancellationToken cancellationToken)
        {
            var companyToUpdate = await GetCompanyById(Id, cancellationToken);
            companyToUpdate.IsDeleted = true;
            companyToUpdate.UpdatedBy = _currentUser.UserId;
            companyToUpdate.UpdatedDate = DateTime.Now;
            companyToUpdate.UpdatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.LookupValuesDA.UpdateLookupValue(Id, companyToUpdate, cancellationToken);
            var _mappedUser = _mapper.Map<CompanyRequestDto>(data);
            return _mappedUser;
        }

        #region PrivateMethods

        private async Task<LookupValues> GetCompanyById(int Id, CancellationToken cancellationToken)
        {
            var companyDataById = await _unitOfWorkDA.LookupValuesDA.GetById(Id, cancellationToken);
            if (companyDataById == null)
            {
                throw new Exception("LookupValues not found");
            }
            return companyDataById;
        }

        #endregion PrivateMethods
    }
}