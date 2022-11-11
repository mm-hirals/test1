using AutoMapper;
using MidCapERP.BusinessLogic.Constants;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Services.SendSMS;
using MidCapERP.Core.Constants;
using MidCapERP.Core.Services.Email;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Interior;
using MidCapERP.Dto.NotificationManagement;
using MidCapERP.Dto.Paging;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class InteriorsBL : IInteriorsBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;
        private readonly ISendSMSservice _sendSMSservice;
        private readonly IEmailHelper _emailHelper;

        public InteriorsBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser, ISendSMSservice sendSMSservice, IEmailHelper emailHelper)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
            _sendSMSservice = sendSMSservice;
            _emailHelper = emailHelper;
        }

        public async Task<IEnumerable<InteriorResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var interiorAllData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            var interiorData = interiorAllData.Where(x => x.CustomerTypeId == (int)CustomerTypeEnum.Interior);
            return _mapper.Map<List<InteriorResponseDto>>(interiorData.ToList());
        }

        public async Task<JsonRepsonse<InteriorResponseDto>> GetFilterInteriorsData(InteriorDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var interiorAllData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            var interiorData = interiorAllData.Where(x => x.CustomerTypeId == (int)CustomerTypeEnum.Interior);
            var interiorFilteredData = FilterInteriorData(dataTableFilterDto, interiorData);
            var interiorGridData = new PagedList<InteriorResponseDto>(_mapper.Map<List<InteriorResponseDto>>(interiorFilteredData).AsQueryable(), dataTableFilterDto);
            return new JsonRepsonse<InteriorResponseDto>(dataTableFilterDto.Draw, interiorGridData.TotalCount, interiorGridData.TotalCount, interiorGridData);
        }

        public async Task<InteriorRequestDto> GetById(Int64 Id, CancellationToken cancellationToken)
        {
            var data = await InteriorGetById(Id, cancellationToken);
            return _mapper.Map<InteriorRequestDto>(data);
        }

        public async Task<InteriorRequestDto> CreateInteriors(InteriorRequestDto model, CancellationToken cancellationToken)
        {
            var interiorToInsert = _mapper.Map<Customers>(model);
            Customers data = null;
            if (model.CustomerAddressesRequestDto.State != null && model.CustomerAddressesRequestDto.Area != null && model.CustomerAddressesRequestDto.City != null && model.CustomerAddressesRequestDto.ZipCode != null)
                await _unitOfWorkDA.BeginTransactionAsync();
            try
            {
                interiorToInsert.RefferedBy = 0;
                interiorToInsert.CustomerTypeId = (int)CustomerTypeEnum.Interior;
                interiorToInsert.Discount = model.Discount;
                interiorToInsert.IsDeleted = false;
                interiorToInsert.TenantId = _currentUser.TenantId;
                interiorToInsert.CreatedBy = _currentUser.UserId;
                interiorToInsert.CreatedDate = DateTime.Now;
                interiorToInsert.CreatedUTCDate = DateTime.UtcNow;
                data = await _unitOfWorkDA.CustomersDA.CreateCustomers(interiorToInsert, cancellationToken);
                if (model.CustomerAddressesRequestDto.State != null && model.CustomerAddressesRequestDto.Area != null && model.CustomerAddressesRequestDto.City != null && model.CustomerAddressesRequestDto.ZipCode != null)
                    await SaveInteriorAddress(model, data, cancellationToken);
            }
            catch (Exception e)
            {
                await _unitOfWorkDA.rollbackTransactionAsync();
                throw new Exception("Interior data is not saved");
            }
            return _mapper.Map<InteriorRequestDto>(data);
        }

        public async Task<InteriorRequestDto> UpdateInteriors(Int64 Id, InteriorRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await InteriorGetById(Id, cancellationToken);
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.CustomersDA.UpdateCustomers(Id, oldData, cancellationToken);
            return _mapper.Map<InteriorRequestDto>(data);
        }

        public async Task SendSMSToInteriors(InteriorsSendSMSDto model, CancellationToken cancellationToken)
        {
            foreach (var item in model.CustomerList)
            {
                var interiorData = await _unitOfWorkDA.CustomersDA.GetById(item, cancellationToken);
                if (interiorData != null)
                {
                    NotificationManagementRequestDto notificationDto = new NotificationManagementRequestDto()
                    {
                        EntityTypeID = await _unitOfWorkDA.SubjectTypesDA.GetCustomerSubjectTypeId(cancellationToken),
                        EntityID = item,
                        NotificationType = "Greetings",
                        NotificationMethod = NotificationMethodConstant.Email,
                        MessageSubject = model.Subject,
                        MessageBody = model.Message,
                        ReceiverEmail = interiorData.EmailId,
                        ReceiverMobile = interiorData.PhoneNumber,
                        Status = 0,
                        CreatedBy = _currentUser.UserId,
                        CreatedDate = DateTime.Now,
                        CreatedUTCDate = DateTime.UtcNow
                    };

                    var notificationModel = _mapper.Map<NotificationManagement>(notificationDto);
                    await _unitOfWorkDA.NotificationManagementDA.CreateNotification(notificationModel, cancellationToken);
                }
            }
        }

        public async Task<bool> ValidateInteriorPhoneNumber(InteriorRequestDto interiorRequestDto, CancellationToken cancellationToken)
        {
                var customerAllData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            var customerAndInteriorData = customerAllData.Where(p => p.CustomerTypeId == (int)CustomerTypeEnum.Customer || p.CustomerTypeId == (int)CustomerTypeEnum.Interior);
            if (interiorRequestDto.CustomerId > 0)
            {
                var getCustomerId = customerAndInteriorData.First(c => c.CustomerId == interiorRequestDto.CustomerId);
                if (getCustomerId.PhoneNumber.Trim() == interiorRequestDto.PhoneNumber.Trim())
                {
                    return true;
                }
                else
                {
                    return !customerAndInteriorData.Any(c => c.PhoneNumber.Trim() == interiorRequestDto.PhoneNumber.Trim());
                }
            }
            else
            {
                return !customerAndInteriorData.Any(c => c.PhoneNumber.Trim() == interiorRequestDto.PhoneNumber.Trim());
            }
        }

        #region PrivateMethods

        private async Task<Customers> InteriorGetById(Int64 Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.CustomersDA.GetById(Id, cancellationToken);
            if (data == null)
            {
                throw new Exception("Interior not found");
            }
            return data;
        }

        private static void MapToDbObject(InteriorRequestDto model, Customers oldData)
        {
            oldData.CustomerTypeId = model.CustomerTypeId;
            oldData.FirstName = model.FirstName;
            oldData.LastName = model.LastName;
            oldData.EmailId = model.EmailId;
            oldData.PhoneNumber = model.PhoneNumber;
            oldData.AltPhoneNumber = model.AltPhoneNumber;
            oldData.GSTNo = model.GSTNo;
            oldData.IsSubscribe = model.IsSubscribe;
            oldData.Discount = model.Discount != null ? model.Discount : 0;
        }

        private async Task SaveInteriorAddress(InteriorRequestDto model, Customers data, CancellationToken cancellationToken)
        {
            CustomerAddresses catDto = new CustomerAddresses()
            {
                CustomerId = data.CustomerId,
                AddressType = "Home",
                Street1 = model.CustomerAddressesRequestDto.Street1 != null ? model.CustomerAddressesRequestDto.Street1 : "",
                Street2 = model.CustomerAddressesRequestDto.Street2 != null ? model.CustomerAddressesRequestDto.Street2 : "",
                Landmark = model.CustomerAddressesRequestDto.Landmark != null ? model.CustomerAddressesRequestDto.Landmark : "",
                Area = model.CustomerAddressesRequestDto.Area != null ? model.CustomerAddressesRequestDto.Area : "",
                City = model.CustomerAddressesRequestDto.City != null ? model.CustomerAddressesRequestDto.City : "",
                State = model.CustomerAddressesRequestDto.State != null ? model.CustomerAddressesRequestDto.State : "",
                ZipCode = model.CustomerAddressesRequestDto.ZipCode != null ? model.CustomerAddressesRequestDto.ZipCode : "",
                IsDefault = true,
                CreatedDate = DateTime.Now,
                CreatedUTCDate = DateTime.UtcNow,
            };
            await _unitOfWorkDA.CustomerAddressesDA.CreateCustomerAddress(catDto, cancellationToken);
            await _unitOfWorkDA.CommitTransactionAsync();
        }

        private static IQueryable<Customers> FilterInteriorData(InteriorDataTableFilterDto dataTableFilterDto, IQueryable<Customers> interiorAllData)
        {
            if (dataTableFilterDto != null)
            {
                if (!string.IsNullOrEmpty(dataTableFilterDto.customerName))
                {
                    interiorAllData = interiorAllData.Where(p => p.FirstName.StartsWith(dataTableFilterDto.customerName) || p.LastName.StartsWith(dataTableFilterDto.customerName));
                }

                if (!string.IsNullOrEmpty(dataTableFilterDto.customerMobileNo))
                {
                    interiorAllData = interiorAllData.Where(p => p.PhoneNumber.StartsWith(dataTableFilterDto.customerMobileNo));
                }

                if (dataTableFilterDto.customerFromDate != DateTime.MinValue)
                {
                    interiorAllData = interiorAllData.Where(p => p.CreatedDate > dataTableFilterDto.customerFromDate || p.UpdatedDate > dataTableFilterDto.customerFromDate);
                }

                if (dataTableFilterDto.customerToDate != DateTime.MinValue)
                {
                    interiorAllData = interiorAllData.Where(p => p.CreatedDate < dataTableFilterDto.customerToDate || p.UpdatedDate > dataTableFilterDto.customerToDate);
                }
            }

            return interiorAllData;
        }

        #endregion PrivateMethods
    }
}