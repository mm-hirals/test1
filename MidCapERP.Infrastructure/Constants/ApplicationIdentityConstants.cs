﻿namespace MidCapERP.Infrastructure.Constants
{
    public static class ApplicationIdentityConstants
    {
        public static readonly string DefaultPassword = "Password@1";

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
                .Union(GeneratePermissionsForModule("SubjectType"))
                .Union(GeneratePermissionsForModule("ContractorCategoryMapping"))
                .Union(GeneratePermissionsForModule("ErrorLogs"))
                .Union(GeneratePermissionsForModule("Category"))
                .Union(GeneratePermissionsForModule("Company"))
                .Union(GeneratePermissionsForModule("Unit"))
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
                public const string View   = "Permissions.Company.View";
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
        }
    }
}