using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.TenantSMTPDetail;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class TenantSMTPDetailBL : ITenantSMTPDetailBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public TenantSMTPDetailBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<TenantSMTPDetailResponseDto> TenantSMTPDetailGetById(long Id, CancellationToken cancellationToken)
        {
            var data = await GetById(Id, cancellationToken);
            return _mapper.Map<TenantSMTPDetailResponseDto>(data);
        }

        public async Task<TenantSMTPDetailRequestDto> UpdateTenantSMTPDetail(TenantSMTPDetailRequestDto model, CancellationToken cancellationToken)
        {
            long Id = model.TenantSMTPDetailId;
            var oldData = await GetById(Id, cancellationToken);
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
            MapToDbObject(model, ref oldData);        
            var data = await _unitOfWorkDA.TenantSMTPDetailDA.UpdateTenantSMTPDetail(oldData.TenantSMTPDetailId,  oldData, cancellationToken);
            return _mapper.Map<TenantSMTPDetailRequestDto>(data);
        }

        #region PrivateMethods

        private static void MapToDbObject(TenantSMTPDetailRequestDto model, ref TenantSMTPDetail  oldData)
        {
            oldData.FromEmail = model.FromEmail;
            oldData.Username = model.Username;
            oldData.Password = model.Password;
            oldData.SMTPServer = model.SMTPServer;
            oldData.SMTPPort = model.SMTPPort;
            oldData.EnableSSL = model.EnableSSL;
        }

        private async Task<TenantSMTPDetail> GetById(long Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.TenantSMTPDetailDA.TenantSMTPDetailGetById(Id, cancellationToken);
            if (data == null)
            {
                throw new Exception("Tenant not found");
            }
            return data;
        }

        #endregion PrivateMethods
    }
}