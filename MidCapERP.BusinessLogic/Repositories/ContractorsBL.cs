using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Constants;
using MidCapERP.Dto.ContractorCategoryMapping;
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
            var contracterResponseData = (from x in contractorAllData
                                          from y in categoryAllData
                                          select new ContractorsResponseDto()
                                          {
                                              ContractorId = x.ContractorId,
                                              ContractorName = x.ContractorName,
                                              PhoneNumber = x.PhoneNumber,
                                              IMEI = x.IMEI,
                                              EmailId = x.EmailId,
                                              CategoryName = y.LookupValueName,
                                              IsDeleted = x.IsDeleted,
                                              CreatedBy = x.CreatedBy,
                                              CreatedDate = x.CreatedDate,
                                              CreatedUTCDate = x.CreatedUTCDate,
                                              UpdatedBy = x.UpdatedBy,
                                              UpdatedDate = x.UpdatedDate,
                                              UpdatedUTCDate = x.UpdatedUTCDate
                                          }).AsQueryable();
            var contractorData = new PagedList<ContractorsResponseDto>(_mapper.Map<List<ContractorsResponseDto>>(contractorAllData).AsQueryable(), dataTableFilterDto);
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
            var data = await GetContractorById(Id, cancellationToken);
            //var contractorCategoryMapping = await _unitOfWorkDA.ContractorCategoryMappingDA.GetById(, cancellationToken);
            //var contractorCategoryMappingData = _mapper.Map<ContractorsRequestDto>(data);
            //contractorCategoryMappingData.CategoryId = contractorCategoryMapping.CategoryId;
            //return contractorCategoryMappingData;
            return null;
        }

        public async Task<ContractorsRequestDto> CreateContractor(ContractorsRequestDto model, CancellationToken cancellationToken)
        {
            var contractorToInsert = _mapper.Map<Contractors>(model);
            contractorToInsert.IsDeleted = false;
            contractorToInsert.TenantId = _currentUser.TenantId;
            contractorToInsert.CreatedBy = _currentUser.UserId;
            contractorToInsert.CreatedDate = DateTime.Now;
            contractorToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.ContractorsDA.CreateContractor(contractorToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<ContractorsRequestDto>(data);

            //data.ContractorID
            ContractorCategoryMappingRequestDto catDto = new ContractorCategoryMappingRequestDto();
            catDto.CategoryId = model.CategoryId;
            catDto.ContractorId = data.ContractorId;
            var contractorCategoryMappingToInsert = _mapper.Map<ContractorCategoryMapping>(catDto);
            contractorCategoryMappingToInsert.ContractorId = data.ContractorId;
            contractorCategoryMappingToInsert.CreatedDate = DateTime.Now;
            contractorCategoryMappingToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data1 = await _unitOfWorkDA.ContractorCategoryMappingDA.CreateContractorCategoryMapping(contractorCategoryMappingToInsert, cancellationToken);
            var _mappedUser1 = _mapper.Map<ContractorCategoryMappingRequestDto>(data1);
            return _mappedUser;
        }

        public async Task<ContractorsRequestDto> UpdateContractor(int Id, ContractorsRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await GetContractorById(Id, cancellationToken);
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
            UpdateContractor(oldData);
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.ContractorsDA.UpdateContractor(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<ContractorsRequestDto>(data);
            /*
           //update CategoryId
           ContractorCategoryMappingRequestDto catDto = new ContractorCategoryMappingRequestDto();
           var oldData1 = await ContractorCategoryMappingGetById(Id, cancellationToken);
           if (catDto.ContractorId == model.ContractorId)
           {
               var contracterCategoryMapping = await _unitOfWorkDA.ContractorCategoryMappingDA.UpdateContractorCategoryMapping(Id, oldData1, cancellationToken);
               MapToDbObject(catDto, oldData1);
               UpdateContractorCategory(oldData1);
               var _mappedUser1 = _mapper.Map<ContractorsRequestDto>(contracterCategoryMapping);
               return _mappedUser1;
           }*/
            return _mappedUser;
        }

        public async Task<ContractorsRequestDto> DeleteContractor(int Id, CancellationToken cancellationToken)
        {
            var contractorToUpdate = await GetContractorById(Id, cancellationToken);
            contractorToUpdate.IsDeleted = true;
            UpdateContractor(contractorToUpdate);
            var data = await _unitOfWorkDA.ContractorsDA.UpdateContractor(Id, contractorToUpdate, cancellationToken);
            var _mappedUser = _mapper.Map<ContractorsRequestDto>(data);
            return _mappedUser;
        }

        #region PrivateMethods

        private static void MapToDbObject(ContractorCategoryMappingRequestDto model, ContractorCategoryMapping oldData)
        {
            oldData.CategoryId = model.CategoryId;
        }

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
            var contractorCategoryMappingDataById = await _unitOfWorkDA.ContractorCategoryMappingDA.GetById(Id, cancellationToken);
            if (contractorCategoryMappingDataById == null)
            {
                throw new Exception("ContractorCategoryMapping not found");
            }
            return contractorCategoryMappingDataById;
        }

        #endregion PrivateMethods
    }
}