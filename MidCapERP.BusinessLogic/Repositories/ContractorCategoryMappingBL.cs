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
            var DataToReturn = _mapper.Map<List<ContractorCategoryMappingResponseDto>>(data.ToList());
            return DataToReturn;
        }

        public async Task<ContractorCategoryMappingRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.ContractorCategoryMappingDA.GetById(Id, cancellationToken);
            if (data == null)
            {
                throw new Exception("ContractorCategoryMapping not found");
            }
            return _mapper.Map<ContractorCategoryMappingRequestDto>(data);
        }

        public async Task<ContractorCategoryMappingRequestDto> CreateContractorCategoryMapping(ContractorCategoryMappingRequestDto model, CancellationToken cancellationToken)
        {
            var ContractorCategoryToInsert = _mapper.Map<ContractorCategoryMapping>(model);
            ContractorCategoryToInsert.ContractorId = model.ContractorId;
            ContractorCategoryToInsert.CategoryId =model.CategoryId;
            ContractorCategoryToInsert.IsDeleted = false;
            ContractorCategoryToInsert.CreatedDate = DateTime.Now;
            ContractorCategoryToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.ContractorCategoryMappingDA.CreateContractorCategoryMapping(ContractorCategoryToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<ContractorCategoryMappingRequestDto>(data);
            return _mappedUser;
        }

        public async Task<ContractorCategoryMappingRequestDto> UpdateContractorCategoryMapping(int Id, ContractorCategoryMappingRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await _unitOfWorkDA.ContractorCategoryMappingDA.GetById(Id, cancellationToken);
            if (oldData == null)
            {
                throw new Exception("ContractorCategoryMapping not found");
            }
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
            oldData.IsDeleted = model.IsDeleted;
        }

        public async Task<ContractorCategoryMappingRequestDto> DeleteContractorCategoryMapping(int Id, CancellationToken cancellationToken)
        {
            var lContractorCategoryToUpdate = await _unitOfWorkDA.ContractorCategoryMappingDA.GetById(Id, cancellationToken);
            if (lContractorCategoryToUpdate == null)
            {
                throw new Exception("ContractorCategoryMappingI not found");
            }
            lContractorCategoryToUpdate.IsDeleted = true;
            lContractorCategoryToUpdate.UpdatedDate = DateTime.Now;
            lContractorCategoryToUpdate.UpdatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.ContractorCategoryMappingDA.UpdateContractorCategoryMapping(Id, lContractorCategoryToUpdate, cancellationToken);
            var _mappedUser = _mapper.Map<ContractorCategoryMappingRequestDto>(data);
            return _mappedUser;
        }
    }
}