using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface ISubjectTypesDA
    {
        public Task<IQueryable<SubjectTypes>> GetAll(CancellationToken cancellationToken);

        public Task<SubjectTypes> GetById(int Id, CancellationToken cancellationToken);

        public Task<SubjectTypes> CreateSubjectTypes(SubjectTypes model, CancellationToken cancellationToken);

        public Task<SubjectTypes> UpdateSubjectTypes(int Id, SubjectTypes model, CancellationToken cancellationToken);

        public Task<SubjectTypes> DeleteSubjectTypes(int Id, CancellationToken cancellationToken);

        public Task<int> GetRawMaterialSubjectTypeId(CancellationToken cancellationToken);

        public Task<int> GetPolishSubjectTypeId(CancellationToken cancellationToken);

        public Task<int> GetProductSubjectTypeId(CancellationToken cancellationToken);

        public Task<int> GetFabricSubjectTypeId(CancellationToken cancellationToken);

        public Task<int> GetCustomerSubjectTypeId(CancellationToken cancellationToken);
    }
}