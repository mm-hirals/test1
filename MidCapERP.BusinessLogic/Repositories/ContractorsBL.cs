using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Constants;
using MidCapERP.Dto.Contractors;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Paging;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class ContractorsBL : IContractorsBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public ContractorsBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<ContractorsResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.ContractorsDA.GetAll(cancellationToken);
            return _mapper.Map<List<ContractorsResponseDto>>(data.ToList());
        }

        public async Task<JsonRepsonse<ContractorsResponseDto>> GetFilterContractorData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var contractorAllData = await _unitOfWorkDA.ContractorsDA.GetAll(cancellationToken);
            var lookupValueAllData = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            var categoryAllData = lookupValueAllData.Where(x => x.LookupId == (int)MasterPagesEnum.Category);
            var contractorCategoryMapping = await _unitOfWorkDA.ContractorCategoryMappingDA.GetAll(cancellationToken);
            var contractorResponseData = (from x in contractorAllData
                                          join y in contractorCategoryMapping on new { ContractorId = x.ContractorId } equals new { ContractorId = y.ContractorId }
                                          join z in categoryAllData on new { CategoryId = y.CategoryId } equals new { CategoryId = z.LookupValueId }
                                          select new ContractorsResponseDto()
                                          {
                                              ContractorId = x.ContractorId,
                                              ContractorName = x.ContractorName,
                                              PhoneNumber = x.PhoneNumber,
                                              IMEI = x.IMEI,
                                              EmailId = x.EmailId,
                                              CategoryName = z.LookupValueName,
                                              IsDeleted = x.IsDeleted,
                                              CreatedBy = x.CreatedBy,
                                              CreatedDate = x.CreatedDate,
                                              CreatedUTCDate = x.CreatedUTCDate,
                                              UpdatedBy = x.UpdatedBy,
                                              UpdatedDate = x.UpdatedDate,
                                              UpdatedUTCDate = x.UpdatedUTCDate
                                          }).AsQueryable();
            var contractorData = new PagedList<ContractorsResponseDto>(_mapper.Map<List<ContractorsResponseDto>>(contractorResponseData).AsQueryable(), dataTableFilterDto);
            return new JsonRepsonse<ContractorsResponseDto>(dataTableFilterDto.Draw, contractorData.TotalCount, contractorData.TotalCount, contractorData);
        }

        public async Task<ContractorsResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken)
        {
            Contractors data = await GetContractorById(Id, cancellationToken);
            return _mapper.Map<ContractorsResponseDto>(data);
        }

        public async Task<ContractorsRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            Contractors data = await GetContractorById(Id, cancellationToken);
            return _mapper.Map<ContractorsRequestDto>(data);
        }

        public async Task<ContractorsRequestDto> GetContractorCategoryMappingById(int Id, CancellationToken cancellationToken)
        {
            var contractorData = await GetContractorById(Id, cancellationToken);
            var contractCatAllData = await _unitOfWorkDA.ContractorCategoryMappingDA.GetAll(cancellationToken);
            var contractCatData = contractCatAllData.Where(x => x.ContractorId == Id).FirstOrDefault();
            var contractCatMappData = _mapper.Map<ContractorsRequestDto>(contractorData);
            contractCatMappData.CategoryId = contractCatData.CategoryId;
            return contractCatMappData;
        }

        public async Task<ContractorsRequestDto> CreateContractor(ContractorsRequestDto model, CancellationToken cancellationToken)
        {
            //Add Contractor
            var contractorToInsert = _mapper.Map<Contractors>(model);
            contractorToInsert.IsDeleted = false;
            contractorToInsert.TenantId = _currentUser.TenantId;
            contractorToInsert.CreatedBy = _currentUser.UserId;
            contractorToInsert.CreatedDate = DateTime.Now;
            contractorToInsert.CreatedUTCDate = DateTime.UtcNow;
            var contractor = await _unitOfWorkDA.ContractorsDA.CreateContractor(contractorToInsert, cancellationToken);
            
            //Add ContractorCateagoryMapping based on Contractor
            ContractorCategoryMapping catDto = new ContractorCategoryMapping();
            catDto.CategoryId = model.CategoryId;
            catDto.ContractorId = contractor.ContractorId;
            catDto.CreatedDate = DateTime.Now;
            catDto.CreatedUTCDate = DateTime.UtcNow;
            await _unitOfWorkDA.ContractorCategoryMappingDA.CreateContractorCategoryMapping(catDto, cancellationToken);
            
            return _mapper.Map<ContractorsRequestDto>(contractor);
        }

        public async Task<ContractorsRequestDto> UpdateContractor(int Id, ContractorsRequestDto model, CancellationToken cancellationToken)
        {
            //Update ContractorData
            var oldContractorData = await GetContractorById(Id, cancellationToken);
            UpdateContractor(oldContractorData);
            MapToDbObject(model, oldContractorData);
            var contractorData = await _unitOfWorkDA.ContractorsDA.UpdateContractor(Id, oldContractorData, cancellationToken);

            //Find ContractorCateagoryMapping if cateagory is changed then update ContractorCateagoryMapping 
            
            var categoryOldData = await ContractorCategoryMappingGetById(Id, cancellationToken);
            if (model.CategoryId != categoryOldData.CategoryId)
            {
                categoryOldData.CategoryId = model.CategoryId;
                UpdateContractorCategory(categoryOldData);
                await _unitOfWorkDA.ContractorCategoryMappingDA.UpdateContractorCategoryMapping(Id, categoryOldData, cancellationToken);
            }

            return _mapper.Map<ContractorsRequestDto>(contractorData);
        }

        public async Task<ContractorsRequestDto> DeleteContractor(int Id, CancellationToken cancellationToken)
        {
            //Delete Contractor
            var contractorToUpdate = await GetContractorById(Id, cancellationToken);
            contractorToUpdate.IsDeleted = true;
            UpdateContractor(contractorToUpdate);
            var contractorData = await _unitOfWorkDA.ContractorsDA.UpdateContractor(Id, contractorToUpdate, cancellationToken);

            //Delete ContractorCategoryMapping Base on Contractor
            var contractorCategoryMapping = await ContractorCategoryMappingGetById(Id, cancellationToken);
            contractorCategoryMapping.IsDeleted = true;
            UpdateContractorCategory(contractorCategoryMapping);
            await _unitOfWorkDA.ContractorCategoryMappingDA.DeleteContractorCategoryMapping(Id, cancellationToken);
            
            return _mapper.Map<ContractorsRequestDto>(contractorData);
        }

        #region PrivateMethods

        private static void MapToDbObject(ContractorsRequestDto model, Contractors oldData)
        {
            oldData.ContractorName = model.ContractorName;
            oldData.PhoneNumber = model.PhoneNumber;
            oldData.IMEI = model.IMEI;
            oldData.EmailId = model.EmailId;
        }

        private void UpdateContractor(Contractors oldData)
        {
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
        }

        private void UpdateContractorCategory(ContractorCategoryMapping oldData)
        {
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
        }

        private async Task<Contractors> GetContractorById(int Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.ContractorsDA.GetById(Id, cancellationToken);
            if (data == null)
            {
                throw new Exception("Contractor not found");
            }
            return data;
        }

        private async Task<ContractorCategoryMapping> ContractorCategoryMappingGetById(int Id, CancellationToken cancellationToken)
        {
            var contractCatAllData = await _unitOfWorkDA.ContractorCategoryMappingDA.GetAll(cancellationToken);
            var contractorCategoryMappingDataById = contractCatAllData.Where(x => x.ContractorId == Id).FirstOrDefault();
            if (contractorCategoryMappingDataById == null)
            {
                throw new Exception("Contractor Category Mapping not found");
            }
            return contractorCategoryMappingDataById;
        }

        #endregion PrivateMethods
    }
}