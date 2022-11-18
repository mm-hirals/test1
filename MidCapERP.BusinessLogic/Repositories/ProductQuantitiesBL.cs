using AutoMapper;
using MidCapERP.BusinessLogic.Constants;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Services.ActivityLog;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.ActivityLogs;
using MidCapERP.Dto.Constants;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Paging;
using MidCapERP.Dto.ProductQuantities;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class ProductQuantitiesBL : IProductQuantitiesBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;
        private readonly IActivityLogsService _activityLogsService;

        public ProductQuantitiesBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser, IActivityLogsService activityLogsService)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
            _activityLogsService = activityLogsService;
        }

        public async Task<JsonRepsonse<ProductQuantitiesResponseDto>> GetFilterProductQuantitiesData(ProductQuantitiesDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var productQuantitiesAllData = await _unitOfWorkDA.ProductQuantitiesDA.GetAll(cancellationToken);
            var productAllData = await _unitOfWorkDA.ProductDA.GetAll(cancellationToken);
            var categoryAllData = await _unitOfWorkDA.CategoriesDA.GetAll(cancellationToken);
            var categoryData = categoryAllData.Where(x => x.CategoryTypeId == (int)ProductCategoryTypesEnum.Product);
            var allUsers = await _unitOfWorkDA.UserDA.GetUsers(cancellationToken);
            var productQuantitiesResponseData = (from x in productQuantitiesAllData
                                                 join y in productAllData on x.ProductId equals y.ProductId
                                                 join z in categoryData on y.CategoryId equals z.CategoryId
                                                 join u in allUsers on x.LastModifiedBy equals u.UserId
                                                 select new ProductQuantitiesResponseDto()
                                                 {
                                                     ProductQuantityId = x.ProductQuantityId,
                                                     ProductId = x.ProductId,
                                                     ProductTitle = y.ProductTitle,
                                                     CategoryId = y.CategoryId,
                                                     CategoryName = z.CategoryName,
                                                     QuantityDate = x.QuantityDate,
                                                     Quantity = x.Quantity,
                                                     LastModifiedBy = x.LastModifiedBy,
                                                     LastModifiedByName = u.FullName,
                                                     LastModifiedDate = x.LastModifiedDate,
                                                     LastModifiedUTCDate = x.LastModifiedUTCDate
                                                 }).AsQueryable();
            var productQuantitiesFilteredData = FilterProductQuantitiesData(dataTableFilterDto, productQuantitiesResponseData);
            var productQuantitiesData = new PagedList<ProductQuantitiesResponseDto>(productQuantitiesFilteredData, dataTableFilterDto);
            return new JsonRepsonse<ProductQuantitiesResponseDto>(dataTableFilterDto.Draw, productQuantitiesData.TotalCount, productQuantitiesData.TotalCount, productQuantitiesData);
        }

        public async Task<ProductQuantitiesRequestDto> GetById(long Id, CancellationToken cancellationToken)
        {
            var productQuantitiesAllData = await _unitOfWorkDA.ProductQuantitiesDA.GetAll(cancellationToken);
            var productAllData = await _unitOfWorkDA.ProductDA.GetAll(cancellationToken);
            var categoryAllData = await _unitOfWorkDA.CategoriesDA.GetAll(cancellationToken);
            var categoryData = categoryAllData.Where(x => x.CategoryTypeId == (int)ProductCategoryTypesEnum.Product);
            var productQuantity = (from x in productQuantitiesAllData
                                   join y in productAllData on x.ProductId equals y.ProductId
                                   join z in categoryData on y.CategoryId equals z.CategoryId
                                   where x.ProductQuantityId == Id
                                   select new ProductQuantitiesRequestDto()
                                   {
                                       ProductQuantityId = x.ProductQuantityId,
                                       ProductId = x.ProductId,
                                       ProductTitle = y.ProductTitle,
                                       CategoryName = z.CategoryName,
                                       QuantityDate = x.QuantityDate,
                                       Quantity = x.Quantity,
                                       LastModifiedBy = x.LastModifiedBy,
                                       LastModifiedDate = x.LastModifiedDate,
                                       LastModifiedUTCDate = x.LastModifiedUTCDate
                                   }).FirstOrDefault();
            await GetProductQtyActivityLog(Id, productQuantity, cancellationToken);
            return _mapper.Map<ProductQuantitiesRequestDto>(productQuantity);
        }

        public async Task<ProductQuantitiesRequestDto> UpdateProductQuantities(long Id, ProductQuantitiesRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await GetProductQuantityById(Id, cancellationToken);
            var oldQuantity = oldData.Quantity;
            var sign = model.UpdatedQuantity > 0 ? "+" : "";
            UpdateData(oldData);
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.ProductQuantitiesDA.UpdateProductQuantities(Id, oldData, cancellationToken);
            await _activityLogsService.PerformActivityLog(await _unitOfWorkDA.SubjectTypesDA.GetProductQuantitySubjectTypeId(cancellationToken), Id, "Product quantity has been updated from " + oldQuantity + " to " + data.Quantity + " (" + sign + model.UpdatedQuantity + ")", ActivityLogStringConstant.Update, cancellationToken);
            var _mappedProductQuantities = _mapper.Map<ProductQuantitiesRequestDto>(data);
            return _mappedProductQuantities;
        }

        #region Private Methods

        private async Task<ProductQuantities> GetProductQuantityById(long Id, CancellationToken cancellationToken)
        {
            var productQuantityDataById = await _unitOfWorkDA.ProductQuantitiesDA.GetById(Id, cancellationToken);
            if (productQuantityDataById == null)
            {
                throw new Exception("ProductQuantity not found");
            }
            return productQuantityDataById;
        }

        private void UpdateData(ProductQuantities oldData)
        {
            oldData.LastModifiedBy = _currentUser.UserId;
            oldData.LastModifiedDate = DateTime.Now;
            oldData.LastModifiedUTCDate = DateTime.UtcNow;
        }

        private static void MapToDbObject(ProductQuantitiesRequestDto model, ProductQuantities oldData)
        {
            oldData.QuantityDate = DateTime.Now.Date;
            oldData.Quantity = oldData.Quantity + model.UpdatedQuantity;
        }

        private static IQueryable<ProductQuantitiesResponseDto> FilterProductQuantitiesData(ProductQuantitiesDataTableFilterDto productQuantitiesDataTableFilterDto, IQueryable<ProductQuantitiesResponseDto> productQuantitiesResponseDto)
        {
            if (productQuantitiesDataTableFilterDto != null)
            {
                if (productQuantitiesDataTableFilterDto.CategoryId > 0)
                {
                    productQuantitiesResponseDto = productQuantitiesResponseDto.Where(p => p.CategoryId == productQuantitiesDataTableFilterDto.CategoryId);
                }

                if (!string.IsNullOrEmpty(productQuantitiesDataTableFilterDto.ProductTitle))
                {
                    productQuantitiesResponseDto = productQuantitiesResponseDto.Where(p => p.ProductTitle.StartsWith(productQuantitiesDataTableFilterDto.ProductTitle));
                }
                if (productQuantitiesDataTableFilterDto.QuantityDate != DateTime.MinValue)
                {
                    productQuantitiesResponseDto = productQuantitiesResponseDto.Where(p => p.QuantityDate == productQuantitiesDataTableFilterDto.QuantityDate);
                }
            }
            return productQuantitiesResponseDto;
        }

        private async Task GetProductQtyActivityLog(long Id, ProductQuantitiesRequestDto? productQuantity, CancellationToken cancellationToken)
        {
            var subjectTypeIdQty = await _unitOfWorkDA.SubjectTypesDA.GetProductQuantitySubjectTypeId(cancellationToken);
            var data = await _unitOfWorkDA.ActivityLogsDA.GetAll(cancellationToken);
            data = data.Where(p => p.SubjectTypeId == subjectTypeIdQty && p.SubjectId == Id);
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
                                }).OrderByDescending(p => p.CreatedDate).ToList();

            productQuantity.activityLogs = dataResponse;
        }

        #endregion Private Methods
    }
}