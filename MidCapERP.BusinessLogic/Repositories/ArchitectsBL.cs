using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Services.SendSMS;
using MidCapERP.Core.Constants;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Customers;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Paging;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class ArchitectsBL : IArchitectsBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;
        private readonly ISendSMSservice _sendSMSservice;

        public ArchitectsBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser, ISendSMSservice sendSMSservice)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
            _sendSMSservice = sendSMSservice;
        }

        public async Task<IEnumerable<CustomersResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            return _mapper.Map<List<CustomersResponseDto>>(data.ToList());
        }

        public async Task<JsonRepsonse<CustomersResponseDto>> GetFilterArchitectsData(CustomerDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var architectAllData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            var architectData = architectAllData.Where(x => x.CustomerTypeId == (int)ArchitectTypeEnum.Architect);
            var architectFilteredData = FilterArchitectData(dataTableFilterDto, architectData);
            var architectGridData = new PagedList<CustomersResponseDto>(_mapper.Map<List<CustomersResponseDto>>(architectFilteredData).AsQueryable(), dataTableFilterDto);
            return new JsonRepsonse<CustomersResponseDto>(dataTableFilterDto.Draw, architectGridData.TotalCount, architectGridData.TotalCount, architectGridData);
        }

        public async Task<CustomersRequestDto> GetById(Int64 Id, CancellationToken cancellationToken)
        {
            var data = await ArchitectGetById(Id, cancellationToken);
            return _mapper.Map<CustomersRequestDto>(data);
        }

        public async Task<CustomersRequestDto> CreateArchitects(CustomersRequestDto model, CancellationToken cancellationToken)
        {
            var architectToInsert = _mapper.Map<Customers>(model);
            Customers data = null;
            await _unitOfWorkDA.BeginTransactionAsync();

            try
            {
                architectToInsert.RefferedBy = 0;
                architectToInsert.CustomerTypeId = (int)ArchitectTypeEnum.Architect;
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

            return _mapper.Map<CustomersRequestDto>(data);
        }

        public async Task<CustomersRequestDto> UpdateArchitects(Int64 Id, CustomersRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await ArchitectGetById(Id, cancellationToken);
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.CustomersDA.UpdateCustomers(Id, oldData, cancellationToken);
            return _mapper.Map<CustomersRequestDto>(data);
        }

        public async Task SendSMSToArchitects(CustomersSendSMSDto model, CancellationToken cancellationToken)
        {
            List<string> architectPhoneList = new List<string>();
            //foreach (var item in model)
            //{
            //    var architectData = await _unitOfWorkDA.CustomersDA.GetById(item, cancellationToken);
            //    var architectPhone = architectData.PhoneNumber;
            //    architectPhoneList.Add(architectPhone);
            //}

            foreach (var item in architectPhoneList)
            {
                //var msg = _sendSMSservice.SendSMS("7567086864", "Hi. This is test message for greeting customers.");
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

        private static void MapToDbObject(CustomersRequestDto model, Customers oldData)
        {
            oldData.CustomerTypeId = model.CustomerTypeId;
            oldData.FirstName = model.FirstName;
            oldData.LastName = model.LastName;
            oldData.EmailId = model.EmailId;
            oldData.PhoneNumber = model.PhoneNumber;
            oldData.AltPhoneNumber = model.AltPhoneNumber;
            oldData.GSTNo = model.GSTNo;
        }

        private async Task SaveArchitectAddress(CustomersRequestDto model, Customers data, CancellationToken cancellationToken)
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

        private static IQueryable<Customers> FilterArchitectData(CustomerDataTableFilterDto dataTableFilterDto, IQueryable<Customers> architectAllData)
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