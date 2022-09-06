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
            productMainRequestDto.ProductRequestDto = _mapper.Map<ProductRequestDto>(data);
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
            foreach (var item in model.Files)
            {
                ProductImageRequestDto productImageRequestDto = new ProductImageRequestDto();
                productImageRequestDto.ImagePath = await _fileStorageService.StoreFile(item, ApplicationFileStorageConstants.FilePaths.Product);
                productImageRequestDto.ImageName = item.FileName;

                var productImageToInsert = _mapper.Map<ProductImage>(productImageRequestDto);
                productImageToInsert.ProductId = productToInsert.ProductId;
                productImageToInsert.ImagePath = productImageRequestDto.ImagePath;
                productImageToInsert.ImageName = productImageRequestDto.ImageName;
                productImageToInsert.CreatedBy = _currentUser.UserId;
                productImageToInsert.CreatedDate = DateTime.Now;
                productImageToInsert.CreatedUTCDate = DateTime.UtcNow;
                await _unitOfWorkDA.ProductImageDA.CreateProductImage(productImageToInsert, cancellationToken);
            }

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

        #endregion Private Method
    }
}