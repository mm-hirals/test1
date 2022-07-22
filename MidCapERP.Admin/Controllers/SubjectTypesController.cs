using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.SubjectTypes;
using MidCapERP.Infrastructure.Constants;

namespace MidCapERP.Admin.Controllers
{
    public class SubjectTypesController : Controller
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public SubjectTypesController(IUnitOfWorkBL unitOfWorkBL)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.SubjectTypes.View)]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await GetAllSubjectTypes(cancellationToken));
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.SubjectTypes.Create)]
        public async Task<IActionResult> Create(int Id, CancellationToken cancellationToken)
        {
            return PartialView("_SubjectTypesPartial");
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.SubjectTypes.Create)]
        public async Task<IActionResult> Create(int Id, SubjectTypesRequestDto SubjectTypesRequestDto, CancellationToken cancellationToken)
        {
            var subjectTypes = await _unitOfWorkBL.SubjectTypesBL.CreateSubjectTypes(SubjectTypesRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.SubjectTypes.Update)]
        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            var SubjectTypes = await _unitOfWorkBL.SubjectTypesBL.GetById(Id, cancellationToken);
            return PartialView("_SubjectTypesPartial", SubjectTypes);
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.SubjectTypes.Update)]
        public async Task<IActionResult> Update(int Id, SubjectTypesRequestDto SubjectTypesRequestDto, CancellationToken cancellationToken)
        {
            Id = SubjectTypesRequestDto.SubjectTypeId;
            var SubjectTypes = await _unitOfWorkBL.SubjectTypesBL.UpdateSubjectTypes(Id, SubjectTypesRequestDto, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(ApplicationIdentityConstants.Permissions.SubjectTypes.Delete)]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            var SubjectTypes = await _unitOfWorkBL.SubjectTypesBL.DeleteSubjectTypes(Id, cancellationToken);
            return RedirectToAction("Index");
        }

        #region privateMethods
        private async Task<IEnumerable<SubjectTypesResponseDto>> GetAllSubjectTypes(CancellationToken cancellationToken)
        {
            return await _unitOfWorkBL.SubjectTypesBL.GetAll(cancellationToken);
        }
        #endregion
    }
}
