using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.ContractorCategoryMapping;
using MidCapERP.Infrastructure.Constants;

namespace MidCapERP.Admin.Controllers
{
    public class ContractorCategoryMappingController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public ContractorCategoryMappingController(IUnitOfWorkBL unitOfWorkBL)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.ContractorCategoryMapping.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await GetAllContractorCategoryMapping(cancellationToken));
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.ContractorCategoryMapping.Create)]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            return PartialView("_ContractorCategoryMappingPartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.ContractorCategoryMapping.Create)]
        public async Task<IActionResult> Create(ContractorCategoryMappingRequestDto contractorCategoryMappingRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.ContractorCategoryMappingBL.CreateContractorCategoryMapping(contractorCategoryMappingRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.ContractorCategoryMapping.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            var contractorCategoryMapping = await _unitOfWorkBL.ContractorCategoryMappingBL.GetById(Id, cancellationToken);
            return PartialView("_ContractorCategoryMappingPartial", contractorCategoryMapping);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.ContractorCategoryMapping.Update)]
        public async Task<IActionResult> Update(int Id, ContractorCategoryMappingRequestDto contractorCategoryMappingRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.ContractorCategoryMappingBL.UpdateContractorCategoryMapping(Id, contractorCategoryMappingRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.ContractorCategoryMapping.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.ContractorCategoryMappingBL.DeleteContractorCategoryMapping(Id, cancellationToken);
            return RedirectToAction("Index");
        }

        #region PrivateMethods

        private async Task<IEnumerable<ContractorCategoryMappingResponseDto>> GetAllContractorCategoryMapping(CancellationToken cancellationToken)
        {
            return await _unitOfWorkBL.ContractorCategoryMappingBL.GetAll(cancellationToken);
        }

        #endregion PrivateMethods
    }
}