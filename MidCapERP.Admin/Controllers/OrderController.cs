using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Order;
using MidCapERP.Infrastructure.Constants;

namespace MidCapERP.Admin.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public OrderController(IStringLocalizer<BaseController> localizer, IUnitOfWorkBL unitOfWorkBL) : base(localizer)
        {
            _unitOfWorkBL = unitOfWorkBL;
        }

        [Authorize(ApplicationIdentityConstants.Permissions.Order.View)]
        public IActionResult Index(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpPost]
        [Authorize(ApplicationIdentityConstants.Permissions.Order.View)]
        public async Task<IActionResult> GetOrderData([FromForm] DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkBL.OrderBL.GetFilterOrderData(dataTableFilterDto, cancellationToken);
            return Ok(data);
        }

        public async Task<IActionResult> OrderDetail(long Id, CancellationToken cancellationToken)
        {
            OrderResponseDto orderData = new OrderResponseDto();
            if (Id > 0)
            {
                orderData = await _unitOfWorkBL.OrderBL.GetOrderDetailData(Id, cancellationToken);
            }
            return View(orderData);
        }

        public async Task<IActionResult> CustomerDetail(long CustomerId, CancellationToken cancellationToken)
        {
            var customerById = await _unitOfWorkBL.CustomersBL.GetById(CustomerId, cancellationToken);
            return PartialView("Order_CustomerPartial", customerById);
        }
    }
}