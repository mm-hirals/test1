using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto;
using MidCapERP.Dto.CustomerAddresses;
using MidCapERP.Dto.Customers;
using MidCapERP.Dto.WrkImportFiles;
using NToastNotify;
using System.Data;

namespace MidCapERP.Admin.Controllers
{
    public class CustomerController : BaseController
    {
        private IUnitOfWorkBL _unitOfWorkBL;
        private CurrentUser _currentUser;
        private ILogger<CustomerController> _logger;
        private readonly IToastNotification _toastNotification;
        public CustomerController(IUnitOfWorkBL unitOfWorkBL, IStringLocalizer<BaseController> localizer, CurrentUser currentUser, ILogger<CustomerController> logger, IToastNotification toastNotification) : base(localizer)
        {
            _unitOfWorkBL = unitOfWorkBL;
            _currentUser = currentUser;
            _logger = logger;
            _toastNotification = toastNotification;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.PortalCustomer.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            await FillRefferedDropDown(cancellationToken);
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.PortalCustomer.View)]
        public async Task<IActionResult> GetCustomersData([FromForm] CustomerDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.CustomersBL.GetFilterCustomersData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.PortalCustomer.View)]
        public async Task<IActionResult> GetCustomerAddressesData([FromForm] CustomerAddressDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.CustomerAddressesBL.GetFilterCustomerAddressesData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.PortalCustomer.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            await FillRefferedDropDown(cancellationToken);
            return PartialView("CustomerEdit");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.PortalCustomer.Create)]
        public async Task<IActionResult> Create(CustomersRequestDto customersRequestDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.CustomersBL.CreateCustomers(customersRequestDto, cancellationToken);
            return RedirectToAction("Update", "Customer", new { id = data.CustomerId });
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.PortalCustomer.Create)]
        public async Task<IActionResult> CreateCustomerAddress(int customerId, CancellationToken cancellationToken)
        {
            CustomerAddressesRequestDto dto = new();
            dto.CustomerId = customerId;
            return PartialView("_CustomerAddressPartial", dto);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.PortalCustomer.Create)]
        public async Task<IActionResult> CreateCustomerAddress(CustomerAddressesRequestDto customersRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.CustomerAddressesBL.CreateCustomerAddresses(customersRequestDto, cancellationToken);
            return View("_CustomerAddressPartial");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.PortalCustomer.Create)]
        public async Task<IActionResult> Update(Int64 Id, CancellationToken cancellationToken)
        {
            await FillRefferedDropDown(cancellationToken);
            var customers = await _unitOfWorkBL.CustomersBL.GetById(Id, cancellationToken);
            return View("CustomerEdit", customers);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.PortalCustomer.Create)]
        public async Task<IActionResult> Update(Int64 Id, CustomersRequestDto customersRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.CustomersBL.UpdateCustomers(Id, customersRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.PortalCustomer.Create)]
        public async Task<IActionResult> Delete(Int64 id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.CustomersBL.DeleteCustomers(id, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.PortalCustomer.Create)]
        public async Task<IActionResult> UpdateCustomerAddresses(Int64 Id, CancellationToken cancellationToken)
        {
            var customersAddress = await _unitOfWorkBL.CustomerAddressesBL.GetById(Id, cancellationToken);
            return PartialView("_CustomerAddressPartial", customersAddress);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.PortalCustomer.Create)]
        public async Task<IActionResult> UpdateCustomerAddresses(Int64 Id, CustomerAddressesRequestDto customersAddressRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.CustomerAddressesBL.UpdateCustomerAddresses(Id, customersAddressRequestDto, cancellationToken);
            return RedirectToAction("CustomerEdit");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.PortalCustomer.Delete)]
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
        public async Task<ActionResult> ImportCustomer(WrkImportFilesRequestDto entity, CancellationToken cancellationToken)
        {
            try
            {
                if (entity != null && entity.formFile != null)
                {
                    var getfileExtention = Path.GetExtension(entity.formFile.FileName);

                    if (getfileExtention.ToLower() != ".csv")
                    {
                        _toastNotification.AddErrorToastMessage("Please upload only csv file");
                        return View();
                    }
                    else
                    {
                        var wrkFileEntity = CovertRequestDtoToWrkImportFilesDto(entity);
                        var wrkFileData = await _unitOfWorkBL.WrkImportFilesBL.CreateWrkImportFiles(wrkFileEntity, cancellationToken);
                        var getWrkCustomerList = _unitOfWorkBL.CustomersBL.CustomerFileImport(entity, wrkFileData.WrkImportFileID);
                        _logger.LogWarning("Thread Started");
                        _ = Task.Run(async () =>
                        {
                            await _unitOfWorkBL.WrkImportCustomersBL.CreateWrkCustomer(getWrkCustomerList, cancellationToken);
                            await _unitOfWorkBL.CustomersBL.ImportCustomers(wrkFileData.WrkImportFileID, cancellationToken);
                        }, cancellationToken).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error : ImportCustomer");
                throw ex;
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
                FileType = Enum.GetName(typeof(CustomerTypeEnum), CustomerTypeEnum.Customer),
                ImportFileName = entity.formFile.FileName,
                TotalRecords = count - 1,
                Status = (int)FileUploadStatusEnum.Pending,
            };
        }

        #endregion Private Method
    }
}