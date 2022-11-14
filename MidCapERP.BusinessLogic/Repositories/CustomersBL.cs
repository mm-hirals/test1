using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.Core.Constants;
using MidCapERP.Core.Services.Email;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataAccess.Repositories;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Customers;
using MidCapERP.Dto.CustomersTypes;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.MegaSearch;
using MidCapERP.Dto.NotificationManagement;
using MidCapERP.Dto.Paging;
using System.Net;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class CustomersBL : ICustomersBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;
        private readonly IEmailHelper _emailHelper;
        private readonly IOTPLoginDA _loginDA;

        public CustomersBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser, IEmailHelper emailHelper, IOTPLoginDA otpLoginDA)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
            _emailHelper = emailHelper;
            _loginDA = otpLoginDA;
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
            var response = customerAllData.Where(x => x.PhoneNumber.StartsWith(searchText) || (x.FirstName + " " + x.LastName).StartsWith(searchText) && (x.CustomerTypeId == (int)CustomerTypeEnum.Interior || x.CustomerTypeId == (int)CustomerTypeEnum.Customer))
                .Select(x => new MegaSearchResponse
                (
                    x.CustomerId,
                    x.FirstName + " " + x.LastName,
                    x.PhoneNumber,
                    null,
                    x.CustomerTypeId == (int)CustomerTypeEnum.Interior ? "Interior" : "Customer")
                ).Take(10).ToList();
            return response;
        }

        public async Task<IEnumerable<CustomerApiDropDownResponceDto>> GetSearchCustomerForDropDownNameOrPhoneNumber(string searchText, CancellationToken cancellationToken)
        {
            var interiorCustomerData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            var data = interiorCustomerData.Where(x => (x.FirstName + " " + x.LastName).StartsWith(searchText))
                .Select(p => new CustomerApiDropDownResponceDto
                {
                    RefferedById = p.CustomerId,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    MobileNo = p.PhoneNumber,
                    CustomerType = p.CustomerTypeId == (int)CustomerTypeEnum.Interior ? "Interior" : "Customer"
                }).Take(10).ToList();
            return data;
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
            var customerData = customerAllData.Where(x => x.PhoneNumber.StartsWith(customerNameOrEmailOrMobileNo) || (x.FirstName + " " + x.LastName).StartsWith(customerNameOrEmailOrMobileNo));
            return _mapper.Map<List<CustomersResponseDto>>(customerData.ToList());
        }

        public async Task<JsonRepsonse<CustomersResponseDto>> GetFilterCustomersData(CustomerDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var customerAllData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            var customerData = customerAllData.Where(x => x.CustomerTypeId == (int)CustomerTypeEnum.Customer);
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
            var getAllCustomer = await GetAll(cancellationToken);
            var customerAndInteriorData = getAllCustomer.Where(p => p.CustomerTypeId == (int)CustomerTypeEnum.Customer || p.CustomerTypeId == (int)CustomerTypeEnum.Interior);
            var customerExistOrNot = customerAndInteriorData.FirstOrDefault(p => p.PhoneNumber == model.PhoneNumber);
            if (customerExistOrNot == null)
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
            else
                throw new Exception("Phone Number already exist. Please enter a different Phone Number.");
        }

        public async Task<CustomerApiRequestDto> UpdateCustomerApi(Int64 Id, CustomerApiRequestDto model, CancellationToken cancellationToken)
        {
            var getAllCustomer = await GetAll(cancellationToken);
            var customerAndInteriorData = getAllCustomer.Where(p => p.CustomerTypeId == (int)CustomerTypeEnum.Customer || p.CustomerTypeId == (int)CustomerTypeEnum.Interior);
            var customerExistOrNot = customerAndInteriorData.FirstOrDefault(p => p.PhoneNumber == model.PhoneNumber);
            if (customerExistOrNot == null)
            {
                var oldData = await CustomerGetById(Id, cancellationToken);
                oldData.UpdatedBy = _currentUser.UserId;
                oldData.UpdatedDate = DateTime.Now;
                oldData.UpdatedUTCDate = DateTime.UtcNow;
                MapToDbObject(model, oldData);
                var data = await _unitOfWorkDA.CustomersDA.UpdateCustomers(Id, oldData, cancellationToken);
                return _mapper.Map<CustomerApiRequestDto>(data);
            }
            else
                throw new Exception("Phone Number already exist. Please enter a different Phone Number.");
        }

        public async Task<CustomersRequestDto> CreateCustomers(CustomersRequestDto model, CancellationToken cancellationToken)
        {
            var customerToInsert = _mapper.Map<Customers>(model);
            Customers data = null;
            if (model.CustomerAddressesRequestDto.State != null && model.CustomerAddressesRequestDto.Area != null && model.CustomerAddressesRequestDto.City != null && model.CustomerAddressesRequestDto.ZipCode != null)
                await _unitOfWorkDA.BeginTransactionAsync();
            try
            {
                customerToInsert.CustomerTypeId = (int)CustomerTypeEnum.Customer;
                customerToInsert.Discount = model.Discount;
                customerToInsert.IsDeleted = false;
                customerToInsert.TenantId = _currentUser.TenantId;
                customerToInsert.CreatedBy = _currentUser.UserId;
                customerToInsert.CreatedDate = DateTime.Now;
                customerToInsert.CreatedUTCDate = DateTime.UtcNow;
                data = await _unitOfWorkDA.CustomersDA.CreateCustomers(customerToInsert, cancellationToken);
                if (model.CustomerAddressesRequestDto.State != null && model.CustomerAddressesRequestDto.Area != null && model.CustomerAddressesRequestDto.City != null && model.CustomerAddressesRequestDto.ZipCode != null)
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
            foreach (var item in model.CustomerList)
            {
                var customerData = await _unitOfWorkDA.CustomersDA.GetById(item, cancellationToken);
                if (customerData != null)
                {
                    NotificationManagementRequestDto notificationDto = new NotificationManagementRequestDto()
                    {
                        EntityTypeID = await _unitOfWorkDA.SubjectTypesDA.GetCustomerSubjectTypeId(cancellationToken),
                        EntityID = item,
                        NotificationType = "Greetings",
                        NotificationMethod = "Email",
                        MessageSubject = model.Subject,
                        MessageBody = model.Message,
                        ReceiverEmail = customerData.EmailId,
                        ReceiverMobile = customerData.PhoneNumber,
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

        public async Task<CustomersApiResponseDto> GetCustomerByIdAPI(Int64 id, CancellationToken cancellationToken)
        {
            var customerData = await _unitOfWorkDA.CustomersDA.GetById(id, cancellationToken);
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

        public async Task<bool> ValidateCustomerPhoneNumber(CustomersRequestDto customerRequestDto, CancellationToken cancellationToken)
        {
            var getAllCustomer = await GetAll(cancellationToken);
            var customerAndInteriorData = getAllCustomer.Where(p => p.CustomerTypeId == (int)CustomerTypeEnum.Customer || p.CustomerTypeId == (int)CustomerTypeEnum.Interior);
            if (customerRequestDto.CustomerId > 0)
            {
                var getCustomerById = customerAndInteriorData.First(c => c.CustomerId == customerRequestDto.CustomerId);
                if (getCustomerById.PhoneNumber.Trim() == customerRequestDto.PhoneNumber.Trim())
                {
                    return true;
                }
                else
                {
                    return !customerAndInteriorData.Any(c => c.PhoneNumber.Trim() == customerRequestDto.PhoneNumber.Trim());
                }
            }
            else
            {
                return !customerAndInteriorData.Any(c => c.PhoneNumber.Trim() == customerRequestDto.PhoneNumber.Trim());
            }
        }

        public async Task<string> SendCustomerOtpAPI(CustomerApiRequestDto model, CancellationToken cancellationToken)
        {
            string data = string.Empty;
            //var otpLogin = await _loginDA.GetAll(cancellationToken);

            OTPLogin loginToken = new OTPLogin()
            {
                PhoneNumber = model.PhoneNumber,
                OTP = "0000", //new Random().Next(1, 9999).ToString("D4"),
                ExpiryTime = DateTime.UtcNow.AddMinutes(10),
            };
            var createdToken = await _loginDA.CreateLoginToken(loginToken, cancellationToken);
            data = createdToken.OTP;
            return data;
            //Send OTP to customer through SMS
            //SendOTPToCustomer(model.PhoneNumber);

        }

        public static string SendOTPToCustomer(string phoneNumber)
        {
            string MainUrl = "SMSAPIURL"; //Here need to give SMS API URL
            string UserName = "username"; //Here need to give username
            string Password = "Password"; //Here need to give Password
            string SenderId = "SenderId";
            string strMobileno = phoneNumber;
            string URL = "";
            URL = MainUrl + "username=" + UserName + "&msg_token=" + Password + "&sender_id=" + SenderId + "&mobile=" + strMobileno.Trim() + "";
            string strResponse = GetResponse(URL);
            string msg = "";
            if (strResponse.Equals("Fail"))
            {
                msg = "Fail";
            }
            else
            {
                msg = strResponse;
            }
            return msg;
        }

        public static string GetResponse(string smsURL)
        {
            try
            {
                WebClient objWebClient = new WebClient();
                System.IO.StreamReader reader = new System.IO.StreamReader(objWebClient.OpenRead(smsURL));
                string ResultHTML = reader.ReadToEnd();
                return ResultHTML;
            }
            catch (Exception)
            {
                return "Fail";
            }
        }

        //public async Task<bool> ValidateCustomerOtpAPI(CustomersSendOtpDto customersSendOtpDto, CancellationToken cancellationToken)
        //{
        //    // Get all OTP table data
        //    //var getAllCustomerOTPData = await GetAll(cancellationToken);
        //    var customerData = await _loginDA.GetAll(cancellationToken);
        //    //var customerData = getAllCustomerOTPData.Where(p => p.CustomerTypeId == (int)CustomerTypeEnum.Customer);
        //    //if (customersSendOtpDto.CustomerId > 0)
        //    //{
            
        //    var getCustomerById = customerData.Any(c => c.OTP == customersSendOtpDto.OTP);
        //    if (getCustomerById)
        //    {
        //        CreateCustomerApi(customersSendOtpDto, cancellationToken);
        //    }

        //    //if (getCustomerById.PhoneNumber.Trim() == customersSendOtpDto.PhoneNumber.Trim())
        //    //{
        //    //    return true;
        //    //}
        //    //else
        //    //{
        //    //    return !customerData.Any(c => c.PhoneNumber.Trim() == customersSendOtpDto.PhoneNumber.Trim());
        //    //}

        //    //}
        //    //else
        //    //{
        //    //    return !customerData.Any(c => c.PhoneNumber.Trim() == customersSendOtpDto.PhoneNumber.Trim());
        //    //}
        //}


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
            oldData.CustomerTypeId = (int)CustomerTypeEnum.Customer;
            oldData.FirstName = model.FirstName;
            oldData.LastName = model.LastName;
            oldData.EmailId = model.EmailId;
            oldData.PhoneNumber = model.PhoneNumber;
            oldData.AltPhoneNumber = model.AltPhoneNumber;
            oldData.GSTNo = model.GSTNo;
            oldData.IsSubscribe = model.IsSubscribe;
            oldData.Discount = model.Discount != null ? model.Discount : 0;
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
            oldData.IsSubscribe = model.IsSubscribe;
            oldData.CustomerTypeId = model.CustomerTypeId;
            oldData.RefferedBy = model.RefferedBy;
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
                Area = model.CustomerAddressesRequestDto.Area != null ? model.CustomerAddressesRequestDto.Area :
                String.Empty,
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
                if (dataTableFilterDto.RefferedBy != null && dataTableFilterDto.RefferedBy != 0)
                {
                    customerAllData = customerAllData.Where(p => p.RefferedBy == dataTableFilterDto.RefferedBy);
                }
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