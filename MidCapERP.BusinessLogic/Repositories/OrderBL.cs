using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Services.FileStorage;
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
                    var polishData = await _unitOfWorkDA.PolishDA.GetAll(cancellationToken);
                    var fabricData = await _unitOfWorkDA.FabricDA.GetAll(cancellationToken);
                    var productSubjectTypeId = await GetProductSubjectTypeId(cancellationToken);
                    var polishSubjectTypeId = await GetPolishSubjectTypeId(cancellationToken);
                    var fabricSubjectTypeId = await GetFabricSubjectTypeId(cancellationToken);

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
            var saveOrder = await SaveOrder(model, cancellationToken);
            OrderAddressesApiRequestDto orderAddressesApiRequestDto = new OrderAddressesApiRequestDto();
            orderAddressesApiRequestDto.OrderId = saveOrder.OrderId;
            //Create OrderAddress Base On Address
            await SaveOrderAddress(orderAddressesApiRequestDto, model, cancellationToken);

            //Create Orderset Base On Order
            foreach (var setData in model.OrderSetRequestDto)
            {
                setData.OrderId = saveOrder.OrderId;
                await SaveOrderSet(setData, cancellationToken);
                //Create OrderSetItem Base On OrderSet
                foreach (var itemData in setData.OrderSetItemRequestDto)
                {
                    itemData.OrderId = saveOrder.OrderId;
                    await SaveOrderSetItem(itemData, cancellationToken);
                }
            }
            return _mapper.Map<OrderApiRequestDto>(saveOrder);
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

                if (orderById == null)
                {
                    throw new Exception("Order not found");
                }
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
                    var rawMaterialData = await _unitOfWorkDA.RawMaterialDA.GetAll(cancellationToken);

                    var orderSetItemsData = (from x in orderSetItemDataById
                                             join y in productData on x.SubjectId equals y.ProductId into productM

                                             from productMat in productM.DefaultIfEmpty()
                                             join z in polishData on x.SubjectId equals z.PolishId into polishM

                                             from rawMaterial in rawMaterialData.DefaultIfEmpty()
                                             join b in fabricData on x.SubjectId equals b.FabricId into rawMateria

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

        public async Task<OrderApiRequestDto> UpdateOrderApi(Int64 Id, OrderApiRequestDto model, CancellationToken cancellationToken)
        {
            var productData = await _unitOfWorkDA.ProductDA.GetAll(cancellationToken);
            var polishData = await _unitOfWorkDA.PolishDA.GetAll(cancellationToken);
            var fabricData = await _unitOfWorkDA.FabricDA.GetAll(cancellationToken);
            var rawMaterialData = await _unitOfWorkDA.RawMaterialDA.GetAll(cancellationToken);
            var rawMaterialSubjectTypeId = await GetRawMaterialSubjectTypeId(cancellationToken);
            var polishSubjectTypeId = await GetPolishSubjectTypeId(cancellationToken);
            var ProductSubjectTypeId = await GetProductSubjectTypeId(cancellationToken);
            var FrabriSubjectTypeId = await GetFabricSubjectTypeId(cancellationToken);

            var oldData = await OrderGetById(model.OrderId, cancellationToken);
            oldData.IsDraft = true;
            oldData.Status = model.Status;
            oldData.UpdatedBy = model.UpdatedBy;
            oldData.UpdatedDate = model.UpdatedDate;
            var data = await _unitOfWorkDA.OrderDA.UpdateOrder(Id, oldData, cancellationToken);
            //Start Update OrderSet
            foreach (var orderSet in model.OrderSetRequestDto)
            {
                if (!orderSet.IsDeleted)
                {
                    var oldOrderSet = await OrderSetGetById(Id, cancellationToken);
                    if (oldOrderSet != null)
                    {
                        oldOrderSet.SetName = orderSet.SetName;
                        oldOrderSet.UpdatedBy = orderSet.UpdatedBy;
                        oldOrderSet.UpdatedDate = orderSet.UpdatedDate;
                        await _unitOfWorkDA.OrderSetDA.UpdateOrderSet(orderSet.OrderSetId, oldOrderSet, cancellationToken);
                        //Start Update OrderSetItem
                        foreach (var orderSetItem in orderSet.OrderSetItemRequestDto)
                        {
                            if (!orderSetItem.IsDeleted)
                            {
                                var oldOrderSetItem = await OrderSetItemGetById(Id, cancellationToken);
                                if (oldOrderSetItem != null)
                                {
                                    oldOrderSetItem.SubjectTypeId = orderSetItem.SubjectTypeId;
                                    oldOrderSetItem.SubjectId = orderSetItem.SubjectId;
                                    oldOrderSetItem.Height = orderSetItem.Height;
                                    oldOrderSetItem.Width = orderSetItem.Width;
                                    oldOrderSetItem.Depth = orderSetItem.Depth;
                                    oldOrderSetItem.Quantity = orderSetItem.Quantity;
                                    oldOrderSetItem.UnitPrice = orderSetItem.UnitPrice;
                                    oldOrderSetItem.DiscountPrice = orderSetItem.DiscountPrice;
                                    oldOrderSetItem.TotalAmount = orderSetItem.TotalAmount;
                                    oldOrderSetItem.Comment = orderSetItem.Comment;
                                    oldOrderSetItem.Status = orderSetItem.Status;
                                    oldOrderSetItem.UpdatedDate = DateTime.Now;
                                    oldOrderSetItem.UpdatedBy = orderSetItem.CreatedBy;
                                    await _unitOfWorkDA.OrderSetItemDA.UpdateOrderSetItem(orderSetItem.OrderSetItemId, oldOrderSetItem, cancellationToken);
                                }
                            }
                        }
                    }
                    //End Update OrderSetItem
                }
            }
            return _mapper.Map<OrderApiRequestDto>(data);
            //End Update OrderSet
        }

        public async Task<int> GetPolishSubjectTypeId(CancellationToken cancellationToken)
        {
            var subjectTypeAllData = await _unitOfWorkDA.SubjectTypesDA.GetAll(cancellationToken);
            var subjectTypeId = subjectTypeAllData.Where(x => x.SubjectTypeName == nameof(SubjectTypesEnum.Polish)).Select(x => x.SubjectTypeId).FirstOrDefault();
            return subjectTypeId;
        }

        public async Task<int> GetProductSubjectTypeId(CancellationToken cancellationToken)
        {
            var subjectTypeAllData = await _unitOfWorkDA.SubjectTypesDA.GetAll(cancellationToken);
            var subjectTypeId = subjectTypeAllData.Where(x => x.SubjectTypeName == nameof(SubjectTypesEnum.Products)).Select(x => x.SubjectTypeId).FirstOrDefault();
            return subjectTypeId;
        }

        public async Task<int> GetFabricSubjectTypeId(CancellationToken cancellationToken)
        {
            var subjectTypeAllData = await _unitOfWorkDA.SubjectTypesDA.GetAll(cancellationToken);
            var subjectTypeId = subjectTypeAllData.Where(x => x.SubjectTypeName == nameof(SubjectTypesEnum.Fabrics)).Select(x => x.SubjectTypeId).FirstOrDefault();
            return subjectTypeId;
        }

        public async Task<int> GetRawMaterialSubjectTypeId(CancellationToken cancellationToken)
        {
            var subjectTypeAllData = await _unitOfWorkDA.SubjectTypesDA.GetAll(cancellationToken);
            var subjectTypeId = subjectTypeAllData.Where(x => x.SubjectTypeName == nameof(SubjectTypesEnum.RawMaterials)).Select(x => x.SubjectTypeId).FirstOrDefault();
            return subjectTypeId;
        }

        #region Private Method

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
            if (customerData == null)
            {
                throw new Exception("Customer not found");
            }

            //GetAll Customer Addresses
            var customerAddressData = await _unitOfWorkDA.CustomerAddressesDA.GetAll(cancellationToken);
            if (customerAddressData == null)
            {
                throw new Exception("Customer Address not found");
            }

            var customer = customerData.FirstOrDefault(x => x.CustomerId == orderRequestDto.CustomerID);
            if (customer == null)
            {
                throw new Exception("Customer Id Not Valid");
            }

            //Find Customer Address Base On Customer Id
            var customerAddress = customerAddressData.FirstOrDefault(x => x.CustomerId == orderRequestDto.CustomerID && x.AddressType == "Home");
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
            orderAddressesToInsert.AddressType = customerAddress.AddressType;
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
            orderSet.TotalAmount = orderSetRequestDto.TotalAmount;
            orderSet.IsDeleted = false;
            orderSet.CreatedBy = _currentUser.UserId;
            orderSet.CreatedDate = DateTime.Now;
            orderSet.CreatedUTCDate = DateTime.UtcNow;
            var orderSetToInsert = _mapper.Map<OrderSet>(orderSet);

            var data = await _unitOfWorkDA.OrderSetDA.CreateOrderSet(orderSetToInsert, cancellationToken);
            return _mapper.Map<OrderSetRequestDto>(data);
        }

        private async Task<OrderSetItemRequestDto> SaveOrderSetItem(OrderSetItemRequestDto orderSetItemRequestDto, CancellationToken cancellationToken)
        {
            var productData = await _unitOfWorkDA.ProductDA.GetAll(cancellationToken);
            var rawMaterialData = await _unitOfWorkDA.RawMaterialDA.GetAll(cancellationToken);
            var fabricData = await _unitOfWorkDA.FabricDA.GetAll(cancellationToken);
            var polishData = await _unitOfWorkDA.PolishDA.GetAll(cancellationToken);

            var ProductSubjectTypeId = await GetProductSubjectTypeId(cancellationToken);
            var rawMaterialSubjectTypeId = await GetRawMaterialSubjectTypeId(cancellationToken);
            var polishSubjectTypeId = await GetPolishSubjectTypeId(cancellationToken);
            var FrabriSubjectTypeId = await GetFabricSubjectTypeId(cancellationToken);

            OrderSetItemRequestDto orderSetItem = new OrderSetItemRequestDto();
            orderSetItem.OrderId = orderSetItemRequestDto.OrderId;
            orderSetItem.OrderSetId = orderSetItemRequestDto.OrderSetId;
            orderSetItem.SubjectTypeId = orderSetItemRequestDto.SubjectTypeId;
            orderSetItem.SubjectId = orderSetItemRequestDto.SubjectId;

            if (orderSetItemRequestDto.SubjectTypeId == ProductSubjectTypeId)
            {
                var image = productData.FirstOrDefault(p => p.ProductId == orderSetItem.SubjectId)?.QRImage;
                if (image == null)
                {
                    throw new Exception("Product Image not found");
                }
                orderSetItem.ProductImage = image;
            }
            else if (orderSetItemRequestDto.SubjectTypeId == rawMaterialSubjectTypeId)
            {
                var image = rawMaterialData.FirstOrDefault(p => p.RawMaterialId == orderSetItem.SubjectId)?.ImagePath;
                if (image == null)
                {
                    throw new Exception("Product Image not found");
                }
                orderSetItem.ProductImage = string.IsNullOrEmpty(image) ? String.Empty : image;
            }
            else if (orderSetItemRequestDto.SubjectTypeId == polishSubjectTypeId)
            {
                var image = polishData.FirstOrDefault(p => p.PolishId == orderSetItem.SubjectId)?.ImagePath;
                if (image == null)
                {
                    throw new Exception("Product Image not found");
                }
                orderSetItem.ProductImage = image;
            }
            else if (orderSetItemRequestDto.SubjectTypeId == FrabriSubjectTypeId)
            {
                var image = fabricData.FirstOrDefault(p => p.FabricId == orderSetItem.SubjectId)?.ImagePath;
                if (image == null)
                {
                    throw new Exception("Product Image not found");
                }
                orderSetItem.ProductImage = image;
            }
            orderSetItem.Width = orderSetItemRequestDto.Width;
            orderSetItem.Height = orderSetItemRequestDto.Height;
            orderSetItem.Depth = orderSetItemRequestDto.Depth;
            orderSetItem.Quantity = orderSetItemRequestDto.Quantity;
            orderSetItem.UnitPrice = orderSetItemRequestDto.UnitPrice;
            orderSetItem.DiscountPrice = orderSetItemRequestDto.DiscountPrice;
            orderSetItem.TotalAmount = orderSetItemRequestDto.TotalAmount;
            orderSetItem.Comment = orderSetItemRequestDto.Comment;
            orderSetItem.Status = (byte)ProductStatusEnum.Pending;
            orderSetItem.IsDeleted = false;
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

        private static void MapToDbObject(OrderApiRequestDto model, Order oldData, OrderSetRequestDto orderSet, OrderSetItemRequestDto orderSetItem)
        {
            oldData.OrderNo = model.OrderNo;
            oldData.CustomerID = model.CustomerID;
            oldData.GrossTotal = model.GrossTotal;
            oldData.Discount = model.Discount;
            oldData.ReferralDiscount = model.ReferralDiscount;
            oldData.TotalAmount = model.TotalAmount;
            oldData.GSTTaxAmount = model.GSTTaxAmount;
            oldData.PayableAmount = model.PayableAmount;
            oldData.DeliveryDate = model.DeliveryDate;
            oldData.Comments = model.Comments;
            oldData.GSTNo = model.GSTNo;
            oldData.Status = model.Status;
            oldData.IsDraft = model.IsDraft;
            foreach (var set in model.OrderSetRequestDto)
            {
                orderSet.SetName = set.SetName;
                orderSet.TotalAmount = set.TotalAmount;
                foreach (var item in set.OrderSetItemRequestDto)
                {
                    orderSetItem.SubjectTypeId = item.SubjectTypeId;
                    orderSetItem.SubjectId = item.SubjectId;
                    orderSetItem.ProductImage = item.ProductImage;
                    orderSetItem.Width = item.Width;
                    orderSetItem.Height = item.Height;
                    orderSetItem.Depth = item.Depth;
                    orderSetItem.Quantity = item.Quantity;
                    orderSetItem.UnitPrice = item.UnitPrice;
                    orderSetItem.DiscountPrice = item.DiscountPrice;
                    orderSetItem.TotalAmount = item.TotalAmount;
                    orderSetItem.Comment = item.Comment;
                    orderSetItem.Status = (byte)ProductStatusEnum.Pending;
                }
            }
        }

        #endregion Private Method
    }
}