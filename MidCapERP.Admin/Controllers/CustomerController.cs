using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto;
using MidCapERP.Dto.CustomerAddresses;
using MidCapERP.Dto.Customers;
using MidCapERP.Dto.WrkImportCustomers;
using MidCapERP.Dto.WrkImportFiles;
using System.Data;
using System.Globalization;

namespace MidCapERP.Admin.Controllers
{
    public class CustomerController : BaseController
    {
        private IUnitOfWorkBL _unitOfWorkBL;
        private readonly CurrentUser _currentUser;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public CustomerController(IUnitOfWorkBL unitOfWorkBL, CurrentUser currentUser, IStringLocalizer<BaseController> localizer, IServiceScopeFactory serviceScopeFactory) : base(localizer)
        {
            _unitOfWorkBL = unitOfWorkBL;
            _currentUser = currentUser;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Customer.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            await FillRefferedDropDown(cancellationToken);
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.View)]
        public async Task<IActionResult> GetCustomersData([FromForm] CustomerDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.CustomersBL.GetFilterCustomersData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.CustomerAddress.View)]
        public async Task<IActionResult> GetCustomerAddressesData([FromForm] CustomerAddressDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.CustomerAddressesBL.GetFilterCustomerAddressesData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            await FillRefferedDropDown(cancellationToken);
            return PartialView("CustomerEdit");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.Create)]
        public async Task<IActionResult> Create(CustomersRequestDto customersRequestDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.CustomersBL.CreateCustomers(customersRequestDto, cancellationToken);
            return RedirectToAction("Update", "Customer", new { id = data.CustomerId });
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.CustomerAddress.Create)]
        public async Task<IActionResult> CreateCustomerAddress(int customerId, CancellationToken cancellationToken)
        {
            CustomerAddressesRequestDto dto = new();
            dto.CustomerId = customerId;
            return PartialView("_CustomerAddressPartial", dto);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.CustomerAddress.Create)]
        public async Task<IActionResult> CreateCustomerAddress(CustomerAddressesRequestDto customersRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.CustomerAddressesBL.CreateCustomerAddresses(customersRequestDto, cancellationToken);
            return View("_CustomerAddressPartial");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.Update)]
        public async Task<IActionResult> Update(Int64 Id, CancellationToken cancellationToken)
        {
            await FillRefferedDropDown(cancellationToken);
            var customers = await _unitOfWorkBL.CustomersBL.GetById(Id, cancellationToken);
            return View("CustomerEdit", customers);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Customer.Update)]
        public async Task<IActionResult> Update(Int64 Id, CustomersRequestDto customersRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.CustomersBL.UpdateCustomers(Id, customersRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.CustomerAddress.Update)]
        public async Task<IActionResult> UpdateCustomerAddresses(Int64 Id, CancellationToken cancellationToken)
        {
            var customersAddress = await _unitOfWorkBL.CustomerAddressesBL.GetById(Id, cancellationToken);
            return PartialView("_CustomerAddressPartial", customersAddress);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.CustomerAddress.Update)]
        public async Task<IActionResult> UpdateCustomerAddresses(Int64 Id, CustomerAddressesRequestDto customersAddressRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.CustomerAddressesBL.UpdateCustomerAddresses(Id, customersAddressRequestDto, cancellationToken);
            return RedirectToAction("CustomerEdit");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.CustomerAddress.Delete)]
        public async Task<IActionResult> DeleteCustomerAddresses(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.CustomerAddressesBL.DeleteCustomerAddresses(Id, cancellationToken);
            return Json(true);
        }

        [HttpPost]
        public async Task<JsonResult> MultipleSendCustomer(CustomersSendSMSDto model, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWorkBL.CustomersBL.SendSMSToCustomers(model, cancellationToken);
                return Json("success");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult ImportCustomer()
        {
            WrkImportFilesRequestDto wrkImportFilesRequestDto = new WrkImportFilesRequestDto();
            return View(wrkImportFilesRequestDto);
        }

        [HttpPost]
        public async Task<ActionResult> ImportCustomer([FromServices]IServiceScopeFactory
                                    serviceScopeFactory, WrkImportFilesRequestDto entity, CancellationToken cancellationToken)
        {
            try
            {
                if (entity != null && entity.formFile != null)
                {
                    var wrkFileEntity = CovertRequestDtoToWrkImportFilesDto(entity);
                    var wrkFileData = await _unitOfWorkBL.WrkImportFilesBL.CreateWrkImportFiles(wrkFileEntity, cancellationToken);
                    var getWrkCustomerList = CustomerFileImport(entity, wrkFileData.WrkImportFileID);

                    _ = Task.Run(async () =>
                    {
                        using (var scope = serviceScopeFactory.CreateScope())
                        {
                            _unitOfWorkBL = scope.ServiceProvider.GetRequiredService<IUnitOfWorkBL>();
                            await _unitOfWorkBL.WrkImportCustomersBL.CreateWrkCustomer(getWrkCustomerList, cancellationToken);
                            await _unitOfWorkBL.CustomersBL.ImportCustomers(wrkFileData.WrkImportFileID, cancellationToken);
                        }
                    }, cancellationToken).ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("Index");
        }

        public async Task<bool> DuplicateCustomerPhoneNumber(CustomersRequestDto customersRequestDto, CancellationToken cancellationToken)
        {
            return await _unitOfWorkBL.CustomersBL.ValidateCustomerPhoneNumber(customersRequestDto, cancellationToken);
        }

        #region Private Method

        private async Task FillRefferedDropDown(CancellationToken cancellationToken)
        {
            try
            {
                var customerData = await _unitOfWorkBL.CustomersBL.GetAll(cancellationToken);

                var referedByDataSelectedList = customerData.Where(p => p.CustomerTypeId == (int)CustomerTypeEnum.Interior || p.CustomerTypeId == (int)CustomerTypeEnum.Customer).Select(
                                        p => new { p.CustomerId, p.FirstName, p.LastName }).Select(a =>
                                        new SelectListItem
                                        {
                                            Value = Convert.ToString(a.CustomerId),
                                            Text = Convert.ToString(a.FirstName + " " + a.LastName)
                                        }).ToList();
                ViewBag.ReferedBySelectItemList = referedByDataSelectedList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static List<WrkImportCustomersDto> CustomerFileImport(WrkImportFilesRequestDto entity, long WrkImportFileID)
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

        private WrkImportFilesDto CovertRequestDtoToWrkImportFilesDto(WrkImportFilesRequestDto entity)
        {
            int count = 0;
            using (StreamReader file = new(entity.formFile.OpenReadStream()))
            {
                while (file.ReadLine() != null)
                    count++;
                file.Close();
            }

            return new WrkImportFilesDto()
            {
                FileType = "Customer",
                ImportFileName = entity.formFile.FileName,
                TotalRecords = count - 1,
                Status = (int)FileUploadStatusEnum.Pending,
                CreatedBy = _currentUser.UserId,
                TenantId = _currentUser.TenantId,
                CreatedDate = DateTime.Now,
                CreatedUTCDate = DateTime.UtcNow,
            };
        }

        #endregion Private Method
    }
}