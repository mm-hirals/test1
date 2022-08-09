using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.Contractors;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Infrastructure.Constants;
using NToastNotify;

namespace MidCapERP.Admin.Controllers
{
    public class ContractorController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;
        private readonly IToastNotification _toastNotification;

        public ContractorController(IUnitOfWorkBL unitOfWorkBL, IToastNotification toastNotification)
        {
            _unitOfWorkBL = unitOfWorkBL;
            _toastNotification = toastNotification;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Contractor.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return base.View(await GetAllContractors(cancellationToken));
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Contractor.View)]
        public async Task<IActionResult> GetContractorData([FromForm] DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.ContractorsBL.GetFilterContractorData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Contractor.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            return PartialView("_ContractorPartial");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Contractor.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            var contractors = await _unitOfWorkBL.ContractorsBL.GetById(Id, cancellationToken);
            return PartialView("_ContractorPartial", contractors);
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Contractor.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.ContractorsBL.DeleteContractor(Id, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Contractor.Create)]
        public async Task<IActionResult> Create(ContractorsRequestDto contractorsRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.ContractorsBL.CreateContractor(contractorsRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Saved Successfully!");
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Contractor.Update)]
        public async Task<IActionResult> Update(int Id, ContractorsRequestDto contractorsRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.ContractorsBL.UpdateContractor(Id, contractorsRequestDto, cancellationToken);
            _toastNotification.AddSuccessToastMessage("Data Saved Successfully!");
            return RedirectToAction("Index");
        }

        #region PrivateMethods

        private async Task<IEnumerable<ContractorsResponseDto>> GetAllContractors(CancellationToken cancellationToken)
        {
            return await _unitOfWorkBL.ContractorsBL.GetAll(cancellationToken);
        }

        #endregion PrivateMethods
    }
}