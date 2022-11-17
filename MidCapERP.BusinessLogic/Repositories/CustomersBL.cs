using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.Core.Constants;
using MidCapERP.Core.Services.Email;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Customers;
using MidCapERP.Dto.CustomersTypes;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.MegaSearch;
using MidCapERP.Dto.NotificationManagement;
using MidCapERP.Dto.Paging;
using MidCapERP.Dto.WrkImportCustomers;
using MidCapERP.Dto.WrkImportFiles;
using System.Data;
using System.Globalization;
using System.Linq.Dynamic.Core;
using System.Net;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class CustomersBL : ICustomersBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private CurrentUser _currentUser;
        private readonly IEmailHelper _emailHelper;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IOTPLoginDA _otpLoginDA;

        public CustomersBL(IServiceScopeFactory serviceScopeFactory, IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser, IEmailHelper emailHelper, IOTPLoginDA otpLoginDA)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
            _emailHelper = emailHelper;
            _otpLoginDA = otpLoginDA;
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
            var customerExistOrNot = customerAndInteriorData.FirstOrDefault(p => p.CustomerId == Id);
            if (customerExistOrNot != null)
            {
                if (customerExistOrNot.PhoneNumber == model.PhoneNumber)
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
                {
                    var customerPhoneNumberExit = customerAndInteriorData.FirstOrDefault(p => p.PhoneNumber == model.PhoneNumber);
                    if (customerPhoneNumberExit == null && (customerExistOrNot.PhoneNumber != model.PhoneNumber))
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
                    {
                        throw new Exception("Phone Number already exist. Please enter a different Phone Number.");
                    }
                }
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
            var customerTypeId = await _unitOfWorkDA.SubjectTypesDA.GetCustomerSubjectTypeId(cancellationToken);
            foreach (var item in model.CustomerList)
            {
                var customerData = await _unitOfWorkDA.CustomersDA.GetById(item, cancellationToken);
                if (customerData != null)
                {
                    NotificationManagementRequestDto notificationDto = new NotificationManagementRequestDto()
                    {
                        EntityTypeID = customerTypeId,
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

        public List<WrkImportCustomersDto> CustomerFileImport(WrkImportFilesRequestDto entity, long WrkImportFileID)
        {
            string[] customerHeaderArray = { "FirstName", "LastName", "PrimaryContactNumber", "AlternateContactNumber", "EmailID", "GSTNo", "Street1", "Street2", "Landmark", "Area", "City", "State", "PinCode" };
            List<WrkImportCustomersDto> insertWrkImportCustomersDtos = new List<WrkImportCustomersDto>();
            DataTable data = new DataTable();

            if (entity.formFile != null && !string.IsNullOrEmpty(entity.formFile.FileName) && entity.formFile.FileName.ToLower().Contains(".csv"))
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    PrepareHeaderForMatch = args => args.Header.ToLower(),
                };
                using (var reader = new StreamReader(entity.formFile.OpenReadStream()))
                using (var csv = new CsvReader(reader, config))
                {
                    csv.Read();
                    csv.ReadHeader();
                    string[] csvHeaders = csv.HeaderRecord;
                    string[] headers = new string[csvHeaders.Length];
                    for (int i = 0; i < csvHeaders.Length; i++)
                    {
                        if (csvHeaders[i] == null)
                        {
                            headers[i] = "Column" + (i + 1);
                        }
                        else
                        {
                            headers[i] = csvHeaders[i].Trim();
                        }
                    }
                    if (headers.SequenceEqual(customerHeaderArray))
                    {
                        var records = csv.GetRecords<WrkImportCustomersCSV>().ToList();
                        if (records != null && records.Count > 0)
                        {
                            foreach (var item in records)
                            {
                                WrkImportCustomersDto wrkImportCustomersDto = new WrkImportCustomersDto()
                                {
                                    WrkImportFileID = WrkImportFileID,
                                    AlternateContactNumber = item.AlternateContactNumber != "" ? item.AlternateContactNumber : null,
                                    Area = item.Area != "" ? item.Area : null,
                                    City = item.City != "" ? item.City : null,
                                    EmailID = item.EmailID != "" ? item.EmailID : null,
                                    FirstName = item.FirstName != "" ? item.FirstName : null,
                                    GSTNo = item.GSTNo != "" ? item.GSTNo : null,
                                    Landmark = item.Landmark != "" ? item.Landmark : null,
                                    LastName = item.LastName != "" ? item.LastName : null,
                                    PrimaryContactNumber = item.PrimaryContactNumber != "" ? item.PrimaryContactNumber : null,
                                    State = item.State != "" ? item.State : null,
                                    Status = (int)FileUploadStatusEnum.Pending,
                                    Stree2 = item.Stree2 != "" ? item.Stree2 : null,
                                    Street1 = item.Street1 != "" ? item.Street1 : null,
                                    ZipCode = item.PinCode != "" ? item.PinCode : null,
                                };
                                insertWrkImportCustomersDtos.Add(wrkImportCustomersDto);
                            }
                        }
                    }
                }
            }
            return insertWrkImportCustomersDtos;
        }

        public async Task ImportCustomers(long WrkImportFileID, CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _unitOfWorkDA = scope.ServiceProvider.GetRequiredService<IUnitOfWorkDA>();
                try
                {
                    if (WrkImportFileID > 0)
                    {
                        await _unitOfWorkDA.BeginTransactionAsync();
                        var wrkAllFileData = await _unitOfWorkDA.WrkImportFilesDA.GetAll(cancellationToken);
                        var wrkFileData = wrkAllFileData.FirstOrDefault(x => x.WrkImportFileID == WrkImportFileID);
                        wrkFileData.ProcessStartDate = DateTime.Now;
                        var getWrkImportCustomersData = await _unitOfWorkDA.WrkImportCustomersDA.GetAll(cancellationToken);
                        getWrkImportCustomersData = getWrkImportCustomersData.Where(x => x.WrkImportFileID == WrkImportFileID);
                        if (getWrkImportCustomersData != null && getWrkImportCustomersData.Count() > 0)
                        {
                            await _unitOfWorkDA.CustomersDA.CreateScope(_currentUser, cancellationToken);

                            var getCustomer = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
                            var CustomersTOInsert = getCustomer.Where(x => getWrkImportCustomersData.Select(y => y.PrimaryContactNumber).Contains(x.PhoneNumber)).ToList();
                            foreach (var item in getWrkImportCustomersData.ToList())
                            {
                                if (CustomersTOInsert.FirstOrDefault(x => x.PhoneNumber == item.PrimaryContactNumber) == null)
                                {
                                    Customers createdCustomer = await _unitOfWorkDA.CustomersDA.CreateCustomers(MapToDbObjectCustomer(item, new Customers()), cancellationToken);
                                    if (createdCustomer == null)
                                    {
                                        await WrkImportCustomersUpdate(item.WrkCustomerID, (int)FileUploadStatusEnum.Failed, cancellationToken);
                                    }
                                    else
                                    {
                                        await _unitOfWorkDA.CustomerAddressesDA.CreateCustomerAddress(MapDbToObjectCustomerAddress(createdCustomer.CustomerId, item, new CustomerAddresses()), cancellationToken);
                                    }

                                    await WrkImportCustomersUpdate(item.WrkCustomerID, (int)FileUploadStatusEnum.Completed, cancellationToken);
                                }
                                else
                                {
                                    await WrkImportCustomersUpdate(item.WrkCustomerID, (int)FileUploadStatusEnum.Failed, cancellationToken);
                                }
                            }

                            //Update WrkImportFiles Table for Count
                            if (wrkFileData != null)
                            {
                                wrkFileData.Success = getWrkImportCustomersData.Where(x => x.Status == (int)FileUploadStatusEnum.Completed).Count();
                                wrkFileData.Failed = getWrkImportCustomersData.Where(x => x.Status == (int)FileUploadStatusEnum.Failed).Count();
                                wrkFileData.Status = (int)FileUploadStatusEnum.Completed;
                                wrkFileData.ProcessEndDate = DateTime.Now;
                                wrkFileData.UpdatedBy = _currentUser.UserId;
                                wrkFileData.UpdatedDate = DateTime.Now;
                                wrkFileData.UpdatedUTCDate = DateTime.UtcNow;
                                await _unitOfWorkDA.WrkImportFilesDA.Update(wrkFileData, cancellationToken);
                            }
                        }
                        await _unitOfWorkDA.CommitTransactionAsync();
                    }
                }
                catch (Exception ex)
                {
                    await _unitOfWorkDA.rollbackTransactionAsync();
                    throw ex;
                }
            }
        }

        public async Task<OTPLogin> SendCustomerOtpAPI(CustomerApiRequestDto model, CancellationToken cancellationToken)
        {
            //Send OTP to customer through SMS
            //SendOTPToCustomer(model.PhoneNumber);

            OTPLogin loginToken = new OTPLogin()
            {
                PhoneNumber = model.PhoneNumber,
                OTP = new Random().Next(1, 9999).ToString("D4"),
                ExpiryTime = DateTime.UtcNow.AddMinutes(10),
            };
            var createdToken = await _otpLoginDA.CreateLoginToken(loginToken, cancellationToken);
            return createdToken;
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

        public async Task<bool> ValidateCustomerOtpAPI(CustomersRequestOtpDto model, CancellationToken cancellationToken)
        {
            var otpData = await _otpLoginDA.GetAll(cancellationToken);
            var otpDataByPhoneNo = otpData.FirstOrDefault(p => p.PhoneNumber == model.PhoneNumber);
            if (otpDataByPhoneNo != null)
            {
                if (otpDataByPhoneNo.OTP == model.OTP)
                {
                    if (DateTime.UtcNow < otpDataByPhoneNo.ExpiryTime)
                        return true;
                    throw new Exception("OTP is Expired");
                }
            }

            return false;
        }

        public async Task<OTPLogin> ResendCustomerOtpAPI(CustomersRequestOtpDto model, CancellationToken cancellationToken)
        {
            //Send OTP to customer through SMS
            //SendOTPToCustomer(model.PhoneNumber);

            var otpData = await _otpLoginDA.GetAll(cancellationToken);
            var oldOtpDataByPhoneNo = otpData.FirstOrDefault(p => p.PhoneNumber == model.PhoneNumber);

            oldOtpDataByPhoneNo.OTP = new Random().Next(1, 9999).ToString("D4");
            oldOtpDataByPhoneNo.ExpiryTime = DateTime.UtcNow.AddMinutes(10);
            var newOtpData = await _otpLoginDA.UpdateLoginToken(oldOtpDataByPhoneNo, cancellationToken);
            return newOtpData;
        }

        public async Task<CustomerVisitRequestDto> CustomerVisitAPI(CustomerVisitRequestDto model, CancellationToken cancellationToken)
        {
            var allCustomers = await GetAll(cancellationToken);
            var customerExists = allCustomers.FirstOrDefault(p => p.CustomerId == model.CustomerId);
            if (customerExists != null)
            {
                var customerVisitsToInsert = _mapper.Map<CustomerVisits>(model);
                customerVisitsToInsert.CustomerId = model.CustomerId;
                customerVisitsToInsert.Comment = model.Comment;
                customerVisitsToInsert.CreatedBy = _currentUser.UserId;
                customerVisitsToInsert.CreatedDate = DateTime.Now;
                customerVisitsToInsert.CreatedUTCDate = DateTime.UtcNow;
                var data = await _unitOfWorkDA.CustomersDA.CreateCustomerVisits(customerVisitsToInsert, cancellationToken);
                return _mapper.Map<CustomerVisitRequestDto>(data);
            }
            else
                throw new Exception("Customer doesn't exist.");
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

        private Customers MapToDbObjectCustomer(WrkImportCustomers entity, Customers model)
        {
            model.FirstName = entity.FirstName;
            model.LastName = entity.LastName;
            model.EmailId = entity.EmailID;
            model.PhoneNumber = entity.PrimaryContactNumber;
            model.AltPhoneNumber = entity.AlternateContactNumber;
            model.TenantId = _currentUser.TenantId;
            model.GSTNo = entity.GSTNo;
            model.CustomerTypeId = (int)CustomerTypeEnum.Customer;
            model.CreatedBy = _currentUser.UserId;
            model.CreatedDate = DateTime.Now;
            model.CreatedUTCDate = DateTime.UtcNow;
            return model;
        }

        private CustomerAddresses MapDbToObjectCustomerAddress(long CustomerId, WrkImportCustomers entity, CustomerAddresses model)
        {
            model.Street1 = entity.Street1;
            model.Street2 = entity.Stree2;
            model.Landmark = entity.Landmark;
            model.Area = entity.Area;
            model.AddressType = "Home";
            model.City = entity.City;
            model.State = entity.State;
            model.ZipCode = entity.ZipCode;
            model.CustomerId = CustomerId;
            model.CreatedBy = _currentUser.UserId;
            model.CreatedDate = DateTime.Now;
            model.CreatedUTCDate = DateTime.UtcNow;
            return model;
        }

        private async Task WrkImportCustomersUpdate(long WrkCustomerID, int status, CancellationToken cancellationToken)
        {
            var getWrkImportCustomers = await _unitOfWorkDA.WrkImportCustomersDA.GetById(WrkCustomerID, cancellationToken);
            getWrkImportCustomers.Status = status;
            getWrkImportCustomers.UpdatedBy = _currentUser.UserId;
            getWrkImportCustomers.UpdatedDate = DateTime.Now;
            getWrkImportCustomers.UpdatedUTCDate = DateTime.UtcNow;
            await _unitOfWorkDA.WrkImportCustomersDA.Update(getWrkImportCustomers, cancellationToken);
        }

        #endregion PrivateMethods
    }
}