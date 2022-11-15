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
            public static readonly string SuperAdmin = "SuperAdmin";
            public static readonly string[] RolesSupported = { SuperAdmin };
        }

        public static class Permissions
        {
            public static List<string> GeneratePermissionsForModule(string application, string module)
            {
                return new List<string>()
                {
                    $"Permissions.{application}.{module}.View",
                    $"Permissions.{application}.{module}.Create",
                    $"Permissions.{application}.{module}.Delete",
                };
            }

            public static List<string> GeneratePermissionsForModule(string application, string module, string permission)
            {
                return new List<string>() { $"Permissions.{application}.{module}.{permission}" };
            }

            public static List<string> GetAllPermissions()
            {
                return GeneratePermissionsForModule("Portal", "Dashboard", "OrdersView")
                    .Union(GeneratePermissionsForModule("Portal", "Dashboard", "CustomersView"))
                    .Union(GeneratePermissionsForModule("Portal", "Dashboard", "InteriorsView"))
                    .Union(GeneratePermissionsForModule("Portal", "Dashboard", "ProductsView"))
                    .Union(GeneratePermissionsForModule("Portal", "Product"))
                    .Union(GeneratePermissionsForModule("Portal", "Product", "Publish"))
                    .Union(GeneratePermissionsForModule("Portal", "Product", "CostAnalyser"))
                    .Union(GeneratePermissionsForModule("Portal", "Order"))
                    .Union(GeneratePermissionsForModule("Portal", "Customer"))
                    .Union(GeneratePermissionsForModule("Portal", "Customer", "Import"))
                    .Union(GeneratePermissionsForModule("Portal", "Customer", "SendGreetings"))
                    .Union(GeneratePermissionsForModule("Portal", "Interior"))
                    .Union(GeneratePermissionsForModule("Portal", "Interior", "Import"))
                    .Union(GeneratePermissionsForModule("Portal", "Interior", "SendGreetings"))
                    .Union(GeneratePermissionsForModule("Portal", "Category"))
                    .Union(GeneratePermissionsForModule("Portal", "Company"))
                    .Union(GeneratePermissionsForModule("Portal", "Unit"))
                    .Union(GeneratePermissionsForModule("Portal", "RawMaterial"))
                    .Union(GeneratePermissionsForModule("Portal", "Fabric"))
                    .Union(GeneratePermissionsForModule("Portal", "Polish"))
                    .Union(GeneratePermissionsForModule("Portal", "User"))
                    .Union(GeneratePermissionsForModule("Portal", "RolePermission"))
                    .Union(GeneratePermissionsForModule("Portal", "Profile", "View"))
                    .Union(GeneratePermissionsForModule("Portal", "Profile", "Create"))
                    .Union(GeneratePermissionsForModule("App", "Dashboard", "ApprovedOrdersView"))
                    .Union(GeneratePermissionsForModule("App", "Dashboard", "PendingOrdersView"))
                    .Union(GeneratePermissionsForModule("App", "Dashboard", "FollowupOrdersView"))
                    .Union(GeneratePermissionsForModule("App", "Dashboard", "ReceivableOrdersView"))
                    .Union(GeneratePermissionsForModule("App", "Customer", "View"))
                    .Union(GeneratePermissionsForModule("App", "Customer", "Create"))
                    .Union(GeneratePermissionsForModule("App", "Interior", "View"))
                    .Union(GeneratePermissionsForModule("App", "Interior", "Create"))
                    .Union(GeneratePermissionsForModule("App", "Order"))
                    .Union(GeneratePermissionsForModule("App", "Order", "Approve"))
                    .Union(GeneratePermissionsForModule("App", "Order", "Decline"))
                    .Union(GeneratePermissionsForModule("App", "Order", "ShareQuotation"))
                    .Union(GeneratePermissionsForModule("App", "Order", "MaterialReceive"))
                    .Union(GeneratePermissionsForModule("App", "MegaSearch", "View"))
                    .ToList();
            }

            public static class PortalDashboard
            {
                public const string OrdersView = "Permissions.Portal.Dashboard.OrdersView";
                public const string CustomersView = "Permissions.Portal.Dashboard.CustomersView";
                public const string InteriorsView = "Permissions.Portal.Dashboard.InteriorsView";
                public const string ProductsView = "Permissions.Portal.Dashboard.ProductsView";
            }

            public static class PortalProduct
            {
                public const string View = "Permissions.Portal.Product.View";
                public const string Create = "Permissions.Portal.Product.Create";
                public const string Delete = "Permissions.Portal.Product.Delete";
                public const string Publish = "Permissions.Portal.Product.Publish";
                public const string CostAnalyser = "Permissions.Portal.Product.CostAnalyser";
            }

            public static class PortalOrder
            {
                public const string View = "Permissions.Portal.Order.View";
                public const string Create = "Permissions.Portal.Order.Create";
                public const string Delete = "Permissions.Portal.Order.Delete";
            }

            public static class PortalCustomer
            {
                public const string View = "Permissions.Portal.Customer.View";
                public const string Create = "Permissions.Portal.Customer.Create";
                public const string Delete = "Permissions.Portal.Customer.Delete";
                public const string Import = "Permissions.Portal.Customer.Import";
                public const string SendGreetings = "Permissions.Portal.Customer.SendGreetings";
            }

            public static class PortalInterior
            {
                public const string View = "Permissions.Portal.Interior.View";
                public const string Create = "Permissions.Portal.Interior.Create";
                public const string Delete = "Permissions.Portal.Interior.Delete";
                public const string Import = "Permissions.Portal.Interior.Import";
                public const string SendGreetings = "Permissions.Portal.Interior.SendGreetings";
            }

            public static class PortalCategory
            {
                public const string View = "Permissions.Portal.Category.View";
                public const string Create = "Permissions.Portal.Category.Create";
                public const string Delete = "Permissions.Portal.Category.Delete";
            }

            public static class PortalCompany
            {
                public const string View = "Permissions.Portal.Company.View";
                public const string Create = "Permissions.Portal.Company.Create";
                public const string Delete = "Permissions.Portal.Company.Delete";
            }

            public static class PortalUnit
            {
                public const string View = "Permissions.Portal.Unit.View";
                public const string Create = "Permissions.Portal.Unit.Create";
                public const string Delete = "Permissions.Portal.Unit.Delete";
            }

            public static class PortalRawMaterial
            {
                public const string View = "Permissions.Portal.RawMaterial.View";
                public const string Create = "Permissions.Portal.RawMaterial.Create";
                public const string Delete = "Permissions.Portal.RawMaterial.Delete";
            }

            public static class PortalFabric
            {
                public const string View = "Permissions.Portal.Fabric.View";
                public const string Create = "Permissions.Portal.Fabric.Create";
                public const string Delete = "Permissions.Portal.Fabric.Delete";
            }

            public static class PortalPolish
            {
                public const string View = "Permissions.Portal.Polish.View";
                public const string Create = "Permissions.Portal.Polish.Create";
                public const string Delete = "Permissions.Portal.Polish.Delete";
            }

            public static class PortalUser
            {
                public const string View = "Permissions.Portal.User.View";
                public const string Create = "Permissions.Portal.User.Create";
                public const string Delete = "Permissions.Portal.User.Delete";
            }

            public static class PortalRolePermission
            {
                public const string View = "Permissions.Portal.RolePermission.View";
                public const string Create = "Permissions.Portal.RolePermission.Create";
                public const string Delete = "Permissions.Portal.RolePermission.Delete";
            }

            public static class PortalProfile
            {
                public const string View = "Permissions.Portal.Profile.View";
                public const string Create = "Permissions.Portal.Profile.Create";
            }

            public static class PortalErrorLogs
            {
                public const string View = "Permissions.ErrorLogs.View";
            }

            public static class AppDashboard
            {
                public const string ApprovedOrdersView = "Permissions.App.Dashboard.ApprovedOrdersView";
                public const string PendingOrdersView = "Permissions.App.Dashboard.PendingOrdersView";
                public const string FollowupOrdersView = "Permissions.App.Dashboard.FollowupOrdersView";
                public const string ReceivableOrdersView = "Permissions.App.Dashboard.ReceivableOrdersView";
            }

            public static class AppCustomer
            {
                public const string View = "Permissions.App.Customer.View";
                public const string Create = "Permissions.App.Customer.Create";
            }

            public static class AppInterior
            {
                public const string View = "Permissions.App.Interior.View";
                public const string Create = "Permissions.App.Interior.Create";
            }

            public static class AppOrder
            {
                public const string View = "Permissions.App.Order.View";
                public const string Create = "Permissions.App.Order.Create";
                public const string Delete = "Permissions.App.Order.Delete";
                public const string Approve = "Permissions.App.Order.Approve";
                public const string Decline = "Permissions.App.Order.Decline";
                public const string ShareQuotation = "Permissions.App.Order.ShareQuotation";
                public const string MaterialReceive = "Permissions.App.Order.MaterialReceive";
            }

            public static class AppMegaSearch
            {
                public const string View = "Permissions.App.MegaSearch.View";
            }

            public static class PortalContractor
            {
                public const string View = "Permissions.Portal.Contractor.View";
                public const string Create = "Permissions.Portal.Contractor.Create";
                public const string Delete = "Permissions.Portal.Contractor.Delete";
            }
        }
    }
}