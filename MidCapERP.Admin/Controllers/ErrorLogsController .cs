using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto.ErrorLogs;

namespace MidCapERP.Admin.Controllers
{
    public class ErrorLogsController : Controller
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public ErrorLogsController(IUnitOfWorkBL unitOfWorkBL)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.ErrorLogs.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await GetAllErrorLogs(cancellationToken));
        }

        #region PrivateMethods

        private async Task<IEnumerable<ErrorLogsResponseDto>> GetAllErrorLogs(CancellationToken cancellationToken)
        {
            return await _unitOfWorkBL.ErrorLogsBL.GetAll(cancellationToken);
        }

        #endregion PrivateMethods
    }
}