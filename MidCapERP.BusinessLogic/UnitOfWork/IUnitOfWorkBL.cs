﻿using MidCapERP.BusinessLogic.Interface;

namespace MidCapERP.BusinessLogic.UnitOfWork
{
    public interface IUnitOfWorkBL
    {
        ICategoriesBL CategoriesBL { get; }
        ILookupValuesBL LookupValuesBL { get; }
    }
}