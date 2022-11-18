using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Order;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class OrderAnonymousBL : IOrderAnonymousBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        public readonly CurrentUser _currentUser;

        public OrderAnonymousBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<OrderResponseDto> CreateOrderDetailsAnonymous(OrderResponseDto model, CancellationToken cancellationToken)
        {
            OrderAnonymousView orderAnonymousData = new OrderAnonymousView();
            orderAnonymousData.OrderId = model.OrderId;
            orderAnonymousData.CustomerId = model.CustomerID;
            orderAnonymousData.IPAddress = model.IpAddress;
            orderAnonymousData.CreatedDate = DateTime.Now;
            orderAnonymousData.CreatedUTCDate = DateTime.Now;
            var data = await _unitOfWorkDA.OrderAnonymousDA.CreateOrderAnonymousViews(orderAnonymousData, cancellationToken);
            return _mapper.Map<OrderResponseDto>(data);
        }
    }
}
