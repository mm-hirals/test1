﻿using AutoMapper;
using MidCapERP.BusinessLogic.Constants;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Services.FileStorage;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Constants;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Paging;
using MidCapERP.Dto.Product;
using MidCapERP.Dto.ProductImage;
using MidCapERP.Dto.ProductMaterial;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class ProductBL : IProductBL
    {
        private readonly IUnitOfWorkDA _unitOfWorkDA;

        public readonly IMapper _mapper;

        private readonly CurrentUser _currentUser;
        private readonly IFileStorageService _fileStorageService;

        public ProductBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser, IFileStorageService fileStorageService)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
            _fileStorageService = fileStorageService;
        }

        public async Task<JsonRepsonse<ProductResponseDto>> GetFilterProductData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var productAllData = await _unitOfWorkDA.ProductDA.GetAll(cancellationToken);
            var lookupValuesAllData = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            var cateagoryData = lookupValuesAllData.Where(x => x.LookupId == (int)MasterPagesEnum.Category);
            var productResponseData = (from x in productAllData
                                       join y in cateagoryData on x.CategoryId equals y.LookupValueId
                                       select new ProductResponseDto()
                                       {
                                           ProductId = x.ProductId,
                                           CategoryName = y.LookupValueName,
                                           ProductTitle = x.ProductTitle,
                                           ModelNo = x.ModelNo,
                                           CostPrice = x.CostPrice,
                                           RetailerPrice = x.RetailerPrice,
                                           WholesalerPrice = x.WholesalerPrice
                                       }).AsQueryable();
            var productData = new PagedList<ProductResponseDto>(productResponseData, dataTableFilterDto);
            return new JsonRepsonse<ProductResponseDto>(dataTableFilterDto.Draw, productData.TotalCount, productData.TotalCount, productData);
        }

        public async Task<ProductRequestDto> GetById(Int64 Id, CancellationToken cancellationToken)
        {
            ProductRequestDto productRequestDto = new ProductRequestDto();
            var data = await GetProductById(Id, cancellationToken);
            productRequestDto = _mapper.Map<ProductRequestDto>(data);
            return productRequestDto;
        }

        public async Task<List<ProductImageRequestDto>> GetImageByProductId(long Id, CancellationToken cancellationToken)
        {
            var productImageList = await GetProductImageById(Id, cancellationToken);
            return _mapper.Map<List<ProductImageRequestDto>>(productImageList.ToList());
        }

        public async Task<ProductMainRequestDto> GetMaterialByProductId(long Id, CancellationToken cancellationToken)
        {
            var productMaterialList = await GetProductMaterialById(Id, cancellationToken);
            var getProductInfoById = await GetById(Id, cancellationToken);
            ProductMainRequestDto productMain = new ProductMainRequestDto();
            productMain.ProductId = Id;
            productMain.CostPrice = getProductInfoById.CostPrice;
            productMain.WholesalerPrice = getProductInfoById.WholesalerPrice;
            productMain.RetailerPrice = getProductInfoById.RetailerPrice;
            productMain.ProductMaterialRequestDto = _mapper.Map<List<ProductMaterialRequestDto>>(productMaterialList);
            return productMain;
        }

        public async Task<IList<ProductForDetailsByModuleNoResponceDto>> GetProductForDetailsByModuleNo(string modelNo, CancellationToken cancellationToken)
        {
            var productAlldata = await _unitOfWorkDA.ProductDA.GetAll(cancellationToken);
            return productAlldata.Where(x => x.ModelNo == modelNo).Select(x => new ProductForDetailsByModuleNoResponceDto(x.ProductId, x.CategoryId, x.ProductTitle, x.ModelNo, x.Width, x.Height, x.Depth, x.UsedFabric, x.IsVisibleToWholesalers, x.TotalDaysToPrepare, x.Features, x.Comments, x.CostPrice, x.RetailerPrice, x.WholesalerPrice, x.CoverImage)).ToList();
        }

        public async Task<ProductRequestDto> CreateProduct(ProductRequestDto model, CancellationToken cancellationToken)
        {
            var productToInsert = _mapper.Map<Product>(model);
            productToInsert.IsDeleted = false;
            productToInsert.IsPublished = false;
            productToInsert.TenantId = _currentUser.TenantId;
            productToInsert.CreatedBy = _currentUser.UserId;
            productToInsert.CreatedDate = DateTime.Now;
            productToInsert.CreatedUTCDate = DateTime.UtcNow;
            var productData = await _unitOfWorkDA.ProductDA.CreateProduct(productToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<ProductRequestDto>(productData);

            return _mappedUser;
        }

        public async Task<ProductMainRequestDto> CreateProductMaterial(ProductMainRequestDto productMainRequestDto, CancellationToken cancellationToken)
        {
            await DeleteProductMaterials(productMainRequestDto.ProductId, cancellationToken);

            await SaveProductMaterials(productMainRequestDto, cancellationToken);
            await UpdateProductCost(productMainRequestDto, cancellationToken);

            return productMainRequestDto;
        }

        public async Task<ProductImageRequestDto> CreateProductImages(ProductMainRequestDto model, CancellationToken cancellationToken)
        {
            ProductImage saveImage = new ProductImage();

            if (model.ProductId > 0)
            {
                if (model.Files != null)
                {
                    var getImageById = await GetProductImageById(model.ProductId, cancellationToken);
                    if (getImageById.Count() > 0)
                        await DeleteImages(getImageById.ToList(), cancellationToken);
                    await AddImages(model, cancellationToken);
                    return _mapper.Map<ProductImageRequestDto>(saveImage);
                }
                throw new Exception("No Material Image Found");
            }
            throw new Exception("Product Not Found");
        }

        public async Task<ProductRequestDto> UpdateProduct(ProductRequestDto model, CancellationToken cancellationToken)
        {
            if (model.ProductId > 0)
            {
                var getProductById = await GetProductById(model.ProductId, cancellationToken);

                if (getProductById != null)
                {
                    UpdateData(getProductById);
                    getProductById.ProductTitle = model.ProductTitle;
                    getProductById.ModelNo = model.ModelNo;
                    getProductById.Width = model.Width;
                    getProductById.Height = model.Height;
                    getProductById.Depth = model.Depth;
                    var data = await _unitOfWorkDA.ProductDA.UpdateProduct(getProductById, cancellationToken);
                    var _mappedUser = _mapper.Map<ProductRequestDto>(data);

                    return _mappedUser;
                }

                throw new Exception("Product is not found");
            }

            throw new Exception("Product is not found");
        }

        public async Task<ProductRequestDto> UpdateProductDetail(ProductRequestDto model, CancellationToken cancellationToken)
        {
            if (model.ProductId > 0)
            {
                var getProductById = await GetProductById(model.ProductId, cancellationToken);

                if (getProductById != null)
                {
                    UpdateData(getProductById);
                    getProductById.UsedFabric = model.UsedFabric;
                    getProductById.IsVisibleToWholesalers = model.IsVisibleToWholesalers;
                    getProductById.TotalDaysToPrepare = model.TotalDaysToPrepare;
                    getProductById.Features = model.Features;
                    getProductById.Comments = model.Comments;
                    var data = await _unitOfWorkDA.ProductDA.UpdateProduct(getProductById, cancellationToken);
                    var _mappedUser = _mapper.Map<ProductRequestDto>(data);

                    return _mappedUser;
                }

                throw new Exception("Product is not found");
            }

            throw new Exception("Product is not found");
        }

        public async Task<ProductRequestDto?> UpdateProductCost(ProductMainRequestDto model, CancellationToken cancellationToken)
        {
            if (model.ProductId > 0)
            {
                var getProductById = await GetProductById(model.ProductId, cancellationToken);

                if (getProductById != null)
                {
                    UpdateData(getProductById);
                    getProductById.CostPrice = model.CostPrice;
                    getProductById.RetailerPrice = model.RetailerPrice;
                    getProductById.WholesalerPrice = model.WholesalerPrice;
                    var data = await _unitOfWorkDA.ProductDA.UpdateProduct(getProductById, cancellationToken);

                    return _mapper.Map<ProductRequestDto>(data);
                }

                throw new Exception("Product is not found");
            }

            throw new Exception("Product is not found");
        }

        #region API Methods
        public async Task<ProductRequestDto> GetByIdAPI(Int64 Id, CancellationToken cancellationToken)
        {
            var produdctData = await GetProductById(Id, cancellationToken);
            return _mapper.Map<ProductRequestDto>(produdctData);
        }

        public async Task<IEnumerable<ProductForDorpDownByModuleNoResponseDto>> GetProductForDropDownByModuleNo(string modelno, CancellationToken cancellationToken)
        {
            var productAlldata = await _unitOfWorkDA.ProductDA.GetAll(cancellationToken);
            return productAlldata.Where(x => x.ModelNo.StartsWith(modelno)).Select(x => new ProductForDorpDownByModuleNoResponseDto(x.ProductId, x.ProductTitle, x.ModelNo, x.CoverImage, "Product")).ToList();
        }
        #endregion

        #region Private Method

        private async Task<Product> GetProductById(Int64 Id, CancellationToken cancellationToken)
        {
            var productDataById = await _unitOfWorkDA.ProductDA.GetById(Id, cancellationToken);
            if (productDataById == null)
            {
                throw new Exception("Product not found");
            }
            return productDataById;
        }

        private async Task<IQueryable<ProductImage>> GetProductImageById(Int64 Id, CancellationToken cancellationToken)
        {
            var productImage = await _unitOfWorkDA.ProductImageDA.GetAll(cancellationToken);
            return productImage.Where(x => x.ProductId == Id);
        }

        private async Task<List<ProductMaterial>> GetProductMaterialById(Int64 Id, CancellationToken cancellationToken)
        {
            var productMaterial = await _unitOfWorkDA.ProductMaterialDA.GetAll(cancellationToken);
            var productMaterialDataById = productMaterial.Where(x => x.ProductId == Id).OrderBy(p => p.SubjectTypeId).ToList();
            if (productMaterialDataById == null)
            {
                throw new Exception("ProductMaterials not found");
            }
            return productMaterialDataById;
        }

        private void UpdateData(Product oldData)
        {
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
        }

        private static void MapToDbObject(ProductRequestDto model, Product oldData)
        {
            oldData.CategoryId = model.CategoryId;
            oldData.ProductTitle = model.ProductTitle;
            oldData.ModelNo = model.ModelNo;
            oldData.Width = model.Width;
            oldData.Height = model.Height;
            oldData.Depth = model.Depth;
            oldData.UsedFabric = model.UsedFabric;
            oldData.IsVisibleToWholesalers = model.IsVisibleToWholesalers;
            oldData.TotalDaysToPrepare = model.TotalDaysToPrepare;
            oldData.Features = model.Features;
            oldData.Comments = model.Comments;
            oldData.CostPrice = model.CostPrice;
            oldData.RetailerPrice = model.RetailerPrice;
            oldData.WholesalerPrice = model.WholesalerPrice;
            oldData.CoverImage = model.CoverImage;
            oldData.QRImage = model.QRImage;
        }

        private async Task AddImages(ProductMainRequestDto model, CancellationToken cancellationToken)
        {
            foreach (var item in model.Files)
            {
                ProductImageRequestDto productImageRequestDto = new ProductImageRequestDto();
                productImageRequestDto.ImagePath = await _fileStorageService.StoreFile(item, ApplicationFileStorageConstants.FilePaths.Product);
                productImageRequestDto.ImageName = item.FileName;

                var productImageToInsert = _mapper.Map<ProductImage>(productImageRequestDto);
                productImageToInsert.ProductId = model.ProductId;
                productImageToInsert.ImagePath = productImageRequestDto.ImagePath;
                productImageToInsert.ImageName = productImageRequestDto.ImageName;
                productImageToInsert.CreatedBy = _currentUser.UserId;
                productImageToInsert.CreatedDate = DateTime.Now;
                productImageToInsert.CreatedUTCDate = DateTime.UtcNow;
                await _unitOfWorkDA.ProductImageDA.CreateProductImage(productImageToInsert, cancellationToken);
            }
        }

        private async Task DeleteImages(List<ProductImage> getImageById, CancellationToken cancellationToken)
        {
            foreach (var item in getImageById)
            {
                await _unitOfWorkDA.ProductImageDA.DeleteProductImage(item.ProductImageID, cancellationToken);
            }
        }

        private async Task SaveProductMaterials(ProductMainRequestDto model, CancellationToken cancellationToken)
        {
            if (model.ProductMaterialRequestDto != null)
            {
                foreach (var item in model.ProductMaterialRequestDto.Where(p => !p.IsDeleted))
                {
                    item.ProductId = model.ProductId;
                    await CreateProductMaterial(item, cancellationToken);
                }
            }
        }

        private async Task DeleteProductMaterials(Int64 Id, CancellationToken cancellationToken)
        {
            var productMaterialById = await GetProductMaterialById(Id, cancellationToken);
            foreach (var item in productMaterialById)
            {
                await _unitOfWorkDA.ProductMaterialDA.DeleteProductMaterial(item.ProductMaterialID, cancellationToken);
            }
        }

        private async Task CreateProductMaterial(ProductMaterialRequestDto item, CancellationToken cancellationToken)
        {
            var productMaterialToInsert = _mapper.Map<ProductMaterial>(item);
            productMaterialToInsert.ProductId = item.ProductId;
            productMaterialToInsert.CreatedBy = _currentUser.UserId;
            productMaterialToInsert.CreatedDate = DateTime.Now;
            productMaterialToInsert.CreatedUTCDate = DateTime.UtcNow;
            await _unitOfWorkDA.ProductMaterialDA.CreateProductMaterial(productMaterialToInsert, cancellationToken);
        }

        #endregion Private Method
    }
}