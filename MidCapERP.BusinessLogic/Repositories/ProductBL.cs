using AutoMapper;
using MidCapERP.BusinessLogic.Constants;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Services.ActivityLog;
using MidCapERP.BusinessLogic.Services.FileStorage;
using MidCapERP.BusinessLogic.Services.QRCodeGenerate;
using MidCapERP.Core.Constants;
using MidCapERP.DataAccess.Repositories;
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
        private readonly IActivityLogsService _activityLogsService;

        public ProductBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser, IFileStorageService fileStorageService, IQRCodeService iQRCodeService, IActivityLogsService activityLogsService)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
            _fileStorageService = fileStorageService;
            _iQRCodeService = iQRCodeService;
            _activityLogsService = activityLogsService;
        }

        public async Task<JsonRepsonse<ProductResponseDto>> GetFilterProductData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            int lookupId = await GetCategoryLookupId(cancellationToken);
            var productAllData = await _unitOfWorkDA.ProductDA.GetAll(cancellationToken);
            var lookupValuesAllData = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            var cateagoryData = lookupValuesAllData.Where(x => x.LookupId == lookupId);
            var allUsers = await _unitOfWorkDA.UserDA.GetUsers(cancellationToken);
            var productResponseData = (from x in productAllData
                                       join y in cateagoryData on x.CategoryId equals y.LookupValueId
                                       join z in allUsers on x.CreatedBy equals z.UserId
                                       join u in allUsers on x.UpdatedBy equals (int?)u.UserId into updated
                                       from updatedMat in updated.DefaultIfEmpty()
                                       select new ProductResponseDto()
                                       {
                                           ProductId = x.ProductId,
                                           CategoryName = y.LookupValueName,
                                           ProductTitle = x.ProductTitle,
                                           ModelNo = x.ModelNo,
                                           Status = x.Status,
                                           CreatedByName = z.FullName,
                                           CreatedDate = x.CreatedDate,
                                           UpdatedByName = x.UpdatedBy != null ? updatedMat.FullName : z.FullName,
                                           UpdatedDate = x.UpdatedDate != null ? x.UpdatedDate : x.CreatedDate,
                                       }).AsQueryable();
            var productData = new PagedList<ProductResponseDto>(productResponseData, dataTableFilterDto);
            return new JsonRepsonse<ProductResponseDto>(dataTableFilterDto.Draw, productData.TotalCount, productData.TotalCount, productData);
        }

        public async Task<ProductRequestDto> GetById(Int64 Id, CancellationToken cancellationToken)
        {
            try
            {
                if (_currentUser.TenantId == 0)
                {
                    var getProductById = await GetProductById(Id, cancellationToken);
                    if (getProductById != null)
                        _currentUser.TenantId = getProductById.TenantId;
                }

                ProductRequestDto productRequestDto = new ProductRequestDto();
                var allUsers = await _unitOfWorkDA.UserDA.GetUsers(cancellationToken);
                var allProductdata = await _unitOfWorkDA.ProductDA.GetAll(cancellationToken);
                var productData = (from x in allProductdata.Where(x => x.ProductId == Id)
                                   join y in allUsers on x.CreatedBy equals y.UserId
                                   join z in allUsers on x.UpdatedBy equals (int?)z.UserId into updated
                                   from updatedMat in updated.DefaultIfEmpty()
                                   select new ProductRequestDto()
                                   {
                                       ProductId = Id,
                                       CategoryId = x.CategoryId,
                                       ProductTitle = x.ProductTitle,
                                       ModelNo = x.ModelNo,
                                       WidthNumeric = Convert.ToString(Math.Floor(x.Width)),
                                       HeightNumeric = Convert.ToString(Math.Floor(x.Height)),
                                       DepthNumeric = Convert.ToString(Math.Floor(x.Depth)),
                                       Width = x.Width,
                                       Height = x.Height,
                                       Depth = x.Depth,
                                       FabricNeeded = x.FabricNeeded,
                                       IsVisibleToWholesalers = x.IsVisibleToWholesalers,
                                       TotalDaysToPrepare = x.TotalDaysToPrepare,
                                       Features = x.Features,
                                       Comments = x.Comments,
                                       CostPrice = x.CostPrice,
                                       QRImage = x.QRImage,
                                       TenantId = x.TenantId,
                                       CreatedByName = y.FullName,
                                       CreatedDate = x.CreatedDate,
                                       UpdatedByName = x.UpdatedBy != null ? updatedMat.FullName : null,
                                       UpdatedDate = x.UpdatedDate != null ? x.UpdatedDate : null,
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

            var rawMaterialSubjectTypeId = await GetRawMaterialSubjectTypeId(cancellationToken);
            var polishSubjectTypeId = await GetPolishSubjectTypeId(cancellationToken);

            var unitData = await GetAllUnit(cancellationToken);
            var rowMaterial = await GetAllRowMaterial(cancellationToken);
            var polish = await GetAllPolish(cancellationToken);

            var data = (from x in productMaterialList
                            //left join start for rawmaterial
                        join y in rowMaterial on x.SubjectId equals y.RawMaterialId into rowM
                        from rowMat in rowM.DefaultIfEmpty(new RawMaterial())
                            // inner join on unitdata on rowmaterial
                        join ur in unitData on rowMat.UnitId equals ur.LookupValueId
                        where x.SubjectTypeId == rawMaterialSubjectTypeId && rowMat != null

                        select new ProductMaterialRequestDto
                        {
                            ProductMaterialID = x.ProductMaterialID,
                            ProductId = x.ProductId,
                            SubjectTypeId = x.SubjectTypeId,
                            SubjectId = x.SubjectId,
                            Qty = x.Qty,
                            UnitType = ur.LookupValueName,
                            CreatedBy = x.CreatedBy,
                            CreatedDate = x.CreatedDate,
                            CreatedUTCDate = x.CreatedUTCDate,
                            UpdatedBy = x.UpdatedBy,
                            UpdatedDate = x.UpdatedDate,
                            UpdatedUTCDate = x.UpdatedUTCDate,
                            IsDeleted = false
                        }).Union(from x in productMaterialList
                                 join z in polish on x.SubjectId equals z.PolishId into polishM
                                 from polishMat in polishM.DefaultIfEmpty(new Polish())
                                 where x.SubjectTypeId == polishSubjectTypeId && polishMat != null
                                 join up in unitData on polishMat.UnitId equals up.LookupValueId
                                 select new ProductMaterialRequestDto
                                 {
                                     ProductMaterialID = x.ProductMaterialID,
                                     ProductId = x.ProductId,
                                     SubjectTypeId = x.SubjectTypeId,
                                     SubjectId = x.SubjectId,
                                     Qty = x.Qty,
                                     UnitType = up.LookupValueName,
                                     CreatedBy = x.CreatedBy,
                                     CreatedDate = x.CreatedDate,
                                     CreatedUTCDate = x.CreatedUTCDate,
                                     UpdatedBy = x.UpdatedBy,
                                     UpdatedDate = x.UpdatedDate,
                                     UpdatedUTCDate = x.UpdatedUTCDate,
                                     IsDeleted = false
                                 }).ToList();

            productMain.ProductMaterialRequestDto = data.OrderBy(p => p.SubjectTypeId).ToList();
            return productMain;
        }

        public async Task<IList<ProductForDetailsByModuleNoResponceDto>> GetProductForDetailsByModuleNo(string modelNo, CancellationToken cancellationToken)
        {
            var productAlldata = await _unitOfWorkDA.ProductDA.GetAll(cancellationToken);
            return productAlldata.Where(x => x.ModelNo == modelNo).Select(x => new ProductForDetailsByModuleNoResponceDto(x.ProductId, x.CategoryId, x.ProductTitle, x.ModelNo, x.Width, x.Height, x.Depth, x.FabricNeeded, x.IsVisibleToWholesalers, x.TotalDaysToPrepare, x.Features, x.Comments, x.CostPrice)).ToList();
        }

        public async Task<ProductRequestDto> CreateProduct(ProductRequestDto model, CancellationToken cancellationToken)
        {
            model.Width = Convert.ToDecimal(model.WidthNumeric);
            model.Height = Convert.ToDecimal(model.HeightNumeric);
            model.Depth = Convert.ToDecimal(model.DepthNumeric);
            var productToInsert = _mapper.Map<Product>(model);
            productToInsert.Status = 0;
            productToInsert.TenantId = _currentUser.TenantId;
            productToInsert.CreatedBy = _currentUser.UserId;
            productToInsert.CreatedDate = DateTime.Now;
            productToInsert.CreatedUTCDate = DateTime.UtcNow;
            var productData = await _unitOfWorkDA.ProductDA.CreateProduct(productToInsert, cancellationToken);
            await _activityLogsService.PerformActivityLog(1, Convert.ToString(productData.ProductId), "Product Created", ActivityLogStringConstant.Create, CancellationToken.None);
            var _mappedUser = _mapper.Map<ProductRequestDto>(productData);
            if (productData.ProductId > 0)
            {
                productToInsert.QRImage = await _iQRCodeService.GenerateQRCodeImageAsync(Convert.ToString(productData.ProductId));
                await _unitOfWorkDA.ProductDA.UpdateProduct(productToInsert, cancellationToken);
            }
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
                    getProductById.Width = Convert.ToDecimal(model.WidthNumeric);
                    getProductById.Height = Convert.ToDecimal(model.HeightNumeric);
                    getProductById.Depth = Convert.ToDecimal(model.DepthNumeric);
                    var data = await _unitOfWorkDA.ProductDA.UpdateProduct(getProductById, cancellationToken);
                    await _activityLogsService.PerformActivityLog(1, Convert.ToString(model.ProductId), "Product Updated", ActivityLogStringConstant.Update, cancellationToken);
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
                    getProductById.FabricNeeded = model.FabricNeeded;
                    getProductById.IsVisibleToWholesalers = model.IsVisibleToWholesalers;
                    getProductById.TotalDaysToPrepare = model.TotalDaysToPrepare;
                    getProductById.Features = model.Features;
                    getProductById.Comments = model.Comments;
                    var data = await _unitOfWorkDA.ProductDA.UpdateProduct(getProductById, cancellationToken);
                    await _activityLogsService.PerformActivityLog(1, Convert.ToString(model.ProductId), "ProductDetail Updated", ActivityLogStringConstant.Update, cancellationToken);
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
                        getProductById.Status = (int)ProductStatusEnum.Published;
                    else
                        getProductById.Status = (int)ProductStatusEnum.UnPublished;

                    await _unitOfWorkDA.ProductDA.UpdateProduct(getProductById, cancellationToken);
                    await _activityLogsService.PerformActivityLog(1, Convert.ToString(model.ProductId), "ProducStatus Updated", ActivityLogStringConstant.Update, cancellationToken);
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
                    var data = await _unitOfWorkDA.ProductDA.UpdateProduct(getProductById, cancellationToken);
                    await _activityLogsService.PerformActivityLog(1, Convert.ToString(model.ProductId), "ProducCost Updated", ActivityLogStringConstant.Update, cancellationToken);
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
            await _activityLogsService.PerformActivityLog(1, Convert.ToString(Id), "Product Deleted", ActivityLogStringConstant.Delete, cancellationToken);
            var _mappedUser = _mapper.Map<ProductRequestDto>(data);
            return _mappedUser;
        }

        public async Task DeleteProductImage(int productImageId, CancellationToken cancellationToken)
        {
            await _unitOfWorkDA.ProductImageDA.DeleteProductImage(productImageId, cancellationToken);
            await _activityLogsService.PerformActivityLog(1, Convert.ToString(productImageId), "Product Image Deleted", ActivityLogStringConstant.Delete, cancellationToken);
        }

        public async Task UpdateProductImageMarkAsCover(int productImageId, bool IsCover, CancellationToken cancellationToken)
        {
            var getProductImage = await _unitOfWorkDA.ProductImageDA.GetById(productImageId, cancellationToken);
            if (getProductImage != null)
            {
                getProductImage.IsCover = IsCover;
                await _unitOfWorkDA.ProductImageDA.UpdateProductImage(getProductImage, cancellationToken);
                await _activityLogsService.PerformActivityLog(1, Convert.ToString(productImageId), "Image Updated", ActivityLogStringConstant.Update, cancellationToken);
            }
        }

        public async Task<int> GetRawMaterialSubjectTypeId(CancellationToken cancellationToken)
        {
            var subjectTypeAllData = await _unitOfWorkDA.SubjectTypesDA.GetAll(cancellationToken);
            var subjectTypeId = subjectTypeAllData.Where(x => x.SubjectTypeName == nameof(SubjectTypesEnum.RawMaterials)).Select(x => x.SubjectTypeId).FirstOrDefault();
            return subjectTypeId;
        }

        public async Task<int> GetPolishSubjectTypeId(CancellationToken cancellationToken)
        {
            var subjectTypeAllData = await _unitOfWorkDA.SubjectTypesDA.GetAll(cancellationToken);
            var subjectTypeId = subjectTypeAllData.Where(x => x.SubjectTypeName == nameof(SubjectTypesEnum.Polish)).Select(x => x.SubjectTypeId).FirstOrDefault();
            return subjectTypeId;
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
            return productAlldata.Where(x => x.Status == (int)ProductStatusEnum.Published && x.ModelNo.StartsWith(modelno)).Select(x => new MegaSearchResponse(x.ProductId, x.ProductTitle, x.ModelNo, "", "Product")).Take(10).ToList();
        }

        #endregion API Methods

        #region Private Method

        private async Task<IQueryable<LookupValues>> GetAllUnit(CancellationToken cancellationToken)
        {
            var lookupId = await GetUnitLookupId(cancellationToken);
            var lookupValuesAllData = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            return lookupValuesAllData.Where(x => x.LookupId == lookupId);
        }

        private async Task<IQueryable<RawMaterial>> GetAllRowMaterial(CancellationToken cancellationToken)
        {
            return await _unitOfWorkDA.RawMaterialDA.GetAll(cancellationToken);
        }

        private async Task<IQueryable<Polish>> GetAllPolish(CancellationToken cancellationToken)
        {
            return await _unitOfWorkDA.PolishDA.GetAll(cancellationToken);
        }

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
            oldData.FabricNeeded = model.FabricNeeded;
            oldData.IsVisibleToWholesalers = model.IsVisibleToWholesalers;
            oldData.TotalDaysToPrepare = model.TotalDaysToPrepare;
            oldData.Features = model.Features;
            oldData.Comments = model.Comments;
            oldData.CostPrice = model.CostPrice;
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
                await _activityLogsService.PerformActivityLog(1, Convert.ToString(model.ProductId), "Image Added", ActivityLogStringConstant.Create, cancellationToken);
            }
        }


        private async Task DeleteImages(List<ProductImage> getImageById, CancellationToken cancellationToken)
        {
            foreach (var item in getImageById)
            {
                await _unitOfWorkDA.ProductImageDA.DeleteProductImage(item.ProductImageID, cancellationToken);
                await _activityLogsService.PerformActivityLog(1, Convert.ToString(item.ProductImageID), "Image Deleted", ActivityLogStringConstant.Delete, cancellationToken);
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
                await _activityLogsService.PerformActivityLog(1, Convert.ToString(item.ProductMaterialID), "ProductMaterial Created", ActivityLogStringConstant.Create, cancellationToken);
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
            await _activityLogsService.PerformActivityLog(1, Convert.ToString(item.ProductId), "ProductMaterial Created", ActivityLogStringConstant.Create, cancellationToken);
        }

        private async Task<int> GetCategoryLookupId(CancellationToken cancellationToken)
        {
            var lookupsAllData = await _unitOfWorkDA.LookupsDA.GetAll(cancellationToken);
            var lookupId = lookupsAllData.Where(x => x.LookupName == nameof(MasterPagesEnum.Category)).Select(x => x.LookupId).FirstOrDefault();
            return lookupId;
        }

        private async Task<int> GetUnitLookupId(CancellationToken cancellationToken)
        {
            var lookupsAllData = await _unitOfWorkDA.LookupsDA.GetAll(cancellationToken);
            var lookupId = lookupsAllData.Where(x => x.LookupName == nameof(MasterPagesEnum.Unit)).Select(x => x.LookupId).FirstOrDefault();
            return lookupId;
        }

        #endregion Private Method
    }
}