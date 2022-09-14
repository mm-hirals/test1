using AutoMapper;
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

        public Task<IEnumerable<ProductResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductMainRequestDto> GetById(Int64 Id, CancellationToken cancellationToken)
        {
            ProductMainRequestDto productMainRequestDto = new ProductMainRequestDto();
            var data = await GetProductById(Id, cancellationToken);
            var productImage = await GetProductImageById(Id, cancellationToken);
            var productMaterialData = await GetProductMaterialById(Id, cancellationToken);
            productMainRequestDto.ProductRequestDto = _mapper.Map<ProductRequestDto>(data);
            productMainRequestDto.ProductImageRequestDto = _mapper.Map<List<ProductImageRequestDto>>(productImage);
            productMainRequestDto.ProductMaterialRequestDto = _mapper.Map<List<ProductMaterialRequestDto>>(productMaterialData);
            return productMainRequestDto;
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

        public async Task<ProductRequestDto> CreateProduct(ProductMainRequestDto model, CancellationToken cancellationToken)
        {
            // Add Product Details
            var productToInsert = _mapper.Map<Product>(model.ProductRequestDto);
            productToInsert.IsDeleted = false;
            productToInsert.TenantId = _currentUser.TenantId;
            productToInsert.CreatedBy = _currentUser.UserId;
            productToInsert.CreatedDate = DateTime.Now;
            productToInsert.CreatedUTCDate = DateTime.UtcNow;
            var productData = await _unitOfWorkDA.ProductDA.CreateProduct(productToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<ProductRequestDto>(productData);

            // Add Product Images
            await SaveImages(model, productData.ProductId, cancellationToken);

            // Add Product Materials
            await AddProductMaterials(productData.ProductId, model, cancellationToken);

            return _mappedUser;
        }

        public async Task<ProductRequestDto> UpdateProduct(int Id, ProductMainRequestDto model, CancellationToken cancellationToken)
        {
            // Update Product Details
            var getProductById = await GetProductById(Id, cancellationToken);
            UpdateData(getProductById);
            MapToDbObject(model.ProductRequestDto, getProductById);
            var data = await _unitOfWorkDA.ProductDA.UpdateProduct(Id, getProductById, cancellationToken);
            var _mappedUser = _mapper.Map<ProductRequestDto>(data);

            // Delete all Product Materials by product Id
            await DeleteProductMaterials(Id, cancellationToken);

            // Add Product Materials
            await AddProductMaterials(Id, model, cancellationToken);

            return _mappedUser;
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

        private async Task<List<ProductImage>> GetProductImageById(Int64 Id, CancellationToken cancellationToken)
        {
            var productImage = await _unitOfWorkDA.ProductImageDA.GetAll(cancellationToken);
            var productImageDataById = productImage.Where(x => x.ProductId == Id).ToList();
            if (productImageDataById == null)
            {
                throw new Exception("ProductImages not found");
            }
            return productImageDataById;
        }

        private async Task<List<ProductMaterial>> GetProductMaterialById(Int64 Id, CancellationToken cancellationToken)
        {
            var productMaterial = await _unitOfWorkDA.ProductMaterialDA.GetAll(cancellationToken);
            var productMaterialDataById = productMaterial.Where(x => x.ProductId == Id).ToList();
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
            oldData.UsedPolish = model.UsedPolish;
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

        private async Task SaveImages(ProductMainRequestDto model, long productId, CancellationToken cancellationToken)
        {
            if (model.Files != null)
            {
                foreach (var item in model.Files)
                {
                    ProductImageRequestDto productImageRequestDto = new ProductImageRequestDto();
                    productImageRequestDto.ImagePath = await _fileStorageService.StoreFile(item, ApplicationFileStorageConstants.FilePaths.Product);
                    productImageRequestDto.ImageName = item.FileName;

                    var productImageToInsert = _mapper.Map<ProductImage>(productImageRequestDto);
                    productImageToInsert.ProductId = productId;
                    productImageToInsert.ImagePath = productImageRequestDto.ImagePath;
                    productImageToInsert.ImageName = productImageRequestDto.ImageName;
                    productImageToInsert.CreatedBy = _currentUser.UserId;
                    productImageToInsert.CreatedDate = DateTime.Now;
                    productImageToInsert.CreatedUTCDate = DateTime.UtcNow;
                    await _unitOfWorkDA.ProductImageDA.CreateProductImage(productImageToInsert, cancellationToken);
                }
            }
        }

        private async Task AddProductMaterials(long productId, ProductMainRequestDto model, CancellationToken cancellationToken)
        {
            if (model.ProductMaterialRequestDto != null)
            {
                foreach (var item in model.ProductMaterialRequestDto)
                {
                    item.ProductId = productId;
                    await CreateProductMaterial(item, cancellationToken);
                }
            }
        }

        private async Task DeleteProductMaterials(int Id, CancellationToken cancellationToken)
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