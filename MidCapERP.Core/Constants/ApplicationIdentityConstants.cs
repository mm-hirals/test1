namespace MidCapERP.Core.Constants
{
    public static class ApplicationIdentityConstants
    {
        public static readonly string DefaultPassword = "Password@1";
        public static readonly string TenantCookieName = "TenantId";
        public static readonly string TenantHeaderName = "TenantId";
        public static readonly string EncryptionSecret = "MAGNUSMINDS_SAB_KA_BAAP";

        public static class Roles
        {
            public static readonly string Administrator = "Administrator";
            public static readonly string SalesRepresentative = "SalesRepresentative";
            public static readonly string Supervisor = "Supervisor";
            public static readonly string Contractor = "Contractor";
            public static readonly string StoreManager = "StoreManager";
            public static readonly string[] RolesSupported = { Administrator, StoreManager, Supervisor, SalesRepresentative, Contractor };
        }

        public static class Permissions
        {
            public static List<string> GeneratePermissionsForModule(string module)
            {
                return new List<string>()
                {
                    $"Permissions.{module}.Create",
                    $"Permissions.{module}.View",
                    $"Permissions.{module}.Update",
                    $"Permissions.{module}.Delete",
                };
            }

            public static List<string> GetAllPermissions()
            {
                return GeneratePermissionsForModule("Users")
                .Union(GeneratePermissionsForModule("Role"))
                .Union(GeneratePermissionsForModule("Dashboard"))
                .Union(GeneratePermissionsForModule("Lookup"))
                .Union(GeneratePermissionsForModule("Status"))
                .Union(GeneratePermissionsForModule("Contractor"))
                .Union(GeneratePermissionsForModule("SubjectType"))
                .Union(GeneratePermissionsForModule("LookupValues"))
                .Union(GeneratePermissionsForModule("ContractorCategoryMapping"))
                .Union(GeneratePermissionsForModule("Customer"))
                .Union(GeneratePermissionsForModule("ErrorLogs"))
                .Union(GeneratePermissionsForModule("Category"))
                .Union(GeneratePermissionsForModule("Company"))
                .Union(GeneratePermissionsForModule("Unit"))
                .Union(GeneratePermissionsForModule("FrameType"))
                .Union(GeneratePermissionsForModule("AccessoriesType"))
                .Union(GeneratePermissionsForModule("RawMaterial"))
                .Union(GeneratePermissionsForModule("Accessories"))
                .Union(GeneratePermissionsForModule("Fabric"))
                .Union(GeneratePermissionsForModule("Frame"))
                .Union(GeneratePermissionsForModule("Polish"))
                .Union(GeneratePermissionsForModule("User"))
                .Union(GeneratePermissionsForModule("CustomerAddresses"))
                .Union(GeneratePermissionsForModule("CustomerTypes"))
                .Union(GeneratePermissionsForModule("Product"))
                .Union(GeneratePermissionsForModule("RolePermission"))
                .Union(GeneratePermissionsForModule("Product"))
                .Union(GeneratePermissionsForModule("Tenant"))
                .Union(GeneratePermissionsForModule("TenantBankDetail"))
                .ToList();
            }

            public static bool CheckPermission(string permission)
            {
                return GetAllPermissions().Contains(permission);
            }

            public static bool CheckPermission(string permission, string module)
            {
                return GeneratePermissionsForModule(module).Contains(permission);
            }

            public static class Users
            {
                public const string View = "Permissions.Users.View";
                public const string Create = "Permissions.Users.Create";
                public const string Update = "Permissions.Users.Update";
                public const string Delete = "Permissions.Users.Delete";
            }

            public static class Role
            {
                public const string View = "Permissions.Role.View";
                public const string Create = "Permissions.Role.Create";
                public const string Update = "Permissions.Role.Update";
                public const string Delete = "Permissions.Role.Delete";
            }

            public static class Dashboard
            {
                public const string View = "Permissions.Dashboard.View";
                public const string Create = "Permissions.Dashboard.Create";
                public const string Update = "Permissions.Dashboard.Update";
                public const string Delete = "Permissions.Dashboard.Delete";
            }

            public static class Lookup
            {
                public const string View = "Permissions.Lookup.View";
                public const string Create = "Permissions.Lookup.Create";
                public const string Update = "Permissions.Lookup.Update";
                public const string Delete = "Permissions.Lookup.Delete";
            }

            public static class Status
            {
                public const string View = "Permissions.Status.View";
                public const string Create = "Permissions.Status.Create";
                public const string Update = "Permissions.Status.Update";
                public const string Delete = "Permissions.Status.Delete";
            }

            public static class Contractor
            {
                public const string View = "Permissions.Contractor.View";
                public const string Create = "Permissions.Contractor.Create";
                public const string Update = "Permissions.Contractor.Update";
                public const string Delete = "Permissions.Contractor.Delete";
            }

            public static class SubjectType
            {
                public const string View = "Permissions.SubjectType.View";
                public const string Create = "Permissions.SubjectType.Create";
                public const string Update = "Permissions.SubjectType.Update";
                public const string Delete = "Permissions.SubjectType.Delete";
            }

            public static class LookupValues
            {
                public const string View = "Permissions.LookupValues.View";
                public const string Create = "Permissions.LookupValues.Create";
                public const string Update = "Permissions.LookupValues.Update";
                public const string Delete = "Permissions.LookupValues.Delete";
            }

            public static class ContractorCategoryMapping
            {
                public const string View = "Permissions.ContractorCategoryMapping.View";
                public const string Create = "Permissions.ContractorCategoryMapping.Create";
                public const string Update = "Permissions.ContractorCategoryMapping.Update";
                public const string Delete = "Permissions.ContractorCategoryMapping.Delete";
            }

            public static class Customer
            {
                public const string View = "Permissions.Customer.View";
                public const string Create = "Permissions.Customer.Create";
                public const string Update = "Permissions.Customer.Update";
                public const string Delete = "Permissions.Customer.Delete";
            }

            public static class ErrorLogs
            {
                public const string View = "Permissions.ErrorLogs.View";
            }

            public static class Category
            {
                public const string View = "Permissions.Category.View";
                public const string Create = "Permissions.Category.Create";
                public const string Update = "Permissions.Category.Update";
                public const string Delete = "Permissions.Category.Delete";
            }

            public static class Company
            {
                public const string View = "Permissions.Company.View";
                public const string Create = "Permissions.Company.Create";
                public const string Update = "Permissions.Company.Update";
                public const string Delete = "Permissions.Company.Delete";
            }

            public static class Unit
            {
                public const string View = "Permissions.Unit.View";
                public const string Create = "Permissions.Unit.Create";
                public const string Update = "Permissions.Unit.Update";
                public const string Delete = "Permissions.Unit.Delete";
            }

            public static class FrameType
            {
                public const string View = "Permissions.FrameType.View";
                public const string Create = "Permissions.FrameType.Create";
                public const string Update = "Permissions.FrameType.Update";
                public const string Delete = "Permissions.FrameType.Delete";
            }

            public static class AccessoriesType
            {
                public const string View = "Permissions.AccessoriesType.View";
                public const string Create = "Permissions.AccessoriesType.Create";
                public const string Update = "Permissions.AccessoriesType.Update";
                public const string Delete = "Permissions.AccessoriesType.Delete";
            }

            public static class RawMaterial
            {
                public const string View = "Permissions.RawMaterial.View";
                public const string Create = "Permissions.RawMaterial.Create";
                public const string Update = "Permissions.RawMaterial.Update";
                public const string Delete = "Permissions.RawMaterial.Delete";
            }

            public static class Accessories
            {
                public const string View = "Permissions.Accessories.View";
                public const string Create = "Permissions.Accessories.Create";
                public const string Update = "Permissions.Accessories.Update";
                public const string Delete = "Permissions.Accessories.Delete";
            }

            public static class Fabric
            {
                public const string View = "Permissions.Fabric.View";
                public const string Create = "Permissions.Fabric.Create";
                public const string Update = "Permissions.Fabric.Update";
                public const string Delete = "Permissions.Fabric.Delete";
            }

            public static class Frame
            {
                public const string View = "Permissions.Frame.View";
                public const string Create = "Permissions.Frame.Create";
                public const string Update = "Permissions.Frame.Update";
                public const string Delete = "Permissions.Frame.Delete";
            }

            public static class Polish
            {
                public const string View = "Permissions.Polish.View";
                public const string Create = "Permissions.Polish.Create";
                public const string Update = "Permissions.Polish.Update";
                public const string Delete = "Permissions.Polish.Delete";
            }

            public static class RolePermission
            {
                public const string View = "Permissions.RolePermission.View";
                public const string Create = "Permissions.RolePermission.Create";
                public const string Update = "Permissions.RolePermission.Update";
                public const string Delete = "Permissions.RolePermission.Delete";
            }

            public static class CustomerAddresses
            {
                public const string View = "Permissions.CustomerAddresses.View";
                public const string Create = "Permissions.CustomerAddresses.Create";
                public const string Update = "Permissions.CustomerAddresses.Update";
                public const string Delete = "Permissions.CustomerAddresses.Delete";
            }

            public static class CustomerTypes
            {
                public const string View = "Permissions.CustomerTypes.View";
            }

            public static class Product
            {
                public const string View = "Permissions.Product.View";
                public const string Create = "Permissions.Product.Create";
                public const string Update = "Permissions.Product.Update";
                public const string Delete = "Permissions.Product.Delete";
            }
            public static class Tenant
            {
                public const string View = "Permissions.Tenant.View";
                public const string Update = "Permissions.Tenant.Update";
              
            }
            public static class TenantBankDetail
            {
                public const string View = "Permissions.TenantBankDetail.View";
                public const string Create = "Permissions.TenantBankDetail.Create";
                public const string Update = "Permissions.TenantBankDetail.Update";
                public const string Delete = "Permissions.TenantBankDetail.Delete";
            }
        }
    }
}
