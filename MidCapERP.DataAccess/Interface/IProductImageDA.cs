﻿using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IProductImageDA
    {
        public Task<ProductImage> CreateProductImage(ProductImage model, CancellationToken cancellationToken);
    }
}