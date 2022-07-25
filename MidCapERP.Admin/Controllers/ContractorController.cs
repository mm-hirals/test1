using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.Contractors;
using MidCapERP.Infrastructure.Constants;

namespace MidCapERP.Admin.Controllers
{
    public class ContractorController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public ContractorController(IUnitOfWorkBL unitOfWorkBL)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Contractor.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await _unitOfWorkBL.ContractorsBL.GetAllContractor(cancellationToken));
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
            var contractors = await _unitOfWorkBL.ContractorsBL.DeleteContractor(Id, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Contractor.Create)]
        public async Task<IActionResult> Create(ContractorsRequestDto contractorsRequestDto, CancellationToken cancellationToken)
        {
            var contractors = await _unitOfWorkBL.ContractorsBL.CreateContractor(contractorsRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Contractor.Update)]
        public async Task<IActionResult> Update(int Id, ContractorsRequestDto contractorsRequestDto, CancellationToken cancellationToken)
        {
            Id = contractorsRequestDto.ContractorId;
            var contractors = await _unitOfWorkBL.ContractorsBL.UpdateContractor(Id, contractorsRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }
    }
}
