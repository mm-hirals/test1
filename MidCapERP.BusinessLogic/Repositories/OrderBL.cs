using AutoMapper;
using MidCapERP.BusinessLogic.Constants;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Services.FileStorage;
using MidCapERP.Core.Constants;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Constants;
using MidCapERP.Dto.Customers;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.MegaSearch;
using MidCapERP.Dto.Order;
using MidCapERP.Dto.OrderAddressesApi;
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
        private readonly IFileStorageService _fileStorageService;

        public OrderBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser, IFileStorageService fileStorageService)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
            _fileStorageService = fileStorageService;
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = _unitOfWorkDA.OrderDA.GetAll(cancellationToken);
            return _mapper.Map<List<OrderResponseDto>>(data);
        }

        public async Task<OrderResponseDto> GetById(Int64 Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.OrderDA.GetById(Id, cancellationToken);
            return _mapper.Map<OrderResponseDto>(data);
        }

        public async Task<JsonRepsonse<OrderResponseDto>> GetFilterOrderData(OrderDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
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
                                         CreatedDate = x.CreatedDate,
                                         CustomerName = y.FirstName + " " + y.LastName,
                                         Status = x.Status,
                                         GrossTotal = x.GrossTotal,
                                         Discount = x.Discount,
                                         TotalAmount = x.TotalAmount,
                                         GSTTaxAmount = x.GSTTaxAmount,
                                         PayableAmount = (x.GrossTotal - x.Discount) + x.GSTTaxAmount,
                                         DeliveryDate = x.DeliveryDate,
                                         CreatedBy = x.CreatedBy,
                                         CreatedByName = z.FullName,
                                         RefferedBy = x.RefferedBy,
                                         PhoneNumber = y.PhoneNumber
                                     }).AsQueryable();
            var orderFilterData = FilterOrderData(dataTableFilterDto, orderResponseData);
            var orderData = new PagedList<OrderResponseDto>(orderFilterData, dataTableFilterDto);
            return new JsonRepsonse<OrderResponseDto>(dataTableFilterDto.Draw, orderData.TotalCount, orderData.TotalCount, orderData);
        }

        public async Task<OrderResponseDto> GetOrderDetailData(long Id, CancellationToken cancellationToken)
        {
            try
            {
                OrderResponseDto orderResponseDto = new OrderResponseDto();
                orderResponseDto = await GetOrderById(Id, orderResponseDto, cancellationToken);

                // Get User name
                var userData = await _unitOfWorkDA.UserDA.GetUsers(cancellationToken);
                var userFullname = userData.FirstOrDefault(x => x.UserId == orderResponseDto.CreatedBy);
                orderResponseDto.CreatedByName = userFullname.FullName;

                // Get Order Addresses
                await GetOrderAddress(Id, orderResponseDto, cancellationToken);

                // Get customer data
                await GetCustomerData(orderResponseDto, cancellationToken);
                return orderResponseDto;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<OrderResponseDto> GetOrderSetDetailData(long Id, CancellationToken cancellationToken)
        {
            try
            {
                OrderResponseDto orderResponseDto = new OrderResponseDto();
                // Get Order data by OrderId
                orderResponseDto = await GetOrderById(Id, orderResponseDto, cancellationToken);

                // Get Order Sets Data
                await GetOrderSetData(Id, orderResponseDto, cancellationToken);

                // Get Order Set Items Data
                await GetOrderSetItemData(Id, orderResponseDto, cancellationToken);

                return orderResponseDto;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<IEnumerable<OrderStatusApiResponseDto>> GetOrderForDetailsByStatus(string status, CancellationToken cancellationToken)
        {
            List<OrderStatusApiResponseDto> orderStatusApiResponseDto = new List<OrderStatusApiResponseDto>();
            var orderAllData = await _unitOfWorkDA.OrderDA.GetAll(cancellationToken);
            var orderEnum = Enum.GetValues(typeof(OrderStatusEnum)).Cast<OrderStatusEnum>().ToList();
            int statusValue = 0;

            if (status == Enum.GetName(typeof(OrderStatusEnum), OrderStatusEnum.MaterialReceive))
            {
                var orderData = await _unitOfWorkDA.OrderDA.GetAll(cancellationToken);
                var orderSetData = await _unitOfWorkDA.OrderSetDA.GetAll(cancellationToken);
                var orderSetItemData = await _unitOfWorkDA.OrderSetItemDA.GetAll(cancellationToken);
                var customerData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
                var orderSetItemReceivableData = await _unitOfWorkDA.OrderSetItemReceivableDA.GetAll(cancellationToken);
                var orderResponseData = (from x in orderData.Where(x => x.CreatedBy == _currentUser.UserId)
                                         join y in customerData on x.CustomerID equals y.CustomerId
                                         join z in orderSetItemData.Where(x => x.ReceiveDate != null) on x.OrderId equals z.OrderId
                                         join s in orderSetData on z.OrderSetId equals s.OrderSetId
                                         join a in orderSetItemReceivableData on z.OrderSetItemId equals a.OrderSetItemId into orderSetItemReceivables
                                         from b in orderSetItemReceivables.DefaultIfEmpty()
                                         select new OrderStatusApiResponseDto()
                                         {
                                             OrderId = x.OrderId,
                                             OrderSetItemId = z.OrderSetItemId,
                                             OrderSetItem = s.SetName,
                                             OrderNo = x.OrderNo,
                                             CustomerName = y.FirstName + " " + y.LastName,
                                             TotalAmount = 0,
                                             OrderStatus = status,
                                             OrderDate = x.CreatedDate,
                                             ReceiveDate = z.ReceiveDate,
                                             ProvidedMaterial = z.ProvidedMaterial,
                                             IsOrderItemReceivable = (b == null) ? true : false
                                         }).Where(x => x.IsOrderItemReceivable).ToList();
                orderStatusApiResponseDto = orderResponseData;
            }
            else
            {
                foreach (var item in orderEnum)
                {
                    if (status == Convert.ToString(item))
                        statusValue = (int)Enum.Parse(typeof(OrderStatusEnum), Convert.ToString(item));
                }

                var enumOrderData = orderAllData.Where(x => x.CreatedBy == _currentUser.UserId && x.Status == statusValue).ToList();
                foreach (var orderData in enumOrderData)
                {
                    var customerData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
                    var orderResponseData = (from x in enumOrderData
                                             join y in customerData on x.CustomerID equals y.CustomerId
                                             select new OrderStatusApiResponseDto()
                                             {
                                                 OrderId = x.OrderId,
                                                 OrderSetItemId = 0,
                                                 OrderSetItem = "",
                                                 OrderNo = x.OrderNo,
                                                 CustomerName = y.FirstName + " " + y.LastName,
                                                 TotalAmount = (x.TotalAmount + x.GSTTaxAmount) - x.AdvanceAmount,
                                                 OrderStatus = status,
                                                 OrderDate = x.CreatedDate
                                             }).ToList();
                    orderStatusApiResponseDto = orderResponseData;
                }
            }

            return orderStatusApiResponseDto;
        }

        public async Task<IEnumerable<MegaSearchResponse>> GetOrderForDropDownByOrderNo(string orderNo, CancellationToken cancellationToken)
        {
            var orderAllData = await _unitOfWorkDA.OrderDA.GetAll(cancellationToken);
            return orderAllData.OrderByDescending(x => x.CreatedDate).Where(x => x.OrderNo.StartsWith(orderNo)).Select(x => new MegaSearchResponse(x.OrderId, x.OrderNo, null, null, "Order")).Take(10).ToList();
        }

        public async Task<OrderResponseDto> GetOrderForDetailsByOrderNo(string searchText, CancellationToken cancellationToken)
        {
            var orderAlldata = await _unitOfWorkDA.OrderDA.GetAll(cancellationToken);
            var data = orderAlldata.Where(x => x.OrderNo == searchText);
            return _mapper.Map<OrderResponseDto>(data);
        }

        public async Task<OrderApiResponseDto> GetOrderDetailByOrderIdAPI(long Id, CancellationToken cancellationToken)
        {
            try
            {
                OrderApiResponseDto orderApiResponseDto = new OrderApiResponseDto();
                // Get Order data by OrderId
                var orderById = await _unitOfWorkDA.OrderDA.GetById(Id, cancellationToken);
                if (orderById == null)
                {
                    throw new Exception("Order not found");
                }
                else if (orderById.TenantId != _currentUser.TenantId)
                {
                    throw new Exception("Order not found");
                }
                orderApiResponseDto = _mapper.Map<OrderApiResponseDto>(orderById);
                orderApiResponseDto.PayableAmount = (orderApiResponseDto.GrossTotal - orderApiResponseDto.Discount) + orderApiResponseDto.GSTTaxAmount;
                var allUsers = await _unitOfWorkDA.UserDA.GetUsers(cancellationToken);
                orderApiResponseDto.CreatedBy = allUsers.FirstOrDefault(x => x.UserId == orderById.CreatedBy).FullName;
                orderApiResponseDto.CreatedDate = orderById.CreatedDate;

                //Get Customer Address for Order
                var orderAddressData = await _unitOfWorkDA.OrderAddressDA.GetOrderAddressesByOrderId(Id, cancellationToken);
                if (orderAddressData != null && orderAddressData.Any())
                {
                    orderApiResponseDto.BillingAddressID = orderAddressData.FirstOrDefault(x => x.AddressType == "Billing").CustomerAddressId;
                    orderApiResponseDto.ShippingAddressID = orderAddressData.FirstOrDefault(x => x.AddressType == "Shipping").CustomerAddressId;
                }

                // Get Order Sets Data
                var orderSetAllData = await _unitOfWorkDA.OrderSetDA.GetAll(cancellationToken);
                var orderSetDataByOrderId = orderSetAllData.Where(x => x.OrderId == Id).ToList();
                orderApiResponseDto.OrderSetApiResponseDto = _mapper.Map<List<OrderSetApiResponseDto>>(orderSetDataByOrderId);

                // Get Order Set Items Data
                var productData = await _unitOfWorkDA.ProductDA.GetAll(cancellationToken);
                var polishData = await _unitOfWorkDA.PolishDA.GetAll(cancellationToken);
                var fabricData = await _unitOfWorkDA.FabricDA.GetAll(cancellationToken);
                var productSubjectTypeId = await _unitOfWorkDA.SubjectTypesDA.GetProductSubjectTypeId(cancellationToken);
                var polishSubjectTypeId = await _unitOfWorkDA.SubjectTypesDA.GetPolishSubjectTypeId(cancellationToken);
                var fabricSubjectTypeId = await _unitOfWorkDA.SubjectTypesDA.GetFabricSubjectTypeId(cancellationToken);
                var orderSetItemAllData = await _unitOfWorkDA.OrderSetItemDA.GetAll(cancellationToken);
                var orderSetItemReceivableData = await _unitOfWorkDA.OrderSetItemReceivableDA.GetAll(cancellationToken);

                foreach (var item in orderApiResponseDto.OrderSetApiResponseDto)
                {
                    var orderSetItemDataById = orderSetItemAllData.Where(x => x.OrderId == Id && x.OrderSetId == item.OrderSetId).ToList();

                    var orderSetItemsData = (from x in orderSetItemDataById
                                             join y in productData on x.SubjectId equals y.ProductId into productM
                                             from productMat in productM.DefaultIfEmpty()
                                             join z in polishData on x.SubjectId equals z.PolishId into polishM
                                             from polishMat in polishM.DefaultIfEmpty()
                                             join a in fabricData on x.SubjectId equals a.FabricId into fabricM
                                             join osir in orderSetItemReceivableData on x.OrderSetItemId equals osir.OrderSetItemId into orderSetItemReceivables
                                             from osirData in orderSetItemReceivables.DefaultIfEmpty()
                                             from fabricMat in fabricM.DefaultIfEmpty()
                                             select new OrderSetItemApiResponseDto
                                             {
                                                 OrderSetItemId = x.OrderSetItemId,
                                                 OrderId = x.OrderId,
                                                 OrderSetId = x.OrderSetId,
                                                 SubjectTypeId = x.SubjectTypeId,
                                                 SubjectId = x.SubjectId,
                                                 ProductImage = string.IsNullOrEmpty(x.ProductImage) ? null : "https://midcaperp.magnusminds.net/" + x.ProductImage,
                                                 Width = x.Width,
                                                 Height = x.Height,
                                                 Depth = x.Depth,
                                                 Quantity = x.Quantity,
                                                 ProductTitle = (x.SubjectTypeId == productSubjectTypeId ? productMat.ProductTitle : (x.SubjectTypeId == polishSubjectTypeId ? polishMat.Title : (x.SubjectTypeId == fabricSubjectTypeId ? fabricMat.Title : ""))),
                                                 ModelNo = (x.SubjectTypeId == productSubjectTypeId ? productMat.ModelNo : (x.SubjectTypeId == polishSubjectTypeId ? polishMat.ModelNo : (x.SubjectTypeId == fabricSubjectTypeId ? fabricMat.ModelNo : ""))),
                                                 UnitPrice = x.UnitPrice,
                                                 DiscountPrice = x.DiscountPrice,
                                                 TotalAmount = x.TotalAmount,
                                                 Comment = x.Comment,
                                                 ReceiveDate = x.ReceiveDate,
                                                 ProvidedMaterial = x.ProvidedMaterial,
                                                 Status = x.MakingStatus,
                                                 IsItemReceived = (osirData == null) ? false : true
                                             }).ToList();

                    item.OrderSetItemResponseDto = orderSetItemsData;
                    item.TotalAmount = orderSetItemsData.Sum(x => x.TotalAmount);
                }
                return orderApiResponseDto;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<OrderApiResponseDto> CreateOrderAPI(OrderApiRequestDto model, CancellationToken cancellationToken)
        {
            //Create Order and Cost Calculation
            model.OrderNo = await _unitOfWorkDA.OrderDA.CreateOrderNo("R", cancellationToken);
            model.GrossTotal = model.OrderSetRequestDto.Sum(x => x.OrderSetItemRequestDto.Sum(x => x.UnitPrice * x.Quantity));
            decimal discountAmount = 0.00m;
            foreach (var item in model.OrderSetRequestDto)
                foreach (var item2 in item.OrderSetItemRequestDto)
                    discountAmount += ((item2.UnitPrice * item2.Quantity) * item2.DiscountPrice / 100);
            model.Discount = Math.Round(Math.Round(discountAmount), 2);
            model.TotalAmount = model.GrossTotal - model.Discount;
            model.GSTTaxAmount = Math.Round(Math.Round((model.TotalAmount * 18) / 100), 2);
            var saveOrder = await SaveOrder(model, cancellationToken);

            //Create Orderset Base On Order
            foreach (var setData in model.OrderSetRequestDto)
            {
                setData.OrderId = saveOrder.OrderId;
                var saveOrderData = await SaveOrderSet(setData, cancellationToken);

                //Create OrderSetItem Base On OrderSet
                foreach (var itemData in setData.OrderSetItemRequestDto)
                {
                    itemData.OrderId = saveOrder.OrderId;
                    itemData.OrderSetId = saveOrderData.OrderSetId;
                    await SaveOrderSetItem(itemData, cancellationToken);
                }
            }

            //Create OrderAddress Base On Address
            await SaveOrderAddress(saveOrder.OrderId, model.CustomerID, model.BillingAddressID, "Billing", true, cancellationToken);
            await SaveOrderAddress(saveOrder.OrderId, model.CustomerID, model.ShippingAddressID, "Shipping", true, cancellationToken);

            var allUsers = await _unitOfWorkDA.UserDA.GetUsers(cancellationToken);
            saveOrder.CreatedBy = allUsers.FirstOrDefault(x => x.UserId == _currentUser.UserId).FullName;
            saveOrder.CreatedDate = saveOrder.CreatedDate;

            return await GetOrderDetailByOrderIdAPI(saveOrder.OrderId, cancellationToken);
        }

        public async Task<OrderApiResponseDto> UpdateOrderAPI(Int64 Id, OrderApiRequestDto model, CancellationToken cancellationToken)
        {
            //Update Order and Cost Calculation
            var oldData = await OrderGetById(Id, cancellationToken);
            oldData.GrossTotal = model.OrderSetRequestDto.Sum(x => x.OrderSetItemRequestDto.Sum(x => x.UnitPrice * x.Quantity));
            decimal discountAmount = 0.00m;
            foreach (var item in model.OrderSetRequestDto)
                foreach (var item2 in item.OrderSetItemRequestDto)
                    discountAmount += Math.Round(Math.Round(((item2.UnitPrice * item2.Quantity) * item2.DiscountPrice / 100)), 2);
            oldData.Discount = Math.Round(Math.Round(discountAmount), 2);
            oldData.TotalAmount = oldData.GrossTotal - oldData.Discount;
            oldData.GSTTaxAmount = Math.Round(Math.Round((oldData.TotalAmount * 18) / 100), 2);
            oldData.Comments = model.Comments;
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.OrderDA.UpdateOrder(oldData, cancellationToken);

            //Create or Update Orderset Base On Order
            foreach (var setData in model.OrderSetRequestDto)
            {
                setData.OrderId = oldData.OrderId;
                var saveOrderData = await SaveOrderSet(setData, cancellationToken);

                //Create or Update Base On OrderSet
                foreach (var itemData in setData.OrderSetItemRequestDto)
                {
                    itemData.OrderId = oldData.OrderId;
                    itemData.OrderSetId = saveOrderData.OrderSetId;
                    await SaveOrderSetItem(itemData, cancellationToken);
                }
            }

            //Create OrderAddress Base On Address
            if (data.Status == (int)OrderStatusEnum.Inquiry)
            {
                await SaveOrderAddress(data.OrderId, model.CustomerID, model.BillingAddressID, "Billing", true, cancellationToken);
                await SaveOrderAddress(data.OrderId, model.CustomerID, model.ShippingAddressID, "Shipping", true, cancellationToken);
            }
            else
            {
                await SaveOrderAddress(data.OrderId, model.CustomerID, model.BillingAddressID, "Billing", false, cancellationToken);
                await SaveOrderAddress(data.OrderId, model.CustomerID, model.ShippingAddressID, "Shipping", false, cancellationToken);
            }

            var responseOrder = await GetOrderDetailByOrderIdAPI(data.OrderId, cancellationToken);
            responseOrder.BillingAddressID = model.BillingAddressID;
            responseOrder.ShippingAddressID = model.ShippingAddressID;
            var allUsers = await _unitOfWorkDA.UserDA.GetUsers(cancellationToken);
            responseOrder.CreatedBy = allUsers.FirstOrDefault(x => x.UserId == data.CreatedBy).FullName;
            responseOrder.CreatedDate = data.CreatedDate;
            return responseOrder;
        }

        public async Task<OrderApiResponseDto> UpdateOrderAdvanceAmountAPI(Int64 orderId, decimal advanceAmount, CancellationToken cancellationToken)
        {
            var orderData = await GetOrderDetailByOrderIdAPI(orderId, cancellationToken);
            var orderById = await _unitOfWorkDA.OrderDA.GetById(orderId, cancellationToken);
            orderById.GrossTotal = orderData.OrderSetApiResponseDto.Sum(x => x.OrderSetItemResponseDto.Sum(x => x.UnitPrice * x.Quantity));
            decimal discountAmount = 0.00m;
            foreach (var item in orderData.OrderSetApiResponseDto)
                foreach (var item2 in item.OrderSetItemResponseDto)
                    discountAmount += Math.Round(Math.Round(((item2.UnitPrice * item2.Quantity) * item2.DiscountPrice / 100)), 2);
            orderById.Discount = Math.Round(Math.Round(discountAmount), 2);
            orderById.TotalAmount = orderById.GrossTotal - orderById.Discount;
            orderById.GSTTaxAmount = Math.Round(Math.Round((orderById.TotalAmount * 18) / 100), 2);
            orderById.AdvanceAmount = advanceAmount;
            orderById.UpdatedBy = _currentUser.UserId;
            orderById.UpdatedDate = DateTime.Now;
            orderById.UpdatedUTCDate = DateTime.UtcNow;
            await _unitOfWorkDA.OrderDA.UpdateOrder(orderById, cancellationToken);

            orderData.GrossTotal = orderById.GrossTotal;
            orderData.Discount = orderById.Discount;
            orderData.TotalAmount = orderById.TotalAmount;
            orderData.GSTTaxAmount = orderById.GSTTaxAmount;
            orderData.AdvanceAmount = orderById.AdvanceAmount;
            orderData.PayableAmount = ((orderById.GrossTotal - orderById.Discount) + orderById.GSTTaxAmount) - orderById.AdvanceAmount;
            return orderData;
        }

        public async Task<OrderApiResponseDto> UpdateOrderSendForApproval(OrderUpdateStatusAPI model, CancellationToken cancellationToken)
        {
            var orderById = await _unitOfWorkDA.OrderDA.GetById(model.OrderId, cancellationToken);
            orderById.Comments = model.Comments;
            orderById.Status = (int)OrderStatusEnum.PendingForApproval;
            orderById.UpdatedBy = _currentUser.UserId;
            orderById.UpdatedDate = DateTime.Now;
            orderById.UpdatedUTCDate = DateTime.UtcNow;
            await _unitOfWorkDA.OrderDA.UpdateOrder(orderById, cancellationToken);
            await SaveOrderAddress(model.OrderId, orderById.CustomerID, model.BillingAddressID, "Billing", false, cancellationToken);
            await SaveOrderAddress(model.OrderId, orderById.CustomerID, model.ShippingAddressID, "Shipping", false, cancellationToken);
            return await GetOrderDetailByOrderIdAPI(model.OrderId, cancellationToken);
        }

        public async Task<OrderApiResponseDto> UpdateOrderApprovedOrDeclinedAPI(OrderUpdateApproveOrDeclineAPI model, CancellationToken cancellationToken)
        {
            var orderById = await _unitOfWorkDA.OrderDA.GetById(model.OrderId, cancellationToken);
            orderById.Status = Convert.ToInt32(model.IsOrderApproved == true ? OrderStatusEnum.Approved : OrderStatusEnum.Declined);
            orderById.Comments = model.Comments;
            orderById.UpdatedBy = _currentUser.UserId;
            orderById.UpdatedDate = DateTime.Now;
            orderById.UpdatedUTCDate = DateTime.UtcNow;
            await _unitOfWorkDA.OrderDA.UpdateOrder(orderById, cancellationToken);
            return await GetOrderDetailByOrderIdAPI(model.OrderId, cancellationToken);
        }

        public async Task<OrderApiResponseDto> GetOrderReceivableMaterial(Int64 orderId, Int64 orderSetItemId, CancellationToken cancellationToken)
        {
            var orderData = await GetOrderDetailByOrderIdAPI(orderId, cancellationToken);
            var orderSetItemData = await _unitOfWorkDA.OrderSetItemDA.GetById(orderSetItemId, cancellationToken);
            orderData.OrderSetApiResponseDto = orderData.OrderSetApiResponseDto.Where(x => x.OrderSetId == orderSetItemData.OrderSetId).ToList();
            orderData.OrderSetApiResponseDto.ForEach(x => x.OrderSetItemResponseDto = x.OrderSetItemResponseDto.Where(x => x.OrderSetItemId == orderSetItemId && x.ReceiveDate != null).ToList());
            return orderData;
        }

        public async Task<OrderMaterialReceiveResponseDto> GetOrderReceivedMaterial(Int64 orderId, Int64 orderSetItemId, CancellationToken cancellationToken)
        {
            var orderData = await _unitOfWorkDA.OrderDA.GetAll(cancellationToken);
            var orderSetData = await _unitOfWorkDA.OrderSetDA.GetAll(cancellationToken);
            var orderSetItemData = await _unitOfWorkDA.OrderSetItemDA.GetAll(cancellationToken);
            var orderSetItemReceivableData = await _unitOfWorkDA.OrderSetItemReceivableDA.GetAll(cancellationToken);
            var orderSetItemImageData = await _unitOfWorkDA.OrderSetItemImageDA.GetAll(cancellationToken);
            var customerAllData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            var allUsers = await _unitOfWorkDA.UserDA.GetUsers(cancellationToken);

            var orderMaterialResponseData = (from x in orderData.Where(x => x.OrderId == orderId)
                                             join z in orderSetItemData on x.OrderId equals orderId
                                             join s in orderSetData on z.OrderSetId equals s.OrderSetId
                                             join y in customerAllData on x.CustomerID equals y.CustomerId into customerData
                                             from c in customerData.DefaultIfEmpty()
                                             join a in orderSetItemReceivableData on z.OrderSetItemId equals orderSetItemId
                                             join a1 in orderSetItemReceivableData.Where(d => d.OrderSetItemId == orderSetItemId) on a.OrderSetItemId equals orderSetItemId
                                             join u in allUsers on a.ReceivedBy equals u.UserId into orderSetItemReceivables
                                             from b in orderSetItemReceivables.DefaultIfEmpty()
                                             select new OrderMaterialReceiveResponseDto()
                                             {
                                                 OrderId = z.OrderId,
                                                 OrderNo = x.OrderNo,
                                                 CustomerName = c.FirstName + " " + c.LastName,
                                                 MobileNo = c.PhoneNumber,
                                                 OrderSetItemId = z.OrderSetItemId,
                                                 OrderSetName = s.SetName,
                                                 OrderSetComment = z.Comment,
                                                 ReceivedFrom = a.ReceivedFrom,
                                                 ProvidedMaterial = a.ProvidedMaterial,
                                                 ReceiveDate = (DateTime)z.ReceiveDate,
                                                 ReceivedBy = b.FullName,
                                                 ReceivedComment = a.Comment
                                             }).FirstOrDefault();

            if (orderMaterialResponseData != null)
            {
                var orderItemImages = orderSetItemImageData.Where(x => x.OrderSetItemId == orderSetItemId).ToList();
                List<OrderSetItemImageResponseDto> OrderSetItemImageResponseDto = new List<OrderSetItemImageResponseDto>();
                foreach (var item in orderItemImages)
                {
                    OrderSetItemImageResponseDto objOrderSetItemImageResponseDto = new OrderSetItemImageResponseDto();
                    objOrderSetItemImageResponseDto.OrderSetItemId = orderSetItemId;
                    objOrderSetItemImageResponseDto.DrawImage = "https://midcaperpapi.magnusminds.net/" + item.DrawImage;
                    objOrderSetItemImageResponseDto.OrderSetItemImageId = item.OrderSetItemImageId;
                    objOrderSetItemImageResponseDto.CreatedBy = item.CreatedBy;
                    objOrderSetItemImageResponseDto.CreatedDate = item.CreatedDate;
                    objOrderSetItemImageResponseDto.CreatedUTCDate = item.CreatedUTCDate;
                    OrderSetItemImageResponseDto.Add(objOrderSetItemImageResponseDto);
                }
                orderMaterialResponseData.OrderSetItemImageResponseDto = OrderSetItemImageResponseDto;
            }

            return orderMaterialResponseData;
        }

        public async Task<OrderApiResponseDto> UpdateOrderReceiveMaterial(OrderUpdateReceiveMaterialAPI model, CancellationToken cancellationToken)
        {
            if (model.MaterialImage == null && !model.MaterialImage.Any())
                throw new Exception("Please upload an received material image.");

            OrderSetItemReceivable orderSetItemReceivable = new OrderSetItemReceivable();
            orderSetItemReceivable.OrderSetItemId = model.OrderSetItemId;
            orderSetItemReceivable.ReceivedDate = DateTime.Now;
            orderSetItemReceivable.ProvidedMaterial = model.ReceivedMaterial;
            orderSetItemReceivable.ReceivedFrom = model.ReceivedFrom;
            orderSetItemReceivable.ReceivedBy = _currentUser.UserId;
            orderSetItemReceivable.Comment = model.Comment;
            orderSetItemReceivable.CreatedBy = _currentUser.UserId;
            orderSetItemReceivable.CreatedDate = DateTime.Now;
            orderSetItemReceivable.CreatedUTCDate = DateTime.UtcNow;
            await _unitOfWorkDA.OrderSetItemReceivableDA.CreateOrderSetItemReceivable(orderSetItemReceivable, cancellationToken);

            foreach (var item in model.MaterialImage)
            {
                OrderSetItemImage orderSetItemImage = new OrderSetItemImage();
                orderSetItemImage.OrderSetItemId = model.OrderSetItemId;
                orderSetItemImage.DrawImage = await _fileStorageService.StoreFile(item, ApplicationFileStorageConstants.FilePaths.OrderSetItemReceive);
                orderSetItemImage.CreatedBy = _currentUser.UserId;
                orderSetItemImage.CreatedDate = DateTime.Now;
                orderSetItemImage.CreatedUTCDate = DateTime.UtcNow;
                await _unitOfWorkDA.OrderSetItemImageDA.CreateOrderSetItemImage(orderSetItemImage, cancellationToken);
            }

            return await GetOrderDetailByOrderIdAPI(model.OrderId, cancellationToken);
        }

        public async Task DeleteOrderAPI(OrderDeleteApiRequestDto orderDeleteApiRequestDto, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWorkDA.BeginTransactionAsync();
                if (orderDeleteApiRequestDto.DeleteType == (int)OrderDeleteTypeEnum.Order)
                {
                    var data = await _unitOfWorkDA.OrderSetDA.GetAll(cancellationToken);
                    var orderSetList = data.Where(p => p.OrderId == orderDeleteApiRequestDto.OrderId).Select(p => p.OrderSetId).ToList();

                    foreach (Int64 orderSetId in orderSetList)
                    {
                        var orderSetItem = await _unitOfWorkDA.OrderSetItemDA.GetAll(cancellationToken);
                        var orderSetItemList = orderSetItem.Where(p => p.OrderSetId == orderSetId).Select(p => p.OrderSetItemId).ToList();

                        foreach (Int64 orderItemId in orderSetItemList)
                        {
                            await DeleteOrderSetItem(orderItemId, cancellationToken);
                        }
                        await DeleteOrderSet(orderSetId, cancellationToken);
                    }
                    await DeleteOrderAddress(orderDeleteApiRequestDto.OrderId, cancellationToken);
                    await DeleteOrder(orderDeleteApiRequestDto.OrderId, cancellationToken);
                }
                else if (orderDeleteApiRequestDto.DeleteType == (int)OrderDeleteTypeEnum.OrderSet)
                {
                    var orderSetItem = await _unitOfWorkDA.OrderSetItemDA.GetAll(cancellationToken);
                    var orderSetItemList = orderSetItem.Where(p => p.OrderSetId == orderDeleteApiRequestDto.OrderSetId).Select(p => p.OrderSetItemId).ToList();

                    foreach (Int64 orderItemId in orderSetItemList)
                    {
                        await DeleteOrderSetItem(orderItemId, cancellationToken);
                    }
                    await DeleteOrderSet(orderDeleteApiRequestDto.OrderSetId, cancellationToken);
                    await UpdateOrderPriceCalculation(orderDeleteApiRequestDto.OrderId, cancellationToken);
                }
                else if (orderDeleteApiRequestDto.DeleteType == (int)OrderDeleteTypeEnum.OrderSetItem)
                {
                    await DeleteOrderSetItem(orderDeleteApiRequestDto.OrderSetItemId, cancellationToken);
                    await UpdateOrderPriceCalculation(orderDeleteApiRequestDto.OrderId, cancellationToken);
                }
                else
                {
                    throw new Exception("No Data Deleted");
                }
                await _unitOfWorkDA.CommitTransactionAsync();
            }
            catch (Exception e)
            {
                await _unitOfWorkDA.rollbackTransactionAsync();
                throw new Exception("No Data Deleted");
            }
        }

        public async Task<OrderApiResponseDto> UpdateOrderDiscountAmount(Int64 orderSetItemId, decimal discountPrice, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.OrderSetItemDA.GetById(orderSetItemId, cancellationToken);
            if (data == null)
            {
                throw new Exception("OrderSetItem not found");
            }
            data.DiscountPrice = discountPrice;
            data.TotalAmount = (data.UnitPrice * data.Quantity) - data.DiscountPrice;

            await _unitOfWorkDA.OrderSetItemDA.UpdateOrderSetItem(data, cancellationToken);

            var orderData = await GetOrderDetailByOrderIdAPI(data.OrderId, cancellationToken);

            var orderById = await _unitOfWorkDA.OrderDA.GetById(data.OrderId, cancellationToken);
            orderById.GrossTotal = orderData.OrderSetApiResponseDto.Sum(x => x.OrderSetItemResponseDto.Sum(x => x.UnitPrice * x.Quantity));
            orderById.Discount = orderData.OrderSetApiResponseDto.Sum(x => x.OrderSetItemResponseDto.Sum(x => x.DiscountPrice));
            orderById.TotalAmount = orderData.GrossTotal - orderById.Discount;
            orderById.GSTTaxAmount = (orderById.TotalAmount * 18) / 100;
            orderById.UpdatedBy = _currentUser.UserId;
            orderById.UpdatedDate = DateTime.Now;
            orderById.UpdatedUTCDate = DateTime.UtcNow;
            await _unitOfWorkDA.OrderDA.UpdateOrder(orderById, cancellationToken);
            return orderData;
        }

        public async Task<OrderSetItem> UpdateOrderSetItemDiscount(OrderSetItemRequestDto orderSetItemRequestDto, CancellationToken cancellationToken)
        {
            try
            {
                OrderSetItem saveDiscount = new OrderSetItem();
                var orderSetItemById = await _unitOfWorkDA.OrderSetItemDA.GetById(orderSetItemRequestDto.OrderSetItemId, cancellationToken);
                if (orderSetItemById != null)
                {
                    orderSetItemById.DiscountPrice = Math.Round(orderSetItemRequestDto.DiscountPrice, 2);
                    var totalAmount = orderSetItemById.UnitPrice * orderSetItemById.Quantity;
                    orderSetItemById.TotalAmount = Math.Round(totalAmount - (totalAmount * orderSetItemById.DiscountPrice / 100), 2);
                    saveDiscount = await _unitOfWorkDA.OrderSetItemDA.UpdateOrderSetItem(orderSetItemById, cancellationToken);
                    await UpdateOrderPriceCalculation(orderSetItemById.OrderId, cancellationToken);
                }
                return saveDiscount;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Int64> GetOrderReceivableCount(CancellationToken cancellationToken)
        {
            var orderData = await _unitOfWorkDA.OrderDA.GetAll(cancellationToken);
            var orderSetItemData = await _unitOfWorkDA.OrderSetItemDA.GetAll(cancellationToken);
            var orderSetItemReceivableData = await _unitOfWorkDA.OrderSetItemReceivableDA.GetAll(cancellationToken);
            var orderResponseData = (from x in orderData.Where(x => x.CreatedBy == _currentUser.UserId)
                                     join z in orderSetItemData.Where(x => x.ReceiveDate != null) on x.OrderId equals z.OrderId
                                     join a in orderSetItemReceivableData on z.OrderSetItemId equals a.OrderSetItemId into orderSetItemReceivables
                                     from b in orderSetItemReceivables.DefaultIfEmpty()
                                     select new OrderStatusApiResponseDto()
                                     {
                                         OrderId = x.OrderId,
                                         IsOrderItemReceivable = (b == null) ? true : false
                                     }).ToList().Count(x => x.IsOrderItemReceivable);
            return orderResponseData;
        }

        public async Task<Int64> GetOrderApprovedCount(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.OrderDA.GetAll(cancellationToken);
            return data.Count(x => x.CreatedBy == _currentUser.UserId && x.Status == (int)OrderStatusEnum.Approved);
        }

        public async Task<Int64> GetOrderPendingApprovalCount(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.OrderDA.GetAll(cancellationToken);
            return data.Count(x => x.CreatedBy == _currentUser.UserId && x.Status == (int)OrderStatusEnum.PendingForApproval);
        }

        public async Task<Int64> GetOrderFollowUpCount(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.OrderDA.GetAll(cancellationToken);
            return data.Count(x => x.CreatedBy == _currentUser.UserId && x.Status == (int)OrderStatusEnum.Inquiry);
        }

        public async Task<OrderResponseDto> GetOrderDetailsAnonymous(string orderNo, CancellationToken cancellationToken)
        {
            try
            {
                OrderResponseDto orderResponseDto = new OrderResponseDto();
                if (orderNo != null)
                {
                    var orderData = await _unitOfWorkDA.OrderDA.GetAll(cancellationToken);
                    orderResponseDto = _mapper.Map<OrderResponseDto>(orderData.Where(x => x.OrderNo == orderNo).FirstOrDefault());
                    if (orderResponseDto != null)
                    {
                        orderResponseDto.PayableAmount = ((orderResponseDto.GrossTotal - orderResponseDto.Discount) + orderResponseDto.GSTTaxAmount) - orderResponseDto.AdvanceAmount;

                        // Get Order Addresses
                        await GetOrderAddress(orderResponseDto.OrderId, orderResponseDto, cancellationToken);

                        // Get customer data
                        await GetCustomerData(orderResponseDto, cancellationToken);

                        // Get Order Sets Data
                        await GetOrderSetData(orderResponseDto.OrderId, orderResponseDto, cancellationToken);

                        // Get Order Set Items Data
                        await GetOrderSetItemData(orderResponseDto.OrderId, orderResponseDto, cancellationToken);
                    }
                }
                return orderResponseDto;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #region Private Method

        private async Task DeleteOrder(Int64 orderId, CancellationToken cancellationToken)
        {
            var order = await _unitOfWorkDA.OrderDA.GetById(orderId, cancellationToken);
            if (order != null)
            {
                await _unitOfWorkDA.OrderDA.DeleteOrder(order, cancellationToken);
            }
            else
            {
                throw new Exception("Order not found");
            }
        }

        private async Task DeleteOrderAddress(Int64 orderId, CancellationToken cancellationToken)
        {
            var order = await _unitOfWorkDA.OrderAddressDA.GetOrderAddressesByOrderId(orderId, cancellationToken);
            if (order != null)
            {
                foreach (var item in order.ToList())
                    await _unitOfWorkDA.OrderAddressDA.DeleteOrderAddress(item.OrderAddressId, item, cancellationToken);
            }
            else
            {
                throw new Exception("Order Address not found");
            }
        }

        private async Task DeleteOrderSet(Int64 orderSetId, CancellationToken cancellationToken)
        {
            var order = await _unitOfWorkDA.OrderSetDA.GetById(orderSetId, cancellationToken);
            if (order != null)
            {
                await _unitOfWorkDA.OrderSetDA.DeleteOrderSet(order, cancellationToken);
            }
            else
            {
                throw new Exception("OrderSet not found");
            }
        }

        private async Task DeleteOrderSetItem(Int64 orderSetItemId, CancellationToken cancellationToken)
        {
            var order = await _unitOfWorkDA.OrderSetItemDA.GetById(orderSetItemId, cancellationToken);
            if (order != null)
            {
                await _unitOfWorkDA.OrderSetItemDA.DeleteOrderSetItem(order, cancellationToken);
            }
            else
            {
                throw new Exception("OrderSetItem not found");
            }
        }

        private async Task UpdateOrderPriceCalculation(Int64 orderId, CancellationToken cancellationToken)
        {
            var order = await _unitOfWorkDA.OrderDA.GetById(orderId, cancellationToken);
            if (order != null)
            {
                var orderData = await GetOrderDetailByOrderIdAPI(orderId, cancellationToken);
                if (orderData != null)
                {
                    order.GrossTotal = orderData.OrderSetApiResponseDto.Sum(x => x.OrderSetItemResponseDto.Sum(x => x.UnitPrice * x.Quantity));
                    decimal discountAmount = 0.00m;
                    foreach (var item in orderData.OrderSetApiResponseDto)
                        foreach (var item2 in item.OrderSetItemResponseDto)
                            discountAmount += Math.Round(Math.Round(((item2.UnitPrice * item2.Quantity) * item2.DiscountPrice / 100)), 2);
                    order.Discount = Math.Round(Math.Round(discountAmount), 2);
                    order.TotalAmount = order.GrossTotal - order.Discount;
                    order.GSTTaxAmount = Math.Round(Math.Round((order.TotalAmount * 18) / 100), 2);
                }
                await _unitOfWorkDA.OrderDA.UpdateOrder(order, cancellationToken);
            }
            else
            {
                throw new Exception("Order not found");
            }
        }

        private async Task<OrderApiRequestDto> SaveOrder(OrderApiRequestDto orderRequestDto, CancellationToken cancellationToken)
        {
            var orderToInsert = _mapper.Map<Order>(orderRequestDto);
            orderToInsert.TenantId = _currentUser.TenantId;
            orderToInsert.Status = (byte)ProductStatusEnum.Pending;
            orderToInsert.CreatedBy = _currentUser.UserId;
            orderToInsert.CreatedDate = DateTime.Now;
            orderToInsert.CreatedUTCDate = DateTime.Now;
            var data = await _unitOfWorkDA.OrderDA.CreateOrder(orderToInsert, cancellationToken);
            return _mapper.Map<OrderApiRequestDto>(data);
        }

        private async Task<OrderAddressesApiRequestDto> SaveOrderAddress(long orderId, long customerId, long customerAddressId, string orderAddressType, bool isCreate, CancellationToken cancellationToken)
        {
            OrderAddressesApiRequestDto objOrderAddressesApiRequestDto = new OrderAddressesApiRequestDto();

            var customerData = await _unitOfWorkDA.CustomersDA.GetById(customerId, cancellationToken);
            if (customerData == null)
            {
                throw new Exception("Customer Id Not Valid");
            }

            var customerAddresses = await _unitOfWorkDA.CustomerAddressesDA.GetAll(cancellationToken);
            if (customerAddressId == 0)
            {
                var customerAddress = customerAddresses.FirstOrDefault(x => x.CustomerId == customerId && x.IsDefault == true);
                if (customerAddress != null)
                    customerAddressId = customerAddress.CustomerAddressId;
            }

            if (customerAddressId == 0 && isCreate == true)
            {
                return objOrderAddressesApiRequestDto;
            }

            var customerAddressData = await _unitOfWorkDA.CustomerAddressesDA.GetById(customerAddressId, cancellationToken);
            if (customerAddressData == null)
            {
                throw new Exception("Customer Address not found");
            }

            var orderAddresses = await _unitOfWorkDA.OrderAddressDA.GetOrderAddressesByOrderId(orderId, cancellationToken);
            if (orderAddresses.FirstOrDefault(x => x.AddressType == orderAddressType) == null)
            {
                OrderAddressesApiRequestDto orderAddressesToInsert = new OrderAddressesApiRequestDto();
                orderAddressesToInsert.OrderId = orderId;
                orderAddressesToInsert.CustomerAddressId = customerAddressId;
                orderAddressesToInsert.FirstName = customerData.FirstName;
                orderAddressesToInsert.LastName = customerData.LastName;
                orderAddressesToInsert.EmailId = customerData.EmailId;
                orderAddressesToInsert.PhoneNumber = customerData.PhoneNumber;
                orderAddressesToInsert.AddressType = orderAddressType;
                orderAddressesToInsert.Street1 = customerAddressData.Street1;
                orderAddressesToInsert.Street2 = customerAddressData.Street2;
                orderAddressesToInsert.Landmark = customerAddressData.Landmark;
                orderAddressesToInsert.Area = customerAddressData.Area;
                orderAddressesToInsert.City = customerAddressData.City;
                orderAddressesToInsert.State = customerAddressData.State;
                orderAddressesToInsert.ZipCode = customerAddressData.ZipCode;
                orderAddressesToInsert.CreatedBy = customerData.CreatedBy;
                orderAddressesToInsert.CreatedDate = DateTime.Now;
                orderAddressesToInsert.CreatedUTCDate = DateTime.UtcNow;
                var orderAddress = _mapper.Map<OrderAddress>(orderAddressesToInsert);
                var data = await _unitOfWorkDA.OrderAddressDA.CreateOrderAddress(orderAddress, cancellationToken);
                return _mapper.Map<OrderAddressesApiRequestDto>(data);
            }
            else
            {
                var orderAddressId = orderAddresses.FirstOrDefault(x => x.AddressType == orderAddressType)?.OrderAddressId;
                if (orderAddressId != null)
                {
                    var orderAddress = await _unitOfWorkDA.OrderAddressDA.GetById(Convert.ToInt64(orderAddressId), cancellationToken);
                    if (orderAddress != null)
                    {
                        orderAddress.CustomerAddressId = customerAddressId;
                        orderAddress.FirstName = customerData.FirstName;
                        orderAddress.LastName = customerData.LastName;
                        orderAddress.EmailId = customerData.EmailId;
                        orderAddress.PhoneNumber = customerData.PhoneNumber;
                        orderAddress.Street1 = customerAddressData.Street1;
                        orderAddress.Street2 = customerAddressData.Street2;
                        orderAddress.Landmark = customerAddressData.Landmark;
                        orderAddress.Area = customerAddressData.Area;
                        orderAddress.City = customerAddressData.City;
                        orderAddress.State = customerAddressData.State;
                        orderAddress.ZipCode = customerAddressData.ZipCode;
                        orderAddress.UpdatedBy = customerData.CreatedBy;
                        orderAddress.UpdatedDate = DateTime.Now;
                        orderAddress.UpdatedUTCDate = DateTime.UtcNow;
                        var data = await _unitOfWorkDA.OrderAddressDA.UpdateOrderAddress(orderAddress.OrderAddressId, orderAddress, cancellationToken);
                        return _mapper.Map<OrderAddressesApiRequestDto>(data);
                    }
                }
            }
            return objOrderAddressesApiRequestDto;
        }

        private async Task<OrderSetRequestDto> SaveOrderSet(OrderSetRequestDto orderSetRequestDto, CancellationToken cancellationToken)
        {
            var oldOrderSet = await OrderSetGetById(orderSetRequestDto.OrderSetId, cancellationToken);
            if (oldOrderSet != null)
            {
                oldOrderSet.SetName = orderSetRequestDto.SetName;
                oldOrderSet.UpdatedBy = _currentUser.UserId;
                oldOrderSet.UpdatedDate = DateTime.Now;
                oldOrderSet.UpdatedUTCDate = DateTime.UtcNow;
                var data = await _unitOfWorkDA.OrderSetDA.UpdateOrderSet(oldOrderSet, cancellationToken);
                return _mapper.Map<OrderSetRequestDto>(data);
            }
            else
            {
                OrderSetRequestDto orderSet = new OrderSetRequestDto();
                orderSet.OrderId = orderSetRequestDto.OrderId;
                orderSet.SetName = orderSetRequestDto.SetName;
                orderSet.CreatedBy = _currentUser.UserId;
                orderSet.CreatedDate = DateTime.Now;
                orderSet.CreatedUTCDate = DateTime.UtcNow;
                var orderSetToInsert = _mapper.Map<OrderSet>(orderSet);
                var data = await _unitOfWorkDA.OrderSetDA.CreateOrderSet(orderSetToInsert, cancellationToken);
                return _mapper.Map<OrderSetRequestDto>(data);
            }
        }

        private async Task<OrderSetItemRequestDto> SaveOrderSetItem(OrderSetItemRequestDto orderSetItemRequestDto, CancellationToken cancellationToken)
        {
            var oldOrderSetItem = await OrderSetItemGetById(orderSetItemRequestDto.OrderSetItemId, cancellationToken);
            if (oldOrderSetItem != null)
            {
                oldOrderSetItem.Height = orderSetItemRequestDto.Height;
                oldOrderSetItem.Width = orderSetItemRequestDto.Width;
                oldOrderSetItem.Depth = orderSetItemRequestDto.Depth;
                oldOrderSetItem.Quantity = orderSetItemRequestDto.Quantity;
                oldOrderSetItem.UnitPrice = orderSetItemRequestDto.UnitPrice;
                oldOrderSetItem.DiscountPrice = orderSetItemRequestDto.DiscountPrice;
                var discountAmountP = Math.Round(Math.Round(((orderSetItemRequestDto.UnitPrice * orderSetItemRequestDto.Quantity) * orderSetItemRequestDto.DiscountPrice / 100)), 2);
                oldOrderSetItem.TotalAmount = (orderSetItemRequestDto.UnitPrice * orderSetItemRequestDto.Quantity) - discountAmountP;
                oldOrderSetItem.Comment = orderSetItemRequestDto.Comment;
                oldOrderSetItem.ReceiveDate = orderSetItemRequestDto.ReceiveDate;
                oldOrderSetItem.ProvidedMaterial = orderSetItemRequestDto.ProvidedMaterial;
                oldOrderSetItem.UpdatedBy = _currentUser.UserId;
                oldOrderSetItem.UpdatedDate = DateTime.Now;
                oldOrderSetItem.UpdatedUTCDate = DateTime.UtcNow;
                var data = await _unitOfWorkDA.OrderSetItemDA.UpdateOrderSetItem(oldOrderSetItem, cancellationToken);
                return _mapper.Map<OrderSetItemRequestDto>(data);
            }
            else
            {
                var ProductSubjectTypeId = await _unitOfWorkDA.SubjectTypesDA.GetProductSubjectTypeId(cancellationToken);
                var polishSubjectTypeId = await _unitOfWorkDA.SubjectTypesDA.GetPolishSubjectTypeId(cancellationToken);
                var FrabriSubjectTypeId = await _unitOfWorkDA.SubjectTypesDA.GetFabricSubjectTypeId(cancellationToken);

                OrderSetItemRequestDto orderSetItem = new OrderSetItemRequestDto();
                orderSetItem.OrderId = orderSetItemRequestDto.OrderId;
                orderSetItem.OrderSetId = orderSetItemRequestDto.OrderSetId;
                orderSetItem.SubjectTypeId = orderSetItemRequestDto.SubjectTypeId;
                orderSetItem.SubjectId = orderSetItemRequestDto.SubjectId;

                if (orderSetItemRequestDto.SubjectTypeId == ProductSubjectTypeId)
                {
                    var productData = await _unitOfWorkDA.ProductImageDA.GetAllByProductId(orderSetItem.SubjectId, cancellationToken);
                    orderSetItem.ProductImage = productData.FirstOrDefault(x => x.IsCover == true)?.ImagePath;
                }
                else if (orderSetItemRequestDto.SubjectTypeId == polishSubjectTypeId)
                {
                    var polishData = await _unitOfWorkDA.PolishDA.GetById(Convert.ToInt32(orderSetItem.SubjectId), cancellationToken);
                    orderSetItem.ProductImage = polishData?.ImagePath;
                }
                else if (orderSetItemRequestDto.SubjectTypeId == FrabriSubjectTypeId)
                {
                    var fabricData = await _unitOfWorkDA.FabricDA.GetById(Convert.ToInt32(orderSetItem.SubjectId), cancellationToken);
                    orderSetItem.ProductImage = fabricData?.ImagePath;
                }

                orderSetItem.Width = orderSetItemRequestDto.Width;
                orderSetItem.Height = orderSetItemRequestDto.Height;
                orderSetItem.Depth = orderSetItemRequestDto.Depth;
                orderSetItem.Quantity = orderSetItemRequestDto.Quantity;
                orderSetItem.UnitPrice = orderSetItemRequestDto.UnitPrice;
                orderSetItem.DiscountPrice = orderSetItemRequestDto.DiscountPrice;
                var discountAmount = Math.Round(Math.Round(((orderSetItemRequestDto.UnitPrice * orderSetItemRequestDto.Quantity) * orderSetItemRequestDto.DiscountPrice / 100)), 2);
                orderSetItem.TotalAmount = (orderSetItemRequestDto.UnitPrice * orderSetItemRequestDto.Quantity) - discountAmount;
                orderSetItem.Comment = orderSetItemRequestDto.Comment;
                orderSetItem.ReceiveDate = orderSetItemRequestDto.ReceiveDate;
                orderSetItem.ProvidedMaterial = orderSetItemRequestDto.ProvidedMaterial;
                orderSetItem.MakingStatus = (int)ProductStatusEnum.Pending;
                orderSetItem.CreatedBy = _currentUser.UserId;
                orderSetItem.CreatedDate = DateTime.Now;
                orderSetItem.CreatedUTCDate = DateTime.UtcNow;
                var orderSetItemToInsert = _mapper.Map<OrderSetItem>(orderSetItem);
                var data = await _unitOfWorkDA.OrderSetItemDA.CreateOrderSetItem(orderSetItemToInsert, cancellationToken);
                return _mapper.Map<OrderSetItemRequestDto>(data);
            }
        }

        private async Task<Order> OrderGetById(Int64 Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.OrderDA.GetById(Id, cancellationToken);
            if (data == null)
            {
                throw new Exception("Order not found");
            }
            return data;
        }

        private async Task<OrderSet> OrderSetGetById(Int64 Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.OrderSetDA.GetById(Id, cancellationToken);
            return data;
        }

        private async Task<OrderSetItem> OrderSetItemGetById(Int64 Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.OrderSetItemDA.GetById(Id, cancellationToken);
            return data;
        }

        private async Task<int> GetPolishSubjectTypeId(CancellationToken cancellationToken)
        {
            var subjectTypeAllData = await _unitOfWorkDA.SubjectTypesDA.GetAll(cancellationToken);
            var subjectTypeId = subjectTypeAllData.Where(x => x.SubjectTypeName == nameof(SubjectTypesEnum.Polish)).Select(x => x.SubjectTypeId).FirstOrDefault();
            return subjectTypeId;
        }

        private async Task<int> GetProductSubjectTypeId(CancellationToken cancellationToken)
        {
            var subjectTypeAllData = await _unitOfWorkDA.SubjectTypesDA.GetAll(cancellationToken);
            var subjectTypeId = subjectTypeAllData.Where(x => x.SubjectTypeName == nameof(SubjectTypesEnum.Products)).Select(x => x.SubjectTypeId).FirstOrDefault();
            return subjectTypeId;
        }

        private async Task<int> GetFabricSubjectTypeId(CancellationToken cancellationToken)
        {
            var subjectTypeAllData = await _unitOfWorkDA.SubjectTypesDA.GetAll(cancellationToken);
            var subjectTypeId = subjectTypeAllData.Where(x => x.SubjectTypeName == nameof(SubjectTypesEnum.Fabrics)).Select(x => x.SubjectTypeId).FirstOrDefault();
            return subjectTypeId;
        }

        private static IQueryable<OrderResponseDto> FilterOrderData(OrderDataTableFilterDto orderDataTableFilterDto, IQueryable<OrderResponseDto> orderResponseDto)
        {
            if (orderDataTableFilterDto != null)
            {
                if (orderDataTableFilterDto.RefferedBy != null)
                {
                    orderResponseDto = orderResponseDto.Where(p => p.RefferedBy == orderDataTableFilterDto.RefferedBy);
                }
                if (!string.IsNullOrEmpty(orderDataTableFilterDto.CustomerName))
                {
                    orderResponseDto = orderResponseDto.Where(p => p.CustomerName.StartsWith(orderDataTableFilterDto.CustomerName));
                }
                if (!string.IsNullOrEmpty(orderDataTableFilterDto.PhoneNumber))
                {
                    orderResponseDto = orderResponseDto.Where(p => p.PhoneNumber.StartsWith(orderDataTableFilterDto.PhoneNumber));
                }
                if (orderDataTableFilterDto.Status != null)
                {
                    orderResponseDto = orderResponseDto.Where(p => p.Status == orderDataTableFilterDto.Status);
                }
                if (orderDataTableFilterDto.orderFromDate != DateTime.MinValue)
                {
                    orderResponseDto = orderResponseDto.Where(p => p.CreatedDate > orderDataTableFilterDto.orderFromDate);
                }
                if (orderDataTableFilterDto.orderToDate != DateTime.MinValue)
                {
                    orderResponseDto = orderResponseDto.Where(p => p.CreatedDate < orderDataTableFilterDto.orderToDate);
                    // p.UpdatedDate < orderDataTableFilterDto.orderToDate
                }
                if (orderDataTableFilterDto.DeliveryFromDate != DateTime.MinValue)
                {
                    orderResponseDto = orderResponseDto.Where(p => p.DeliveryDate > orderDataTableFilterDto.DeliveryFromDate);
                }
                if (orderDataTableFilterDto.DeliveryToDate != DateTime.MinValue)
                {
                    orderResponseDto = orderResponseDto.Where(p => p.DeliveryDate < orderDataTableFilterDto.DeliveryToDate);
                }
            }
            return orderResponseDto;
        }

        private async Task<OrderResponseDto> GetOrderById(long Id, OrderResponseDto orderResponseDto, CancellationToken cancellationToken)
        {
            var orderById = await _unitOfWorkDA.OrderDA.GetById(Id, cancellationToken);
            orderResponseDto = _mapper.Map<OrderResponseDto>(orderById);
            orderResponseDto.PayableAmount = ((orderResponseDto.GrossTotal - orderResponseDto.Discount) + orderResponseDto.GSTTaxAmount) - orderResponseDto.AdvanceAmount;
            return orderResponseDto;
        }

        private async Task GetOrderAddress(long Id, OrderResponseDto orderResponseDto, CancellationToken cancellationToken)
        {
            var orderAddressesData = await _unitOfWorkDA.OrderAddressDA.GetOrderAddressesByOrderId(Id, cancellationToken);
            orderResponseDto.OrderAddressesResponseDto = _mapper.Map<List<OrderAddressesResponseDto>>(orderAddressesData.ToList());
        }

        private async Task GetCustomerData(OrderResponseDto orderResponseDto, CancellationToken cancellationToken)
        {
            var customerById = await _unitOfWorkDA.CustomersDA.GetById(orderResponseDto.CustomerID, cancellationToken);
            if (customerById != null)
            {
                CustomersResponseDto customerModel = new CustomersResponseDto();
                if (customerById.RefferedBy > 0)
                {
                    var customerAllData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
                    var referredById = customerAllData.Where(x => x.CustomerId == customerById.RefferedBy).FirstOrDefault();
                    if (referredById != null)
                    {
                        customerModel.RefferedName = referredById.FirstName + " " + referredById.LastName;
                    }
                }
                customerModel.FirstName = customerById.FirstName;
                customerModel.LastName = customerById.LastName;
                customerModel.PhoneNumber = customerById.PhoneNumber;
                customerModel.EmailId = customerById.EmailId;
                orderResponseDto.customersResponseDto = customerModel;
            }
        }

        private async Task GetOrderSetData(long Id, OrderResponseDto orderResponseDto, CancellationToken cancellationToken)
        {
            var orderSetAllData = await _unitOfWorkDA.OrderSetDA.GetAll(cancellationToken);
            var orderSetDataByOrderId = orderSetAllData.Where(x => x.OrderId == Id).ToList();
            orderResponseDto.OrderSetResponseDto = _mapper.Map<List<OrderSetResponseDto>>(orderSetDataByOrderId);
        }

        private async Task GetOrderSetItemData(long Id, OrderResponseDto orderResponseDto, CancellationToken cancellationToken)
        {
            var productData = await _unitOfWorkDA.ProductDA.GetAll(cancellationToken);
            var polishData = await _unitOfWorkDA.PolishDA.GetAll(cancellationToken);
            var fabricData = await _unitOfWorkDA.FabricDA.GetAll(cancellationToken);
            var productSubjectTypeId = await _unitOfWorkDA.SubjectTypesDA.GetProductSubjectTypeId(cancellationToken);
            var polishSubjectTypeId = await _unitOfWorkDA.SubjectTypesDA.GetPolishSubjectTypeId(cancellationToken);
            var fabricSubjectTypeId = await _unitOfWorkDA.SubjectTypesDA.GetFabricSubjectTypeId(cancellationToken);
            var orderSetItemAllData = await _unitOfWorkDA.OrderSetItemDA.GetAll(cancellationToken);
            foreach (var item in orderResponseDto.OrderSetResponseDto)
            {
                var orderSetItemDataById = orderSetItemAllData.Where(x => x.OrderId == Id && x.OrderSetId == item.OrderSetId).ToList();

                var orderSetItemsData = (from x in orderSetItemDataById

                                         join y in productData on x.SubjectId equals y.ProductId into productM
                                         from productMat in productM.DefaultIfEmpty()

                                         join z in polishData on x.SubjectId equals z.PolishId into polishM
                                         from polishMat in polishM.DefaultIfEmpty()

                                         join a in fabricData on x.SubjectId equals a.FabricId into fabricM
                                         from fabricMat in fabricM.DefaultIfEmpty()

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
                                             ProductTitle = (x.SubjectTypeId == productSubjectTypeId ? productMat.ProductTitle : (x.SubjectTypeId == polishSubjectTypeId ? polishMat.Title : (x.SubjectTypeId == fabricSubjectTypeId ? fabricMat.Title : ""))),
                                             ModelNo = (x.SubjectTypeId == productSubjectTypeId ? productMat.ModelNo : (x.SubjectTypeId == polishSubjectTypeId ? polishMat.ModelNo : (x.SubjectTypeId == fabricSubjectTypeId ? fabricMat.ModelNo : ""))),
                                             UnitPrice = x.UnitPrice,
                                             DiscountPrice = x.DiscountPrice,
                                             TotalAmount = x.TotalAmount,
                                             Comment = x.Comment,
                                             Status = x.MakingStatus
                                         }).ToList();

                item.OrderSetItemResponseDto = orderSetItemsData;
                item.TotalAmount = orderSetItemsData.Sum(x => x.TotalAmount);
            }
        }

        #endregion Private Method
    }
}