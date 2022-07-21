﻿using MidCapERP.DataAccess.Interface;

namespace MidCapERP.DataAccess.UnitOfWork
{
    public interface IUnitOfWorkDA
    {
        ICategoriesDA CategoriesDA { get; }
        ISubjectTypesDA SubjectTypesDA { get; }
    }
}