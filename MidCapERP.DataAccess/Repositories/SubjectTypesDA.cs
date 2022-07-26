using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class SubjectTypesDA : ISubjectTypesDA
    {
        private readonly ISqlRepository<SubjectTypes> _SubjectTypes;

        public SubjectTypesDA(ISqlRepository<SubjectTypes> subjectTypes)
        {
            _SubjectTypes = subjectTypes;
        }

        public async Task<IQueryable<SubjectTypes>> GetAll(CancellationToken cancellationToken)
        {
            return await _SubjectTypes.GetAsync(cancellationToken);
        }

        public async Task<SubjectTypes> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _SubjectTypes.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<SubjectTypes> CreateSubjectTypes(SubjectTypes model, CancellationToken cancellationToken)
        {
            return await _SubjectTypes.InsertAsync(model, cancellationToken);
        }

        public async Task<SubjectTypes> UpdateSubjectTypes(int Id, SubjectTypes model, CancellationToken cancellationToken)
        {
            return await _SubjectTypes.UpdateAsync(model, cancellationToken);
        }

        public async Task<SubjectTypes> DeleteSubjectTypes(int Id, CancellationToken cancellationToken)
        {
            var entity = await _SubjectTypes.GetByIdAsync(Id, cancellationToken);
            if (entity != null)
            {
                return await _SubjectTypes.UpdateAsync(entity, cancellationToken);
            }
            return entity;
        }
    }
}

