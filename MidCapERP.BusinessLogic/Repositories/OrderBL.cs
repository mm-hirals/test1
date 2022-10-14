using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
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
                // Get Order data by OrderId
                orderResponseDto = await GetOrderById(Id, orderResponseDto, cancellationToken);

                // Get Order Addresses
                var orderAddressesData = await _unitOfWorkDA.OrderAddressDA.GetOrderAddressesByOrderId(Id, cancellationToken);
                orderResponseDto.OrderAddressesResponseDto = _mapper.Map<List<OrderAddressesResponseDto>>(orderAddressesData.ToList());

                // Get customer data
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
                var orderSetAllData = await _unitOfWorkDA.OrderDA.GetAllOrderSet(cancellationToken);
                var orderSetDataByOrderId = orderSetAllData.Where(x => x.OrderId == Id).ToList();
                orderResponseDto.OrderSetResponseDto = _mapper.Map<List<OrderSetResponseDto>>(orderSetDataByOrderId);

                // Get Order Set Items Data
                var productData = await _unitOfWorkDA.ProductDA.GetAll(cancellationToken);
                var polishData = await _unitOfWorkDA.PolishDA.GetAll(cancellationToken);
                var fabricData = await _unitOfWorkDA.FabricDA.GetAll(cancellationToken);
                var productSubjectTypeId = await _unitOfWorkDA.SubjectTypesDA.GetProductSubjectTypeId(cancellationToken);
                var polishSubjectTypeId = await _unitOfWorkDA.SubjectTypesDA.GetPolishSubjectTypeId(cancellationToken);
                var fabricSubjectTypeId = await _unitOfWorkDA.SubjectTypesDA.GetFabricSubjectTypeId(cancellationToken);
                var orderSetItemAllData = await _unitOfWorkDA.OrderDA.GetAllOrderSetItem(cancellationToken);
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
            foreach (var i in orderEnum)
            {
                if (status == Convert.ToString(i))
                {
                    statusValue = (int)Enum.Parse(typeof(OrderStatusEnum), Convert.ToString(i));
                }
            }
            var enumOrderData = orderAllData.Where(p => p.Status == statusValue).ToList();
            foreach (var orderData in enumOrderData)
            {
                var customerData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
                var orderResponseData = (from x in enumOrderData
                                         join y in customerData on x.CustomerID equals y.CustomerId
                                         select new OrderStatusApiResponseDto()
                                         {
                                             OrderId = x.OrderId,
                                             OrderNo = x.OrderNo,
                                             CustomerName = y.FirstName + " " + y.LastName,
                                             TotalAmount = x.TotalAmount,
                                             OrderStatus = status,
                                             OrderDate = x.CreatedDate
                                         }).ToList();
                orderStatusApiResponseDto = orderResponseData;
            }
            return orderStatusApiResponseDto;
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
                orderApiResponseDto = _mapper.Map<OrderApiResponseDto>(orderById);
                orderApiResponseDto.PayableAmount = (orderApiResponseDto.GrossTotal - orderApiResponseDto.Discount) + orderApiResponseDto.GSTTaxAmount;

                // Get Order Sets Data
                var orderSetAllData = await _unitOfWorkDA.OrderDA.GetAllOrderSet(cancellationToken);
                var orderSetDataByOrderId = orderSetAllData.Where(x => x.OrderId == Id).ToList();
                orderApiResponseDto.OrderSetApiResponseDto = _mapper.Map<List<OrderSetApiResponseDto>>(orderSetDataByOrderId);

                // Get Order Set Items Data
                var productData = await _unitOfWorkDA.ProductDA.GetAll(cancellationToken);
                var polishData = await _unitOfWorkDA.PolishDA.GetAll(cancellationToken);
                var fabricData = await _unitOfWorkDA.FabricDA.GetAll(cancellationToken);
                var productSubjectTypeId = await _unitOfWorkDA.SubjectTypesDA.GetProductSubjectTypeId(cancellationToken);
                var polishSubjectTypeId = await _unitOfWorkDA.SubjectTypesDA.GetPolishSubjectTypeId(cancellationToken);
                var fabricSubjectTypeId = await _unitOfWorkDA.SubjectTypesDA.GetFabricSubjectTypeId(cancellationToken);
                var orderSetItemAllData = await _unitOfWorkDA.OrderDA.GetAllOrderSetItem(cancellationToken);

                foreach (var item in orderApiResponseDto.OrderSetApiResponseDto)
                {
                    var orderSetItemDataById = orderSetItemAllData.Where(x => x.OrderId == Id && x.OrderSetId == item.OrderSetId).ToList();

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
            
            //Create OrderAddress Base On Address
            OrderAddressesApiRequestDto orderAddressesApiRequestDto = new OrderAddressesApiRequestDto();
            orderAddressesApiRequestDto.OrderId = saveOrder.OrderId;
            orderAddressesApiRequestDto.AddressType = "Billing";
            await SaveOrderAddress(orderAddressesApiRequestDto, model, cancellationToken);
            orderAddressesApiRequestDto.AddressType = "Shipping";
            await SaveOrderAddress(orderAddressesApiRequestDto, model, cancellationToken);

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
            return _mapper.Map<OrderApiResponseDto>(saveOrder);
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
            return _mapper.Map<OrderApiResponseDto>(data);
        }

        public async Task<OrderApiResponseDto> UpdateOrderDiscountAmountAPI(Int64 orderSetItemId, decimal discountPrice, CancellationToken cancellationToken)
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

            orderData.GrossTotal = orderById.GrossTotal;
            orderData.Discount = orderById.Discount;
            orderData.TotalAmount = orderById.TotalAmount;
            orderData.GSTTaxAmount = orderById.GSTTaxAmount;
            orderData.PayableAmount = (orderData.GrossTotal - orderData.Discount) + orderData.GSTTaxAmount;
            orderData.UpdatedBy = orderById.UpdatedBy;
            orderData.UpdatedDate = orderById.UpdatedDate;
            orderData.UpdatedUTCDate = orderById.UpdatedUTCDate;
            return orderData;
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
                }
                else if (orderDeleteApiRequestDto.DeleteType == (int)OrderDeleteTypeEnum.OrderSetItem)
                {
                    await DeleteOrderSetItem(orderDeleteApiRequestDto.OrderSetItemId, cancellationToken);
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

        private async Task DeleteOrderSet(Int64 orderSetId, CancellationToken cancellationToken)
        {
            var order = await _unitOfWorkDA.OrderSetDA.GetById(orderSetId, cancellationToken);
            if (order != null)
            {
                await _unitOfWorkDA.OrderSetDA.DeleteOrderSet(order, cancellationToken);
                await UpdateOrderPriceCalculation(order.OrderId, cancellationToken);
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
                await UpdateOrderPriceCalculation(order.OrderId, cancellationToken);
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

        private async Task<OrderAddressesApiRequestDto> SaveOrderAddress(OrderAddressesApiRequestDto orderAddressApiRequestDto, OrderApiRequestDto orderRequestDto, CancellationToken cancellationToken)
        {
            //GetAll Customer Data
            var customerData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);

            //GetAll Customer Addresses
            var customerAddressData = await _unitOfWorkDA.CustomerAddressesDA.GetAll(cancellationToken);
            var customer = customerData.FirstOrDefault(x => x.CustomerId == orderRequestDto.CustomerID);
            if (customer == null)
            {
                throw new Exception("Customer Id Not Valid");
            }

            //Find Customer Address Base On Customer Id
            var customerAddress = customerAddressData.FirstOrDefault(x => x.CustomerId == orderRequestDto.CustomerID);
            if (customerAddress == null)
            {
                throw new Exception("Customer Address not found");
            }

            //Create Order Address Base On Customer and Customer Address
            OrderAddressesApiRequestDto orderAddressesToInsert = new OrderAddressesApiRequestDto();
            orderAddressesToInsert.OrderId = orderAddressApiRequestDto.OrderId;
            orderAddressesToInsert.FirstName = customer.FirstName;
            orderAddressesToInsert.LastName = customer.LastName;
            orderAddressesToInsert.EmailId = customer.EmailId;
            orderAddressesToInsert.PhoneNumber = customer.PhoneNumber;
            orderAddressesToInsert.AddressType = orderAddressApiRequestDto.AddressType;
            orderAddressesToInsert.Street1 = customerAddress.Street1;
            orderAddressesToInsert.Street2 = customerAddress.Street2;
            orderAddressesToInsert.Landmark = customerAddress.Landmark;
            orderAddressesToInsert.Area = customerAddress.Area;
            orderAddressesToInsert.City = customerAddress.City;
            orderAddressesToInsert.State = customerAddress.State;
            orderAddressesToInsert.ZipCode = customerAddress.ZipCode;
            orderAddressesToInsert.CreatedBy = customerAddress.CreatedBy;
            orderAddressesToInsert.CreatedDate = customerAddress.CreatedDate;
            orderAddressesToInsert.CreatedUTCDate = customerAddress.CreatedUTCDate;
            var orderAddress = _mapper.Map<OrderAddress>(orderAddressesToInsert);
            var data = await _unitOfWorkDA.OrderAddressDA.CreateOrderAddress(orderAddress, cancellationToken);
            return _mapper.Map<OrderAddressesApiRequestDto>(data);
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
            orderResponseDto.PayableAmount = (orderResponseDto.GrossTotal - orderResponseDto.Discount) + orderResponseDto.GSTTaxAmount;
            return orderResponseDto;
        }

        #endregion Private Method
    }
}