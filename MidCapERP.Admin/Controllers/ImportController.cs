using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto;
using MidCapERP.Dto.WrkImportFiles;
using NToastNotify;

namespace MidCapERP.Admin.Controllers
{
    public class ImportController : BaseController
    {
        private IUnitOfWorkBL _unitOfWorkBL;
        private CurrentUser _currentUser;
        private ILogger<CustomerController> _logger;
        private readonly IToastNotification _toastNotification;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImportController(IHttpContextAccessor httpContextAccessor, IUnitOfWorkBL unitOfWorkBL, IStringLocalizer<BaseController> localizer, CurrentUser currentUser, ILogger<CustomerController> logger, IToastNotification toastNotification) : base(localizer)
        {
            _unitOfWorkBL = unitOfWorkBL;
            _currentUser = currentUser;
            _logger = logger;
            _toastNotification = toastNotification;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public ActionResult Index(string Id, CancellationToken cancellationToken)
        {
            WrkImportFilesRequestDto wrkImportFilesRequestDto = new WrkImportFilesRequestDto();
            return View(wrkImportFilesRequestDto);
        }

        [HttpPost]
        public async Task<ActionResult> Index(string Id, WrkImportFilesRequestDto entity, CancellationToken cancellationToken)
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
                        if (Id == "Customer")
                        {
                            var CustomerType = Enum.GetName(typeof(CustomerTypeEnum), CustomerTypeEnum.Customer);
                            var wrkFileEntity = CovertRequestDtoToWrkImportFilesDto(CustomerType, entity);
                            var wrkFileData = await _unitOfWorkBL.WrkImportFilesBL.CreateWrkImportFiles(wrkFileEntity, cancellationToken);
                            var getWrkCustomerList = _unitOfWorkBL.CustomersBL.CustomerFileImport(entity, wrkFileData.WrkImportFileID);
                            _logger.LogWarning("Thread Started");
                            _ = Task.Run(async () =>
                            {
                                await _unitOfWorkBL.WrkImportCustomersBL.CreateWrkCustomer(getWrkCustomerList, cancellationToken);
                                await _unitOfWorkBL.CustomersBL.ImportCustomers(wrkFileData.WrkImportFileID, cancellationToken);
                            }, cancellationToken).ConfigureAwait(false);
                            return Redirect("/Customer");
                        }
                        else if (Id == "Interior")
                        {
                            var CustomerType = Enum.GetName(typeof(CustomerTypeEnum), CustomerTypeEnum.Interior);
                            var wrkFileEntity = CovertRequestDtoToWrkImportFilesDto(CustomerType, entity);
                            var wrkFileData = await _unitOfWorkBL.WrkImportFilesBL.CreateWrkImportFiles(wrkFileEntity, cancellationToken);
                            var getWrkCustomerList = _unitOfWorkBL.CustomersBL.CustomerFileImport(entity, wrkFileData.WrkImportFileID);
                            _logger.LogWarning("Thread Started");
                            _ = Task.Run(async () =>
                            {
                                await _unitOfWorkBL.WrkImportCustomersBL.CreateWrkCustomer(getWrkCustomerList, cancellationToken);
                                await _unitOfWorkBL.CustomersBL.ImportCustomers(wrkFileData.WrkImportFileID, cancellationToken);
                            }, cancellationToken).ConfigureAwait(false);
                            return Redirect("/Interior");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                _logger.LogError(1, ex, "Error : ImportCustomer");
                throw ex;
            }
            return View();
        }

        #region private Method

        private WrkImportFilesDto CovertRequestDtoToWrkImportFilesDto(string CustomerType, WrkImportFilesRequestDto entity)
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
                FileType = CustomerType,
                ImportFileName = entity.formFile.FileName,
                TotalRecords = count - 1,
                Status = (int)FileUploadStatusEnum.Pending,
            };
        }

        #endregion private Method
    }
}