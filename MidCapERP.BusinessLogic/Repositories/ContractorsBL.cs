using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Contractors;

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

        public async Task<IEnumerable<ContractorsResponseDto>> GetAllContractor(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.ContractorsDA.GetAllContractor(cancellationToken);
            var DataToReturn = _mapper.Map<List<ContractorsResponseDto>>(data.ToList());
            return DataToReturn;
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
            return _mappedUser;
        }

        public async Task<ContractorsRequestDto> UpdateContractor(int Id, ContractorsRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await GetContractorById(Id, cancellationToken);
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.ContractorsDA.UpdateContractor(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<ContractorsRequestDto>(data);
            return _mappedUser;
        }

        private static void MapToDbObject(ContractorsRequestDto model, Contractors oldData)
        {
            oldData.ContractorName = model.ContractorName;
            oldData.PhoneNumber = model.PhoneNumber;
            oldData.IMEI = model.IMEI;
            oldData.EmailId = model.EmailId;
        }

        public async Task<ContractorsRequestDto> DeleteContractor(int Id, CancellationToken cancellationToken)
        {
            var contractorToUpdate = await GetContractorById(Id, cancellationToken);
            contractorToUpdate.IsDeleted = true;
            contractorToUpdate.UpdatedDate = DateTime.Now;
            contractorToUpdate.UpdatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.ContractorsDA.UpdateContractor(Id, contractorToUpdate, cancellationToken);
            var _mappedUser = _mapper.Map<ContractorsRequestDto>(data);
            return _mappedUser;
        }

        #region PrivateMethods

        private async Task<Contractors> GetContractorById(int Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.ContractorsDA.GetById(Id, cancellationToken);
            if (data == null)
            {
                throw new Exception("Contractor not found");
            }
            return data;
        }

        #endregion PrivateMethods
    }
}