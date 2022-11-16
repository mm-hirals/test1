using AutoMapper;
using MidCapERP.BusinessLogic.Constants;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Services.FileStorage;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Paging;
using MidCapERP.Dto.Tenant;
using MidCapERP.Dto.TenantSMTPDetail;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class TenantBL : ITenantBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;
        private readonly IFileStorageService _fileStorageService;

        public TenantBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser, IFileStorageService fileStorageService)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
            _fileStorageService = fileStorageService;
        }

        public async Task<IEnumerable<TenantResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            return _mapper.Map<List<TenantResponseDto>>(data.ToList());
        }

        public async Task<JsonRepsonse<TenantResponseDto>> GetFilterCustomersData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var TenantAllData = await _unitOfWorkDA.TenantDA.GetAll(cancellationToken);
            var TenantData = new PagedList<TenantResponseDto>(_mapper.Map<List<TenantResponseDto>>(TenantAllData).AsQueryable(), dataTableFilterDto);
            return new JsonRepsonse<TenantResponseDto>(dataTableFilterDto.Draw, TenantData.TotalCount, TenantData.TotalCount, TenantData);
        }

        public async Task<TenantResponseDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var data = _mapper.Map<TenantResponseDto>(await TenantGetById(Id, cancellationToken));
            data.tenantSMTPDetailResponseDto = _mapper.Map<TenantSMTPDetailResponseDto>(await _unitOfWorkDA.TenantSMTPDetailDA.TenantSMTPDetailGetById(Id, cancellationToken));
            return data;
        }

        public async Task<TenantRequestDto> UpdateTenant(TenantRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await TenantGetById(model.TenantId, cancellationToken);
            MapToDbObject(model, ref oldData);
            if (model.UploadImage != null)
                oldData.LogoPath = await _fileStorageService.StoreFile(model.UploadImage, ApplicationFileStorageConstants.FilePaths.OrganizationLogo);
            oldData.SendOTP = model.SendOtp;
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;

            var data = await _unitOfWorkDA.TenantDA.UpdateTenant(model.TenantId, oldData, cancellationToken);
            return _mapper.Map<TenantRequestDto>(data);
        }

        #region PrivateMethods

        private static void MapToDbObject(TenantRequestDto model, ref Tenant oldData)
        {
            oldData.TenantName = model.TenantName;
            oldData.FirstName = model.FirstName;
            oldData.EmailId = model.EmailId;
            oldData.PhoneNumber = model.PhoneNumber;
            oldData.StreetAddress1 = model.StreetAddress1;
            oldData.StreetAddress2 = model.StreetAddress2;
            oldData.Landmark = model.Landmark;
            oldData.City = model.City;
            oldData.State = model.State;
            oldData.Pincode = model.Pincode;
            oldData.WebsiteURL = model.WebsiteURL;
            oldData.TwitterURL = model.TwitterURL;
            oldData.FacebookURL = model.FacebookURL;
            oldData.InstagramURL = model.InstagramURL;
            oldData.GSTNo = model.GSTNo;
            oldData.ProductRSPPercentage = model.ProductRSPPercentage;
            oldData.ProductWSPPercentage = model.ProductWSPPercentage;
            oldData.ArchitectDiscount = model.ArchitectDiscount;
            oldData.FabricRSPPercentage = model.FabricRSPPercentage;
            oldData.FabricWSPPercentage = model.FabricWSPPercentage;
            oldData.AmountRoundMultiple = model.AmountRoundMultiple;
        }

        private async Task<Tenant> TenantGetById(int Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.TenantDA.GetById(Id, cancellationToken);
            if (data == null)
            {
                throw new Exception("Tenant not found");
            }
            return data;
        }

        #endregion PrivateMethods
    }
}