using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.DataGrid;
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
                                         CreatedDate = x.CreatedDate,
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
                var orderById = await _unitOfWorkDA.OrderDA.GetById(Id, cancellationToken);
                var orderById1 = await _unitOfWorkDA.OrderDA.GetAll(cancellationToken);
                orderResponseDto = _mapper.Map<OrderResponseDto>(orderById);
                var customerData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);

                // Order Set Data by Order Id
                var orderSetAllData = await _unitOfWorkDA.OrderDA.GetAllOrderSet(cancellationToken);
                var orderSetDataByOrderId = orderSetAllData.Where(x => x.OrderId == Id).ToList();
                orderResponseDto.OrderSetResponseDto = _mapper.Map<List<OrderSetResponseDto>>(orderSetDataByOrderId);

                // Order Set Item by Order Id and Order Set Id
                var orderSetItemAllData = await _unitOfWorkDA.OrderDA.GetAllOrderSetItem(cancellationToken);
                foreach (var item in orderResponseDto.OrderSetResponseDto)
                {
                    var orderSetItemDataById = orderSetItemAllData.Where(x => x.OrderId == Id && x.OrderSetId == item.OrderSetId).ToList();
                    item.OrderSetItemResponseDto = _mapper.Map<List<OrderSetItemResponseDto>>(orderSetItemDataById);
                }
                return orderResponseDto;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<OrderRequestDto> CreateOrder(OrderRequestDto model, CancellationToken cancellationToken)
        {
            var orderToInsert = _mapper.Map<Order>(model);
            orderToInsert.Status = 0;
            orderToInsert.CreatedBy = _currentUser.UserId;
            orderToInsert.CreatedDate = DateTime.Now;
            orderToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.OrderDA.CreateOrder(orderToInsert, cancellationToken);
            return _mapper.Map<OrderRequestDto>(data);
        }

        public async Task<IEnumerable<OrderForDorpDownByOrderNoResponseDto>> GetOrderForDropDownByOrderNo(string orderNo, CancellationToken cancellationToken)
        {
            var orderAllData = await _unitOfWorkDA.OrderDA.GetAll(cancellationToken);
            return orderAllData.Where(x => x.OrderNo.StartsWith(orderNo)).Select(x => new OrderForDorpDownByOrderNoResponseDto(x.OrderId, x.OrderNo, "Order")).ToList();
        }

        public async Task<OrderResponseDto> GetOrderForDetailsByOrderNo(string searchText, CancellationToken cancellationToken)
        {
            var orderAlldata = await _unitOfWorkDA.OrderDA.GetAll(cancellationToken);
            var data = orderAlldata.Where(x => x.OrderNo == searchText);
            return _mapper.Map<OrderResponseDto>(data);
        }
    }
}