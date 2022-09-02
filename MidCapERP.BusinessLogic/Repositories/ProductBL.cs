using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Services.FileStorage;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Constants;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Paging;
using MidCapERP.Dto.Product;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class ProductBL : IProductBL
    {
        private readonly IUnitOfWorkDA _unitOfWorkDA;

        //private readonly IUnitOfWorkBL _unitOfWorkBL;
        public readonly IMapper _mapper;

        private readonly CurrentUser _currentUser;
        private readonly IFileStorageService _fileStorageService;

        public ProductBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser, IFileStorageService fileStorageService)
        {
            _unitOfWorkDA = unitOfWorkDA;
            //_unitOfWorkBL = unitOfWorkBL;
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
            productMainRequestDto.ProductRequestDto.ProductId = data.ProductId;
            productMainRequestDto.ProductRequestDto.CategoryId = data.CategoryId;
            productMainRequestDto.ProductRequestDto.ProductTitle = data.ProductTitle;
            productMainRequestDto.ProductRequestDto.ModelNo = data.ModelNo;
            productMainRequestDto.ProductRequestDto.Width = data.Width;
            productMainRequestDto.ProductRequestDto.Height = data.Height;
            productMainRequestDto.ProductRequestDto.Depth = data.Depth;
            productMainRequestDto.ProductRequestDto.UsedFabric = data.UsedFabric;
            productMainRequestDto.ProductRequestDto.UsedPolish = data.UsedPolish;
            productMainRequestDto.ProductRequestDto.IsVisibleToWholesalers = data.IsVisibleToWholesalers;
            productMainRequestDto.ProductRequestDto.TotalDaysToPrepare = data.TotalDaysToPrepare;
            productMainRequestDto.ProductRequestDto.Features = data.Features;
            productMainRequestDto.ProductRequestDto.Comments = data.Comments;
            productMainRequestDto.ProductRequestDto.CostPrice = data.CostPrice;
            productMainRequestDto.ProductRequestDto.RetailerPrice = data.RetailerPrice;
            productMainRequestDto.ProductRequestDto.WholesalerPrice = data.WholesalerPrice;
            productMainRequestDto.ProductRequestDto.CoverImage = data.CoverImage;
            productMainRequestDto.ProductRequestDto.QRImage = data.QRImage;
            productMainRequestDto.ProductRequestDto.TenantId = data.TenantId;
            return _mapper.Map<ProductMainRequestDto>(productMainRequestDto);
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
            var productToInsert = _mapper.Map<Product>(model.ProductRequestDto);
            productToInsert.IsDeleted = false;
            productToInsert.TenantId = _currentUser.TenantId;
            productToInsert.CreatedBy = _currentUser.UserId;
            productToInsert.CreatedDate = DateTime.Now;
            productToInsert.CreatedUTCDate = DateTime.UtcNow;
            var productData = await _unitOfWorkDA.ProductDA.CreateProduct(productToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<ProductRequestDto>(productData);
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