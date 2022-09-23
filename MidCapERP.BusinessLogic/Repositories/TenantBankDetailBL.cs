using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.TenantBankDetail;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class TenantBankDetailBL : ITenantBankDetailBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public TenantBankDetailBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<TenantBankDetailResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.TenantBankDetailDA.GetAll(cancellationToken);
            return _mapper.Map<List<TenantBankDetailResponseDto>>(data.ToList());
        }


        public async Task<TenantBankDetailRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var data = await TenantGetById(Id, cancellationToken);
            return _mapper.Map<TenantBankDetailRequestDto>(data);
        }

        public async Task<TenantBankDetailRequestDto> UpdateTenantBankDetail(int Id, TenantBankDetailRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await TenantGetById(Id, cancellationToken);
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
            MapToDbObject(model, ref oldData);
            var data = await _unitOfWorkDA.TenantBankDetailDA.UpdateTenantBankDetail(Id, oldData, cancellationToken);
            return _mapper.Map<TenantBankDetailRequestDto>(data);
        }

        #region PrivateMethods

        private static void MapToDbObject(TenantBankDetailRequestDto model, ref TenantBankDetail oldData)
        {
            oldData.TenantId = model.TenantId;
            oldData.BankName = model.BankName;
            oldData.AccountName = model.AccountName;
            oldData.AccountNo = model.AccountNo;
            oldData.BranchName = model.BranchName;
            oldData.AccountType = model.AccountType;
            oldData.IFSCCode = model.IFSCCode;
            oldData.UPIId = model.UPIId;
            oldData.QRCode = model.QRCode;
        }

        private async Task<TenantBankDetail> TenantGetById(int Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.TenantBankDetailDA.GetById(Id, cancellationToken);
            if (data == null)
            {
                throw new Exception("Tenant not found");
            }
            return data;
        }

        #endregion PrivateMethods
    }
}