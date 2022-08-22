using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Services.FileStorage;
using MidCapERP.DataEntities;

namespace MidCapERP.BusinessLogic.UnitOfWork
{
    public class UnitOfWorkBL : IUnitOfWorkBL
    {
        private readonly ApplicationDbContext _context;
        public IContractorsBL ContractorsBL { get; }
        public ISubjectTypesBL SubjectTypesBL { get; }
        public IContractorCategoryMappingBL ContractorCategoryMappingBL { get; }
        public ICustomersBL CustomersBL { get; }
        public IErrorLogsBL ErrorLogsBL { get; }
        public ICategoryBL CategoryBL { get; }
        public ICompanyBL CompanyBL { get; }
        public IUnitBL UnitBL { get; }
        public IWoodTypeBL WoodTypeBL { get; }
        public IAccessoriesTypeBL AccessoriesTypeBL { get; }
        public IRawMaterialBL RawMaterialBL { get; }
        public IFileStorageService FileStorageService { get; }
        public IAccessoriesBL AccessoriesBL { get; }
        public IFabricBL FabricBL { get; }
        public IWoodBL WoodBL { get; }
        public IPolishBL PolishBL { get; }
        public IUserTenantMappingBL UserTenantMappingBL { get; }
        public IUserBL UserBL { get; }

        public UnitOfWorkBL(ApplicationDbContext context, IContractorsBL contractorsBL, ISubjectTypesBL subjectTypesBL, IContractorCategoryMappingBL contractorCategoryMapping, ICustomersBL customersBL, IErrorLogsBL errorLogsBL, ICategoryBL categoryBL, ICompanyBL companyBL, IUnitBL unitBL, IWoodTypeBL woodTypeBL, IAccessoriesTypeBL accessoriesTypesBL, IRawMaterialBL rawMaterialBL, IAccessoriesBL accessoriesBL, IFileStorageService fileStorageService, IFabricBL fabricBL, IWoodBL woodBL, IPolishBL polishBL, IUserTenantMappingBL userTenantMappingBL, IUserBL userBL)
        {
            this._context = context;
            this.ContractorsBL = contractorsBL;
            this.SubjectTypesBL = subjectTypesBL;
            this.ContractorCategoryMappingBL = contractorCategoryMapping;
            this.CustomersBL = customersBL;
            this.ErrorLogsBL = errorLogsBL;
            this.CategoryBL = categoryBL;
            this.CompanyBL = companyBL;
            this.UnitBL = unitBL;
            this.WoodTypeBL = woodTypeBL;
            this.AccessoriesTypeBL = accessoriesTypesBL;
            this.RawMaterialBL = rawMaterialBL;
            this.FileStorageService = fileStorageService;
            this.AccessoriesBL = accessoriesBL;
            this.FabricBL = fabricBL;
            this.WoodBL = woodBL;
            this.PolishBL = polishBL;
            this.UserTenantMappingBL = userTenantMappingBL;
            this.UserBL = userBL;
        }

        #region DisposeMethod

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion DisposeMethod
    }
}