using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.SubjectTypes;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class SubjectTypesBL : ISubjectTypesBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public SubjectTypesBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<SubjectTypesResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.SubjectTypesDA.GetAll(cancellationToken);
            return _mapper.Map<List<SubjectTypesResponseDto>>(data.ToList());
        }

        public async Task<SubjectTypesResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken)
        {
            var data = await GetSubjectTypeById(Id, cancellationToken);
            return _mapper.Map<SubjectTypesResponseDto>(data);
        }

        public async Task<SubjectTypesRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var data = await GetSubjectTypeById(Id, cancellationToken);
            return _mapper.Map<SubjectTypesRequestDto>(data);
        }

        public async Task<SubjectTypesRequestDto> CreateSubjectTypes(SubjectTypesRequestDto model, CancellationToken cancellationToken)
        {
            var subjectTypesToInsert = _mapper.Map<SubjectTypes>(model);
            subjectTypesToInsert.IsDeleted = true;
            subjectTypesToInsert.TenantId = _currentUser.TenantId;
            subjectTypesToInsert.CreatedBy = _currentUser.UserId;
            subjectTypesToInsert.CreatedDate = DateTime.Now;
            subjectTypesToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.SubjectTypesDA.CreateSubjectTypes(subjectTypesToInsert, cancellationToken);
            return _mapper.Map<SubjectTypesRequestDto>(data);
        }

        public async Task<SubjectTypesRequestDto> UpdateSubjectTypes(int Id, SubjectTypesRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await GetSubjectTypeById(Id, cancellationToken);
            oldData.Comments = model.Comments;
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.SubjectTypesDA.UpdateSubjectTypes(Id, oldData, cancellationToken);
            return _mapper.Map<SubjectTypesRequestDto>(data);
        }

        public async Task<SubjectTypesRequestDto> DeleteSubjectTypes(int Id, CancellationToken cancellationToken)
        {
            var subjectTypesToUpdate = await GetSubjectTypeById(Id, cancellationToken);
            subjectTypesToUpdate.IsDeleted = true;
            subjectTypesToUpdate.UpdatedBy = _currentUser.UserId;
            subjectTypesToUpdate.UpdatedDate = DateTime.Now;
            subjectTypesToUpdate.UpdatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.SubjectTypesDA.UpdateSubjectTypes(Id, subjectTypesToUpdate, cancellationToken);
            return _mapper.Map<SubjectTypesRequestDto>(data);
        }

        #region Private Methods

        private static void MapToDbObject(SubjectTypesRequestDto model, SubjectTypes oldData)
        {
            oldData.SubjectTypeName = model.SubjectTypeName;
        }

        private async Task<SubjectTypes> GetSubjectTypeById(int Id, CancellationToken cancellationToken)
        {
            var subjectTypesToUpdate = await _unitOfWorkDA.SubjectTypesDA.GetById(Id, cancellationToken);
            if (subjectTypesToUpdate == null)
            {
                throw new Exception("SubjectType not found");
            }
            return subjectTypesToUpdate;
        }

        #endregion Private Methods
    }
}