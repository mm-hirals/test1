using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.Core.Constants;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.MegaSearch;
using MidCapERP.Dto.Order;
using MidCapERP.Dto.OrderSet;
using MidCapERP.Dto.OrderSetItem;
using MidCapERP.Dto.Paging;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class OrderBL : IOrderBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        public readonly CurrentUser _currentUser;

        public OrderBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = _unitOfWorkDA.OrderDA.GetAll(cancellationToken);
            return _mapper.Map<List<OrderResponseDto>>(data);
        }

        public async Task<JsonRepsonse<OrderResponseDto>> GetFilterOrderData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var orderAllData = await _unitOfWorkDA.OrderDA.GetAll(cancellationToken);
            var customerData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            var userData = await _unitOfWorkDA.UserDA.GetUsers(cancellationToken);
            var orderResponseData = (from x in orderAllData
                                     join y in customerData on x.CustomerID equals y.CustomerId
                                     join z in userData on x.CreatedBy equals z.UserId
                                     select new OrderResponseDto()
                                     {
                                         OrderId = x.OrderId,
                                         OrderNo = x.OrderNo,
                                         CreatedDateFormat = x.CreatedDate.ToString("dd/MM/yyyy hh:mm"),
                                         CustomerName = y.FirstName,
                                         Status = x.Status,
                                         GrossTotal = x.GrossTotal,
                                         Discount = x.Discount,
                                         TotalAmount = x.TotalAmount,
                                         GSTNo = x.GSTNo,
                                         CreatedByName = z.FullName
                                     }).AsQueryable();
            var orderData = new PagedList<OrderResponseDto>(orderResponseData, dataTableFilterDto);
            return new JsonRepsonse<OrderResponseDto>(dataTableFilterDto.Draw, orderData.TotalCount, orderData.TotalCount, orderData);
        }

        public async Task<OrderResponseDto> GetOrderDetailData(long Id, CancellationToken cancellationToken)
        {
            try
            {
                OrderResponseDto orderResponseDto = new OrderResponseDto();
                // Get Order data by OrderId
                var orderById = await _unitOfWorkDA.OrderDA.GetById(Id, cancellationToken);
                orderResponseDto = _mapper.Map<OrderResponseDto>(orderById);

                // Get Order Sets Data
                var orderSetAllData = await _unitOfWorkDA.OrderDA.GetAllOrderSet(cancellationToken);
                var orderSetDataByOrderId = orderSetAllData.Where(x => x.OrderId == Id).ToList();
                orderResponseDto.OrderSetResponseDto = _mapper.Map<List<OrderSetResponseDto>>(orderSetDataByOrderId);

                // Get Order Set Items Data
                var orderSetItemAllData = await _unitOfWorkDA.OrderDA.GetAllOrderSetItem(cancellationToken);
                foreach (var item in orderResponseDto.OrderSetResponseDto)
                {
                    var orderSetItemDataById = orderSetItemAllData.Where(x => x.OrderId == Id && x.OrderSetId == item.OrderSetId).ToList();
                    var productData = await _unitOfWorkDA.ProductDA.GetAll(cancellationToken);

                    var orderSetItemsData = (from x in orderSetItemDataById
                                             join y in productData on x.SubjectId equals y.ProductId
                                             select new OrderSetItemResponseDto
                                             {
                                                 OrderSetItemId = x.OrderSetItemId,
                                                 OrderId = x.OrderId,
                                                 OrderSetId = x.OrderSetId,
                                                 SubjectTypeId = x.SubjectTypeId,
                                                 SubjectId = x.SubjectId,
                                                 ProductImage = x.ProductImage,
                                                 Width = x.Width,
                                                 Height = x.Height,
                                                 Depth = x.Depth,
                                                 Quantity = x.Quantity,
                                                 ProductTitle = y.ProductTitle,
                                                 ModelNo = y.ModelNo,
                                                 UnitPrice = x.UnitPrice,
                                                 DiscountPrice = x.DiscountPrice,
                                                 TotalAmount = x.TotalAmount,
                                                 Comment = x.Comment,
                                                 Status = x.Status
                                             }).ToList();

                    item.OrderSetItemResponseDto = orderSetItemsData;
                }
                return orderResponseDto;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<OrderApiRequestDto> CreateOrder(OrderApiRequestDto model, CancellationToken cancellationToken)
        {
            var orderToInsert = _mapper.Map<Order>(model);
            orderToInsert.Status = (byte)ProductStatusEnum.Pending;
            orderToInsert.CreatedBy = _currentUser.UserId;
            orderToInsert.CreatedDate = DateTime.Now;
            orderToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.OrderDA.CreateOrder(orderToInsert, cancellationToken);

            //Create Orderset Base On Order
            foreach (var setData in model.OrderSetRequestDto)
            {
                OrderSetRequestDto orderSet = new OrderSetRequestDto();
                orderSet.OrderId = data.OrderId;
                orderSet.SetName = setData.SetName;
                orderSet.TotalAmount = model.TotalAmount;
                orderSet.IsDeleted = false;
                orderSet.CreatedBy = _currentUser.UserId;
                orderSet.CreatedDate = DateTime.Now;
                orderSet.CreatedUTCDate = DateTime.UtcNow;
                var orderSetToInsert = _mapper.Map<OrderSet>(orderSet);
                var orderSetData = await _unitOfWorkDA.OrderSetDA.CreateOrderSet(orderSetToInsert, cancellationToken);

                foreach (var itemData in setData.OrderSetItemRequestDto)
                {
                    var subjectTypes = await _unitOfWorkDA.SubjectTypesDA.GetAll(cancellationToken);

                    //Create OrderSetItem Base On OrderSet
                    OrderSetItemRequestDto orderSetItem = new OrderSetItemRequestDto();
                    orderSetItem.OrderId = data.OrderId;
                    orderSetItem.OrderSetId = orderSetData.OrderSetId;
                    orderSetItem.SubjectTypeId = itemData.SubjectTypeId;
                    orderSetItem.SubjectId = itemData.SubjectId;

                    //var subjectTypeName = (from x in subjectTypes where x.TenantId == data.TenantId && x.SubjectTypeId == itemData.SubjectTypeId select x.SubjectTypeName);



                    //var productImage = (from x in subjectTypeName where x.)
                    //orderSetItem.ProductImage = itemData.ProductImage;
                    //orderSetItem.Width = itemData.Width;
                    //orderSetItem.Height = itemData.Height;
                    //orderSetItem.Depth = itemData.Depth;
                    //orderSetItem.Quantity = itemData.Quantity;
                    //orderSetItem.UnitPrice = itemData.UnitPrice;
                    //orderSetItem.DiscountPrice = itemData.DiscountPrice;
                    //orderSetItem.TotalAmount = itemData.TotalAmount;
                    //orderSetItem.Comment = itemData.Comment;
                    //orderSetItem.Status = (byte)ProductStatusEnum.Pending;
                    //orderSetItem.IsDeleted = false;
                    //orderSetItem.CreatedBy = _currentUser.UserId;
                    //orderSetItem.CreatedDate = DateTime.Now;
                    //orderSetItem.CreatedUTCDate = DateTime.UtcNow;
                    //var orderSetItemToInsert = _mapper.Map<OrderSetItem>(orderSetItem);
                    //await _unitOfWorkDA.OrderSetItemDA.CreateOrderSetItem(orderSetItemToInsert, cancellationToken);
                }
            }
            return _mapper.Map<OrderApiRequestDto>(data);
        }

        public async Task<IEnumerable<MegaSearchResponse>> GetOrderForDropDownByOrderNo(string orderNo, CancellationToken cancellationToken)
        {
            var orderAllData = await _unitOfWorkDA.OrderDA.GetAll(cancellationToken);
            return orderAllData.Where(x => x.OrderNo.StartsWith(orderNo)).Select(x => new MegaSearchResponse(x.OrderId, x.OrderNo, null, null, "Order")).Take(10).ToList();
        }

        public async Task<OrderResponseDto> GetOrderForDetailsByOrderNo(string searchText, CancellationToken cancellationToken)
        {
            var orderAlldata = await _unitOfWorkDA.OrderDA.GetAll(cancellationToken);
            var data = orderAlldata.Where(x => x.OrderNo == searchText);
            return _mapper.Map<OrderResponseDto>(data);
        }

        public async Task<OrderApiResponseDto> GetOrderAll(int Id, CancellationToken cancellationToken)
        {
            try
            {
                OrderApiResponseDto orderApiResponseDto = new OrderApiResponseDto();
                // Get Order data by OrderId
                var orderById = await _unitOfWorkDA.OrderDA.GetById(Id, cancellationToken);
                orderApiResponseDto = _mapper.Map<OrderApiResponseDto>(orderById);

                // Get Order Sets Data
                var orderSetAllData = await _unitOfWorkDA.OrderDA.GetAllOrderSet(cancellationToken);
                var orderSetDataByOrderId = orderSetAllData.Where(x => x.OrderId == Id).ToList();
                orderApiResponseDto.OrderSetApiResponseDto = _mapper.Map<List<OrderSetApiResponseDto>>(orderSetDataByOrderId);

                // Get Order Set Items Data
                var orderSetItemAllData = await _unitOfWorkDA.OrderDA.GetAllOrderSetItem(cancellationToken);
                foreach (var item in orderApiResponseDto.OrderSetApiResponseDto)
                {
                    var orderSetItemDataById = orderSetItemAllData.Where(x => x.OrderId == Id && x.OrderSetId == item.OrderSetId).ToList();
                    var productData = await _unitOfWorkDA.ProductDA.GetAll(cancellationToken);
                    var polishData = await _unitOfWorkDA.PolishDA.GetAll(cancellationToken);
                    var fabricData = await _unitOfWorkDA.FabricDA.GetAll(cancellationToken);

                    var orderSetItemsData = (from x in orderSetItemDataById
                                             join y in productData on x.SubjectId equals y.ProductId into productM

                                             from productMat in productM.DefaultIfEmpty()
                                             join z in polishData on x.SubjectId equals z.PolishId into polishM

                                             from polishMat in polishM.DefaultIfEmpty()
                                             join a in fabricData on x.SubjectId equals a.FabricId into fabricM

                                             from fabricMat in fabricM.DefaultIfEmpty()
                                             select new OrderSetItemApiResponseDto
                                             {
                                                 OrderSetItemId = x.OrderSetItemId,
                                                 OrderId = x.OrderId,
                                                 OrderSetId = x.OrderSetId,
                                                 SubjectTypeId = x.SubjectTypeId,
                                                 SubjectId = x.SubjectId,
                                                 ProductImage = x.ProductImage,
                                                 Width = x.Width,
                                                 Height = x.Height,
                                                 Depth = x.Depth,
                                                 Quantity = x.Quantity,
                                                 ProductTitle = (x.SubjectTypeId == 1 ? productMat.ProductTitle : (x.SubjectTypeId == 3 ? polishMat.Title : fabricMat.Title)),
                                                 ModelNo = (x.SubjectTypeId == 1 ? productMat.ModelNo : (x.SubjectTypeId == 3 ? polishMat.ModelNo : fabricMat.ModelNo)),
                                                 UnitPrice = x.UnitPrice,
                                                 DiscountPrice = x.DiscountPrice,
                                                 TotalAmount = x.TotalAmount,
                                                 Comment = x.Comment,
                                                 Status = x.Status
                                             }).ToList();

                    item.OrderSetItemResponseDto = orderSetItemsData;
                }
                return orderApiResponseDto;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}