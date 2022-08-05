using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Services.FileStorage;

namespace MidCapERP.BusinessLogic.UnitOfWork
{
    public interface IUnitOfWorkBL
    {
        IContractorsBL ContractorsBL { get; }
        ISubjectTypesBL SubjectTypesBL { get; }
        IContractorCategoryMappingBL ContractorCategoryMappingBL { get; }
        ICustomersBL CustomersBL { get; }
        IErrorLogsBL ErrorLogsBL { get; }
        ICategoryBL CategoryBL { get; }
        ICompanyBL CompanyBL { get; }
        IUnitBL UnitBL { get; }
        IWoodTypeBL WoodTypeBL { get; }
        IAccessoriesTypeBL AccessoriesTypeBL { get; }
        IRawMaterialBL RawMaterialBL { get; }
        IFileStorageService FileStorageService { get; }
    }
}