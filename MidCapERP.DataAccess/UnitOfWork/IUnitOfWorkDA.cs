﻿using MidCapERP.DataAccess.Interface;
using MidCapERP.DataAccess.Repositories;

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
        IAccessoriesTypesDA AccessoriesTypesDA { get; }
        IRawMaterialDA RawMaterialDA { get; }
    }
}