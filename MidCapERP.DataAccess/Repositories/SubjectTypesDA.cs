using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;

namespace MidCapERP.DataAccess.Repositories
{
    public class SubjectTypesDA : ISubjectTypesDA
    {
        private readonly ISqlRepository<SubjectTypes> _subjectTypes;
        private readonly CurrentUser _currentUser;

        public SubjectTypesDA(ISqlRepository<SubjectTypes> subjectTypes, CurrentUser currentUser)
        {
            _subjectTypes = subjectTypes;
            _currentUser = currentUser;
        }

        public async Task<IQueryable<SubjectTypes>> GetAll(CancellationToken cancellationToken)
        {
            return await _subjectTypes.GetAsync(cancellationToken, x => x.TenantId == _currentUser.TenantId && x.IsDeleted == false);
        }

        public async Task<SubjectTypes> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _subjectTypes.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<SubjectTypes> CreateSubjectTypes(SubjectTypes model, CancellationToken cancellationToken)
        {
            return await _subjectTypes.InsertAsync(model, cancellationToken);
        }

        public async Task<SubjectTypes> UpdateSubjectTypes(int Id, SubjectTypes model, CancellationToken cancellationToken)
        {
            return await _subjectTypes.UpdateAsync(model, cancellationToken);
        }

        public async Task<SubjectTypes> DeleteSubjectTypes(int Id, CancellationToken cancellationToken)
        {
            var entity = await _subjectTypes.GetByIdAsync(Id, cancellationToken);
            if (entity != null)
            {
                return await _subjectTypes.UpdateAsync(entity, cancellationToken);
            }
            return entity;
        }
    }
}