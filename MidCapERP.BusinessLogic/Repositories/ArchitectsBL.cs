using AutoMapper;
using MidCapERP.BusinessLogic.Constants;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Services.SendSMS;
using MidCapERP.Core.Constants;
using MidCapERP.Core.Services.Email;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Architect;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.NotificationManagement;
using MidCapERP.Dto.Paging;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class ArchitectsBL : IArchitectsBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;
        private readonly ISendSMSservice _sendSMSservice;
        private readonly IEmailHelper _emailHelper;

        public ArchitectsBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser, ISendSMSservice sendSMSservice, IEmailHelper emailHelper)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
            _sendSMSservice = sendSMSservice;
            _emailHelper = emailHelper;
        }

        public async Task<IEnumerable<ArchitectResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var architectAllData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            var architectData = architectAllData.Where(x => x.CustomerTypeId == (int)CustomerTypeEnum.Architect);
            return _mapper.Map<List<ArchitectResponseDto>>(architectData.ToList());
        }

        public async Task<JsonRepsonse<ArchitectResponseDto>> GetFilterArchitectsData(ArchitectDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var architectAllData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            var architectData = architectAllData.Where(x => x.CustomerTypeId == (int)CustomerTypeEnum.Architect);
            var architectFilteredData = FilterArchitectData(dataTableFilterDto, architectData);
            var architectGridData = new PagedList<ArchitectResponseDto>(_mapper.Map<List<ArchitectResponseDto>>(architectFilteredData).AsQueryable(), dataTableFilterDto);
            return new JsonRepsonse<ArchitectResponseDto>(dataTableFilterDto.Draw, architectGridData.TotalCount, architectGridData.TotalCount, architectGridData);
        }

        public async Task<ArchitectRequestDto> GetById(Int64 Id, CancellationToken cancellationToken)
        {
            var data = await ArchitectGetById(Id, cancellationToken);
            return _mapper.Map<ArchitectRequestDto>(data);
        }

        public async Task<ArchitectRequestDto> CreateArchitects(ArchitectRequestDto model, CancellationToken cancellationToken)
        {
            var architectToInsert = _mapper.Map<Customers>(model);
            Customers data = null;
            await _unitOfWorkDA.BeginTransactionAsync();

            try
            {
                architectToInsert.RefferedBy = 0;
                architectToInsert.CustomerTypeId = (int)CustomerTypeEnum.Architect;
                architectToInsert.Discount = model.Discount != null ? model.Discount : 0;
                architectToInsert.IsDeleted = false;
                architectToInsert.TenantId = _currentUser.TenantId;
                architectToInsert.CreatedBy = _currentUser.UserId;
                architectToInsert.CreatedDate = DateTime.Now;
                architectToInsert.CreatedUTCDate = DateTime.UtcNow;
                data = await _unitOfWorkDA.CustomersDA.CreateCustomers(architectToInsert, cancellationToken);

                await SaveArchitectAddress(model, data, cancellationToken);
            }
            catch (Exception e)
            {
                await _unitOfWorkDA.rollbackTransactionAsync();
                throw new Exception("Architect data is not saved");
            }

            return _mapper.Map<ArchitectRequestDto>(data);
        }

        public async Task<ArchitectRequestDto> UpdateArchitects(Int64 Id, ArchitectRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await ArchitectGetById(Id, cancellationToken);
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.CustomersDA.UpdateCustomers(Id, oldData, cancellationToken);
            return _mapper.Map<ArchitectRequestDto>(data);
        }

        public async Task SendSMSToArchitects(ArchitectsSendSMSDto model, CancellationToken cancellationToken)
        {
            //List<string> architectEmailList = new List<string>();
            foreach (var item in model.CustomerList)
            {
                var architectData = await _unitOfWorkDA.CustomersDA.GetById(item, cancellationToken);
                if (architectData != null)
                {
                    //architectEmailList.Add(architectData.EmailId);
                    //await _emailHelper.SendEmail(model.Subject, model.Message, architectEmailList);
                    NotificationManagementRequestDto notificationDto = new NotificationManagementRequestDto()
                    {
                        EntityTypeID = await _unitOfWorkDA.SubjectTypesDA.GetCustomerSubjectTypeId(cancellationToken),
                        EntityID = item,
                        NotificationType = "Greetings",
                        NotificationMethod = NotificationMethodConstant.Email,
                        MessageSubject = model.Subject,
                        MessageBody = model.Message,
                        ReceiverEmail = architectData.EmailId,
                        ReceiverMobile = architectData.PhoneNumber,
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

        #region PrivateMethods

        private async Task<Customers> ArchitectGetById(Int64 Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.CustomersDA.GetById(Id, cancellationToken);
            if (data == null)
            {
                throw new Exception("Architect not found");
            }
            return data;
        }

        private static void MapToDbObject(ArchitectRequestDto model, Customers oldData)
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

        private async Task SaveArchitectAddress(ArchitectRequestDto model, Customers data, CancellationToken cancellationToken)
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

        private static IQueryable<Customers> FilterArchitectData(ArchitectDataTableFilterDto dataTableFilterDto, IQueryable<Customers> architectAllData)
        {
            if (dataTableFilterDto != null)
            {
                if (!string.IsNullOrEmpty(dataTableFilterDto.customerName))
                {
                    architectAllData = architectAllData.Where(p => p.FirstName.StartsWith(dataTableFilterDto.customerName) || p.LastName.StartsWith(dataTableFilterDto.customerName));
                }

                if (!string.IsNullOrEmpty(dataTableFilterDto.customerMobileNo))
                {
                    architectAllData = architectAllData.Where(p => p.PhoneNumber.StartsWith(dataTableFilterDto.customerMobileNo));
                }

                if (dataTableFilterDto.customerFromDate != DateTime.MinValue)
                {
                    architectAllData = architectAllData.Where(p => p.CreatedDate > dataTableFilterDto.customerFromDate || p.UpdatedDate > dataTableFilterDto.customerFromDate);
                }

                if (dataTableFilterDto.customerToDate != DateTime.MinValue)
                {
                    architectAllData = architectAllData.Where(p => p.CreatedDate < dataTableFilterDto.customerToDate || p.UpdatedDate > dataTableFilterDto.customerToDate);
                }
            }

            return architectAllData;
        }

        #endregion PrivateMethods
    }
}