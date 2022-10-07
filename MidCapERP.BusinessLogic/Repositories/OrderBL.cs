using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.Core.Constants;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Constants;
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
                                         CreatedDate = x.CreatedDate,
                                         CustomerName = y.FirstName + " " + y.LastName,
                                         Status = x.Status,
                                         GrossTotal = x.GrossTotal,
                                         Discount = x.Discount,
                                         TotalAmount = x.TotalAmount,
                                         GSTTaxAmount = x.GSTTaxAmount,
                                         PayableAmount = (x.GrossTotal - x.Discount) + x.GSTTaxAmount,
                                         DeliveryDate = x.DeliveryDate,
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
                orderResponseDto.PayableAmount = (orderResponseDto.GrossTotal - orderResponseDto.Discount) + orderResponseDto.GSTTaxAmount;

                // Get Order Addresses
                var orderAddressesData = await _unitOfWorkDA.OrderAddressDA.GetOrderAddressesByOrderId(Id, cancellationToken);
                orderResponseDto.OrderAddressesResponseDto = _mapper.Map<List<OrderAddressesResponseDto>>(orderAddressesData.ToList());

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
            //Cost Calculation
            Random generator = new Random();
            model.OrderNo = await _unitOfWorkDA.OrderDA.CreateOrderNo("R", cancellationToken); ;
            //model.OrderNo = Convert.ToString(DateTime.Now.Year) + "-" + "R" + generator.Next(1, 99999).ToString("D5");
            model.GrossTotal = model.OrderSetRequestDto.Sum(x => x.OrderSetItemRequestDto.Sum(x => x.UnitPrice * x.Quantity));
            model.Discount = model.OrderSetRequestDto.Sum(x => x.OrderSetItemRequestDto.Sum(x => x.DiscountPrice));
            model.TotalAmount = model.GrossTotal - model.Discount;
            model.GSTTaxAmount = Math.Round(Math.Round((model.TotalAmount * 18) / 100), 2);

            var saveOrder = await SaveOrder(model, cancellationToken);
            OrderAddressesApiRequestDto orderAddressesApiRequestDto = new OrderAddressesApiRequestDto();
            orderAddressesApiRequestDto.OrderId = saveOrder.OrderId;
            //Create OrderAddress Base On Address
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
            var oldData = await OrderGetById(Id, cancellationToken);
            oldData.GrossTotal = model.OrderSetRequestDto.Sum(x => x.OrderSetItemRequestDto.Sum(x => x.UnitPrice * x.Quantity));
            oldData.Discount = model.OrderSetRequestDto.Sum(x => x.OrderSetItemRequestDto.Sum(x => x.DiscountPrice));
            oldData.TotalAmount = model.GrossTotal - model.Discount;
            oldData.GSTTaxAmount = Math.Round(Math.Round((model.TotalAmount * 18) / 100), 2);
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.OrderDA.UpdateOrder(oldData, cancellationToken);
            //Start Update OrderSet
            foreach (var orderSet in model.OrderSetRequestDto)
            {
                var oldOrderSet = await OrderSetGetById(orderSet.OrderSetId, cancellationToken);
                oldOrderSet.SetName = orderSet.SetName;
                oldOrderSet.UpdatedBy = _currentUser.UserId;
                oldOrderSet.UpdatedDate = DateTime.Now;
                oldOrderSet.UpdatedUTCDate = DateTime.UtcNow;
                await _unitOfWorkDA.OrderSetDA.UpdateOrderSet(oldOrderSet, cancellationToken);
                //Start Update OrderSetItem
                foreach (var orderSetItem in orderSet.OrderSetItemRequestDto)
                {
                    var oldOrderSetItem = await OrderSetItemGetById(orderSetItem.OrderSetItemId, cancellationToken);
                    if (oldOrderSetItem != null)
                    {
                        oldOrderSetItem.Height = orderSetItem.Height;
                        oldOrderSetItem.Width = orderSetItem.Width;
                        oldOrderSetItem.Depth = orderSetItem.Depth;
                        oldOrderSetItem.Quantity = orderSetItem.Quantity;
                        oldOrderSetItem.UnitPrice = orderSetItem.UnitPrice;
                        oldOrderSetItem.DiscountPrice = orderSetItem.DiscountPrice;
                        oldOrderSetItem.TotalAmount = (orderSetItem.UnitPrice * orderSetItem.Quantity) - orderSetItem.DiscountPrice;
                        oldOrderSetItem.Comment = orderSetItem.Comment;
                        oldOrderSetItem.UpdatedBy = _currentUser.UserId;
                        oldOrderSetItem.UpdatedDate = DateTime.Now;
                        oldOrderSetItem.UpdatedUTCDate = DateTime.UtcNow;
                        await _unitOfWorkDA.OrderSetItemDA.UpdateOrderSetItem(oldOrderSetItem, cancellationToken);
                    }
                }
            }
            return _mapper.Map<OrderApiResponseDto>(data);
            //End Update OrderSet
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

        private async Task<OrderSetItemRequestDto> SaveOrderSetItem(OrderSetItemRequestDto orderSetItemRequestDto, CancellationToken cancellationToken)
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
            orderSetItem.TotalAmount = (orderSetItemRequestDto.UnitPrice * orderSetItemRequestDto.Quantity) - orderSetItemRequestDto.DiscountPrice;
            orderSetItem.Comment = orderSetItemRequestDto.Comment;
            orderSetItem.MakingStatus = (int)ProductStatusEnum.Pending;
            orderSetItem.CreatedBy = _currentUser.UserId;
            orderSetItem.CreatedDate = DateTime.Now;
            orderSetItem.CreatedUTCDate = DateTime.UtcNow;
            var orderSetItemToInsert = _mapper.Map<OrderSetItem>(orderSetItem);
            var data = await _unitOfWorkDA.OrderSetItemDA.CreateOrderSetItem(orderSetItemToInsert, cancellationToken);

            return _mapper.Map<OrderSetItemRequestDto>(data);
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
            if (data == null)
            {
                throw new Exception("Order Set not found");
            }
            return data;
        }

        private async Task<OrderSetItem> OrderSetItemGetById(Int64 Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.OrderSetItemDA.GetById(Id, cancellationToken);
            if (data == null)
            {
                throw new Exception("Order Set Item not found");
            }
            return data;
        }

        #endregion Private Method
    }
}