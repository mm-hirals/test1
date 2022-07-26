using MidCapERP.DataAccess.Interface;

namespace MidCapERP.DataAccess.UnitOfWork
{
    public interface IUnitOfWorkDA
    {
        ICategoriesDA CategoriesDA { get; }
        ILookupsDA LookupsDA { get; }
        IStatusesDA StatusesDA { get; }  
        IContractorsDA ContractorsDA { get; }
        ISubjectTypesDA SubjectTypesDA { get; }
        ILookupValuesDA LookupValuesDA { get; }
        IContractorCategoryMappingDA ContractorCategoryMappingDA { get; }
        ICustomersDA CustomersDA { get; }
    }
}