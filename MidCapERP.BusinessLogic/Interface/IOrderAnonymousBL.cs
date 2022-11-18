using MidCapERP.Dto.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IOrderAnonymousBL
    {
        public Task<OrderResponseDto> CreateOrderDetailsAnonymous(OrderResponseDto model, CancellationToken cancellationToken);
    }
}
