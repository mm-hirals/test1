using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Constants;

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

        public async Task<int> GetRawMaterialSubjectTypeId(CancellationToken cancellationToken)
        {
            var subjectTypeAllData = await GetAll(cancellationToken);
            var subjectType = subjectTypeAllData.FirstOrDefault(x => x.SubjectTypeName == nameof(SubjectTypesEnum.RawMaterials));
            return subjectType == null ? 0 : subjectType.SubjectTypeId;
        }

        public async Task<int> GetPolishSubjectTypeId(CancellationToken cancellationToken)
        {
            var subjectTypeAllData = await GetAll(cancellationToken);
            var subjectType = subjectTypeAllData.FirstOrDefault(x => x.SubjectTypeName == nameof(SubjectTypesEnum.Polish));
            return subjectType == null ? 0 : subjectType.SubjectTypeId;
        }

        public async Task<int> GetProductSubjectTypeId(CancellationToken cancellationToken)
        {
            var subjectTypeAllData = await GetAll(cancellationToken);
            var subjectType = subjectTypeAllData.FirstOrDefault(x => x.SubjectTypeName == nameof(SubjectTypesEnum.Products));
            return subjectType == null ? 0 : subjectType.SubjectTypeId;
        }

        public async Task<int> GetFabricSubjectTypeId(CancellationToken cancellationToken)
        {
            var subjectTypeAllData = await GetAll(cancellationToken);
            var subjectType = subjectTypeAllData.FirstOrDefault(x => x.SubjectTypeName == nameof(SubjectTypesEnum.Fabrics));
            return subjectType == null ? 0 : subjectType.SubjectTypeId;
        }

        public async Task<int> GetCustomerSubjectTypeId(CancellationToken cancellationToken)
        {
            var subjectTypeAllData = await GetAll(cancellationToken);
            var subjectType = subjectTypeAllData.FirstOrDefault(x => x.SubjectTypeName == nameof(SubjectTypesEnum.Customers));
            return subjectType == null ? 0 : subjectType.SubjectTypeId;
        }

        public async Task<int> GetProductQuantitySubjectTypeId(CancellationToken cancellationToken)
        {
            var subjectTypeAllData = await GetAll(cancellationToken);
            var subjectType = subjectTypeAllData.FirstOrDefault(x => x.SubjectTypeName == nameof(SubjectTypesEnum.ProductQuantity));
            return subjectType == null ? 0 : subjectType.SubjectTypeId;
        }

        public async Task<int> GetOrderSubjectTypeId(CancellationToken cancellationToken)
        {
            var subjectTypeAllData = await GetAll(cancellationToken);
            var subjectType = subjectTypeAllData.FirstOrDefault(x => x.SubjectTypeName == nameof(SubjectTypesEnum.Orders));
            return subjectType == null ? 0 : subjectType.SubjectTypeId;
        }
    }
}