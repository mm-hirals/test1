using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Order;

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

        public async Task<IEnumerable<OrderForDorpDownByOrderNoResponseDto>> GetCustomerForDropDownByOrderNo(string orderNo, CancellationToken cancellationToken)
        {
            var orderAllData = await _unitOfWorkDA.OrderDA.GetAll(cancellationToken);
            return orderAllData.Where(x => x.OrderNo.StartsWith(orderNo)).Select(x => new OrderForDorpDownByOrderNoResponseDto(x.OrderId, x.OrderNo , "Order")).ToList();
        }

        public async Task<OrderResponseDto> GetOrderForDetailsByOrderNo(string searchText, CancellationToken cancellationToken)
        {
            var orderAlldata = await _unitOfWorkDA.OrderDA.GetAll(cancellationToken);
            var data = orderAlldata.FirstOrDefault(x => x.OrderNo.StartsWith(searchText));
            return _mapper.Map<OrderResponseDto>(data);
        }
    }
}