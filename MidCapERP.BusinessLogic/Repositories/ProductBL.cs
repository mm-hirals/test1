using AutoMapper;
using MidCapERP.BusinessLogic.Constants;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Services.FileStorage;
using MidCapERP.BusinessLogic.Services.QRCodeGenerate;
using MidCapERP.Core.Constants;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Constants;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.MegaSearch;
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
        private readonly IQRCodeService _iQRCodeService;

        public ProductBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser, IFileStorageService fileStorageService, IQRCodeService iQRCodeService)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
            _fileStorageService = fileStorageService;
            _iQRCodeService = iQRCodeService;
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
                                           Status = x.Status,
                                           CreatedBy = x.CreatedBy,
                                           CreatedDate = x.CreatedDate,
                                           UpdatedBy = x.UpdatedBy,
                                           UpdatedDate = x.UpdatedDate
                                       }).AsQueryable();
            var productData = new PagedList<ProductResponseDto>(productResponseData, dataTableFilterDto);
            return new JsonRepsonse<ProductResponseDto>(dataTableFilterDto.Draw, productData.TotalCount, productData.TotalCount, productData);
        }

        public async Task<ProductRequestDto> GetById(Int64 Id, CancellationToken cancellationToken)
        {
            try
            {
                ProductRequestDto productRequestDto = new ProductRequestDto();
                var allProductdata = await _unitOfWorkDA.ProductDA.GetAll(cancellationToken);
                var productData = (from x in allProductdata.Where(x => x.ProductId == Id)
                                   join y in await _unitOfWorkDA.UserDA.GetUsers(cancellationToken)
                                   on x.CreatedBy equals y.UserId
                                   select new ProductRequestDto()
                                   {
                                       ProductId = Id,
                                       CategoryId = x.CategoryId,
                                       ProductTitle = x.ProductTitle,
                                       ModelNo = x.ModelNo,
                                       Width = x.Width,
                                       Height = x.Height,
                                       Depth = x.Depth,
                                       UsedFabric = x.UsedFabric,
                                       IsVisibleToWholesalers = x.IsVisibleToWholesalers,
                                       TotalDaysToPrepare = x.TotalDaysToPrepare,
                                       Features = x.Features,
                                       Comments = x.Comments,
                                       CostPrice = x.CostPrice,
                                       RetailerPrice = x.RetailerPrice,
                                       WholesalerPrice = x.WholesalerPrice,
                                       CoverImage = x.CoverImage,
                                       QRImage = x.QRImage,
                                       TenantId = x.TenantId,
                                       CreatedByName = y.FullName,
                                       CreatedDate = x.CreatedDate,
                                       UpdatedByName = y.FullName,
                                       UpdatedDate = x.UpdatedDate,
                                       //IsDeleted = x.IsDeleted,
                                       //IsPublished = x.IsPublished
                                   }).FirstOrDefault();

                productRequestDto = _mapper.Map<ProductRequestDto>(productData);
                return productRequestDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

            var unitData = await GetAllUnit(cancellationToken);
            var rowMaterial = await GetAllRowMaterial(cancellationToken);
            var polish = await GetAllPolish(cancellationToken);
            var data = (from x in productMaterialList
                            //left join start for rawmaterial
                        join y in rowMaterial on x.SubjectId equals y.RawMaterialId into rowM
                        from rowMat in rowM.DefaultIfEmpty()
                            // inner join on unitdata on rowmaterial
                        join ur in unitData on rowMat.UnitId equals ur.LookupValueId
                        // left join end

                        //left join start for POlise
                        join z in polish on x.SubjectId equals z.PolishId into polishM
                        from polishMat in polishM.DefaultIfEmpty()
                            // inner join on unitdata on policede
                        join up in unitData on rowMat.UnitId equals up.LookupValueId
                        // left join end

                        select new ProductMaterialRequestDto
                        {
                            ProductMaterialID = x.ProductMaterialID,
                            ProductId = x.ProductId,
                            SubjectTypeId = x.SubjectTypeId,
                            SubjectId = x.SubjectId,
                            Qty = x.Qty,
                            MaterialPrice = x.MaterialPrice,
                            Comments = x.Comments,
                            UnitType = x.SubjectTypeId == 2 ? ur.LookupValueName : up.LookupValueName,
                            CreatedBy = x.CreatedBy,
                            CreatedDate = x.CreatedDate,
                            CreatedUTCDate = x.CreatedUTCDate,
                            UpdatedBy = x.UpdatedBy,
                            UpdatedDate = x.UpdatedDate,
                            UpdatedUTCDate = x.UpdatedUTCDate,
                            IsDeleted = false
                        }).OrderBy(p => p.SubjectTypeId).ToList();

            productMain.ProductMaterialRequestDto = data;
            return productMain;
        }

        public async Task<IList<ProductForDetailsByModuleNoResponceDto>> GetProductForDetailsByModuleNo(string modelNo, CancellationToken cancellationToken)
        {
            var productAlldata = await _unitOfWorkDA.ProductDA.GetAll(cancellationToken);
            return productAlldata.Where(x => x.ModelNo == modelNo).Select(x => new ProductForDetailsByModuleNoResponceDto(x.ProductId, x.CategoryId, x.ProductTitle, x.ModelNo, x.Width, x.Height, x.Depth, x.UsedFabric, x.IsVisibleToWholesalers, x.TotalDaysToPrepare, x.Features, x.Comments, x.CostPrice, x.RetailerPrice, x.WholesalerPrice, x.CoverImage)).ToList();
        }

        public async Task<ProductRequestDto> CreateProduct(ProductRequestDto model, CancellationToken cancellationToken)
        {
            try
            {
                var productToInsert = _mapper.Map<Product>(model);
                if (model.UploadImage != null)
                    productToInsert.CoverImage = await _fileStorageService.StoreFile(model.UploadImage, ApplicationFileStorageConstants.FilePaths.Product);
                productToInsert.Status = 0;
                productToInsert.TenantId = _currentUser.TenantId;
                productToInsert.CreatedBy = _currentUser.UserId;
                productToInsert.CreatedDate = DateTime.Now;
                productToInsert.CreatedUTCDate = DateTime.UtcNow;
                var productData = await _unitOfWorkDA.ProductDA.CreateProduct(productToInsert, cancellationToken);
                var _mappedUser = _mapper.Map<ProductRequestDto>(productData);
                if (productData.ProductId > 0)
                {
                    productToInsert.QRImage = await _iQRCodeService.GenerateQRCodeImageAsync(productData.ProductId.ToString());
                    await _unitOfWorkDA.ProductDA.UpdateProduct(productToInsert, cancellationToken);
                }
                return _mappedUser;
            }
            catch (Exception e)
            {
                throw;
            }
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
                    if (model.UploadImage != null)
                        getProductById.CoverImage = await _fileStorageService.StoreFile(model.UploadImage, ApplicationFileStorageConstants.FilePaths.Product);
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

        public async Task UpdateProductStatus(ProductMainRequestDto model, CancellationToken cancellationToken)
        {
            if (model.ProductId > 0)
            {
                var getProductById = await GetProductById(model.ProductId, cancellationToken);

                if (getProductById != null)
                {
                    UpdateData(getProductById);
                    if (model.Status == "true")
                        getProductById.Status = (byte)ProductStatusEnum.Published;
                    else
                        getProductById.Status = (byte)ProductStatusEnum.UnPublished;

                    await _unitOfWorkDA.ProductDA.UpdateProduct(getProductById, cancellationToken);
                }
            }
            else
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

        public async Task<ProductRequestDto> DeleteProduct(int Id, CancellationToken cancellationToken)
        {
            var productToDelete = await GetProductById(Id, cancellationToken);
            productToDelete.Status = (int)ProductStatusEnum.Delete;
            UpdateData(productToDelete);
            var data = await _unitOfWorkDA.ProductDA.DeleteProduct(Id, cancellationToken);
            var _mappedUser = _mapper.Map<ProductRequestDto>(data);
            return _mappedUser;
        }

        #region API Methods

        public async Task<ProductRequestDto> GetByIdAPI(Int64 Id, CancellationToken cancellationToken)
        {
            var produdctData = await GetProductById(Id, cancellationToken);
            return _mapper.Map<ProductRequestDto>(produdctData);
        }

        public async Task<IEnumerable<MegaSearchResponse>> GetProductForDropDownByModuleNo(string modelno, CancellationToken cancellationToken)
        {
            var productAlldata = await _unitOfWorkDA.ProductDA.GetAll(cancellationToken);
            return productAlldata.Where(x => x.Status == (int)ProductStatusEnum.Published && x.ModelNo.StartsWith(modelno)).Select(x => new MegaSearchResponse(x.ProductId, x.ProductTitle, x.ModelNo, x.CoverImage, "Product")).Take(10).ToList();
        }

        #endregion API Methods

        public async Task<IQueryable<LookupValues>> GetAllUnit(CancellationToken cancellationToken)
        {
            var lookupValuesAllData = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            return lookupValuesAllData.Where(x => x.LookupId == (int)MasterPagesEnum.Unit);
        }

        public async Task<IQueryable<RawMaterial>> GetAllRowMaterial(CancellationToken cancellationToken)
        {
            return await _unitOfWorkDA.RawMaterialDA.GetAll(cancellationToken);
        }

        public async Task<IQueryable<Polish>> GetAllPolish(CancellationToken cancellationToken)
        {
            return await _unitOfWorkDA.PolishDA.GetAll(cancellationToken);
        }

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