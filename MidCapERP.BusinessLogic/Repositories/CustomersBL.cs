using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.Core.Constants;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Customers;
using MidCapERP.Dto.CustomersTypes;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.MegaSearch;
using MidCapERP.Dto.Paging;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class CustomersBL : ICustomersBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public CustomersBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<CustomersResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            return _mapper.Map<List<CustomersResponseDto>>(data.ToList());
        }

        public async Task<int> GetCustomerCount(CancellationToken cancellationToken)
        {
            DateTime oldDate = DateTime.UtcNow.Date.AddDays(-6);
            var data = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            return data.Where(x => x.CreatedUTCDate >= oldDate).Count();
        }

        public async Task<CustomersApiResponseDto> GetCustomerById(long customerId, CancellationToken cancellationToken)
        {
            var customerData = await _unitOfWorkDA.CustomersDA.GetById(customerId, cancellationToken);
            if (customerData == null)
            {
                throw new Exception("Customer not found");
            }
            if (customerData.RefferedBy != null)
            {
                var customerApiResponseData = _mapper.Map<CustomersApiResponseDto>(customerData);
                var refferedByCustomerData = await CustomerGetByRefferedId((long)customerData.RefferedBy, cancellationToken);
                if (refferedByCustomerData == null)
                {
                    return _mapper.Map<CustomersApiResponseDto>(customerApiResponseData);
                }
                var refferedCustomerResponseData = _mapper.Map<CustomersApiResponseDto>(refferedByCustomerData);
                customerApiResponseData.Reffered = refferedCustomerResponseData;
                return _mapper.Map<CustomersApiResponseDto>(customerApiResponseData);
            }
            else
                return _mapper.Map<CustomersApiResponseDto>(customerData);
        }

        public async Task<CustomersApiResponseDto> GetCustomerByMobileNumberOrEmailId(string phoneNumberOrEmailId, CancellationToken cancellationToken)
        {
            var customerAllData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            var customerMobileNumberOrEmailId = customerAllData.FirstOrDefault(x => x.PhoneNumber == phoneNumberOrEmailId || x.EmailId == phoneNumberOrEmailId);
            if (customerMobileNumberOrEmailId == null)
            {
                throw new Exception("Customer not found");
            }
            if (customerMobileNumberOrEmailId.RefferedBy != null)
            {
                var customerApiResponseData = _mapper.Map<CustomersApiResponseDto>(customerMobileNumberOrEmailId);
                var refferedByCustomerData = await CustomerGetByRefferedId((long)customerMobileNumberOrEmailId.RefferedBy, cancellationToken);
                if (refferedByCustomerData == null)
                {
                    return _mapper.Map<CustomersApiResponseDto>(customerApiResponseData);
                }
                var refferedCustomerResponseData = _mapper.Map<CustomersApiResponseDto>(refferedByCustomerData);
                customerApiResponseData.Reffered = refferedCustomerResponseData;
                return _mapper.Map<CustomersApiResponseDto>(customerApiResponseData);
            }
            else
                return _mapper.Map<CustomersApiResponseDto>(customerMobileNumberOrEmailId);
        }

        public async Task<bool> CheckCustomerExistOrNot(string phoneNumberOrEmail, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            var customerExistOrNot = data.FirstOrDefault(x => x.PhoneNumber == phoneNumberOrEmail || x.EmailId == phoneNumberOrEmail);
            if (customerExistOrNot != null)
                return true;
            else
                return false;
        }

        public async Task<IEnumerable<MegaSearchResponse>> GetCustomerForDropDownByMobileNo(string searchText, CancellationToken cancellationToken)
        {
            var customerAllData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            return customerAllData.Where(x => x.PhoneNumber.StartsWith(searchText)).Select(x => new MegaSearchResponse(x.CustomerId, x.FirstName + " " + x.LastName, x.PhoneNumber, null, "Customer")).Take(10).ToList();
        }

        public async Task<IEnumerable<CustomerApiDropDownResponceDto>> GetSearchCustomerForDropDownNameOrPhoneNumber(string searchText, CancellationToken cancellationToken)
        {
            var customerAllData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            var architectCustomerData = customerAllData.Where(p => p.CustomerTypeId == (int)ArchitectTypeEnum.Architect);
            var data = architectCustomerData.Where(x => x.FirstName.StartsWith(searchText) || x.LastName.StartsWith(searchText) || (x.FirstName + x.LastName).StartsWith(searchText)).Select(p => new CustomerApiDropDownResponceDto { RefferedById = p.CustomerId, FirstName = p.FirstName, LastName = p.LastName }).Take(10);
            return data.ToList();
        }

        public async Task<CustomersResponseDto> GetCustomerForDetailsByMobileNo(string searchText, CancellationToken cancellationToken)
        {
            var cutomerAlldata = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            var data = cutomerAlldata.Where(x => x.PhoneNumber == searchText);
            return _mapper.Map<CustomersResponseDto>(data);
        }

        public async Task<IEnumerable<CustomersTypesResponseDto>> CustomersTypesGetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.CustomerTypesDA.GetAll(cancellationToken);
            return _mapper.Map<List<CustomersTypesResponseDto>>(data.ToList());
        }

        public async Task<IEnumerable<CustomersResponseDto>> SearchCustomer(string customerNameOrEmailOrMobileNo, CancellationToken cancellationToken)
        {
            var customerAllData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            var customerData = customerAllData.Where(x => x.PhoneNumber.StartsWith(customerNameOrEmailOrMobileNo) || x.FirstName.StartsWith(customerNameOrEmailOrMobileNo) || x.LastName.StartsWith(customerNameOrEmailOrMobileNo) || x.EmailId.StartsWith(customerNameOrEmailOrMobileNo) || (x.FirstName + " " + x.LastName).StartsWith(customerNameOrEmailOrMobileNo));
            return _mapper.Map<List<CustomersResponseDto>>(customerData.ToList());
        }

        public async Task<JsonRepsonse<CustomersResponseDto>> GetFilterCustomersData(CustomerDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var customerAllData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            var customerData = customerAllData.Where(x => x.CustomerTypeId == (int)CustomerTypeEnum.Customer || x.CustomerTypeId == (int)CustomerTypeEnum.Wholesaler);
            var customerFilteredData = FilterCustomerData(dataTableFilterDto, customerData);
            var customerGridData = new PagedList<CustomersResponseDto>(_mapper.Map<List<CustomersResponseDto>>(customerFilteredData).AsQueryable(), dataTableFilterDto);
            return new JsonRepsonse<CustomersResponseDto>(dataTableFilterDto.Draw, customerGridData.TotalCount, customerGridData.TotalCount, customerGridData);
        }

        public async Task<CustomersTypesResponseDto> CustomersTypesGetDetailsById(Int64 Id, CancellationToken cancellationToken)
        {
            var data = await CustomerTypesGetById(Id, cancellationToken);
            return _mapper.Map<CustomersTypesResponseDto>(data);
        }

        public async Task<CustomersRequestDto> GetById(Int64 Id, CancellationToken cancellationToken)
        {
            var data = await CustomerGetById(Id, cancellationToken);
            return _mapper.Map<CustomersRequestDto>(data);
        }

        public async Task<CustomerApiRequestDto> CreateCustomerApi(CustomerApiRequestDto model, CancellationToken cancellationToken)
        {
            var customerToInsert = _mapper.Map<Customers>(model);
            customerToInsert.CustomerTypeId = model.CustomerTypeId;
            customerToInsert.IsDeleted = false;
            customerToInsert.TenantId = _currentUser.TenantId;
            customerToInsert.CreatedBy = _currentUser.UserId;
            customerToInsert.CreatedDate = DateTime.Now;
            customerToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.CustomersDA.CreateCustomers(customerToInsert, cancellationToken);
            return _mapper.Map<CustomerApiRequestDto>(data);
        }

        public async Task<CustomerApiRequestDto> UpdateCustomerApi(Int64 Id, CustomerApiRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await CustomerGetById(Id, cancellationToken);
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.CustomersDA.UpdateCustomers(Id, oldData, cancellationToken);
            return _mapper.Map<CustomerApiRequestDto>(data);
        }

        public async Task<CustomersRequestDto> CreateCustomers(CustomersRequestDto model, CancellationToken cancellationToken)
        {
            var customerToInsert = _mapper.Map<Customers>(model);
            Customers data = null;
            await _unitOfWorkDA.BeginTransactionAsync();

            try
            {
                customerToInsert.IsDeleted = false;
                customerToInsert.TenantId = _currentUser.TenantId;
                customerToInsert.CreatedBy = _currentUser.UserId;
                customerToInsert.CreatedDate = DateTime.Now;
                customerToInsert.CreatedUTCDate = DateTime.UtcNow;

                if (model.RefferedNumber != null)
                {
                    await AddCustomerAndReferralUser(model, customerToInsert, cancellationToken);
                }
                else
                {
                    customerToInsert.RefferedBy = 0;
                }
                data = await _unitOfWorkDA.CustomersDA.CreateCustomers(customerToInsert, cancellationToken);

                await SaveCustomerAddress(model, data, cancellationToken);
            }
            catch (Exception e)
            {
                await _unitOfWorkDA.rollbackTransactionAsync();
                throw new Exception("Customer data is not saved");
            }

            return _mapper.Map<CustomersRequestDto>(data);
        }

        public async Task<CustomersRequestDto> UpdateCustomers(Int64 Id, CustomersRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await CustomerGetById(Id, cancellationToken);
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.CustomersDA.UpdateCustomers(Id, oldData, cancellationToken);
            return _mapper.Map<CustomersRequestDto>(data);
        }

        public async Task SendSMSToCustomers(CustomersSendSMSDto model, CancellationToken cancellationToken)
        {
            List<string> customerPhoneList = new List<string>();

            foreach (var item in model.CustomerList)
            {
                var customerData = await _unitOfWorkDA.CustomersDA.GetById(item, cancellationToken);
                if (customerData != null)
                {
                    var customerPhone = customerData.PhoneNumber;
                    customerPhoneList.Add(customerPhone);
                }
            }

            foreach (var item in customerPhoneList)
            {
                if (item != null)
                {
                    //var msg = _sendSMSservice.SendSMS("7567086864", "Hi. This is test message for greeting customers.");
                    //var msg = _sendSMSservice.SendSMS(item, model.Message);
                }
            }
        }

        #region PrivateMethods

        private async Task<Customers> CustomerGetById(Int64 Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.CustomersDA.GetById(Id, cancellationToken);
            if (data == null)
            {
                throw new Exception("Customer not found");
            }
            return data;
        }

        private async Task<Customers> CustomerGetByRefferedId(Int64 Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.CustomersDA.GetById(Id, cancellationToken);
            return data;
        }

        private async Task<CustomerTypes> CustomerTypesGetById(Int64 Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.CustomerTypesDA.GetById(Id, cancellationToken);
            if (data == null)
            {
                throw new Exception("Customer Type not found");
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
            oldData.IsSubscribe = model.IsSubscribe;
        }

        private static void MapToDbObject(CustomerApiRequestDto model, Customers oldData)
        {
            oldData.FirstName = model.FirstName;
            oldData.LastName = model.LastName;
            oldData.EmailId = model.EmailId;
            oldData.PhoneNumber = model.PhoneNumber;
            oldData.AltPhoneNumber = model.AltPhoneNumber;
            oldData.GSTNo = model.GSTNo;
            oldData.IsSubscribe = model.IsSubscribe;
        }

        private async Task AddCustomerAndReferralUser(CustomersRequestDto model, Customers customerToInsert, CancellationToken cancellationToken)
        {
            var customerAllData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            var customerExistOrNot = customerAllData.FirstOrDefault(p => p.PhoneNumber == Convert.ToString(model.RefferedNumber) && p.CustomerTypeId == (int)ArchitectTypeEnum.Architect);
            if (customerExistOrNot != null)
            {
                customerToInsert.RefferedBy = customerExistOrNot.CustomerId;
            }
            else
            {
                Customers refferedCustomer = new Customers()
                {
                    TenantId = _currentUser.TenantId,
                    LastName = String.Empty,
                    FirstName = model.RefferedName != null ? model.RefferedName : "",
                    PhoneNumber = model.RefferedNumber,
                    CustomerTypeId = (int)ArchitectTypeEnum.Architect,
                    RefferedBy = 0,
                    CreatedBy = _currentUser.UserId,
                    CreatedDate = DateTime.Now,
                    CreatedUTCDate = DateTime.UtcNow,
                };
                var customer = await _unitOfWorkDA.CustomersDA.CreateCustomers(refferedCustomer, cancellationToken);
                customerToInsert.RefferedBy = customer.CustomerId;
            }
        }

        private async Task SaveCustomerAddress(CustomersRequestDto model, Customers data, CancellationToken cancellationToken)
        {
            CustomerAddresses catDto = new CustomerAddresses()
            {
                CustomerId = data.CustomerId,
                AddressType = "Home",
                Street1 = model.CustomerAddressesRequestDto.Street1 != null ? model.CustomerAddressesRequestDto.Street1 : String.Empty,
                Street2 = model.CustomerAddressesRequestDto.Street2 != null ? model.CustomerAddressesRequestDto.Street2 : String.Empty,
                Landmark = model.CustomerAddressesRequestDto.Landmark != null ? model.CustomerAddressesRequestDto.Landmark : String.Empty,
                Area = model.CustomerAddressesRequestDto.Area != null ? model.CustomerAddressesRequestDto.Area : String.Empty,
                City = model.CustomerAddressesRequestDto.City != null ? model.CustomerAddressesRequestDto.City : String.Empty,
                State = model.CustomerAddressesRequestDto.State != null ? model.CustomerAddressesRequestDto.State : String.Empty,
                ZipCode = model.CustomerAddressesRequestDto.ZipCode != null ? model.CustomerAddressesRequestDto.ZipCode : String.Empty,
                IsDefault = true,
                CreatedDate = DateTime.Now,
                CreatedUTCDate = DateTime.UtcNow,
            };
            await _unitOfWorkDA.CustomerAddressesDA.CreateCustomerAddress(catDto, cancellationToken);
            await _unitOfWorkDA.CommitTransactionAsync();
        }

        private static IQueryable<Customers> FilterCustomerData(CustomerDataTableFilterDto dataTableFilterDto, IQueryable<Customers> customerAllData)
        {
            if (dataTableFilterDto != null)
            {
                if (!string.IsNullOrEmpty(dataTableFilterDto.customerName))
                {
                    customerAllData = customerAllData.Where(p => p.FirstName.StartsWith(dataTableFilterDto.customerName) || p.LastName.StartsWith(dataTableFilterDto.customerName));
                }

                if (!string.IsNullOrEmpty(dataTableFilterDto.customerMobileNo))
                {
                    customerAllData = customerAllData.Where(p => p.PhoneNumber.StartsWith(dataTableFilterDto.customerMobileNo));
                }

                if (dataTableFilterDto.customerFromDate != DateTime.MinValue)
                {
                    customerAllData = customerAllData.Where(p => p.CreatedDate > dataTableFilterDto.customerFromDate || p.UpdatedDate > dataTableFilterDto.customerFromDate);
                }

                if (dataTableFilterDto.customerToDate != DateTime.MinValue)
                {
                    customerAllData = customerAllData.Where(p => p.CreatedDate < dataTableFilterDto.customerToDate || p.UpdatedDate < dataTableFilterDto.customerToDate);
                }
            }

            return customerAllData;
        }

        #endregion PrivateMethods
    }
}