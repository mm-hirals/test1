using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.Dto.Contractors;
using MidCapERP.Dto.DataGrid;

namespace MidCapERP.Admin.Controllers
{
    public class ContractorController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public ContractorController(IUnitOfWorkBL unitOfWorkBL, IStringLocalizer<BaseController> localizer) : base(localizer)
        {
            _unitOfWorkBL = unitOfWorkBL;
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
            await FillCategoryDropDown(cancellationToken);
            return PartialView("_ContractorPartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Contractor.Create)]
        public async Task<IActionResult> Create(ContractorsRequestDto contractorsRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.ContractorsBL.CreateContractor(contractorsRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Contractor.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            var contractors = await _unitOfWorkBL.ContractorsBL.GetContractorCategoryMappingById(Id, cancellationToken);
            await FillCategoryDropDown(cancellationToken);
            return PartialView("_ContractorPartial", contractors);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Contractor.Update)]
        public async Task<IActionResult> Update(int Id, ContractorsRequestDto contractorsRequestDto, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.ContractorsBL.UpdateContractor(Id, contractorsRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.Contractor.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            await _unitOfWorkBL.ContractorsBL.DeleteContractor(Id, cancellationToken);
            return RedirectToAction("Index");
        }

        #region PrivateMethods

        private async Task FillCategoryDropDown(CancellationToken cancellationToken)
        {
            var categoryTypeData = await _unitOfWorkBL.CategoryBL.GetAll(cancellationToken);
            var categorySelectedList = categoryTypeData.Select(a =>
                                 new SelectListItem
                                 {
                                     Value = Convert.ToString(a.LookupValueId),
                                     Text = a.LookupValueName
                                 }).ToList();
            ViewBag.ContractorSelectedListSelectItemList = categorySelectedList;
        }

        private async Task<IEnumerable<ContractorsResponseDto>> GetAllContractors(CancellationToken cancellationToken)
        {
            return await _unitOfWorkBL.ContractorsBL.GetAll(cancellationToken);
        }

        #endregion PrivateMethods
    }
}