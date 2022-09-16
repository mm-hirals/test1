﻿using Microsoft.AspNetCore.Http;
using MidCapERP.Dto.ProductImage;
using MidCapERP.Dto.ProductMaterial;

namespace MidCapERP.Dto.Product
{
    public class ProductMainRequestDto
    {
        public ProductMainRequestDto()
        {
            ProductRequestDto = new ProductRequestDto();
            ProductImageRequestDto = new List<ProductImageRequestDto>();
            ProductMaterialRequestDto = new List<ProductMaterialRequestDto>();
        }

        public int ProductId { get; set; } = 0;

        public ProductRequestDto ProductRequestDto { get; set; }
        public List<ProductImageRequestDto> ProductImageRequestDto { get; set; }
        public List<ProductMaterialRequestDto> ProductMaterialRequestDto { get; set; }

        public List<IFormFile> Files { get; set; }
    }
}