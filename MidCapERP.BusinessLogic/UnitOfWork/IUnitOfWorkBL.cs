using MidCapERP.BusinessLogic.Interface;

namespace MidCapERP.BusinessLogic.UnitOfWork
{
    public interface IUnitOfWorkBL
    {
        ILookupsBL LookupsBL { get; }
        IStatusBL StatusBL { get; }
        IContractorsBL ContractorsBL { get; }
        ISubjectTypesBL SubjectTypesBL { get; }
        ILookupValuesBL LookupValuesBL { get; }
        IContractorCategoryMappingBL ContractorCategoryMappingBL { get; }
        ICustomersBL CustomersBL { get; }
        IErrorLogsBL ErrorLogsBL { get; }
        ICategoryBL CategoryBL { get; }
        IUnitBL UnitBL { get; }
    }
}