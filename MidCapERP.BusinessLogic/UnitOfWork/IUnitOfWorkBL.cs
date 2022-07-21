using MidCapERP.BusinessLogic.Interface;

namespace MidCapERP.BusinessLogic.UnitOfWork
{
    public interface IUnitOfWorkBL
    {
        ICategoriesBL CategoriesBL { get; }
        ISubjectTypesBL SubjectTypesBL { get; }
    }
}