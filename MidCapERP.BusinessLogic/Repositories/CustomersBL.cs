using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
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

        public async Task<CustomersResponseDto> GetCustomerByMobileNumberOrEmailId(string phoneNumberOrEmailId, CancellationToken cancellationToken)
        {
            var customerData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            var customerMobileNumberOrEmailId = customerData.FirstOrDefault(x => x.PhoneNumber == phoneNumberOrEmailId || x.EmailId == phoneNumberOrEmailId);
            return _mapper.Map<CustomersResponseDto>(customerMobileNumberOrEmailId);
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

            customerAllData = FilterCustomerData(dataTableFilterDto, customerAllData);

            var customerData = new PagedList<CustomersResponseDto>(_mapper.Map<List<CustomersResponseDto>>(customerAllData).AsQueryable(), dataTableFilterDto);
            return new JsonRepsonse<CustomersResponseDto>(dataTableFilterDto.Draw, customerData.TotalCount, customerData.TotalCount, customerData);
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
                    customerAllData = customerAllData.Where(p => p.CreatedDate < dataTableFilterDto.customerToDate || p.UpdatedDate > dataTableFilterDto.customerToDate);
                }
            }

            return customerAllData;
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
            customerToInsert.IsDeleted = false;
            customerToInsert.TenantId = _currentUser.TenantId;
            customerToInsert.CreatedBy = _currentUser.UserId;
            customerToInsert.CreatedDate = DateTime.Now;
            customerToInsert.CreatedUTCDate = DateTime.UtcNow;
            var customerAllData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            var customerExistOrNot = customerAllData.FirstOrDefault(p => p.PhoneNumber == Convert.ToString(model.RefferedNumber));
            if(customerExistOrNot != null)
            {
                customerToInsert.RefferedBy = customerExistOrNot.CustomerId;
                var  data = await _unitOfWorkDA.CustomersDA.CreateCustomers(customerToInsert, cancellationToken);
                return _mapper.Map<CustomerApiRequestDto>(data);
            }
            else
            {
                Customers refferedCustomer = new Customers();
                refferedCustomer.TenantId = _currentUser.TenantId;
                refferedCustomer.LastName = String.Empty;
                refferedCustomer.FirstName = model.RefferedName;
                refferedCustomer.PhoneNumber = model.RefferedNumber;
                refferedCustomer.CreatedBy = _currentUser.UserId;
                refferedCustomer.CreatedDate = DateTime.Now;
                refferedCustomer.CreatedUTCDate = DateTime.UtcNow;
                var customer = await _unitOfWorkDA.CustomersDA.CreateCustomers(refferedCustomer, cancellationToken);
                customerToInsert.RefferedBy = customer.CustomerId;
                var  data = await _unitOfWorkDA.CustomersDA.CreateCustomers(customerToInsert, cancellationToken);
                return _mapper.Map<CustomerApiRequestDto>(data);
            }
        }

        public async Task<CustomerApiRequestDto> UpdateCustomerApi(Int64 Id,CustomerApiRequestDto model, CancellationToken cancellationToken)
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
                data = await _unitOfWorkDA.CustomersDA.CreateCustomers(customerToInsert, cancellationToken);

                CustomerAddresses catDto = new CustomerAddresses();
                catDto.CustomerId = data.CustomerId;
                catDto.AddressType = model.CustomerAddressesRequestDto.AddressType;
                catDto.Street1 = model.CustomerAddressesRequestDto.Street1;
                catDto.Street2 = model.CustomerAddressesRequestDto.Street2;
                catDto.Landmark = model.CustomerAddressesRequestDto.Landmark;
                catDto.Area = model.CustomerAddressesRequestDto.Area;
                catDto.City = model.CustomerAddressesRequestDto.City;
                catDto.State = model.CustomerAddressesRequestDto.State;
                catDto.ZipCode = model.CustomerAddressesRequestDto.ZipCode;
                catDto.IsDefault = model.CustomerAddressesRequestDto.IsDefault;
                catDto.CreatedDate = DateTime.Now;
                catDto.CreatedUTCDate = DateTime.UtcNow;
                await _unitOfWorkDA.CustomerAddressesDA.CreateCustomerAddress(catDto, cancellationToken);

                await _unitOfWorkDA.CommitTransactionAsync();
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
            oldData.RefferedBy = model.RefferedBy;
        }

        private static void MapToDbObject(CustomerApiRequestDto model, Customers oldData)
        {
            oldData.FirstName = model.FirstName;
            oldData.LastName = model.LastName;
            oldData.EmailId = model.EmailId;
            oldData.PhoneNumber = model.PhoneNumber;
            oldData.AltPhoneNumber = model.AltPhoneNumber;
            oldData.GSTNo = model.GSTNo;
        }

        #endregion PrivateMethods
    }
}