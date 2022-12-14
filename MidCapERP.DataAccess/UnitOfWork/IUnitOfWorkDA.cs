using MidCapERP.DataAccess.Interface;

namespace MidCapERP.DataAccess.UnitOfWork
{
    public interface IUnitOfWorkDA
    {
        ILookupsDA LookupsDA { get; }
        IContractorsDA ContractorsDA { get; }
        ISubjectTypesDA SubjectTypesDA { get; }
        ILookupValuesDA LookupValuesDA { get; }
        IContractorCategoryMappingDA ContractorCategoryMappingDA { get; }
        ICustomersDA CustomersDA { get; }
        IErrorLogsDA ErrorLogsDA { get; }
        IAccessoriesTypeDA AccessoriesTypeDA { get; }
        IRawMaterialDA RawMaterialDA { get; }
        IAccessoriesDA AccessoriesDA { get; }
        IFabricDA FabricDA { get; }
        IFrameDA FrameDA { get; }
        IPolishDA PolishDA { get; }
        ITenantDA TenantDA { get; }
        IUserTenantMappingDA UserTenantMappingDA { get; }
        IUserDA UserDA { get; }
        ICustomerAddressesDA CustomerAddressesDA { get; }
        ICustomerTypesDA CustomerTypesDA { get; }
        IProductDA ProductDA { get; }
        IProductImageDA ProductImageDA { get; }
        IProductMaterialDA ProductMaterialDA { get; }
        IRoleDA RoleDA { get; }
        IRolePermissionDA RolePermissionDA { get; }
        IOrderDA OrderDA { get; }

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task rollbackTransactionAsync();
    }
}