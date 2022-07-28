using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.ContractorCategoryMapping;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class ContractorCategoryMappingBL : IContractorCategoryMappingBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public ContractorCategoryMappingBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<ContractorCategoryMappingResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.ContractorCategoryMappingDA.GetAll(cancellationToken);
            var dataToReturn = _mapper.Map<List<ContractorCategoryMappingResponseDto>>(data.ToList());
            return dataToReturn;
        }

        public async Task<ContractorCategoryMappingRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var data = await ContractorCategoryMappingGetById(Id, cancellationToken);
            return _mapper.Map<ContractorCategoryMappingRequestDto>(data);
        }

        public async Task<ContractorCategoryMappingRequestDto> CreateContractorCategoryMapping(ContractorCategoryMappingRequestDto model, CancellationToken cancellationToken)
        {
            var contractorCategoryToInsert = _mapper.Map<ContractorCategoryMapping>(model);
            contractorCategoryToInsert.ContractorId = model.ContractorId;
            contractorCategoryToInsert.CategoryId = model.CategoryId;
            contractorCategoryToInsert.IsDeleted = false;
            contractorCategoryToInsert.CreatedDate = DateTime.Now;
            contractorCategoryToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.ContractorCategoryMappingDA.CreateContractorCategoryMapping(contractorCategoryToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<ContractorCategoryMappingRequestDto>(data);
            return _mappedUser;
        }

        public async Task<ContractorCategoryMappingRequestDto> UpdateContractorCategoryMapping(int Id, ContractorCategoryMappingRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await ContractorCategoryMappingGetById(Id, cancellationToken);
            oldData.ContractorId = model.ContractorId;
            oldData.CategoryId = model.CategoryId;
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.ContractorCategoryMappingDA.UpdateContractorCategoryMapping(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<ContractorCategoryMappingRequestDto>(data);
            return _mappedUser;
        }

        private static void MapToDbObject(ContractorCategoryMappingRequestDto model, ContractorCategoryMapping oldData)
        {
            oldData.ContractorCategoryMappingId = model.ContractorCategoryMappingId;
        }

        public async Task<ContractorCategoryMappingRequestDto> DeleteContractorCategoryMapping(int Id, CancellationToken cancellationToken)
        {
            var contractorCategoryToUpdate = await ContractorCategoryMappingGetById(Id, cancellationToken);
            contractorCategoryToUpdate.IsDeleted = true;
            contractorCategoryToUpdate.UpdatedDate = DateTime.Now;
            contractorCategoryToUpdate.UpdatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.ContractorCategoryMappingDA.UpdateContractorCategoryMapping(Id, contractorCategoryToUpdate, cancellationToken);
            var _mappedUser = _mapper.Map<ContractorCategoryMappingRequestDto>(data);
            return _mappedUser;
        }

        #region PrivateMethods

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