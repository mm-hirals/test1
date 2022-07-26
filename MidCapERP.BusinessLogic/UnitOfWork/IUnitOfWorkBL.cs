using MidCapERP.BusinessLogic.Interface;

namespace MidCapERP.BusinessLogic.UnitOfWork
{
    public interface IUnitOfWorkBL
    {
        ICategoriesBL CategoriesBL { get; }
        ILookupsBL LookupsBL { get; }
        IStatusesBL StatusesBL { get; }
        IContractorsBL ContractorsBL { get; }
        ISubjectTypesBL SubjectTypesBL { get; }
        ILookupValuesBL LookupValuesBL { get; }
    }
}