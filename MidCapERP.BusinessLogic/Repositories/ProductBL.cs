using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using MidCapERP.BusinessLogic.Constants;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Services.ActivityLog;
using MidCapERP.BusinessLogic.Services.FileStorage;
using MidCapERP.BusinessLogic.Services.QRCodeGenerate;
using MidCapERP.Core.CommonHelper;
using MidCapERP.Core.Constants;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.ActivityLogs;
using MidCapERP.Dto.Constants;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.MegaSearch;
using MidCapERP.Dto.OrderCalculation;
using MidCapERP.Dto.Paging;
using MidCapERP.Dto.Polish;
using MidCapERP.Dto.Product;
using MidCapERP.Dto.ProductImage;
using MidCapERP.Dto.ProductMaterial;
using MidCapERP.Dto.SearchResponse;
using MidCapERP.Dto.Tenant;
using System.Text;
using Wkhtmltopdf.NetCore;

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
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IGeneratePdf _generatePdf;


        public ProductBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser, IFileStorageService fileStorageService, IQRCodeService iQRCodeService, IActivityLogsService activityLogsService, IHostingEnvironment hostingEnvironment, IGeneratePdf generatePdf)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
            _fileStorageService = fileStorageService;
            _iQRCodeService = iQRCodeService;
            _activityLogsService = activityLogsService;
            this._hostingEnvironment = hostingEnvironment;
            _generatePdf = generatePdf;
        }

        public async Task<JsonRepsonse<ProductResponseDto>> GetFilterProductData(ProductDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
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
                                           UpdatedBy = x.UpdatedBy != null ? x.UpdatedBy : x.CreatedBy,
                                           UpdatedByName = x.UpdatedBy != null ? updatedMat.FullName : z.FullName,
                                           UpdatedDate = x.UpdatedDate != null ? x.UpdatedDate : x.CreatedDate,
                                       }).AsQueryable();
            var productFilteredData = FilterProductData(dataTableFilterDto, productResponseData);
            var productData = new PagedList<ProductResponseDto>(productFilteredData, dataTableFilterDto);
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
                                       WidthNumeric = Convert.ToString(Math.Floor(Convert.ToDecimal(x.Width))),
                                       HeightNumeric = Convert.ToString(Math.Floor(Convert.ToDecimal(x.Height))),
                                       DepthNumeric = Convert.ToString(Math.Floor(Convert.ToDecimal(x.Depth))),
                                       DiameterNumeric = Convert.ToString(Math.Floor(Convert.ToDecimal(x.Diameter))),
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

        public async Task<ProductDetailResponseDto> GetProductDetailById(Int64 Id, CancellationToken cancellationToken)
        {
            try
            {
                ProductDetailResponseDto productDetailResponseDto = new ProductDetailResponseDto();
                var getProductById = await GetProductById(Id, cancellationToken);
                if (getProductById != null)
                    _currentUser.TenantId = getProductById.TenantId;
                if (_currentUser.TenantId > 0)
                {
                    var tenantDetails = await _unitOfWorkDA.TenantDA.GetById(_currentUser.TenantId, cancellationToken);
                    var allProductdata = await _unitOfWorkDA.ProductDA.GetAll(cancellationToken);
                    var productData = (from x in allProductdata.Where(x => x.ProductId == Id)
                                       select new ProductDetailResponseDto()
                                       {
                                           ProductId = Id,
                                           CategoryId = x.CategoryId,
                                           ProductTitle = x.ProductTitle,
                                           ModelNo = x.ModelNo,
                                           Width = Convert.ToString(Math.Floor(Convert.ToDecimal(x.Width))),
                                           Height = Convert.ToString(Math.Floor(Convert.ToDecimal(x.Height))),
                                           Depth = Convert.ToString(Math.Floor(Convert.ToDecimal(x.Depth))),
                                           Diameter = Convert.ToString(Math.Floor(Convert.ToDecimal(x.Diameter))),
                                           FabricNeeded = x.FabricNeeded,
                                           TotalDaysToPrepare = x.TotalDaysToPrepare,
                                           Features = x.Features,
                                           Comments = x.Comments,
                                           RetailerPrice = tenantDetails.ProductRSPPercentage == null || tenantDetails.ProductRSPPercentage == 0 ? Math.Round(x.CostPrice, 2) : Math.Round(x.CostPrice + Math.Round(((x.CostPrice * (decimal)tenantDetails.ProductRSPPercentage) / 100)), 2),
                                           QRImage = x.QRImage,
                                       }).FirstOrDefault();
                    productDetailResponseDto = _mapper.Map<ProductDetailResponseDto>(productData);

                    var productImageList = await GetProductImageById(Id, cancellationToken);
                    if (productImageList != null)
                        productDetailResponseDto.ProductImageResponseDto = _mapper.Map<List<ProductImageResponseDto>>(productImageList.ToList());

                    var productMaterialList = await GetProductMaterialById(Id, cancellationToken);
                    var polishSubjectTypeId = await _unitOfWorkDA.SubjectTypesDA.GetPolishSubjectTypeId(cancellationToken);
                    var unitData = await GetAllUnit(cancellationToken);
                    var polish = await GetAllPolish(cancellationToken);

                    var data = (from x in productMaterialList
                                join z in polish on x.SubjectId equals z.PolishId into polishM
                                from polishMat in polishM.DefaultIfEmpty(new Polish())
                                where x.SubjectTypeId == polishSubjectTypeId && polishMat != null
                                join up in unitData on polishMat.UnitId equals up.LookupValueId
                                select new PolishResponseDto
                                {
                                    PolishId = polishMat.PolishId,
                                    Title = polishMat.Title,
                                    ModelNo = polishMat.ModelNo,
                                }).ToList();

                    if (data.Any())
                        productDetailResponseDto.Polish = string.Join(", ", data.Select(x => x.Title + " - " + x.ModelNo));

                    productDetailResponseDto.TenantResponseDto = _mapper.Map<TenantResponseDto>(tenantDetails);
                }
                return productDetailResponseDto;
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

            var rawMaterialSubjectTypeId = await _unitOfWorkDA.SubjectTypesDA.GetRawMaterialSubjectTypeId(cancellationToken);
            var polishSubjectTypeId = await _unitOfWorkDA.SubjectTypesDA.GetPolishSubjectTypeId(cancellationToken);

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
                            CostPrice = rowMat.UnitPrice,
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
                                     CostPrice = polishMat.UnitPrice,
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

        public async Task<ProductForDetailsByModuleNoResponceDto> GetProductForDetailsByModuleNo(string modelNo, CancellationToken cancellationToken)
        {
            var productAlldata = await _unitOfWorkDA.ProductDA.GetAll(cancellationToken);
            var productData = productAlldata.FirstOrDefault(x => x.ModelNo == modelNo);
            if (productData == null)
                throw new Exception("Product is not found");
            else
                if (productData.TenantId != _currentUser.TenantId)
                throw new Exception("Product is not found");

            var productSubjectTypeId = await _unitOfWorkDA.SubjectTypesDA.GetProductSubjectTypeId(cancellationToken);
            var tenantData = await _unitOfWorkDA.TenantDA.GetById(productData.TenantId, cancellationToken);
            productData.CostPrice = CommonMethod.GetCalculatedPrice(productData.CostPrice, tenantData.ProductRSPPercentage, tenantData.AmountRoundMultiple);
            return new ProductForDetailsByModuleNoResponceDto(productData.ProductId, productData.CategoryId, productData.ProductTitle, productData.ModelNo, productData.Width, productData.Height, productData.Depth, productData.Diameter, productData.FabricNeeded, productData.IsVisibleToWholesalers, productData.TotalDaysToPrepare, productData.Features, productData.Comments, productData.CostPrice, productData.QRImage, productSubjectTypeId);
        }

        public async Task<ProductRequestDto> CreateProduct(ProductRequestDto model, CancellationToken cancellationToken)
        {
            model.Width = Convert.ToDecimal(model.WidthNumeric);
            model.Height = Convert.ToDecimal(model.HeightNumeric);
            model.Depth = Convert.ToDecimal(model.DepthNumeric);
            model.Diameter = Convert.ToDecimal(model.DiameterNumeric);
            var productToInsert = _mapper.Map<Product>(model);
            productToInsert.Status = 0;
            productToInsert.TenantId = _currentUser.TenantId;
            productToInsert.CreatedBy = _currentUser.UserId;
            productToInsert.CreatedDate = DateTime.Now;
            productToInsert.CreatedUTCDate = DateTime.UtcNow;
            var productData = await _unitOfWorkDA.ProductDA.CreateProduct(productToInsert, cancellationToken);
            await _activityLogsService.PerformActivityLog(await _unitOfWorkDA.SubjectTypesDA.GetProductSubjectTypeId(cancellationToken), productData.ProductId, "Product Created", ActivityLogStringConstant.Create, CancellationToken.None);
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
                    getProductById.Diameter = Convert.ToDecimal(model.DiameterNumeric);
                    var data = await _unitOfWorkDA.ProductDA.UpdateProduct(getProductById, cancellationToken);
                    await _activityLogsService.PerformActivityLog(await _unitOfWorkDA.SubjectTypesDA.GetProductSubjectTypeId(cancellationToken), model.ProductId, "Product Updated", ActivityLogStringConstant.Update, cancellationToken);
                    var _mappedUser = _mapper.Map<ProductRequestDto>(data);
                    return _mappedUser;
                }

                throw new Exception("Product Not Found");
            }

            throw new Exception("Product Not Found");
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
                    await _activityLogsService.PerformActivityLog(await _unitOfWorkDA.SubjectTypesDA.GetProductSubjectTypeId(cancellationToken), model.ProductId, "ProductDetail Updated", ActivityLogStringConstant.Update, cancellationToken);
                    var _mappedUser = _mapper.Map<ProductRequestDto>(data);
                    return _mappedUser;
                }

                throw new Exception("Product Not Found");
            }

            throw new Exception("Product Not Found");
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
                    await _activityLogsService.PerformActivityLog(await _unitOfWorkDA.SubjectTypesDA.GetProductSubjectTypeId(cancellationToken), model.ProductId, "ProducStatus Updated", ActivityLogStringConstant.Update, cancellationToken);
                }
            }
            else
                throw new Exception("Product Not Found");
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
                    await _activityLogsService.PerformActivityLog(await _unitOfWorkDA.SubjectTypesDA.GetProductSubjectTypeId(cancellationToken), model.ProductId, "ProducCost Updated", ActivityLogStringConstant.Update, cancellationToken);
                    return _mapper.Map<ProductRequestDto>(data);
                }

                throw new Exception("Product Not Found");
            }

            throw new Exception("Product Not Found");
        }

        public async Task<ProductRequestDto> DeleteProduct(int Id, CancellationToken cancellationToken)
        {
            var productToDelete = await GetProductById(Id, cancellationToken);
            productToDelete.Status = (int)ProductStatusEnum.Delete;
            UpdateData(productToDelete);
            var data = await _unitOfWorkDA.ProductDA.DeleteProduct(Id, cancellationToken);
            await _activityLogsService.PerformActivityLog(await _unitOfWorkDA.SubjectTypesDA.GetProductSubjectTypeId(cancellationToken), Id, "Product Deleted", ActivityLogStringConstant.Delete, cancellationToken);
            var _mappedUser = _mapper.Map<ProductRequestDto>(data);
            return _mappedUser;
        }

        public async Task DeleteProductImage(int productImageId, CancellationToken cancellationToken)
        {
            await _unitOfWorkDA.ProductImageDA.DeleteProductImage(productImageId, cancellationToken);
            await _activityLogsService.PerformActivityLog(await _unitOfWorkDA.SubjectTypesDA.GetProductSubjectTypeId(cancellationToken), productImageId, "Product Image Deleted", ActivityLogStringConstant.Delete, cancellationToken);
        }

        public async Task UpdateProductImageMarkAsCover(int productImageId, bool IsCover, CancellationToken cancellationToken)
        {
            var getProductImage = await _unitOfWorkDA.ProductImageDA.GetById(productImageId, cancellationToken);
            if (getProductImage != null)
            {
                getProductImage.IsCover = IsCover;
                await _unitOfWorkDA.ProductImageDA.UpdateProductImage(getProductImage, cancellationToken);
                await _activityLogsService.PerformActivityLog(await _unitOfWorkDA.SubjectTypesDA.GetProductSubjectTypeId(cancellationToken), productImageId, "Image Updated", ActivityLogStringConstant.Update, cancellationToken);
            }
        }

        public async Task<ProductDimensionsApiResponseDto> GetPriceByDimensionsAPI(ProductDimensionsApiRequestDto orderCalculationApiRequestDto, CancellationToken cancellationToken)
        {
            ProductDimensionsApiResponseDto orderCalculationData = new ProductDimensionsApiResponseDto();
            var productSubjectTypeId = await _unitOfWorkDA.SubjectTypesDA.GetProductSubjectTypeId(cancellationToken);
            if (productSubjectTypeId == orderCalculationApiRequestDto.SubjectTypeId)
            {
                var productData = await _unitOfWorkDA.ProductDA.GetById(orderCalculationApiRequestDto.SubjectId, cancellationToken);
                if (productData != null)
                {
                    var tenantData = await _unitOfWorkDA.TenantDA.GetById(productData.TenantId, cancellationToken);
                    var lookupValueData = await _unitOfWorkDA.LookupValuesDA.GetById(productData.CategoryId, cancellationToken);
                    if (lookupValueData.LookupValueName == "Sofa / Corner / Lounger")
                    {
                        decimal costPerCubic = productData.CostPrice / Convert.ToDecimal(productData.Width);
                        decimal totalCubic = Convert.ToDecimal(orderCalculationApiRequestDto.Width);
                        decimal newCostPrice = totalCubic * costPerCubic;
                        newCostPrice = CommonMethod.GetCalculatedPrice(Math.Round(newCostPrice), 0, tenantData.AmountRoundMultiple);
                        decimal retailerPrice = CommonMethod.GetCalculatedPrice(newCostPrice, tenantData.ProductRSPPercentage, tenantData.AmountRoundMultiple);
                        decimal totalPrice = Math.Round(Math.Round(retailerPrice * orderCalculationApiRequestDto.Quantity, 2));
                        orderCalculationData.TotalAmount = totalPrice;
                    }
                    else if (lookupValueData.LookupValueName == "Marble")
                    {
                        decimal costPerCubic = productData.CostPrice / (Convert.ToDecimal(productData.Width) * Convert.ToDecimal(productData.Height));
                        decimal totalCubic = Convert.ToDecimal(orderCalculationApiRequestDto.Width * orderCalculationApiRequestDto.Height);
                        decimal newCostPrice = totalCubic * costPerCubic;
                        newCostPrice = CommonMethod.GetCalculatedPrice(Math.Round(newCostPrice), 0, tenantData.AmountRoundMultiple);
                        decimal retailerPrice = CommonMethod.GetCalculatedPrice(newCostPrice, tenantData.ProductRSPPercentage, tenantData.AmountRoundMultiple);
                        decimal totalPrice = Math.Round(Math.Round(retailerPrice * orderCalculationApiRequestDto.Quantity, 2));
                        orderCalculationData.TotalAmount = totalPrice;
                    }
                    else
                    {
                        orderCalculationData.TotalAmount = orderCalculationApiRequestDto.TotalAmount;
                    }
                    orderCalculationData.SubjectId = orderCalculationApiRequestDto.SubjectId;
                    orderCalculationData.SubjectTypeId = orderCalculationApiRequestDto.SubjectTypeId;
                    orderCalculationData.Quantity = orderCalculationApiRequestDto.Quantity;
                    orderCalculationData.Width = orderCalculationApiRequestDto.Width;
                    orderCalculationData.Height = orderCalculationApiRequestDto.Height;
                    orderCalculationData.Depth = orderCalculationApiRequestDto.Depth;
                    orderCalculationData.Diameter = orderCalculationApiRequestDto.Diameter;
                }
            }
            return _mapper.Map<ProductDimensionsApiResponseDto>(orderCalculationData);
        }

        public async Task<int> GetRawMaterialSubjectTypeId(CancellationToken cancellationToken)
        {
            return await _unitOfWorkDA.SubjectTypesDA.GetRawMaterialSubjectTypeId(cancellationToken);
        }

        public async Task<int> GetPolishSubjectTypeId(CancellationToken cancellationToken)
        {
            return await _unitOfWorkDA.SubjectTypesDA.GetPolishSubjectTypeId(cancellationToken);
        }

        public async Task<int> GetProductSubjectTypeId(CancellationToken cancellationToken)
        {
            return await _unitOfWorkDA.SubjectTypesDA.GetProductSubjectTypeId(cancellationToken);
        }

        public async Task<IEnumerable<ActivityLogsResponseDto>> GetProductActivityByProductId(Int64 productId, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.ActivityLogsDA.GetAll(cancellationToken);
            data = data.Where(p => p.SubjectId == productId);
            var userData = await _unitOfWorkDA.UserDA.GetUsers(cancellationToken);
            var dataResponse = (from x in data
                                join y in userData on new { UserId = x.CreatedBy } equals new { UserId = y.UserId }
                                select new ActivityLogsResponseDto()
                                {
                                    Description = x.Description,
                                    Action = x.Action,
                                    CreatedBy = x.CreatedBy,
                                    CreatedByName = y.FirstName + " " + y.LastName,
                                    CreatedDate = x.CreatedDate,
                                    ActivityLogID = x.ActivityLogID,
                                }).OrderByDescending(p => p.ActivityLogID).AsQueryable();

            return dataResponse;
        }

        public async Task<JsonRepsonse<ActivityLogsResponseDto>> GetFilterProductActivityData(ProductActivityDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.ActivityLogsDA.GetAll(cancellationToken);
            data = data.Where(p => p.SubjectId == dataTableFilterDto.productId);
            var userData = await _unitOfWorkDA.UserDA.GetUsers(cancellationToken);
            var dataResponse = (from x in data
                                join y in userData on new { UserId = x.CreatedBy } equals new { UserId = y.UserId }
                                select new ActivityLogsResponseDto()
                                {
                                    Description = x.Description,
                                    Action = x.Action,
                                    CreatedBy = x.CreatedBy,
                                    CreatedByName = y.FirstName + " " + y.LastName,
                                    CreatedDate = x.CreatedDate,
                                    ActivityLogID = x.ActivityLogID,
                                }).OrderByDescending(p => p.ActivityLogID).AsQueryable();
            var productData = new PagedList<ActivityLogsResponseDto>(dataResponse, dataTableFilterDto);
            return new JsonRepsonse<ActivityLogsResponseDto>(dataTableFilterDto.Draw, productData.TotalCount, productData.TotalCount, productData);
        }

        public async Task<int> GetFabricSubjectTypeId(CancellationToken cancellationToken)
        {
            return await _unitOfWorkDA.SubjectTypesDA.GetFabricSubjectTypeId(cancellationToken);
        }

        public async Task<byte[]> PrintProductDetail(ProductPrintDto model, CancellationToken cancellationToken)
        {
            ProductResponseDto productResponseDto = new ProductResponseDto();

            //This Pdf Code
            string paths = _hostingEnvironment.WebRootPath;
            var random = Guid.NewGuid();

            var htmlStart = @"<html>
                        <head>
                            <title>Pdf Title</title>
                        </head>
                        <body>";
            var htmlEnd = @"</body></html>";

            StringBuilder sb = new StringBuilder();
            sb.Append(htmlStart);
            sb.Append("<table>");
            foreach (var item in model.ProductList)
            {
                var getProductById = await GetProductById(item, cancellationToken);
                var categoryName = await _unitOfWorkDA.LookupValuesDA.GetById(getProductById.CategoryId, cancellationToken);
                productResponseDto.ProductTitle = getProductById.ProductTitle;
                productResponseDto.CategoryName = categoryName.LookupValueName;
                productResponseDto.ModelNo = getProductById.ModelNo;
                productResponseDto.QRImage = getProductById.QRImage;

                sb.Append("<tr>");
                sb.Append("<td><b>Product Title:</b> &nbsp'" + productResponseDto.ProductTitle + "'");
                sb.Append("<br><b>Cateagory Name: </b> &nbsp'" + productResponseDto.CategoryName + "'");
                sb.Append("<br><b>Model No :</b> &nbsp'" + productResponseDto.ModelNo + "'");
                sb.Append("</td>");
                var pathMerge = paths + productResponseDto.QRImage;
                sb.Append("<td><img height='250px' width='250px' src='"+ productResponseDto.QRImage + "'</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            sb.Append(htmlEnd);

            return _generatePdf.GetPDF(sb.ToString());
        }

        #region API Methods

        public async Task<ProductRequestDto> GetByIdAPI(Int64 Id, CancellationToken cancellationToken)
        {
            var productData = await GetProductById(Id, cancellationToken);
            if (productData.TenantId != _currentUser.TenantId)
                throw new Exception("Product Not Found");

            var tenantData = await _unitOfWorkDA.TenantDA.GetById(productData.TenantId, cancellationToken);
            productData.CostPrice = CommonMethod.GetCalculatedPrice(productData.CostPrice, tenantData.ProductRSPPercentage, tenantData.AmountRoundMultiple);
            return _mapper.Map<ProductRequestDto>(productData);
        }

        public async Task<IEnumerable<MegaSearchResponse>> GetProductMegaSearchForDropDownByModuleNo(string modelno, CancellationToken cancellationToken)
        {
            var productSubjectTypeId = await _unitOfWorkDA.SubjectTypesDA.GetRawMaterialSubjectTypeId(cancellationToken);
            var productAlldata = await _unitOfWorkDA.ProductDA.GetAll(cancellationToken);
            return productAlldata.Where(x => x.Status == (int)ProductStatusEnum.Published && x.ModelNo.StartsWith(modelno)).Select(x => new MegaSearchResponse(x.ProductId, x.ProductTitle, x.ModelNo, "", "Product")).Take(10).ToList();
        }

        public async Task<IEnumerable<SearchResponse>> GetProductForDropDownByModuleNo(string modelno, CancellationToken cancellationToken)
        {
            var productSubjectTypeId = await _unitOfWorkDA.SubjectTypesDA.GetProductSubjectTypeId(cancellationToken);
            var productAlldata = await _unitOfWorkDA.ProductDA.GetAll(cancellationToken);
            return productAlldata.Where(x => x.Status == (int)ProductStatusEnum.Published && x.ModelNo.StartsWith(modelno)).Select(x => new SearchResponse(x.ProductId, x.ProductTitle, x.ModelNo, "", "Product", productSubjectTypeId)).Take(10).ToList();
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
            oldData.Diameter = model.Diameter;
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
                await _activityLogsService.PerformActivityLog(await _unitOfWorkDA.SubjectTypesDA.GetProductSubjectTypeId(cancellationToken), model.ProductId, "Image Added", ActivityLogStringConstant.Create, cancellationToken);
            }
        }

        private async Task DeleteImages(List<ProductImage> getImageById, CancellationToken cancellationToken)
        {
            foreach (var item in getImageById)
            {
                await _unitOfWorkDA.ProductImageDA.DeleteProductImage(item.ProductImageID, cancellationToken);
                await _activityLogsService.PerformActivityLog(await _unitOfWorkDA.SubjectTypesDA.GetProductSubjectTypeId(cancellationToken), item.ProductImageID, "Image Deleted", ActivityLogStringConstant.Delete, cancellationToken);
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
                await _activityLogsService.PerformActivityLog(await _unitOfWorkDA.SubjectTypesDA.GetProductSubjectTypeId(cancellationToken), item.ProductMaterialID, "ProductMaterial Created", ActivityLogStringConstant.Create, cancellationToken);
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
            await _activityLogsService.PerformActivityLog(await _unitOfWorkDA.SubjectTypesDA.GetProductSubjectTypeId(cancellationToken), item.ProductId, "ProductMaterial Created", ActivityLogStringConstant.Create, cancellationToken);
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

        private static IQueryable<ProductResponseDto> FilterProductData(ProductDataTableFilterDto productDataTableFilterDto, IQueryable<ProductResponseDto> productResponseDto)
        {
            if (productDataTableFilterDto != null)
            {
                if (!string.IsNullOrEmpty(productDataTableFilterDto.CategoryName))
                {
                    productResponseDto = productResponseDto.Where(p => p.CategoryName.StartsWith(productDataTableFilterDto.CategoryName));
                }
                if (!string.IsNullOrEmpty(productDataTableFilterDto.ModelNo))
                {
                    productResponseDto = productResponseDto.Where(p => p.ModelNo.StartsWith(productDataTableFilterDto.ModelNo));
                }
                if (!string.IsNullOrEmpty(productDataTableFilterDto.ProductTitle))
                {
                    productResponseDto = productResponseDto.Where(p => p.ProductTitle.StartsWith(productDataTableFilterDto.ProductTitle));
                }
                if (productDataTableFilterDto.publishStatus != null)
                {
                    productResponseDto = productResponseDto.Where(p => p.Status == productDataTableFilterDto.publishStatus);
                }
            }
            return productResponseDto;
        }

        #endregion Private Method
    }
}