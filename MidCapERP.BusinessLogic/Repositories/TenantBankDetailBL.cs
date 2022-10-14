﻿using AutoMapper;
using MidCapERP.BusinessLogic.Constants;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Services.FileStorage;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Paging;
using MidCapERP.Dto.TenantBankDetail;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class TenantBankDetailBL : ITenantBankDetailBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;
        private readonly IFileStorageService _fileStorageService;

        public TenantBankDetailBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser, IFileStorageService fileStorageService)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
            _fileStorageService = fileStorageService;
        }

        public async Task<IEnumerable<TenantBankDetailResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.TenantBankDetailDA.GetAll(cancellationToken);
            return _mapper.Map<List<TenantBankDetailResponseDto>>(data.ToList());
        }

        public async Task<JsonRepsonse<TenantBankDetailResponseDto>> GetFilterTenantBankDetailData(TenantBankDetailDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var tenantBankDetailAllData = await _unitOfWorkDA.TenantBankDetailDA.GetAll(cancellationToken);
            var tenantBankDetailResponseData = (from x in tenantBankDetailAllData
                                                select new TenantBankDetailResponseDto()
                                                {
                                                    TenantBankDetailId = Convert.ToInt16(x.TenantBankDetailId),
                                                    TenantId = Convert.ToInt16(x.TenantId),
                                                    BankName = x.BankName,
                                                    AccountName = x.AccountName,
                                                    AccountNo = x.AccountNo,
                                                    BranchName = x.BranchName,
                                                    AccountType = x.AccountType,
                                                    IFSCCode = x.IFSCCode,
                                                    UPIId = x.UPIId,
                                                    QRCode = x.QRCode,
                                                }).AsQueryable();
            var tenantBankDetailData = new PagedList<TenantBankDetailResponseDto>(tenantBankDetailResponseData, dataTableFilterDto);
            return new JsonRepsonse<TenantBankDetailResponseDto>(dataTableFilterDto.Draw, tenantBankDetailData.TotalCount, tenantBankDetailData.TotalCount, tenantBankDetailData);
        }

        public async Task<TenantBankDetailRequestDto> CreateTenantBankDetail(TenantBankDetailRequestDto model, CancellationToken cancellationToken)
        {
            var tenantBankDetailoInsert = _mapper.Map<TenantBankDetail>(model);
            if (model.QRImageUpload != null)
                tenantBankDetailoInsert.QRCode = await _fileStorageService.StoreFile(model.QRImageUpload, ApplicationFileStorageConstants.FilePaths.QRImageTenantBankDetail);
            tenantBankDetailoInsert.IsDeleted = false;
            tenantBankDetailoInsert.TenantId = _currentUser.TenantId;
            tenantBankDetailoInsert.CreatedBy = _currentUser.UserId;
            tenantBankDetailoInsert.CreatedDate = DateTime.Now;
            tenantBankDetailoInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.TenantBankDetailDA.CreateTenantBankDetail(tenantBankDetailoInsert, cancellationToken);
            return _mapper.Map<TenantBankDetailRequestDto>(data);
        }

        public async Task<TenantBankDetailRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var data = await TenantGetById(Id, cancellationToken);
            return _mapper.Map<TenantBankDetailRequestDto>(data);
        }

        public async Task<TenantBankDetailRequestDto> UpdateTenantBankDetail(int Id, TenantBankDetailRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await TenantGetById(Id, cancellationToken);
            if (model.QRImageUpload != null)
                model.QRCode = await _fileStorageService.StoreFile(model.QRImageUpload, ApplicationFileStorageConstants.FilePaths.QRImageTenantBankDetail);
            UpdateTenantBankDetail(oldData);
            MapToDbObject(model, ref oldData);
            var data = await _unitOfWorkDA.TenantBankDetailDA.UpdateTenantBankDetail(Id, oldData, cancellationToken);
            return _mapper.Map<TenantBankDetailRequestDto>(data);
        }

        public async Task<TenantBankDetailRequestDto> DeleteTenantBankDetail(int Id, CancellationToken cancellationToken)
        {
            var deleteTenantBankDetail = await TenantGetById(Id, cancellationToken);
            deleteTenantBankDetail.IsDeleted = true;
            deleteTenantBankDetail.UpdatedBy = _currentUser.UserId;
            deleteTenantBankDetail.UpdatedDate = DateTime.Now;
            deleteTenantBankDetail.UpdatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.TenantBankDetailDA.UpdateTenantBankDetail(Id, deleteTenantBankDetail, cancellationToken);
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

        private void UpdateTenantBankDetail(TenantBankDetail oldData)
        {
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
        }

        #endregion PrivateMethods
    }
}